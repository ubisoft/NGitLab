using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class ProjectHooksClientTests
    {
        [Test]
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
            Assert.That(hooksClient.All.ToArray().Length, Is.EqualTo(1));

            Assert.AreEqual(toCreate.MergeRequestsEvents, created.MergeRequestsEvents);
            Assert.AreEqual(toCreate.PushEvents, created.PushEvents);
            Assert.AreEqual(toCreate.IssuesEvents, created.IssuesEvents);
            Assert.AreEqual(toCreate.JobEvents, created.JobEvents);
            Assert.AreEqual(toCreate.NoteEvents, created.NoteEvents);
            Assert.AreEqual(toCreate.PipelineEvents, created.PipelineEvents);
            Assert.AreEqual(toCreate.TagPushEvents, created.TagPushEvents);
            Assert.AreEqual(toCreate.EnableSslVerification, created.EnableSslVerification);
            Assert.AreEqual(toCreate.Url, created.Url);

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
            Assert.That(hooksClient.All.ToArray().Length, Is.EqualTo(1));

            Assert.AreEqual(toUpdate.MergeRequestsEvents, updated.MergeRequestsEvents);
            Assert.AreEqual(toUpdate.PushEvents, updated.PushEvents);
            Assert.AreEqual(toUpdate.IssuesEvents, updated.IssuesEvents);
            Assert.AreEqual(toUpdate.JobEvents, updated.JobEvents);
            Assert.AreEqual(toUpdate.NoteEvents, updated.NoteEvents);
            Assert.AreEqual(toUpdate.PipelineEvents, updated.PipelineEvents);
            Assert.AreEqual(toUpdate.TagPushEvents, updated.TagPushEvents);
            Assert.AreEqual(toUpdate.EnableSslVerification, updated.EnableSslVerification);
            Assert.AreEqual(toUpdate.Url, updated.Url);

            hooksClient.Delete(updated.Id);
            Assert.That(hooksClient.All.ToArray().Length, Is.EqualTo(0));
        }
    }
}
