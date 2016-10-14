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

        [OneTimeSetUp]
        public void Setup()
        {
            // Login using User name & Password
            var host = Environment.GetEnvironmentVariable("GITLAB_HOST");
            var username = Environment.GetEnvironmentVariable("GITLAB_USERNAME");
            var password = Environment.GetEnvironmentVariable("GITLAB_PASSWORD");

            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));

            if(string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            GitLabClient = GitLabClient.Connect(host, username, password);

            // Create a test project with merge request etc.
            CreateProject("Unit_Test");
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            // Remove the test project again
            DeleteProject("Unit_Test");
        }

        private void CreateProject(string name)
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

            GitLabClient.GetRepository(createdProject.Id).ProjectHooks.Create(new ProjectHookUpsert
            {
                MergeRequestsEvents = true,
                PushEvents = true,
                Url = new Uri("http://unit.test.scooletz.com"),
            });
        }

        private void DeleteProject(string name)
        {
            var project = GitLabClient.Projects.Owned.Single(x => x.Name == name);
            GitLabClient.Projects.Delete(project.Id);
        }
    }
}
