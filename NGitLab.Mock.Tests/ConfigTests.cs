using System.Linq;
using FluentAssertions;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

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
        Assert.That(config, Is.Not.Null);

        Assert.That(config.Users, Has.One.Items);
        Assert.That(config.Users[0].Username, Is.EqualTo("user1"));
        Assert.That(config.Groups, Has.One.Items);
        Assert.That(config.Groups[0].Name, Is.EqualTo("unit-tests"));
        Assert.That(config.Projects, Has.One.Items);
        Assert.That(config.Projects[0].Name, Is.EqualTo("test-project"));
        Assert.That(config.Projects[0].Namespace, Is.EqualTo("unit-tests"));
        Assert.That(config.Projects[0].Description, Is.EqualTo("Test project"));
        Assert.That(config.Projects[0].DefaultBranch, Is.EqualTo("default"));
        Assert.That(config.Projects[0].Visibility, Is.EqualTo(VisibilityLevel.Public));
        Assert.That(config.Projects[0].Labels, Has.One.Items);
        Assert.That(config.Projects[0].Labels[0].Name, Is.EqualTo("label1"));
        Assert.That(config.Projects[0].Issues, Has.One.Items);
        Assert.That(config.Projects[0].Issues[0].Title, Is.EqualTo("Issue #1"));
        Assert.That(config.Projects[0].Issues[0].Description, Is.EqualTo("My issue"));
        Assert.That(config.Projects[0].Issues[0].Author, Is.EqualTo("user1"));
        Assert.That(config.Projects[0].Issues[0].Labels, Has.One.Items);
        Assert.That(config.Projects[0].Issues[0].Labels[0], Is.EqualTo("label1"));
        Assert.That(config.Projects[0].MergeRequests, Has.One.Items);
        Assert.That(config.Projects[0].MergeRequests[0].Title, Is.EqualTo("Merge request #1"));
        Assert.That(config.Projects[0].MergeRequests[0].Description, Is.EqualTo("My merge request"));
        Assert.That(config.Projects[0].MergeRequests[0].Author, Is.EqualTo("user1"));
        Assert.That(config.Projects[0].Permissions, Has.One.Items);
        Assert.That(config.Projects[0].Permissions[0].User, Is.EqualTo("user1"));
        Assert.That(config.Projects[0].Permissions[0].Level, Is.EqualTo(AccessLevel.Owner));
    }

    [Test]
    public void Test_config_can_be_serialized()
    {
        var config = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("project-1", description: "Project #1", visibility: VisibilityLevel.Public,
                configure: project => project
                    .WithCommit("Initial commit")
                    .WithCommit("Create branch", sourceBranch: "branch-01")
                    .WithIssue("Issue #1")
                    .WithMergeRequest("branch-01", title: "Merge request #1")
                    .WithUserPermission("user1", AccessLevel.Maintainer))
            .WithProject("project-2");

        var content = config.Serialize();
        Assert.That(content, Is.Not.Empty);

        var config2 = GitLabConfig.Deserialize(content);
        Assert.That(config2, Is.Not.Null);

        Assert.That(config2.Users, Has.One.Items);
        Assert.That(config2.Users[0].Username, Is.EqualTo("user1"));
        Assert.That(config2.Projects, Has.Exactly(2).Items);
        Assert.That(config2.Projects[0].Name, Is.EqualTo("project-1"));
        Assert.That(config2.Projects[0].Description, Is.EqualTo("Project #1"));
        Assert.That(config2.Projects[0].Visibility, Is.EqualTo(VisibilityLevel.Public));
        Assert.That(config2.Projects[0].Commits, Has.Exactly(2).Items);
        Assert.That(config2.Projects[0].Commits[0].Message, Is.EqualTo("Initial commit"));
        Assert.That(config2.Projects[0].Commits[1].Message, Is.EqualTo("Create branch"));
        Assert.That(config2.Projects[0].Issues, Has.One.Items);
        Assert.That(config2.Projects[0].Issues[0].Title, Is.EqualTo("Issue #1"));
        Assert.That(config2.Projects[0].MergeRequests, Has.One.Items);
        Assert.That(config2.Projects[0].MergeRequests[0].Title, Is.EqualTo("Merge request #1"));
        Assert.That(config2.Projects[0].Permissions, Has.One.Items);
        Assert.That(config2.Projects[0].Permissions[0].User, Is.EqualTo("user1"));
        Assert.That(config2.Projects[1].Name, Is.EqualTo("project-2"));

        using var server = config2.BuildServer();
        Assert.That(server, Is.Not.Null);
    }

    [Test]
    public void Test_job_ids_are_unique()
    {
        var config = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("project-1", description: "Project #1", visibility: VisibilityLevel.Public,
                configure: project => project
                    .WithCommit("Initial commit", alias: "C1")
                    .WithPipeline("C1", p => p.WithJob().WithJob().WithJob()))
            .WithProject("project-2", description: "Project #2", visibility: VisibilityLevel.Public,
                configure: project => project
                    .WithCommit("Initial commit", alias: "C1")
                    .WithPipeline("C1", p => p.WithJob().WithJob()));

        using var server = config.BuildServer();
        Assert.That(server, Is.Not.Null);

        var project1 = server.AllProjects.FirstOrDefault();
        Assert.That(project1, Is.Not.Null);

        project1.Jobs.Should().BeEquivalentTo(new[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } });

        var project2 = server.AllProjects.LastOrDefault();
        Assert.That(project2, Is.Not.Null);

        project2.Jobs.Should().BeEquivalentTo(new[] { new { Id = 4 }, new { Id = 5 } });
    }
}
