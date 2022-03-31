using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;
using NUnit.Framework;
using Polly;

namespace NGitLab.Tests.Docker
{
    public sealed class GitLabTestContext : IDisposable
    {
        private const int MaxRetryCount = 10;

        private static readonly Policy s_gitlabRetryPolicy = Policy.Handle<GitLabException>()
            .WaitAndRetry(MaxRetryCount, _ => TimeSpan.FromSeconds(1), (exception, timespan, retryCount, context) =>
            {
                TestContext.WriteLine($"[{TestContext.CurrentContext.Test.FullName}] {exception.Message} -> Polly Retry {retryCount} of {MaxRetryCount}...");
            });

        private static readonly HashSet<string> s_generatedValues = new(StringComparer.Ordinal);
        private static readonly SemaphoreSlim s_prepareRunnerLock = new(1, 1);

        private readonly GitLabTestContextRequestOptions _customRequestOptions = new();
        private readonly List<IGitLabClient> _clients = new();

        public GitLabDockerContainer DockerContainer { get; set; }

        private GitLabTestContext(GitLabDockerContainer container)
        {
            DockerContainer = container;
            AdminClient = CreateClient(DockerContainer.Credentials.AdminUserToken);
            Client = CreateClient(DockerContainer.Credentials.UserToken);

            HttpClient = new HttpClient()
            {
                BaseAddress = DockerContainer.GitLabUrl,
            };

            AdminHttpClient = new HttpClient()
            {
                BaseAddress = DockerContainer.GitLabUrl,
                DefaultRequestHeaders =
                {
                    { "Cookie", "_gitlab_session=" + DockerContainer.Credentials.AdminCookies },
                },
            };
        }

