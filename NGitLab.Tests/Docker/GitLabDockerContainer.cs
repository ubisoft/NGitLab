#pragma warning disable MA0004
#pragma warning disable MA0006
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using Docker.DotNet;
using Docker.DotNet.Models;
using NGitLab.Models;
using NUnit.Framework;
using Polly;

namespace NGitLab.Tests.Docker
{
    public class GitLabDockerContainer
    {
        public const string ContainerName = "NGitLabClientTests";
        public const string ImageName = "gitlab/gitlab-ee";

        // https://hub.docker.com/r/gitlab/gitlab-ee/tags/
        public const string GitLabDockerVersion = "14.2.5-ee.0"; // Keep in sync with .github/workflows/ci.yml

        private static string s_creationErrorMessage;
        private static readonly SemaphoreSlim s_setupLock = new(initialCount: 1, maxCount: 1);
        private static GitLabDockerContainer s_instance;

        public string Host { get; private set; } = "localhost";

        public int HttpPort { get; private set; } = 48624;

        public string AdminUserName { get; } = "root";

        public static string AdminPassword
        {
            get
            {
                var env = Environment.GetEnvironmentVariable("GITLAB_ROOT_PASSWORD");
                if (!string.IsNullOrEmpty(env))
                    return env;

                return "Pa$$w0rd";
            }
        }

        public string LicenseFile { get; set; }

        public Uri GitLabUrl => new("http://" + Host + ":" + HttpPort.ToString(CultureInfo.InvariantCulture));

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

                    try
                    {
                        var instance = new GitLabDockerContainer();
                        await instance.SetupAsync().ConfigureAwait(false);
                        s_instance = instance;
                    }
                    catch (Exception ex)
                    {
                        s_creationErrorMessage = ex.ToString();
                        throw;
                    }
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
                s_creationErrorMessage = "Cannot connect to Docker service. Make sure it's running on your machine before launching any tests.\nDetails: " + ex.ToString();
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
                            var result = await httpClient.GetStringAsync(GitLabUrl).ConfigureAwait(false);
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

