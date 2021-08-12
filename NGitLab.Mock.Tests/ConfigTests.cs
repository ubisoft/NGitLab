using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class ConfigTests
    {
        [Test]
        public void Test_server_can_be_saved_in_config()
        {
            using var server = new GitLabServer();
            var user = new User("user1");
            server.Users.Add(user);
            var group = new Group("unit-tests");
            server.Groups.Add(group);
            var project = new Project("test-project")
            {
                Description = "Test project",
                DefaultBranch = "default",
                Visibility = VisibilityLevel.Public,
            };
            group.Projects.Add(project);
            project.Labels.Add("label1");
            project.Issues.Add(new Issue
            {
                Title = "Issue #1",
                Description = "My issue",
                Author = new UserRef(user),
                Labels = new[] { "label1" },
            });
            project.MergeRequests.Add(new MergeRequest
            {
                Title = "Merge request #1",
                Description = "My merge request",
                Author = new UserRef(user),
            });
            project.Permissions.Add(new Permission(user, AccessLevel.Owner));

            var config = server.ToConfig();
            Assert.IsNotNull(config);

            Assert.That(config.Users, Has.One.Items);
            Assert.AreEqual("user1", config.Users[0].Username);
            Assert.That(config.Groups, Has.One.Items);
            Assert.AreEqual("unit-tests", config.Groups[0].Name);
            Assert.That(config.Projects, Has.One.Items);
            Assert.AreEqual("test-project", config.Projects[0].Name);
            Assert.AreEqual("unit-tests", config.Projects[0].Namespace);
            Assert.AreEqual("Test project", config.Projects[0].Description);
            Assert.AreEqual("default", config.Projects[0].DefaultBranch);
            Assert.AreEqual(VisibilityLevel.Public, config.Projects[0].Visibility);
            Assert.That(config.Projects[0].Labels, Has.One.Items);
            Assert.AreEqual("label1", config.Projects[0].Labels[0].Name);
            Assert.That(config.Projects[0].Issues, Has.One.Items);
            Assert.AreEqual("Issue #1", config.Projects[0].Issues[0].Title);
            Assert.AreEqual("My issue", config.Projects[0].Issues[0].Description);
            Assert.AreEqual("user1", config.Projects[0].Issues[0].Author);
            Assert.That(config.Projects[0].Issues[0].Labels, Has.One.Items);
            Assert.AreEqual("label1", config.Projects[0].Issues[0].Labels[0]);
            Assert.That(config.Projects[0].MergeRequests, Has.One.Items);
            Assert.AreEqual("Merge request #1", config.Projects[0].MergeRequests[0].Title);
            Assert.AreEqual("My merge request", config.Projects[0].MergeRequests[0].Description);
            Assert.AreEqual("user1", config.Projects[0].MergeRequests[0].Author);
            Assert.That(config.Projects[0].Permissions, Has.One.Items);
            Assert.AreEqual("user1", config.Projects[0].Permissions[0].User);
            Assert.AreEqual(AccessLevel.Owner, config.Projects[0].Permissions[0].Level);
        }

        [Test]
        public void Test_config_can_be_serialized()
        {
            var config = new GitLabConfig()
                .WithUser("user1", asDefault: true)
                .WithProject("project-1", description: "Project #1", visibility: VisibilityLevel.Public,
                    configure: project => project
                        .WithCommit("Initial commit")
                        .WithCommit("Create branch", sourceBranch: "branch-01")
                        .WithIssue("Issue #1")
                        .WithMergeRequest("Merge request #1", "branch-01")
                        .WithUserPermission("user1"))
                .WithProject("project-2");

            var content = config.Serialize();
            Assert.IsNotEmpty(content);

            var config2 = GitLabConfig.Deserialize(content);
            Assert.IsNotNull(config2);

            Assert.That(config2.Users, Has.One.Items);
            Assert.AreEqual("user1", config2.Users[0].Username);
            Assert.That(config2.Projects, Has.Exactly(2).Items);
            Assert.AreEqual("project-1", config2.Projects[0].Name);
            Assert.AreEqual("Project #1", config2.Projects[0].Description);
            Assert.AreEqual(VisibilityLevel.Public, config2.Projects[0].Visibility);
            Assert.That(config2.Projects[0].Commits, Has.Exactly(2).Items);
            Assert.AreEqual("Initial commit", config2.Projects[0].Commits[0].Message);
            Assert.AreEqual("Create branch", config2.Projects[0].Commits[1].Message);
            Assert.That(config2.Projects[0].Issues, Has.One.Items);
            Assert.AreEqual("Issue #1", config2.Projects[0].Issues[0].Title);
            Assert.That(config2.Projects[0].MergeRequests, Has.One.Items);
            Assert.AreEqual("Merge request #1", config2.Projects[0].MergeRequests[0].Title);
            Assert.That(config2.Projects[0].Permissions, Has.One.Items);
            Assert.AreEqual("user1", config2.Projects[0].Permissions[0].User);
            Assert.AreEqual("project-2", config2.Projects[1].Name);

            using var server = config2.BuildServer();
            Assert.IsNotNull(server);
        }
    }
}
