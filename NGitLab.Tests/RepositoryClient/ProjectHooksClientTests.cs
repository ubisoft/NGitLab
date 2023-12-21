using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient;

public class ProjectHooksClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task CreateUpdateDelete()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var hooksClient = context.Client.GetRepository(project.Id).ProjectHooks;

        var toCreate = new ProjectHookUpsert
        {
            MergeRequestsEvents = true,
            PushEvents = true,
            IssuesEvents = true,
            JobEvents = true,
            NoteEvents = true,
            PipelineEvents = true,
            TagPushEvents = true,
            Token = "sample",
            EnableSslVerification = true,
            Url = new Uri("https://www.example.com"),
        };

        var created = hooksClient.Create(toCreate);
        Assert.That(hooksClient.All.ToArray(), Has.Length.EqualTo(1));

        Assert.That(created.MergeRequestsEvents, Is.EqualTo(toCreate.MergeRequestsEvents));
        Assert.That(created.PushEvents, Is.EqualTo(toCreate.PushEvents));
        Assert.That(created.IssuesEvents, Is.EqualTo(toCreate.IssuesEvents));
        Assert.That(created.JobEvents, Is.EqualTo(toCreate.JobEvents));
        Assert.That(created.NoteEvents, Is.EqualTo(toCreate.NoteEvents));
        Assert.That(created.PipelineEvents, Is.EqualTo(toCreate.PipelineEvents));
        Assert.That(created.TagPushEvents, Is.EqualTo(toCreate.TagPushEvents));
        Assert.That(created.EnableSslVerification, Is.EqualTo(toCreate.EnableSslVerification));
        Assert.That(created.Url, Is.EqualTo(toCreate.Url));

        var toUpdate = new ProjectHookUpsert
        {
            MergeRequestsEvents = false,
            PushEvents = false,
            IssuesEvents = false,
            JobEvents = false,
            NoteEvents = false,
            PipelineEvents = false,
            TagPushEvents = false,
            Token = "sampleEdited",
            EnableSslVerification = false,
            Url = new Uri("https://www.example.com"),
        };

        var updated = hooksClient.Update(created.Id, toUpdate);
        Assert.That(hooksClient.All.ToArray(), Has.Length.EqualTo(1));

        Assert.That(updated.MergeRequestsEvents, Is.EqualTo(toUpdate.MergeRequestsEvents));
        Assert.That(updated.PushEvents, Is.EqualTo(toUpdate.PushEvents));
        Assert.That(updated.IssuesEvents, Is.EqualTo(toUpdate.IssuesEvents));
        Assert.That(updated.JobEvents, Is.EqualTo(toUpdate.JobEvents));
        Assert.That(updated.NoteEvents, Is.EqualTo(toUpdate.NoteEvents));
        Assert.That(updated.PipelineEvents, Is.EqualTo(toUpdate.PipelineEvents));
        Assert.That(updated.TagPushEvents, Is.EqualTo(toUpdate.TagPushEvents));
        Assert.That(updated.EnableSslVerification, Is.EqualTo(toUpdate.EnableSslVerification));
        Assert.That(updated.Url, Is.EqualTo(toUpdate.Url));

        hooksClient.Delete(updated.Id);
        Assert.That(hooksClient.All.ToArray(), Is.Empty);
    }
}
