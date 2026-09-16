#pragma warning disable MA0004
#pragma warning disable MA0006
using System;
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
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
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

    /// <summary>
    /// Set only when the container was spawned locally via Testcontainers.
    /// On CI, GitLab already runs as a pre-existing service container, so this stays null
    /// and credential generation falls back to a raw Docker Engine API exec call.
    /// </summary>
    private IContainer _container;

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

        // https://docs.gitlab.com/omnibus/settings/configuration.html
        // GitLab reports "healthy" long before it's actually ready to serve requests, so we wait
        // on the HTTP endpoint itself rather than on the container's health status.
        // WithReuse keeps an existing container running across local test runs (GitLab takes
        // minutes to boot); bumping LocalGitLabDockerVersion requires removing the old container
        // manually (`docker rm -f NGitLabClientTests`) since reuse matching is name+config based.
        _container = new ContainerBuilder()
            .WithImage(ImageName + ":" + LocalGitLabDockerVersion)
            .WithName(ContainerName)
            .WithHostname("localhost")
            .WithPortBinding(HttpPort, HttpPort)
            .WithEnvironment("GITLAB_ROOT_PASSWORD", AdminPassword)
            .WithEnvironment("GITLAB_OMNIBUS_CONFIG", string.Join("; ", omnibusConfig))
            .WithCreateParameterModifier(p => p.HostConfig.ShmSize = 512 * 1024 * 1024) // Default 64mb is too small and causes intermittent GitLab crashes
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(
                    r => r.ForPort((ushort)HttpPort).ForPath("/"),
                    o => o.WithTimeout(TimeSpan.FromMinutes(10)).WithInterval(TimeSpan.FromSeconds(5))))
            .WithReuse(true)
            .Build();

        TestContext.Progress.WriteLine("Starting the GitLab Docker container (this can take several minutes on first run)");
        await _container.StartAsync().ConfigureAwait(false);

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
            credentials.AdminUserToken = await retryPolicy.ExecuteAsync(() => RunGitLabRailsRunnerAsync(script)).ConfigureAwait(false);
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

    // When we spawned the container ourselves (local dev), Testcontainers already holds a reference to it
    // and can exec into it directly. On CI, GitLab runs as a pre-existing service container we didn't create,
    // so we fall back to the raw Docker Engine API to find it and exec into it.
    private async Task<string> RunGitLabRailsRunnerAsync(string script)
    {
        string stdout;
        string stderr;
        long? exitCode;

        if (_container != null)
        {
            var result = await _container.ExecAsync(["gitlab-rails", "runner", script]).ConfigureAwait(false);
            (stdout, stderr, exitCode) = (result.Stdout, result.Stderr, result.ExitCode);
        }
        else
        {
            using var client = new DockerClientBuilder()
                .WithEndpoint(new Uri(OperatingSystem.IsWindows() ? "npipe://./pipe/docker_engine" : "unix:///var/run/docker.sock"))
                .Build();
            await ValidateDockerIsEnabled(client).ConfigureAwait(false);

            var containerId = await ResolveGitLabContainerIdAsync(client).ConfigureAwait(false);
            var execCreateResponse = await client.Exec.CreateContainerExecAsync(containerId, new ContainerExecCreateParameters
            {
                AttachStdout = true,
                AttachStderr = true,
                Cmd = ["gitlab-rails", "runner", script],
            }).ConfigureAwait(false);

            using (var stream = await client.Exec.StartContainerExecAsync(execCreateResponse.ID, new ContainerExecStartParameters { TTY = false }).ConfigureAwait(false))
            {
                (stdout, stderr) = await stream.ReadOutputToEndAsync(CancellationToken.None).ConfigureAwait(false);
            }

            var inspectResponse = await client.Exec.InspectContainerExecAsync(execCreateResponse.ID).ConfigureAwait(false);
            exitCode = inspectResponse.ExitCode;
        }

        if (exitCode != 0)
            throw new InvalidOperationException($"'gitlab-rails runner' failed with exit code {exitCode}.\nStdout: {stdout}\nStderr: {stderr}");

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