        public static async Task<GitLabTestContext> CreateAsync()
        {
            // Disable proxy
            Environment.SetEnvironmentVariable("http_proxy", "", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("https_proxy", "", EnvironmentVariableTarget.Process);
            var container = await GitLabDockerContainer.GetOrCreateInstance().ConfigureAwait(false);
            return new GitLabTestContext(container);
        }

        public HttpClient HttpClient { get; }

        public HttpClient AdminHttpClient { get; }

        public IGitLabClient AdminClient { get; }

        public IGitLabClient Client { get; }

        public WebRequest LastRequest => _customRequestOptions.AllRequests[_customRequestOptions.AllRequests.Count - 1];

        private static bool IsUnique(string str)
        {
            lock (s_generatedValues)
            {
                return s_generatedValues.Add(str);
            }
        }

        public IGitLabClient CreateNewUserAsync() => CreateNewUser(out _);

        public IGitLabClient CreateNewUser(out User user)
        {
            var username = "user_" + DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString("N");
            var email = username + "@dummy.com";
            var password = "Pa$$w0rd";
            var client = AdminClient;

            user = client.Users.Create(new Models.UserUpsert()
            {
                Email = email,
                Username = username,
                Name = username,
                Password = password,
                CanCreateGroup = true,
                SkipConfirmation = true,
            });

            var token = client.Users.CreateToken(new Models.UserTokenCreate()
            {
                UserId = user.Id,
                Name = "UnitTest",
                Scopes = new[] { "api", "read_user" },
            });
            return CreateClient(token.Token);
        }

        public Project CreateProject(Action<ProjectCreate> configure = null, bool initializeWithCommits = false)
        {
            var client = Client;
            var defaultBranch = "main";

            var project = s_gitlabRetryPolicy.Execute(() =>
                {
                    var projectCreate = new ProjectCreate()
                    {
                        Name = GetUniqueRandomString(),
                        DefaultBranch = defaultBranch,
                        Description = "Test project",
                        IssuesEnabled = true,
                        MergeRequestsEnabled = true,
                        SnippetsEnabled = true,
                        VisibilityLevel = VisibilityLevel.Internal,
                        WikiEnabled = true,
                    };

                    configure?.Invoke(projectCreate);
                    return client.Projects.Create(projectCreate);
                });

            // When creating a project, GitLab's JSON response should indicate the 'default_branch'. However, it
            // currently returns null instead (at least in versions <= 13.10.3). This info would come in handy, since
            // a 'default_branch' might be specified at the GitLab instance, group, subgroup and/or project levels, and
            // we don't want to query those levels to determine what the default branch is for the current project.
            // Until https://gitlab.com/gitlab-org/gitlab/-/issues/330622 is resolved, we'll patch the value ourselves.
            project.DefaultBranch ??= defaultBranch;

            if (initializeWithCommits)
            {
                AddSomeCommits();
                s_gitlabRetryPolicy.Execute(() =>
                {
                    var projectUpdate = new ProjectUpdate()
                    {
                        DefaultBranch = defaultBranch,
                    };

                    return client.Projects.Update(project.Id.ToString(CultureInfo.InvariantCulture), projectUpdate);
                });
            }

            return project;

            void AddSomeCommits()
            {
                s_gitlabRetryPolicy.Execute(() =>
                    client.GetRepository(project.Id).Files.Create(new FileUpsert
                    {
                        Branch = project.DefaultBranch,
                        CommitMessage = "add readme",
                        Path = "README.md",
                        RawContent = "this project should only live during the unit tests, you can delete if you find some",
                    }));

                for (var i = 0; i < 3; i++)
                {
                    s_gitlabRetryPolicy.Execute(() =>
                        client.GetRepository(project.Id).Files.Create(new FileUpsert
                        {
                            Branch = project.DefaultBranch,
                            CommitMessage = $"add test file {i.ToString(CultureInfo.InvariantCulture)}",
                            Path = $"TestFile{i.ToString(CultureInfo.InvariantCulture)}.txt",
                            RawContent = "this project should only live during the unit tests, you can delete if you find some",
                        }));
                }
            }
        }

        public Group CreateGroup(Action<GroupCreate> configure = null)
        {
            var client = Client;
            var name = GetUniqueRandomString();
            var groupCreate = new GroupCreate()
            {
                Name = name,
                Path = name,
                Description = "Test group",
                Visibility = VisibilityLevel.Internal,
            };

            configure?.Invoke(groupCreate);
            return client.Groups.Create(groupCreate);
        }

        public (Project Project, MergeRequest MergeRequest) CreateMergeRequest()
        {
            var client = Client;
            var project = CreateProject(initializeWithCommits: true);

            const string BranchForMRName = "branch-for-mr";
            s_gitlabRetryPolicy.Execute(() => client.GetRepository(project.Id).Files.Create(new FileUpsert { Branch = project.DefaultBranch, CommitMessage = "test", Content = "test", Path = "test.md" }));
            s_gitlabRetryPolicy.Execute(() => client.GetRepository(project.Id).Branches.Create(new BranchCreate { Name = BranchForMRName, Ref = project.DefaultBranch }));

            // Restore the default branch because sometimes GitLab change the default branch to "branch-for-mr"
            project = client.Projects.Update(project.Id.ToString(CultureInfo.InvariantCulture), new ProjectUpdate
            {
                DefaultBranch = project.DefaultBranch,
            });

            var branch = client.GetRepository(project.Id).Branches.All.FirstOrDefault(b => string.Equals(b.Name, project.DefaultBranch, StringComparison.Ordinal));
            Assert.NotNull(branch, $"Branch '{project.DefaultBranch}' should exist");
            Assert.IsTrue(branch.Default, $"Branch '{project.DefaultBranch}' should be the default one");

            branch = client.GetRepository(project.Id).Branches.All.FirstOrDefault(b => string.Equals(b.Name, BranchForMRName, StringComparison.Ordinal));
            Assert.NotNull(branch, $"Branch '{BranchForMRName}' should exist");
            Assert.IsFalse(branch.Protected, $"Branch '{BranchForMRName}' should not be protected");

            s_gitlabRetryPolicy.Execute(() => client.GetRepository(project.Id).Files.Update(new FileUpsert { Branch = BranchForMRName, CommitMessage = "test", Content = "test2", Path = "test.md" }));

            var mr = client.GetMergeRequest(project.Id).Create(new MergeRequestCreate
            {
                SourceBranch = BranchForMRName,
                TargetBranch = project.DefaultBranch,
                Title = "test",
            });

            return (project, mr);
        }

        [SuppressMessage("Performance", "MA0038:Make method static", Justification = "By design")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By design")]
        public string GetUniqueRandomString()
        {
            for (var i = 0; i < 1000; i++)
            {
                var result = "GitLabClientTests_" + Guid.NewGuid().ToString("N");
                if (IsUnique(result))
                    return result;
            }

            throw new InvalidOperationException("Cannot generate a new random unique string");
        }

        [SuppressMessage("Design", "MA0038:Make method static", Justification = "By design")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "By design")]
        public int GetRandomNumber()
        {
            return RandomNumberGenerator.GetInt32(0, int.MaxValue);
        }

        private IGitLabClient CreateClient(string token)
        {
            var client = new GitLabClient(DockerContainer.GitLabUrl.ToString(), token, _customRequestOptions);
            _clients.Add(client);
            return client;
        }

        internal static bool IsContinuousIntegration()
        {
            return string.Equals(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"), "true", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(Environment.GetEnvironmentVariable("GITLAB_CI"), "true", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IDisposable> StartRunnerForOneJobAsync(int projectId)
        {
            // Download runner (windows / linux, GitLab version)
            await s_prepareRunnerLock.WaitAsync().ConfigureAwait(false);

            try
            {
                var path = Path.Combine(Path.GetTempPath(), "GitLabClient", "Runners", "gitlab-runner.exe");
                if (!File.Exists(path))
                {
                    if (!File.Exists(path))
                    {
                        Uri url;
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            url = new Uri($"https://gitlab-runner-downloads.s3.amazonaws.com/latest/binaries/gitlab-runner-windows-amd64.exe");
                        }
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            url = new Uri($"https://gitlab-runner-downloads.s3.amazonaws.com/latest/binaries/gitlab-runner-linux-amd64");
                        }
                        else
                        {
                            throw new InvalidOperationException($"OS '{RuntimeInformation.OSDescription}' is not supported");
                        }

                        var stream = await HttpClient.GetStreamAsync(url).ConfigureAwait(false);
                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));

                            var fs = File.OpenWrite(path);
                            try
                            {
                                await stream.CopyToAsync(fs).ConfigureAwait(false);
                            }
                            finally
                            {
                                await fs.DisposeAsync().ConfigureAwait(false);
                            }
                        }
                        finally
                        {
                            await stream.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                }

                TestContext.WriteLine("Test runner downloaded");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    using var chmodProcess = Process.Start("chmod", "+x \"" + path + "\"");
                    chmodProcess.WaitForExit();
                    if (chmodProcess.ExitCode != 0)
                        throw new InvalidOperationException("chmod failed");

                    TestContext.WriteLine("chmod run");
                }

                if (!IsContinuousIntegration())
                {
                    // Update the git configuration to remove any proxy for this host
                    // git config --global http.http://localhost:48624.proxy ""
                    using var gitConfigProcess = Process.Start("git", "config --global http.http://localhost:48624.proxy \"\"");
                    gitConfigProcess.WaitForExit();
                    if (gitConfigProcess.ExitCode != 0)
                        throw new InvalidOperationException("git config failed");

                    TestContext.WriteLine("git config changed");
                }

                var project = AdminClient.Projects[projectId];
                if (project.RunnersToken == null)
                    throw new InvalidOperationException("Project runner token is null");

                var runner = AdminClient.Runners.Register(new RunnerRegister { Token = project.RunnersToken, Description = "test" });
                if (runner.Token == null)
                    throw new InvalidOperationException("Runner token is null");

                TestContext.WriteLine($"Runner registered '{runner.Token}'");

                // Use run-single, so we don't need to manage the configuration file.
                // Also, I don't think there is a need to run multiple jobs in a test
                var buildDir = Path.Combine(Path.GetTempPath(), "GitLabClient", "Runners", "build");
                Directory.CreateDirectory(buildDir);
                var psi = new ProcessStartInfo
                {
                    FileName = path,
                    ArgumentList =
                {
                    "run-single",
                    "--url", DockerContainer.GitLabUrl.ToString(),
                    "--executor", "shell",
                    "--shell", RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "powershell" : "pwsh",
                    "--builds-dir", buildDir,
                    "--wait-timeout", "240", // in seconds
                    "--token", runner.Token,
                },
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                };
                var process = Process.Start(psi);
                if (process == null)
                    throw new InvalidOperationException("Cannot start the runner");

                if (process.HasExited)
                    throw new InvalidOperationException("The runner has exited");

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.ErrorDataReceived += (sender, e) => Console.Error.WriteLine(e.Data);
                process.OutputDataReceived += (sender, e) => Console.Error.WriteLine(e.Data);

                TestContext.WriteLine($"Runner started for project '{project.Id}' on '{DockerContainer.GitLabUrl}'");
                return new ProcessKill(process);
            }
            finally
            {
                s_prepareRunnerLock.Release();
            }
        }

        public static async Task<T> RetryUntilAsync<T>(Func<T> action, Func<T, bool> predicate, TimeSpan timeSpan)
        {
            using var cts = new CancellationTokenSource(timeSpan);
            return await RetryUntilAsync(action, predicate, cts.Token).ConfigureAwait(false);
        }

        public static async Task<T> RetryUntilAsync<T>(Func<T> action, Func<T, bool> predicate, CancellationToken cancellationToken)
        {
            var retryCount = 1;
            var result = action();
            while (!predicate(result))
            {
                cancellationToken.ThrowIfCancellationRequested();
                TestContext.WriteLine($"[{TestContext.CurrentContext.Test.FullName}] RetryUntilAsync {retryCount++}...");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

                result = action();
            }

            return result;
        }

        public void Dispose()
        {
        }

        private sealed class ProcessKill : IDisposable
        {
            private readonly Process _process;

            public ProcessKill(Process process)
            {
                _process = process;
            }

            public void Dispose()
            {
                _process.Kill(entireProcessTree: true);
                _process.WaitForExit();
            }
        }
    }
}
