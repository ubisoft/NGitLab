using System;
using System.Linq;
using System.Net;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class MergeRequestDiscussionsMockTests
{
    [Test]
    public void AddDiscussion_GeneralComment_IsRetrievableThroughBothCommentsAndDiscussionsClients()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var discussion = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate
            {
                Body = "General comment",
            });

            Assert.That(discussion.Notes[0].Body, Is.EqualTo("General comment"));
            Assert.That(discussion.Notes[0].Position, Is.Null);
            Assert.That(discussion.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(discussion.IndividualNote, Is.False, "a discussion created via the discussions endpoint is not an individual note, even with a single comment");

            var comments = client.Comments(mr.Iid).All.ToArray();
            Assert.That(comments.Any(c => string.Equals(c.Body, "General comment", StringComparison.Ordinal)), Is.True);

            var rereadFromCommentsClient = client.Comments(mr.Iid).Discussions.Single(d => string.Equals(d.Notes[0].Body, "General comment", StringComparison.Ordinal));
            Assert.That(rereadFromCommentsClient.Id, Is.EqualTo(discussion.Id));
            Assert.That(rereadFromCommentsClient.IndividualNote, Is.False);

            var rereadById = client.Discussions(mr.Iid).Get(discussion.Id);
            Assert.That(rereadById.Notes[0].Body, Is.EqualTo("General comment"));
        }
    }

    [Test]
    public void AddDiscussion_InlineComment_RoundtripsPositionAndHeadSha()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);
            var version = client.GetVersionsAsync(mr.Iid).First();
            var position = MergeRequestMockTestHelper.CreateTextPosition(version);

            var discussion = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate
            {
                Body = "Inline comment",
                Position = position,
            });

            Assert.That(discussion.Notes[0].Position, Is.Not.Null);
            Assert.That(discussion.Notes[0].Position.NewPath, Is.EqualTo("file.txt"));
            Assert.That(discussion.Notes[0].Position.NewLine, Is.EqualTo(1));
            Assert.That(discussion.Notes[0].Position.HeadSha.ToString(), Is.EqualTo(new Sha1(version.HeadCommitSha).ToString()));
            Assert.That(discussion.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(discussion.IndividualNote, Is.False);

            var discussions = client.Comments(mr.Iid).Discussions.ToArray();
            var reread = discussions.Single(d => string.Equals(d.Notes[0].Body, "Inline comment", StringComparison.Ordinal));
            Assert.That(reread.Notes[0].Position, Is.Not.Null);
            Assert.That(reread.Notes[0].Position.NewPath, Is.EqualTo("file.txt"));
            Assert.That(reread.Notes[0].Position.NewLine, Is.EqualTo(1));
            Assert.That(reread.Id, Is.EqualTo(discussion.Id));
            Assert.That(reread.IndividualNote, Is.False);
        }
    }

    [Test]
    public void DeleteDiscussion_RemovesBothNoteAndDiscussion()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var discussion = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate { Body = "To be deleted" });
            client.Discussions(mr.Iid).Delete(discussion.Id, discussion.Notes[0].Id);

            Assert.That(client.Comments(mr.Iid).All.Any(c => c.Id == discussion.Notes[0].Id), Is.False);
            Assert.That(client.Discussions(mr.Iid).All.Any(d => string.Equals(d.Id, discussion.Id, StringComparison.Ordinal)), Is.False);
        }
    }

    [Test]
    public void Resolve_MarksNoteResolvedInResponse_ButRereadStillReportsUnresolved()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var discussion = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate { Body = "Resolvable comment" });
            Assert.That(discussion.Notes[0].Resolved, Is.False);

            var resolved = client.Discussions(mr.Iid).Resolve(new MergeRequestDiscussionResolve { Id = discussion.Id, Resolved = true });
            Assert.That(resolved.Notes[0].Resolved, Is.True, "Resolve() returns notes marked as resolved");

            var reread = client.Discussions(mr.Iid).Get(discussion.Id);
            Assert.That(reread.Notes[0].Resolved, Is.False, "known limitation: Resolve() mutates the returned projection but does not persist resolution onto the stored comment");
        }
    }

    [Test]
    public void AddDiscussion_CalledTwice_CreatesSeparateSingleNoteThreads()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var first = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate { Body = "First reply" });
            var second = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate { Body = "Second reply" });

            Assert.That(first.Id, Is.Not.EqualTo(second.Id), "known limitation: every Add() mints a brand-new thread id, so there is no public way to append a note to an existing thread");

            var discussions = client.Comments(mr.Iid).Discussions.ToArray();
            Assert.That(discussions, Has.Length.EqualTo(2));
            Assert.That(discussions.All(d => d.Notes.Length == 1), Is.True);
        }
    }

    [Test]
    public void Discussions_UnknownId_ThrowNotFound()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Discussions(mr.Iid).Get("unknown-id"))).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Discussions(mr.Iid).Delete("unknown-id", 1))).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Discussions(mr.Iid).Resolve(new MergeRequestDiscussionResolve { Id = "unknown-id", Resolved = true }))).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [Test]
    public void ArchivedProject_AddDiscussion_ThrowsForbidden()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            project.Archived = true;
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate { Body = "Should fail" }))).StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
