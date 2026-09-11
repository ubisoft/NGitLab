using NGitLab.Models;

namespace NGitLab.Mock.Tests;

internal static class MergeRequestMockTestHelper
{
    public static Position CreateTextPosition(MergeRequestVersion version)
    {
        return new Position
        {
            NewPath = "file.txt",
            NewLine = 1,
            PositionType = new DynamicEnum<PositionType>(PositionType.Text),
            BaseSha = new Sha1(version.BaseCommitSha),
            StartSha = new Sha1(version.StartCommitSha),
            HeadSha = new Sha1(version.HeadCommitSha),
        };
    }

    public static (GitLabServer Server, Project Project, MergeRequest MergeRequest, User User) CreateProjectWithMergeRequest()
    {
        var server = new GitLabServer();
        var user = server.Users.AddNew("maintainer");
        var group = new Group("TestGroup");
        server.Groups.Add(group);
        var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
        group.Projects.Add(project);
        project.Permissions.Add(new Permission(user, AccessLevel.Maintainer));

        project.Repository.Commit(user, "Initial commit");
        project.Repository.CreateAndCheckoutBranch("feature");
        project.Repository.Commit(user, "add file", new[] { File.CreateFromText("file.txt", "new content") });

        var mr = project.CreateMergeRequest(user, "A title", "A description", project.DefaultBranch, "feature");

        return (server, project, mr, user);
    }
}