            TestContext.Progress.WriteLine("Looking up GitLab Docker containers");
            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true }).ConfigureAwait(false);
            var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName, StringComparer.Ordinal));
            if (container != null)
            {
                TestContext.Progress.WriteLine("Verifying if the GitLab Docker container is using the right image");
                var inspect = await client.Containers.InspectContainerAsync(container.ID).ConfigureAwait(false);
                var inspectImage = await client.Images.InspectImageAsync(ImageName + ":" + GitLabDockerVersion).ConfigureAwait(false);
                if (inspect.Image != inspectImage.ID)
                {
                    TestContext.Progress.WriteLine("Ending GitLab Docker container, as it's using the wrong image");
                    await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters() { Force = true }).ConfigureAwait(false);
                    container = null;
                }
            }

            if (container == null)
            {
                // Download GitLab images
                TestContext.Progress.WriteLine("Making sure the right GitLab Docker image is available locally");
                await client.Images.CreateImageAsync(new ImagesCreateParameters() { FromImage = ImageName, Tag = GitLabDockerVersion }, new AuthConfig() { }, new Progress<JSONMessage>()).ConfigureAwait(false);

                // Create the container
                TestContext.Progress.WriteLine("Creating the GitLab Docker container");
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
                        "GITLAB_ROOT_PASSWORD=" + AdminPassword,
                    },
                }).ConfigureAwait(false);

                containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true }).ConfigureAwait(false);
                container = containers.First(c => c.ID == response.ID);
            }

            // Start the container
            if (container.State != "running")
            {
                TestContext.Progress.WriteLine("Starting the GitLab Docker container");
                var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters()).ConfigureAwait(false);
                if (!started)
                {
                    Assert.Fail("Cannot start the Docker container");
                }
            }

            // Wait for the container to be ready.
            var stopwatch = Stopwatch.StartNew();
            while (true)
            {
                TestContext.Progress.WriteLine($"Waiting for the GitLab Docker container to be ready ({stopwatch.Elapsed})");
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

            TestContext.Progress.WriteLine("GitLab Docker container is ready");
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
                result = await ReloadIfError(context, result);

                TestContext.Progress.WriteLine("Navigating to " + result.Location);
                if (result.Location.PathName == "/users/password/edit")
                {
                    TestContext.Progress.WriteLine("Creating root password");
                    var form = result.Forms["new_user"];
                    if (form == null)
                        throw new InvalidOperationException($"Cannot set the root password. The page doesn't contain the form 'new_user'");

                    ((IHtmlInputElement)form["user[password]"]).Value = AdminPassword;
                    ((IHtmlInputElement)form["user[password_confirmation]"]).Value = AdminPassword;
                    result = await form.SubmitAsync();
                    TestContext.Progress.WriteLine("Navigating to " + result.Location);
                }

                // Login
                if (result.Location.PathName == "/users/sign_in")
                {
                    result = await SignIn(context);
                }

                // Create a token
                if (result.Location.PathName == "/")
                {
                    result = await GeneratePersonalAccessToken(credentials, context).ConfigureAwait(false);
                }

                // Get X-Profile-Token
                result = await GenerateXProfileToken(credentials, context, result).ConfigureAwait(false);

                // Get admin login cookie
                // result.Cookie: experimentation_subject_id=XXX; _gitlab_session=XXXX; known_sign_in=XXXX
                TestContext.Progress.WriteLine("Extracting GitLab session cookie");
                credentials.AdminCookies = result.Cookie.Split(';').Select(part => part.Trim()).Single(part => part.StartsWith("_gitlab_session=", StringComparison.Ordinal))["_gitlab_session=".Length..];

                Task<IDocument> SignIn(IBrowsingContext context)
                {
                    return Policy.Handle<InvalidOperationException>()
                          .WaitAndRetryAsync(10, i => TimeSpan.FromSeconds(1))
                          .ExecuteAsync(async () =>
                          {
                              var result = await context.OpenAsync(GitLabUrl + "/users/sign_in").ConfigureAwait(false);
                              if (result.StatusCode == HttpStatusCode.BadGateway)
                                  throw new InvalidOperationException("Cannot open sign in page:\n" + result.ToHtml());

                              TestContext.Progress.WriteLine("Logging in root user");
                              var form = result.Forms["new_user"];
                              if (form is null)
                                  throw new InvalidOperationException("Cannot find the form 'new_user' in the page:\n" + result.ToHtml());

                              ((IHtmlInputElement)form["user[login]"]).Value = AdminUserName;
                              ((IHtmlInputElement)form["user[password]"]).Value = AdminPassword;
                              ((IHtmlInputElement)form["user[remember_me]"]).IsChecked = true;
                              result = await form.SubmitAsync();
                              TestContext.Progress.WriteLine("Navigating to " + result.Location);
                              if (result.StatusCode == HttpStatusCode.BadGateway)
                                  throw new InvalidOperationException("Cannot sign in:\n" + result.ToHtml());

                              return result;
                          });
                }

                Task<IDocument> GeneratePersonalAccessToken(GitLabCredential credentials, IBrowsingContext context)
                {
                    return Policy.Handle<InvalidOperationException>()
                         .WaitAndRetryAsync(10, i => TimeSpan.FromSeconds(1))
                         .ExecuteAsync(async () =>
                         {
                             TestContext.Progress.WriteLine("Creating root token");
                             var result = await context.OpenAsync(GitLabUrl + "/profile/personal_access_tokens").ConfigureAwait(false);
                             if (result.StatusCode == HttpStatusCode.NotFound)
                             {
                                 result = await context.OpenAsync(GitLabUrl + "/-/profile/personal_access_tokens").ConfigureAwait(false);
                             }

                             var form = result.Forms["new_personal_access_token"];
                             if (form is null)
                                 throw new InvalidOperationException("The page does not contain the form 'new_personal_access_token':\n" + result.ToHtml());

                             var htmlInputElement = (IHtmlInputElement)form["personal_access_token[name]"];
                             if (htmlInputElement is null)
                                 throw new InvalidOperationException("The page does not contain the field 'new_personal_access_token.personal_access_token[name]':\n" + result.ToHtml());

                             htmlInputElement.Value = $"GitLabClientTest-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
                             foreach (var element in form.Elements.OfType<IHtmlInputElement>().Where(e => e.Name == "personal_access_token[scopes][]"))
                             {
                                 element.IsChecked = true;
                             }

                             result = await form.SubmitAsync().ConfigureAwait(false);
                             TestContext.Progress.WriteLine("Navigating to " + result.Location);
                             if (result.StatusCode == HttpStatusCode.BadGateway)
                                 throw new InvalidOperationException("Cannot generate a personal access token:\n" + result.ToHtml());

                             var personalAccessTokenElement = result.GetElementById("created-personal-access-token");
                             if (personalAccessTokenElement is null)
                                 throw new InvalidOperationException("The page does not contain the element 'created-personal-access-token':\n" + result.ToHtml());

                             credentials.AdminUserToken = personalAccessTokenElement.GetAttribute("value");
                             return result;
                         });
                }

                Task<IDocument> GenerateXProfileToken(GitLabCredential credentials, IBrowsingContext context, IDocument result)
                {
                    return Policy.Handle<InvalidOperationException>()
                        .WaitAndRetryAsync(10, i => TimeSpan.FromSeconds(1))
                        .ExecuteAsync(async () =>
                        {
                            TestContext.Progress.WriteLine("Generating request profiles token");
                            result = await context.OpenAsync(GitLabUrl + "/admin/requests_profiles").ConfigureAwait(false);
                            TestContext.Progress.WriteLine("Navigating to " + result.Location);
                            if (result.StatusCode == HttpStatusCode.BadGateway)
                                throw new InvalidOperationException("Cannot navigate to admin/requests_profiles:\n" + result.ToHtml());

                            var codeElements = result.QuerySelectorAll("code").ToList();
                            var tokenElement = codeElements.SingleOrDefault(n => n.TextContent.Trim().StartsWith("X-Profile-Token:", StringComparison.Ordinal));
                            if (tokenElement == null)
                                throw new InvalidOperationException("Cannot find X-Profile-Token in the page:\n" + result.ToHtml());

                            credentials.ProfileToken = tokenElement.TextContent.Trim()["X-Profile-Token:".Length..].Trim();
                            return result;
                        });
                }
            }

            void GenerateUserToken()
            {
                var retryPolicy = Policy.Handle<GitLabException>().WaitAndRetry(10, _ => TimeSpan.FromSeconds(1));
                var client = new GitLabClient(GitLabUrl.ToString(), credentials.AdminUserToken);
                var user = retryPolicy.Execute(() => client.Users.Get("common_user")).FirstOrDefault();
                if (user == null)
                {
                    try
                    {
                        user = retryPolicy.Execute(() => client.Users.Create(new UserUpsert()
                        {
                            Username = "common_user",
                            Email = "common_user@example.com",
                            IsAdmin = false,
                            Name = "common_user",
                            SkipConfirmation = true,
                            ResetPassword = false,
                            Password = AdminPassword,
                        }));
                    }
                    catch (GitLabException)
                    {
                        user = retryPolicy.Execute(() => client.Users.Get("common_user")).FirstOrDefault();
                        if (user == null)
                            throw new InvalidOperationException("Cannot create the common user");
                    }
                }

                var token = retryPolicy.Execute(() => client.Users.CreateToken(new UserTokenCreate
                {
                    UserId = user.Id,
                    Name = "common_user",
                    Scopes = new[] { "api" },
                }));

                credentials.UserToken = token.Token;
            }

            async Task<IDocument> ReloadIfError(IBrowsingContext context, IDocument document)
            {
                while (document.StatusCode is HttpStatusCode.BadGateway or HttpStatusCode.ServiceUnavailable)
                {
                    await Task.Delay(1000);
                    document = await context.OpenAsync(document.Url);
                }

                return document;
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
