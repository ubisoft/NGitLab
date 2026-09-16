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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using NGitLab.Models;
using NUnit.Framework;
using Polly;

namespace NGitLab.Tests.Docker;

public class GitLabDockerContainer
{
    public const string ContainerName = "NGitLabClientTests";
    public const string ImageName = "gitlab/gitlab-ee";

    /// <summary>
    /// GitLab docker image version to spawn.
    /// Used only on local environment (CI should already have a running GitLab instance from its services)
    /// </summary>
    /// <remarks>
    /// <para>Keep in sync with .github/workflows/ci.yml, use the lowest supported version</para>
    /// <para>List of available versions: https://hub.docker.com/r/gitlab/gitlab-ee/tags/</para>
    /// </remarks>
    private const string LocalGitLabDockerVersion = "19.3.1-ee.0";

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
        if (GitLabTestContext.IsContinuousIntegration())
        {
            await WaitForCiGitLabInstance().ConfigureAwait(false);
        }
        else
        {
            await SpawnDockerContainerAsync().ConfigureAwait(false);
        }

        LoadCredentials();

        if (Credentials != null)
        {
            Console.WriteLine("Using credentials from persisted credential file");
            return;
        }

        await GenerateCredentialsAsync().ConfigureAwait(false);
        PersistCredentialsAsync();
    }

    private static async Task ValidateDockerIsEnabled(DockerClient client)
    {
        try
        {
            await client.Images.ListImagesAsync(new ImagesListParameters()).ConfigureAwait(false);
        }
        catch (ArgumentOutOfRangeException ex) when (ex.Message.StartsWith("The added or subtracted value results in an un-representable DateTime.", StringComparison.Ordinal))
        {
            // Ignore https://github.com/rancher-sandbox/rancher-desktop/issues/5145
        }
        catch (Exception ex)
        {
            s_creationErrorMessage = "Cannot connect to Docker service. Make sure it's running on your machine before launching any tests.\nDetails: " + ex;
            Assert.Fail(s_creationErrorMessage);
        }
    }

    private async Task SpawnDockerContainerAsync()
    {
        Console.WriteLine($"Executing tests locally. Spawning GitLab docker image version '{LocalGitLabDockerVersion}'");
        using var httpClient = new HttpClient();

        // Spawn the container
        // https://docs.gitlab.com/omnibus/settings/configuration.html
        using var conf = new DockerClientConfiguration(new Uri(OperatingSystem.IsWindows() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock"));
        using var client = conf.CreateClient();
        await ValidateDockerIsEnabled(client);

        TestContext.Progress.WriteLine("Looking up GitLab Docker containers");
        var containers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true }).ConfigureAwait(false);
        var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName, StringComparer.Ordinal));
        if (container != null)
        {
            TestContext.Progress.WriteLine("Verifying if the GitLab Docker container is using the right image");
            var inspect = await client.Containers.InspectContainerAsync(container.ID).ConfigureAwait(false);
            var inspectImage = await client.Images.InspectImageAsync(ImageName + ":" + LocalGitLabDockerVersion).ConfigureAwait(false);
            if (inspect.Image != inspectImage.ID)
            {
                TestContext.Progress.WriteLine("Ending GitLab Docker container, as it's using the wrong image");
                await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters { Force = true }).ConfigureAwait(false);
                container = null;
            }
        }

        if (container == null)
        {
            // Download GitLab images
            TestContext.Progress.WriteLine("Making sure the right GitLab Docker image is available locally");
            await client.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = ImageName, Tag = LocalGitLabDockerVersion }, new AuthConfig(), new Progress<JSONMessage>()).ConfigureAwait(false);

            // Create the container
            TestContext.Progress.WriteLine("Creating the GitLab Docker container");
            var hostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>(StringComparer.Ordinal)
                {
                    { HttpPort.ToString(CultureInfo.InvariantCulture) + "/tcp", new List<PortBinding> { new PortBinding { HostPort = HttpPort.ToString(CultureInfo.InvariantCulture) } } },
                },

                // Update size of /dev/shm to to 512mb (default: 64mb)
                // Avoids intermittent crashes of GitLab
                ShmSize = 512 * 1024 * 1024,
            };

            // Disables non-useful features
            // See https://gitlab.com/gitlab-org/omnibus-gitlab/blob/master/files/gitlab-config-template/gitlab.rb.template
            string[] omnibusConfig =
            [
                $"external_url 'http://localhost:{HttpPort.ToString(CultureInfo.InvariantCulture)}/'",
                "gitlab_rails['gitlab_email_enabled'] = false",
                "gitlab_rails['incoming_email_enabled'] = false",
                "gitlab_rails['lfs_enabled'] = false",
                "gitlab_rails['terraform_state_enabled'] = false",
                "gitlab_rails['pages_object_store_enabled'] = false",
                "gitlab_rails['usage_ping_enabled'] = false",
                "gitlab_rails['registry_enabled'] = false",
                "registry['enable'] = false",
                "sidekiq['metrics_enabled'] = false",
                "logrotate['enable'] = false",
                "gitlab_pages['enable'] = false",
                "gitlab_rails['gitlab_kas_enabled'] = false",
                "mattermost['enable'] = false",
                "alertmanager['enable'] = false",
                "node_exporter['enable'] = false",
                "redis_exporter['enable'] = false",
                "postgres_exporter['enable'] = false",
                "pgbouncer_exporter['enable'] = false",
                "gitlab_exporter['enable'] = false",
                "gitlab_rails['kerberos_enabled'] = false",
                "gitlab_rails['packages_enabled'] = false",
                "gitlab_rails['dependency_proxy_enabled'] = false",
            ];

            var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Hostname = "localhost",
                Image = ImageName + ":" + LocalGitLabDockerVersion,
                Name = ContainerName,
                Tty = false,
                HostConfig = hostConfig,
                ExposedPorts = new Dictionary<string, EmptyStruct>(StringComparer.Ordinal)
                {
                    { HttpPort.ToString(CultureInfo.InvariantCulture) + "/tcp", default },
                },
                Env =
                [
                    $"GITLAB_ROOT_PASSWORD={AdminPassword}",
                    $"GITLAB_OMNIBUS_CONFIG={string.Join("; ", omnibusConfig)}",
                ],
            }).ConfigureAwait(false);

            containers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true }).ConfigureAwait(false);
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
            TestContext.Progress.WriteLine($@"Waiting for the GitLab Docker container to be ready ({stopwatch.Elapsed:mm\:ss})");
            var status = await client.Containers.InspectContainerAsync(container.ID);
            if (!status.State.Running)
                throw new InvalidOperationException($"Container '{status.ID}' is not running");

            var healthState = status.State.Health.Status;

            // unhealthy is valid as long as the container is running as it may indicate a slow creation
            if (healthState is "starting" or "unhealthy")
            {
            }
            else if (healthState is "healthy")
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
            }
            else
            {
                throw new InvalidOperationException($"Container status '{healthState}' is not supported");
            }

            await Task.Delay(5000);
        }

        TestContext.Progress.WriteLine("GitLab Docker container is ready");
    }

    private async Task GenerateCredentialsAsync()
    {
        Console.WriteLine("Requesting credentials from GitLab instance");

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

            using var conf = new DockerClientConfiguration(new Uri(OperatingSystem.IsWindows() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock"));
            using var client = conf.CreateClient();
            await ValidateDockerIsEnabled(client).ConfigureAwait(false);

            TestContext.Progress.WriteLine("Creating root token via 'gitlab-rails runner'");

            // Keep only scopes the running GitLab version supports (an unknown scope makes `create!` raise).
            const string script = """
                desired_scopes = %w[api read_user read_api read_repository write_repository sudo admin_mode create_runner manage_runner k8s_proxy]
                available_scopes = Gitlab::Auth.all_available_scopes.map(&:to_s)
                token = User.find_by_username!('root').personal_access_tokens.create!(
                  name: 'NGitLabClientTest',
                  scopes: (desired_scopes & available_scopes),
                  expires_at: 1.year.from_now)
                puts token.token
                """;

            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(20, _ => TimeSpan.FromSeconds(3));
            var token = await retryPolicy.ExecuteAsync(async () =>
            {
                var containerId = await ResolveGitLabContainerIdAsync(client).ConfigureAwait(false);
                return await RunGitLabRailsRunnerAsync(client, containerId, script).ConfigureAwait(false);
            }).ConfigureAwait(false);

            credentials.AdminUserToken = token;
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
                    user = retryPolicy.Execute(() => client.Users.Create(new UserUpsert
                    {
                        Username = "common_user",
                        Email = "common_user@example.com",
                        IsAdmin = false,
                        Name = "common_user",
                        SkipConfirmation = true,
                        ResetPassword = false,
                        Password = AdminPassword,
                        IsPrivateProfile = true, // Set profile to private for LastActivity test cases
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
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            }));

            credentials.UserToken = token.Token;
        }
    }

    private static async Task<string> ResolveGitLabContainerIdAsync(DockerClient client)
    {
        var containers = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true }).ConfigureAwait(false);

        var container = containers.FirstOrDefault(c => c.Names.Contains("/" + ContainerName, StringComparer.Ordinal))
            ?? containers.FirstOrDefault(c => c.Image.StartsWith(ImageName, StringComparison.Ordinal));

        if (container == null)
            throw new InvalidOperationException($"Cannot find a running Docker container for image '{ImageName}' to generate credentials from.");

        return container.ID;
    }

    private static async Task<string> RunGitLabRailsRunnerAsync(DockerClient client, string containerId, string script)
    {
        var execCreateResponse = await client.Exec.ExecCreateContainerAsync(containerId, new ContainerExecCreateParameters
        {
            AttachStdout = true,
            AttachStderr = true,
            Cmd = ["gitlab-rails", "runner", script],
        }).ConfigureAwait(false);

        string stdout;
        string stderr;
        using (var stream = await client.Exec.StartAndAttachContainerExecAsync(execCreateResponse.ID, tty: false).ConfigureAwait(false))
        {
            (stdout, stderr) = await stream.ReadOutputToEndAsync(CancellationToken.None).ConfigureAwait(false);
        }

        var inspectResponse = await client.Exec.InspectContainerExecAsync(execCreateResponse.ID).ConfigureAwait(false);
        if (inspectResponse.ExitCode != 0)
            throw new InvalidOperationException($"'gitlab-rails runner' failed with exit code {inspectResponse.ExitCode}.\nStdout: {stdout}\nStderr: {stderr}");

        var token = stdout
            .Split('\n')
            .Select(line => line.Trim())
            .LastOrDefault(line => line.Length > 0);

        if (string.IsNullOrEmpty(token))
            throw new InvalidOperationException($"'gitlab-rails runner' did not output a token.\nStdout: {stdout}\nStderr: {stderr}");

        return token;
    }

    private void PersistCredentialsAsync()
    {
        var path = GetCredentialsFilePath();
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        var json = JsonSerializer.Serialize(Credentials);
        File.WriteAllText(path, json);
    }

    private void LoadCredentials()
    {
        var file = GetCredentialsFilePath();
        if (!File.Exists(file))
            return;

        var json = File.ReadAllText(file);
        var credentials = JsonSerializer.Deserialize<GitLabCredential>(json);
        if (credentials.AdminUserToken == null || credentials.UserToken == null)
            return;

        var client = new GitLabClient(GitLabUrl.ToString(), credentials.AdminUserToken);
        try
        {
            // Validate token
            _ = client.Users.Current;
            Credentials = credentials;
        }
        catch (GitLabException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
        }
    }

    private static string GetCredentialsFilePath()
    {
        return Path.Combine(Path.GetTempPath(), "ngitlab", "credentials.json");
    }

    private async Task WaitForCiGitLabInstance()
    {
        Console.WriteLine($"Executing tests on CI. Checking GitLab instance...");

        using var httpClient = new HttpClient();
        Console.WriteLine("Testing " + GitLabUrl);

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
