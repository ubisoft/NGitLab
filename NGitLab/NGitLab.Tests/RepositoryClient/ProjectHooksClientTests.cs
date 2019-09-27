using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class ProjectHooksClientTests
    {
        private IProjectHooksClient _hooks;
        private IRepositoryClient _repositoryClient;

        [SetUp]
        public void Setup()
        {
            var project = Initialize.UnitTestProject;
            _repositoryClient = Initialize.GitLabClient.GetRepository(project.Id);
            _hooks = _repositoryClient.ProjectHooks;
        }

        [Test]
        public void GetAll()
        {
            CollectionAssert.IsNotEmpty(_hooks.All.ToArray());
        }

        [Test]
        public void CreateUpdateDelete()
        {
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
                Url = new Uri(Initialize.GitLabHookTest),
            };

            var created = _hooks.Create(toCreate);
            ThereAreTwoHooks();

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
                Url = new Uri(Initialize.GitLabHookTest),
            };

            var updated = _hooks.Update(created.Id, toUpdate);

            ThereAreTwoHooks();

            Assert.AreEqual(toUpdate.MergeRequestsEvents, updated.MergeRequestsEvents);
            Assert.AreEqual(toUpdate.PushEvents, updated.PushEvents);
            Assert.AreEqual(toUpdate.IssuesEvents, updated.IssuesEvents);
            Assert.AreEqual(toUpdate.JobEvents, updated.JobEvents);
            Assert.AreEqual(toUpdate.NoteEvents, updated.NoteEvents);
            Assert.AreEqual(toUpdate.PipelineEvents, updated.PipelineEvents);
            Assert.AreEqual(toUpdate.TagPushEvents, updated.TagPushEvents);
            Assert.AreEqual(toUpdate.EnableSslVerification, updated.EnableSslVerification);
            Assert.AreEqual(toUpdate.Url, updated.Url);

            _hooks.Delete(updated.Id);

            ThereIsOneHook();
        }

        private void ThereAreTwoHooks()
        {
            Assert.That(_hooks.All.ToArray().Length, Is.EqualTo(2));
        }

        private void ThereIsOneHook()
        {
            Assert.That(_hooks.All.ToArray().Length, Is.EqualTo(1));
        }
    }
}