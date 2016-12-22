using NGitLab.Models;
using NUnit.Framework;
using System;
using System.Linq;

namespace NGitLab.Tests
{
    [SetUpFixture]
    public class Initialize
    {
        public static GitLabClient GitLabClient;

        public static Project UnitTestProject;

        [OneTimeSetUp]
        public void Setup()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITLAB_TOKEN")))
            {
                SetEnvironmentVariable(gitlabToken: "");
            }

            // Login using User name & Password
            var host = Environment.GetEnvironmentVariable("GITLAB_HOST");
            var token = Environment.GetEnvironmentVariable("GITLAB_TOKEN");

            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            GitLabClient = new GitLabClient(host, apiToken: token);

            // Create a test project with merge request etc.
            DeleteProject("Unit_Test", @try: true);
            UnitTestProject = CreateProject("Unit_Test");
        }

        public void SetEnvironmentVariable(string gitlabToken)
        {
            Assert.IsNotEmpty(gitlabToken, "TODO enter a gitlab token once to enable the unit tests");

            Environment.SetEnvironmentVariable("GITLAB_HOST", "https://gitlab.example.com/api/v3", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("GITLAB_HOST", "https://gitlab.example.com/api/v3", EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("GITLAB_TOKEN", gitlabToken, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("GITLAB_TOKEN", gitlabToken, EnvironmentVariableTarget.User);
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
                ImportUrl = null,
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = name,
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Public,
                WallEnabled = true,
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

        private void DeleteProject(string name, bool @try = false)
        {
            var project = GitLabClient.Projects.Owned.FirstOrDefault(x => x.Name == name);

            if (project == null)
            {
                if (@try)
                {
                    return;
                }
                Assert.Fail($"Cannot find project {name}");
            }

            GitLabClient.Projects.Delete(project.Id);
        }
    }
}
