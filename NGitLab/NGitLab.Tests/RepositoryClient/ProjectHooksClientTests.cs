using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class ProjectHooksClientTests {
        readonly IProjectClient projects;

        public ProjectHooksClientTests() {
            projects = Config.Connect().Projects;
        }

        [Test]
        [Category("Server_Required")]
        public void CreateUpdateDelete() {
            Project proj = null;
            try {
                CreateProject(out proj, "Test Create Hook");

                var hooks = Config.Connect().GetRepository(proj.Id).ProjectHooks;

                var toCreate = new ProjectHookInsert {
                    MergeRequestsEvents = true,
                    PushEvents = true,
                    Url = new Uri("http://scooletz.com"),
                    Id = proj.Id
                };

                var createdHook = hooks.Create(toCreate);
                hooks.All.Count().ShouldBe(1);

                createdHook.MergeRequestsEvents.ShouldBe(toCreate.MergeRequestsEvents);
                createdHook.PushEvents.ShouldBe(toCreate.PushEvents);
                createdHook.Url.ShouldBe(toCreate.Url);

                var toUpdate = new ProjectHookUpdate {
                    MergeRequestsEvents = true,
                    PushEvents = true,
                    TagPushEvents = true,
                    Url = new Uri("http://scooletz.com"),
                    Id = proj.Id,
                    HookId = createdHook.Id
                };

                var updated = hooks.Update(toUpdate);

                hooks.All.Count().ShouldBe(1);

                Assert.AreEqual(toUpdate.MergeRequestsEvents, updated.MergeRequestsEvents);
                Assert.AreEqual(toUpdate.PushEvents, updated.PushEvents);
                Assert.AreEqual(toUpdate.Url, updated.Url);

                hooks.Delete(updated.Id);

                hooks.All.ShouldBeEmpty();
            }
            finally {
                if (proj != null)
                    projects.Delete(proj.Id);
            }
        }

        ProjectCreate CreateProject(out Project created, string name) {
            var p = new ProjectCreate {
                Description = "desc",
                ImportUrl = null,
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = name,
                NamespaceId = 0,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Public,
                WallEnabled = true,
                WikiEnabled = true
            };

            created = projects.Create(p);

            return p;
        }
    }
}