using System;
using System.Linq;
using System.Net;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class MergeRequestCommentsMockTests
{
    [Test]
    public void AddComment_CreatesIndividualNoteWithSyntheticDiscussionId()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            client.Comments(mr.Iid).Add(new MergeRequestCommentCreate { Body = "Plain note" });

            var discussion = client.Comments(mr.Iid).Discussions.Single(d => string.Equals(d.Notes[0].Body, "Plain note", StringComparison.Ordinal));

            Assert.That(discussion.IndividualNote, Is.True, "a comment added outside the discussions endpoint has no thread id and is reported as an individual note");
            Assert.That(discussion.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(discussion.Notes, Has.Length.EqualTo(1));
        }
    }

    [Test]
    public void EditComment_UpdatesBodyAndPersists()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var comment = client.Comments(mr.Iid).Add(new MergeRequestCommentCreate { Body = "Original body" });
            var edited = client.Comments(mr.Iid).Edit(comment.Id, new MergeRequestCommentEdit { Body = "Edited body" });

            Assert.That(edited.Body, Is.EqualTo("Edited body"));

            var reread = client.Comments(mr.Iid).All.Single(c => c.Id == comment.Id);
            Assert.That(reread.Body, Is.EqualTo("Edited body"));
        }
    }

    [Test]
    public void DeleteComment_RemovesCommentFromMergeRequest()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var comment = client.Comments(mr.Iid).Add(new MergeRequestCommentCreate { Body = "To be deleted" });
            client.Comments(mr.Iid).Delete(comment.Id);

            Assert.That(client.Comments(mr.Iid).All.Any(c => c.Id == comment.Id), Is.False);
        }
    }

    [Test]
    public void GetParticipants_IncludesAuthorAndCommentAuthors()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var commenter = server.Users.AddNew("commenter");

            server.CreateClient(commenter).GetMergeRequest(project.Id).Comments(mr.Iid).Add(new MergeRequestCommentCreate { Body = "A comment" });

            var participants = server.CreateClient(user).GetMergeRequest(project.Id).GetParticipants(mr.Iid).ToArray();

            Assert.That(participants.Select(p => p.Username), Has.Member(user.UserName));
            Assert.That(participants.Select(p => p.Username), Has.Member(commenter.UserName));
        }
    }

    [Test]
    public void Reply_AppendsNoteToExistingDiscussionThread()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            var discussion = client.Discussions(mr.Iid).Add(new MergeRequestDiscussionCreate { Body = "Original" });
            client.Comments(mr.Iid).Add(discussion.Id, new MergeRequestCommentCreate { Body = "A reply" });

            var reread = client.Discussions(mr.Iid).Get(discussion.Id);
            Assert.That(reread.IndividualNote, Is.False);
            Assert.That(reread.Notes.Select(n => n.Body), Is.EqualTo(new[] { "Original", "A reply" }));

            var discussions = client.Comments(mr.Iid).Discussions.ToArray();
            Assert.That(discussions, Has.Length.EqualTo(1), "the reply should join the existing thread instead of creating a new one");
            Assert.That(discussions[0].Notes, Has.Length.EqualTo(2));
        }
    }

    [Test]
    public void Reply_UnknownDiscussionId_ThrowsNotFound()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Comments(mr.Iid).Add("unknown-id", new MergeRequestCommentCreate { Body = "x" }))).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [Test]
    public void Comments_UnknownId_ThrowNotFound()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Comments(mr.Iid).Edit(99999, new MergeRequestCommentEdit { Body = "x" }))).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Comments(mr.Iid).Delete(99999))).StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }

    [Test]
    public void ArchivedProject_AddComment_ThrowsForbidden()
    {
        var (server, project, mr, user) = MergeRequestMockTestHelper.CreateProjectWithMergeRequest();
        using (server)
        {
            project.Archived = true;
            var client = server.CreateClient(user).GetMergeRequest(project.Id);

            Assert.That(Assert.Throws<GitLabException>((Action)(() => client.Comments(mr.Iid).Add(new MergeRequestCommentCreate { Body = "Should fail" }))).StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
