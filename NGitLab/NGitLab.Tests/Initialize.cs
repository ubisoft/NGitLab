using System;
using System.Collections.Generic;
using System.Net;
using NGitLab.Models;
using NUnit.Framework;
using Polly;

namespace NGitLab.Tests
{
    [SetUpFixture]
    public class Initialize
    {
        internal const string TestEntityNamePrefix = "Unit_Test_";    // Name prefix for test groups, projects & issues

        public static GitLabClient GitLabClient;
        public static GitLabClient GitLabClientFront1;
        public static GitLabClient GitLabClientFront2;

        public static Project UnitTestProject;

        public static Group UnitTestGroup;

        public static Trigger UnitTestTrigger;

        public static Issue UnitTestIssue;

        public static IRepositoryClient Repository => GitLabClient.GetRepository(UnitTestProject.Id);

        public static string GitLabHost => "https://gitlab.example.com";

        public static string GitLabHookTest => "https://gitlab.example.com";

        public static string GitLabHostFront1 => "https://pdc-dev-gitfront01/";

        public static string GitLabHostFront2 => " https://pdc-dev-gitfront02/";

        public static string GitLabToken => "dummy";

        public static bool IsAdmin => GitLabClient.Users.Current.IsAdmin;

        public static int AdminAccountId = 42;

        public static string ProjectName;

        public static string GroupName;

        public static string IssueTitle;

        /// <summary>
        /// The last request executed against gitlab.
        /// </summary>
        public static WebRequest LastRequest => s_requests[s_requests.Count - 1];

        private static readonly List<WebRequest> s_requests = new List<WebRequest>();

        [OneTimeSetUp]
        public void Setup()
        {
            if (string.IsNullOrEmpty(GitLabHost))
                throw new ArgumentNullException(nameof(GitLabHost));

            if (string.IsNullOrEmpty(GitLabToken))
                throw new ArgumentNullException(nameof(GitLabToken));

            GitLabClient = new GitLabClient(GitLabHost, apiToken: GitLabToken, options: new CustomRequestOptions(s_requests));
            GitLabClientFront1 = new GitLabClient(GitLabHostFront1, apiToken: GitLabToken, options: new CustomRequestOptions(s_requests));
            GitLabClientFront2 = new GitLabClient(GitLabHostFront2, apiToken: GitLabToken, options: new CustomRequestOptions(s_requests));

            var randomGenerator = new Random();

            // Delete project is really slow now, creating a new project name at each run
            // => https://gitlab.com/gitlab-com/support-forum/issues/1569
            ProjectName = TestEntityNamePrefix + randomGenerator.Next();
            GroupName = TestEntityNamePrefix + randomGenerator.Next();
            IssueTitle = TestEntityNamePrefix + randomGenerator.Next();

            // Create a test project with merge request etc.
            UnitTestGroup = CreateGroup(GroupName);
            UnitTestProject = CreateProject(ProjectName, UnitTestGroup.Id);
            UnitTestIssue = CreateIssue(IssueTitle);
            UnitTestTrigger = CreateTrigger(UnitTestProject.Id);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            // Remove the test project again
            DeleteTestProject();

            // remove group
            DeleteTestGroup();
        }

        internal static void WaitWithTimeoutUntil(Func<bool> predicate)
        {
            Policy
                .HandleResult(result: false)
                .WaitAndRetry(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(retryAttempt))
                .Execute(predicate);
        }

        private static Group CreateGroup(string groupName)
        {
            var group = GitLabClient.Groups.Create(new GroupCreate()
            {
                Name = groupName,
                Path = $"{groupName}Path",
                Visibility = VisibilityLevel.Internal,
                ParentId = null,
            });

            return group;
        }

        private static Trigger CreateTrigger(int projectId)
        {
            return GitLabClient.GetTriggers(projectId).Create("Unit_Test_Description");
        }

        private static void DeleteTestGroup()
        {
            var group = GitLabClient.Groups[UnitTestGroup.Id];

            if (group == null)
                Assert.Fail($"Cannot find group {UnitTestGroup.Id}");

            GitLabClient.Groups.Delete(group.Id);
        }

        private static Project CreateProject(string name, int? groupId = null)
        {
            var createdProject = GitLabClient.Projects.Create(new ProjectCreate
            {
                Description = "desc",
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = name,
                NamespaceId = groupId?.ToString(),
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Internal,
                WikiEnabled = true,
            });

            // GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!
            // https://gitlab.com/gitlab-org/gitlab-ce/issues/2420
            // GitLabClient.GetRepository(createdProject.Id).Branches.Create(new BranchCreate
            // {
            //    Name = "merge-me-to-master",
            //    Ref = "master"
            // });
            AddSomeCommits(createdProject.Id);

            GitLabClient.GetRepository(createdProject.Id).ProjectHooks.Create(new ProjectHookUpsert
            {
                MergeRequestsEvents = true,
                PushEvents = true,
                Url = new Uri(GitLabHookTest),
            });

            return createdProject;
        }

        private static void AddSomeCommits(int projectId)
        {
            GitLabClient.GetRepository(projectId).Files.Create(new FileUpsert
            {
                Branch = "master",
                CommitMessage = "add readme",
                Path = "README.md",
                RawContent = "this project should only live during the unit tests, you can delete if you find some",
            });

            for (var i = 0; i < 3; i++)
            {
                GitLabClient.GetRepository(projectId).Files.Create(new FileUpsert
                {
                    Branch = "master",
                    CommitMessage = $"add test file {i}",
                    Path = $"TestFile{i}.txt",
                    RawContent = "this project should only live during the unit tests, you can delete if you find some",
                });
            }
        }

        private static void DeleteTestProject()
        {
            var project = GitLabClient.Projects[UnitTestProject.Id];

            if (project == null)
            {
                Assert.Fail($"Cannot find project {UnitTestProject.Id}");
            }

            GitLabClient.Projects.Delete(project.Id);
        }

        private static Issue CreateIssue(string title)
        {
            var createIssue = GitLabClient.Issues.Create(new IssueCreate()
            {
                Id = UnitTestProject.Id,
                Title = title,
                Description = "desc",
            });

            return createIssue;
        }

        /// <summary>
        /// Stores all the web requests in a list.
        /// </summary>
        private sealed class CustomRequestOptions : RequestOptions
        {
            private readonly List<WebRequest> _allRequests;

            public CustomRequestOptions(List<WebRequest> allRequests)
                : base(retryCount: 3, retryInterval: TimeSpan.FromSeconds(1), isIncremental: true)
            {
                _allRequests = allRequests;
            }

            public override WebResponse GetResponse(HttpWebRequest request)
            {
                _allRequests.Add(request);
                return base.GetResponse(request);
            }
        }
    }
}
