using NGitLab.Models;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace NGitLab.Tests
{
    [SetUpFixture]
    public class Initialize
    {
        public static GitLabClient GitLabClient;

        public static Project UnitTestProject;

        public static IRepositoryClient Repository => GitLabClient.GetRepository(UnitTestProject.Id);

        public static string GitLabHost => "https://gitlab.example.com/api/v3";

        public static string GitLabToken => "dummy";

        public static bool IsAdmin => GitLabClient.Users.Current.IsAdmin;

        [OneTimeSetUp]
        public void Setup()
        {
            if (string.IsNullOrEmpty(GitLabHost))
                throw new ArgumentNullException(nameof(GitLabHost));

            if (string.IsNullOrEmpty(GitLabToken))
                throw new ArgumentNullException(nameof(GitLabToken));

            GitLabClient = new GitLabClient(GitLabHost, apiToken: GitLabToken);

            // Create a test project with merge request etc.
            if (DeleteProject("Unit_Test", @try: true))
            {
                Console.WriteLine("Cleanup project from last unit test run. Waiting for the server to process.");
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            UnitTestProject = CreateProject("Unit_Test");
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            // Remove the test project again
            DeleteProject("Unit_Test");
        }

        private Project CreateProject(string name)
        {
            var createdProject = GitLabClient.Projects.Create(new ProjectCreate
            {
                Description = "desc",
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = name,
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Internal,
                WikiEnabled = true
            });

            //GitLab API does not allow to create branches on empty projects. Cant test in Docker at the moment!
            //https://gitlab.com/gitlab-org/gitlab-ce/issues/2420
            //GitLabClient.GetRepository(createdProject.Id).Branches.Create(new BranchCreate
            //{
            //    Name = "merge-me-to-master",
            //    Ref = "master"
            //});

            GitLabClient.GetRepository(createdProject.Id).Files.Create(new FileUpsert
            {
                Branch = "master",
                CommitMessage = "add readme",
                Path = "README.md",
                RawContent = "this project should only live during the unit tests, you can delete if you find some",
            });

            GitLabClient.GetRepository(createdProject.Id).ProjectHooks.Create(new ProjectHookUpsert
            {
                MergeRequestsEvents = true,
                PushEvents = true,
                Url = new Uri("http://unit.test.scooletz.com"),
            });

            return createdProject;
        }

        private bool DeleteProject(string name, bool @try = false)
        {
            var project = GitLabClient.Projects.Owned.FirstOrDefault(x => x.Name == name);

            if (project == null)
            {
                if (@try)
                {
                    return false;
                }
                Assert.Fail($"Cannot find project {name}");
            }

            GitLabClient.Projects.Delete(project.Id);
            return true;
        }
    }
}
