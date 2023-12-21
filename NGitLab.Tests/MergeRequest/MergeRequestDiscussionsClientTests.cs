using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MergeRequestDiscussionsClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task AddDiscussionToMergeRequest_DiscussionCreated()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestDiscussions = mergeRequestClient.Discussions(mergeRequest.Iid);

        const string discussionMessage = "Discussion for MR";
        var newDiscussion = new MergeRequestDiscussionCreate
        {
            Body = discussionMessage,
        };
        var discussion = mergeRequestDiscussions.Add(newDiscussion);
        Assert.That(discussion.IndividualNote, Is.False);
        Assert.That(discussion.Notes[0].Body, Is.EqualTo(discussionMessage));
        Assert.That(discussion.Notes[0].Resolvable, Is.True);
        Assert.That(discussion.Notes[0].Resolved, Is.False);

        var discussions = mergeRequestDiscussions.All.ToArray();
        Assert.That(discussions, Is.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetDiscussion_DiscussionFound()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestDiscussions = mergeRequestClient.Discussions(mergeRequest.Iid);

        const string discussionMessage = "Discussion for MR";
        var newDiscussion = new MergeRequestDiscussionCreate
        {
            Body = discussionMessage,
        };
        var discussion = mergeRequestDiscussions.Add(newDiscussion);

        var gotDiscussion = await mergeRequestDiscussions.GetAsync(discussion.Id);
        Assert.That(gotDiscussion, Is.Not.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task EditCommentFromDiscussion_CommentEdited()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestDiscussions = mergeRequestClient.Discussions(mergeRequest.Iid);

        // Create a discussion, it creates automatically one note
        const string discussionMessage = "Discussion for MR";
        var createdAt = new DateTime(2019, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var newDiscussion = new MergeRequestDiscussionCreate
        {
            Body = discussionMessage,
            CreatedAt = createdAt,
        };
        var discussion = mergeRequestDiscussions.Add(newDiscussion);
        Assert.That(discussion.Notes[0].Body, Is.EqualTo(discussionMessage));
        Assert.That(discussion.Notes[0].CreatedAt, Is.EqualTo(createdAt));

        // Edit the note associated with the discussion
        const string discussionMessageEdit = "Discussion for MR edit";
        var mergeRequestComments = mergeRequestClient.Comments(mergeRequest.Iid);

        var editedComment = mergeRequestComments.Edit(discussion.Notes[0].Id, new MergeRequestCommentEdit { Body = discussionMessageEdit });
        Assert.That(editedComment.Id, Is.EqualTo(discussion.Notes[0].Id));
        Assert.That(editedComment.Body, Is.EqualTo(discussionMessageEdit));
        Assert.That(editedComment.CreatedAt, Is.EqualTo(createdAt));
        Assert.That(editedComment.Resolved, Is.False);
    }

    [Test]
    [NGitLabRetry]
    public async Task AddDiscussionToMergeRequestOnArchivedProject()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestDiscussions = mergeRequestClient.Discussions(mergeRequest.Iid);

        const string discussionMessage = "Discussion for MR";
        var newDiscussion = new MergeRequestDiscussionCreate
        {
            Body = discussionMessage,
        };

        var projectClient = context.Client.Projects;
        projectClient.Archive(project.Id);
        var ex = Assert.Throws<GitLabException>(() => mergeRequestDiscussions.Add(newDiscussion));
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [NGitLabRetry]
    public async Task ResolveDiscussion_AllNotesResolved()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestDiscussions = mergeRequestClient.Discussions(mergeRequest.Iid);

        const string discussionMessage = "Discussion for MR";
        var newDiscussion = new MergeRequestDiscussionCreate
        {
            Body = discussionMessage,
        };
        var discussion = mergeRequestDiscussions.Add(newDiscussion);

        var resolve = new MergeRequestDiscussionResolve
        {
            Id = discussion.Id,
            Resolved = true,
        };
        var resolvedDiscussion = mergeRequestDiscussions.Resolve(resolve);

        Assert.That(resolvedDiscussion.Notes[0].Resolved, Is.True);
    }

    [Test]
    [NGitLabRetry]
    public async Task DeleteOneNoteFromDiscussion_DiscussionAndNoteDeleted()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestDiscussions = mergeRequestClient.Discussions(mergeRequest.Iid);

        const string discussionMessage = "Discussion for MR";
        var newDiscussion = new MergeRequestDiscussionCreate
        {
            Body = discussionMessage,
        };
        var discussion = mergeRequestDiscussions.Add(newDiscussion);
        mergeRequestDiscussions.Delete(discussion.Id, discussion.Notes[0].Id);

        var discussions = mergeRequestDiscussions.All.ToArray();
        Assert.That(discussions.All(x => !string.Equals(x.Id, discussion.Id, StringComparison.Ordinal)), Is.True);
    }
}
