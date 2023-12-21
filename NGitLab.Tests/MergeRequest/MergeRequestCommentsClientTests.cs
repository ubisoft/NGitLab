using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MergeRequestCommentsClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task AddCommentToMergeRequest_DeprecatedApi()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestComments = mergeRequestClient.Comments(mergeRequest.Iid);

        const string commentMessage = "Comment for MR";
        var newComment = new MergeRequestCommentCreate
        {
            Body = commentMessage,
        };
        var comment = mergeRequestComments.Add(newComment);
        Assert.That(comment.Body, Is.EqualTo(commentMessage));
    }

    [Test]
    [NGitLabRetry]
    public async Task AddEditCommentToMergeRequest()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestComments = mergeRequestClient.Comments(mergeRequest.Iid);

        // add note
        const string commentMessage = "Comment for MR";
        var createdAt = new DateTime(2019, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        var newComment = new MergeRequestCommentCreate
        {
            Body = commentMessage,
            CreatedAt = createdAt,
        };
        var comment = mergeRequestComments.Add(newComment);
        Assert.That(comment.Body, Is.EqualTo(commentMessage));
        Assert.That(comment.CreatedAt, Is.EqualTo(createdAt));

        // edit note
        const string commentMessageEdit = "Comment for MR edit";
        var editedComment = mergeRequestComments.Edit(comment.Id, new MergeRequestCommentEdit { Body = commentMessageEdit });
        Assert.That(editedComment.Id, Is.EqualTo(comment.Id));
        Assert.That(editedComment.Body, Is.EqualTo(commentMessageEdit));
        Assert.That(editedComment.CreatedAt, Is.EqualTo(createdAt));

        // Get all
        var comments = mergeRequestComments.All.ToArray();
        Assert.That(comments, Is.Not.Empty);

        // Delete
        mergeRequestComments.Delete(comment.Id);
        comments = mergeRequestComments.All.ToArray();
        Assert.That(comments, Is.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task AddCommentToMergeRequestOnArchivedProject()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestComments = mergeRequestClient.Comments(mergeRequest.Iid);

        const string commentMessage = "Comment for MR";
        var newComment = new MergeRequestCommentCreate
        {
            Body = commentMessage,
        };

        context.Client.Projects.Archive(project.Id);
        var ex = Assert.Throws<GitLabException>(() => mergeRequestComments.Add(newComment));
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}
