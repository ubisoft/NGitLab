using System;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class MergeRequestVersionsMockTests
{
    [Test]
    public void GetVersionsAsync_ReturnsVersionMatchingCurrentShas()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var versions = client.GetVersionsAsync(mr.Iid).ToArray();

            Assert.That(versions, Is.Not.Empty);
            Assert.That(versions[0].HeadCommitSha, Is.EqualTo(mr.HeadSha));
            Assert.That(versions[0].BaseCommitSha, Is.EqualTo(mr.BaseSha));
            Assert.That(versions[0].StartCommitSha, Is.EqualTo(mr.StartSha));
        }
    }

    [Test]
    public void GetVersionsAsync_SourceBranchPush_AddsNewestVersionFirstWithUnchangedBase()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var firstVersion = client.GetVersionsAsync(mr.Iid).Single();

            project.Repository.Commit(user, "second change", "feature", new[] { File.CreateFromText("file2.txt", "more content") });

            var versions = client.GetVersionsAsync(mr.Iid).ToArray();

            Assert.That(versions, Has.Length.EqualTo(2));
            Assert.That(versions[0].HeadCommitSha, Is.EqualTo(mr.HeadSha), "newest version is returned first, matching current HEAD");
            Assert.That(versions[1].HeadCommitSha, Is.EqualTo(firstVersion.HeadCommitSha));
            Assert.That(versions[0].HeadCommitSha, Is.Not.EqualTo(versions[1].HeadCommitSha));

            Assert.That(versions[0].BaseCommitSha, Is.EqualTo(firstVersion.BaseCommitSha), "merge base with the target branch is unchanged since only the source branch moved");
            Assert.That(versions[0].StartCommitSha, Is.EqualTo(firstVersion.StartCommitSha), "target branch tip is unchanged since only the source branch moved");
            Assert.That(versions[0].BaseCommitSha, Is.EqualTo(mr.BaseSha));
            Assert.That(versions[0].StartCommitSha, Is.EqualTo(mr.StartSha));
        }
    }

    [Test]
    public void GetVersionsAsync_TargetBranchPush_AddsNewestVersionFirstWithUnchangedHead()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var firstVersion = client.GetVersionsAsync(mr.Iid).Single();

            project.Repository.Commit(user, "target branch change", project.DefaultBranch, new[] { File.CreateFromText("target-file.txt", "target content") });

            var versions = client.GetVersionsAsync(mr.Iid).ToArray();

            Assert.That(versions, Has.Length.EqualTo(2));
            Assert.That(versions[0].StartCommitSha, Is.EqualTo(mr.StartSha), "newest version reflects the new target branch tip");
            Assert.That(versions[0].StartCommitSha, Is.Not.EqualTo(firstVersion.StartCommitSha));

            Assert.That(versions[0].HeadCommitSha, Is.EqualTo(firstVersion.HeadCommitSha), "source branch tip is unchanged since only the target branch moved");
            Assert.That(versions[0].BaseCommitSha, Is.EqualTo(firstVersion.BaseCommitSha), "merge base is unchanged since the previous target tip remains an ancestor of both branches");
            Assert.That(versions[0].HeadCommitSha, Is.EqualTo(mr.HeadSha));
        }
    }

    [Test]
    public void GetVersionsAsync_Rebase_AddsVersionWithNewHeadAndBaseAdvancedToTargetTip()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var firstVersion = client.GetVersionsAsync(mr.Iid).Single();

            project.Repository.Commit(user, "target branch change", project.DefaultBranch, new[] { File.CreateFromText("target-file.txt", "target content") });
            var versionAfterTargetPush = client.GetVersionsAsync(mr.Iid).First();

            client.Rebase(mr.Iid);

            var versions = client.GetVersionsAsync(mr.Iid).ToArray();

            Assert.That(versions, Has.Length.EqualTo(3));
            var newestVersion = versions[0];

            Assert.That(newestVersion.HeadCommitSha, Is.Not.EqualTo(versionAfterTargetPush.HeadCommitSha), "rebasing rewrites the source branch tip onto the target branch tip");
            Assert.That(newestVersion.HeadCommitSha, Is.EqualTo(mr.HeadSha));

            Assert.That(newestVersion.BaseCommitSha, Is.Not.EqualTo(firstVersion.BaseCommitSha), "unlike a plain push, rebasing advances the merge base to the target tip the source was rebased onto");
            Assert.That(newestVersion.BaseCommitSha, Is.EqualTo(newestVersion.StartCommitSha));
            Assert.That(newestVersion.StartCommitSha, Is.EqualTo(mr.StartSha));
        }
    }

    [Test]
    public void GetVersionsAsync_UnknownMergeRequest_ThrowsNotFound()
    {
        var (server, project, _, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var exception = Assert.Throws<GitLabException>((Action)(() => client.GetVersionsAsync(99999).ToArray()));
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
