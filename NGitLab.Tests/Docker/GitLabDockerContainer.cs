#pragma warning disable MA0004
#pragma warning disable MA0006
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using Docker.DotNet;
using Docker.DotNet.Models;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.Docker
{
    public class GitLabDockerContainer
    {
        public const string ContainerName = "NGitLabClientTests";
        public const string ImageName = "gitlab/gitlab-ee";

        // https://hub.docker.com/r/gitlab/gitlab-ee/tags/
        public const string GitLabDockerVersion = "13.8.1-ee.0"; // Keep in sync with .github/workflows/ci.yml

        private static string s_creationErrorMessage;
        private static readonly SemaphoreSlim s_setupLock = new(initialCount: 1, maxCount: 1);
        private static GitLabDockerContainer s_instance;

        public string Host { get; private set; } = "localhost";

        public int HttpPort { get; private set; } = 48624;

        public string AdminUserName { get; } = "root";

        public string AdminPassword { get; } = "Pa$$w0rd";

        public string LicenseFile { get; set; }

        public Uri GitLabUrl => new Uri("http://" + Host + ":" + HttpPort.ToString(CultureInfo.InvariantCulture));

        public GitLabCredential Credentials { get; set; }

        public static async Task<GitLabDockerContainer> GetOrCreateInstance()
        {
            await s_setupLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (s_instance == null)
                {
                    if (s_creationErrorMessage != null)
                    {
                        Assert.Fail(s_creationErrorMessage);
                    }

                    var instance = new GitLabDockerContainer();
                    await instance.SetupAsync().ConfigureAwait(false);
                    s_instance = instance;
                }

                return s_instance;
            }
            finally
            {
                s_setupLock.Release();
            }
        }

        private async Task SetupAsync()
        {
            await SpawnDockerContainerAsync().ConfigureAwait(false);
            await LoadCredentialsAsync().ConfigureAwait(false);
            if (Credentials == null)
            {
                await GenerateCredentialsAsync().ConfigureAwait(false);
                PersistCredentialsAsync();
            }
        }

        private static async Task ValidateDockerIsEnabled(DockerClient client)
        {
            try
            {
                await client.Images.ListImagesAsync(new ImagesListParameters()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                s_creationErrorMessage = "The docker service is not accessible. Be sure to start docker on your machine before starting tests.\nDetails: " + ex.ToString();
                Assert.Fail(s_creationErrorMessage);
            }
        }

        private async Task SpawnDockerContainerAsync()
        {
            // Check if the container is accessible?
            var isContinuousIntegration = GitLabTestContext.IsContinuousIntegration();
            using var httpClient = new HttpClient();
            try
            {
                Console.WriteLine("Testing " + GitLabUrl);
                var result = await httpClient.GetStringAsync(GitLabUrl).ConfigureAwait(false);
                if (isContinuousIntegration) // When not on CI, we want to check the container is on the expected version
                    return;
            }
            catch
            {
                if (isContinuousIntegration)
                {
                    var now = Stopwatch.StartNew();
                    while (now.Elapsed < TimeSpan.FromMinutes(10))
                    {
                        try
                        {
                            var result = await httpClient.GetStringAsync(new Uri("http://ngitlab-test/")).ConfigureAwait(false);
                            HttpPort = 80;
                            Host = "ngitlab-test";
                            return;
                        }
                        catch
                        {
                        }

                        await Task.Delay(1000);
                    }

                    s_creationErrorMessage = "GitLab is not well configured in CI";
                    Assert.Fail(s_creationErrorMessage);
                }
            }

            // Spawn the container
            // https://docs.gitlab.com/omnibus/settings/configuration.html
            using var conf = new DockerClientConfiguration(new Uri(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock"));
            using var client = conf.CreateClient();
            await ValidateDockerIsEnabled(client);

            TestContext.Progress.WriteLine("Searching existing GitLab docker image");
            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true }).ConfigureAwait(false);
            var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName, StringComparer.Ordinal));
            if (container != null)
            {
                TestContext.Progress.WriteLine("Validating existing GitLab docker image");
                var inspect = await client.Containers.InspectContainerAsync(container.ID).ConfigureAwait(false);
                var inspectImage = await client.Images.InspectImageAsync(ImageName + ":" + GitLabDockerVersion).ConfigureAwait(false);
                if (inspect.Image != inspectImage.ID)
                {
                    TestContext.Progress.WriteLine("Removing existing GitLab docker image");
                    await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters() { Force = true }).ConfigureAwait(false);
                    container = null;
                }
            }

            if (container == null)
            {
                // Download GitLab images
                TestContext.Progress.WriteLine("Downloading GitLab docker image");
                await client.Images.CreateImageAsync(new ImagesCreateParameters() { FromImage = ImageName, Tag = GitLabDockerVersion }, new AuthConfig() { }, new Progress<JSONMessage>()).ConfigureAwait(false);

                // Create the container
                TestContext.Progress.WriteLine("Creating GitLab docker instance");
                var hostConfig = new HostConfig()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>(StringComparer.Ordinal)
                    {
                        {  HttpPort.ToString(CultureInfo.InvariantCulture) + "/tcp", new List<PortBinding> { new PortBinding { HostPort = HttpPort.ToString(CultureInfo.InvariantCulture) } } },
                    },
                };

                var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters()
                {
                    Hostname = "localhost",
                    Image = ImageName + ":" + GitLabDockerVersion,
                    Name = ContainerName,
                    Tty = false,
                    HostConfig = hostConfig,
                    ExposedPorts = new Dictionary<string, EmptyStruct>(StringComparer.Ordinal)
                    {
                        { HttpPort.ToString(CultureInfo.InvariantCulture) + "/tcp", default },
                    },
                    Env = new List<string>
                    {
                        "GITLAB_OMNIBUS_CONFIG=external_url 'http://localhost:" + HttpPort.ToString(CultureInfo.InvariantCulture) + "/'",
                    },
                }).ConfigureAwait(false);

                containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true }).ConfigureAwait(false);
                container = containers.First(c => c.ID == response.ID);
            }

            // Start the container
            if (container.State != "running")
            {
                TestContext.Progress.WriteLine("Starting GitLab docker image");
                var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters()).ConfigureAwait(false);
                if (!started)
                {
                    Assert.Fail("Cannot start the docker container");
                }
            }

            // Wait for the container to be ready.
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                TestContext.Progress.WriteLine($"Waiting for GitLab instance to be ready ({stopwatch.Elapsed})");
                var status = await client.Containers.InspectContainerAsync(container.ID);
                if (!status.State.Running)
                    throw new InvalidOperationException($"Container '{status.ID}' is not running");

                var healthState = status.State.Health.Status;

                // unhealthy is valid as long as the container is running as it may indicate a slow creation
                if (healthState == "starting" || healthState == "unhealthy")
                {
                    await Task.Delay(3000);
                }
                else if (healthState == "healthy")
                {
                    // A healthy container doesn't mean the service is actually running.
                    // GitLab has lots of configuration steps that are still running when the container is healthy.
                    try
                    {
                        using var response = await httpClient.GetAsync(GitLabUrl).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                            break;
                    }
                    catch
                    {
                    }

                    await Task.Delay(3000);
                }
                else
                {
                    throw new InvalidOperationException($"Container status '{healthState}' is not supported");
                }
            }

            TestContext.Progress.WriteLine("GitLab docker instance is ready");
        }

        private async Task GenerateCredentialsAsync()
        {
            var credentials = new GitLabCredential();
            await GenerateAdminToken(credentials).ConfigureAwait(false);
            if (credentials.AdminUserToken != null)
            {
                GenerateUserToken();
            }

            Credentials = credentials;

            async Task GenerateAdminToken(GitLabCredential credentials)
            {
                TestContext.Progress.WriteLine("Generating Credentials");

                var conf = Configuration.Default
                  .WithDefaultLoader(new LoaderOptions
                  {
                      IsNavigationDisabled = false,
                      IsResourceLoadingEnabled = true,
                      Filter = request =>
                      {
                          Console.WriteLine("Requesting " + request.Address);
                          return true;
                      },
                  })
                  .WithDefaultCookies()
                  .WithLocaleBasedEncoding();
                using var context = BrowsingContext.New(conf);

                // Change password
                var result = await context.OpenAsync(GitLabUrl.AbsoluteUri).ConfigureAwait(false);
                TestContext.Progress.WriteLine("Navigating to " + result.Location);
                if (result.Location.PathName == "/users/password/edit")
                {
                    TestContext.Progress.WriteLine("Creating root password");
                    var form = result.Forms["new_user"];
                    ((IHtmlInputElement)form["user[password]"]).Value = AdminPassword;
                    ((IHtmlInputElement)form["user[password_confirmation]"]).Value = AdminPassword;
                    result = await form.SubmitAsync();
                    TestContext.Progress.WriteLine("Navigating to " + result.Location);
                }

                // Login
                if (result.Location.PathName == "/users/sign_in")
                {
                    TestContext.Progress.WriteLine("Logging in root user");
                    var form = result.Forms["new_user"];
                    ((IHtmlInputElement)form["user[login]"]).Value = AdminUserName;
                    ((IHtmlInputElement)form["user[password]"]).Value = AdminPassword;
                    ((IHtmlInputElement)form["user[remember_me]"]).IsChecked = true;
                    result = await form.SubmitAsync();
                    TestContext.Progress.WriteLine("Navigating to " + result.Location);
                }

                // Create a token
                if (result.Location.PathName == "/")
                {
                    TestContext.Progress.WriteLine("Creating root token");
                    result = await context.OpenAsync(GitLabUrl + "/profile/personal_access_tokens").ConfigureAwait(false);
                    var form = result.Forms["new_personal_access_token"];
                    ((IHtmlInputElement)form["personal_access_token[name]"]).Value = $"GitLabClientTest-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
                    foreach (var element in form.Elements.OfType<IHtmlInputElement>().Where(e => e.Name == "personal_access_token[scopes][]"))
                    {
                        element.IsChecked = true;
                    }

                    result = await form.SubmitAsync().ConfigureAwait(false);
                    TestContext.Progress.WriteLine("Navigating to " + result.Location);

                    credentials.AdminUserToken = result.GetElementById("created-personal-access-token").GetAttribute("value");
                }

                // Get X-Profile-Token
                TestContext.Progress.WriteLine("Generating request profiles token");
                result = await context.OpenAsync(GitLabUrl + "/admin/requests_profiles").ConfigureAwait(false);
                TestContext.Progress.WriteLine("Navigating to " + result.Location);
                var codeElements = result.QuerySelectorAll("code").ToList();
                var tokenElement = codeElements.SingleOrDefault(n => n.TextContent.StartsWith("X-Profile-Token:", StringComparison.Ordinal));
                if (tokenElement == null)
                {
                    Assert.Fail("Cannot find X-Profile-Token in the page:\n\n" + result.DocumentElement.OuterHtml);
                }

                credentials.ProfileToken = tokenElement.TextContent.Substring("X-Profile-Token:".Length).Trim();

                // Get admin login cookie
                // result.Cookie: experimentation_subject_id=XXX; _gitlab_session=XXXX; known_sign_in=XXXX
                TestContext.Progress.WriteLine("Extracting gitlab session cookie");
                credentials.AdminCookies = result.Cookie.Split(';').Select(part => part.Trim()).Single(part => part.StartsWith("_gitlab_session=", StringComparison.Ordinal)).Substring("_gitlab_session=".Length);
            }

            void GenerateUserToken()
            {
                var client = new GitLabClient(GitLabUrl.ToString(), credentials.AdminUserToken);
                var user = client.Users.Get("common_user").FirstOrDefault();
                if (user == null)
                {
                    user = client.Users.Create(new UserUpsert()
                    {
                        Username = "common_user",
                        Email = "common_user@example.com",
                        IsAdmin = false,
                        Name = "common_user",
                        SkipConfirmation = true,
                        ResetPassword = false,
                        Password = AdminPassword,
                    });
                }

                var token = client.Users.CreateToken(new UserTokenCreate
                {
                    UserId = user.Id,
                    Name = "common_user",
                    Scopes = new[] { "api" },
                });

                credentials.UserToken = token.Token;
            }
        }

        private void PersistCredentialsAsync()
        {
            var path = GetCredentialsFilePath();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var json = JsonSerializer.Serialize(Credentials);
            File.WriteAllText(path, json);
        }

        private async Task LoadCredentialsAsync()
        {
            var file = GetCredentialsFilePath();
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);
                var credentials = JsonSerializer.Deserialize<GitLabCredential>(json);
                if (credentials.AdminUserToken == null || credentials.UserToken == null)
                    return;

                var client = new GitLabClient(GitLabUrl.ToString(), credentials.AdminUserToken);
                try
                {
                    // Validate token
                    var user = client.Users.Current;

                    using var httpClient = new HttpClient()
                    {
                        BaseAddress = GitLabUrl,
                        DefaultRequestHeaders =
                        {
                            { "Cookie", "_gitlab_session=" + credentials.AdminCookies },
                        },
                    };
                    var response = await httpClient.GetAsync(new Uri("/", UriKind.RelativeOrAbsolute));
                    if (response.RequestMessage.RequestUri.PathAndQuery == "/users/sign_in")
                        return;

                    // Validate cookie
                    Credentials = credentials;
                }
                catch (GitLabException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                }
            }
        }

        private static string GetCredentialsFilePath()
        {
            return Path.Combine(Path.GetTempPath(), "ngitlab", "credentials.json");
        }
    }
}
