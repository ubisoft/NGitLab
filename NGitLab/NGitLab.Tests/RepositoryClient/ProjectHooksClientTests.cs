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
            var project = Initialize.GitLabClient.Projects.Owned.First();
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
                Url = new Uri("http://scooletz.com"),
            };

            var created = _hooks.Create(toCreate);
            ThereAreTwoHooks();

            Assert.AreEqual(toCreate.MergeRequestsEvents, created.MergeRequestsEvents);
            Assert.AreEqual(toCreate.PushEvents, created.PushEvents);
            Assert.AreEqual(toCreate.Url, created.Url);

            var toUpdate = new ProjectHookUpsert
            {
                MergeRequestsEvents = false,
                PushEvents = false,
                Url = new Uri("http://git.scooletz.com"),
            };

            var updated = _hooks.Update(created.Id, toUpdate);

            ThereAreTwoHooks();

            Assert.AreEqual(toUpdate.MergeRequestsEvents, updated.MergeRequestsEvents);
            Assert.AreEqual(toUpdate.PushEvents, updated.PushEvents);
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