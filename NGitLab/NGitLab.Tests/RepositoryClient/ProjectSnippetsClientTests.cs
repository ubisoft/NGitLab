using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class ProjectSnippetsClientTests
    {
        readonly IProjectClient projects;

        public ProjectSnippetsClientTests() {
            projects = Config.Connect().Projects;
        }

        [Test]
        [Category("Server_Required")]
        public void CreateUpdateDelete() {
            Project proj = null;
            try {
                CreateProject(out proj, "Test Create Snippets");

                var snipets = Config.Connect().GetRepository(proj.Id).ProjectSnippets;

                var toCreate = new  ProjectSnippetInsert {
                    Id=proj.Id,
                     Visibility= "private",
                      Code="Console.Write();",
                       Description= "Test Create Snippets",
                        FileName= "TestCreateSnippets.cs",
                         Title= "Test Create Snippets"
                };

                var snipetHook = snipets.Create(toCreate);
                snipets.All.Count().ShouldBe(1);

                snipetHook.Title.ShouldBe(toCreate.Title);
                snipetHook.FileName.ShouldBe(toCreate.FileName);
                snipetHook.Description.ShouldBe(toCreate.Description);

                var toUpdate = new ProjectSnippetUpdate {
                    Id = proj.Id,
                    Visibility = "private",
                    Code = "Console.Write();",
                    Description = "Test Create Snippets update",
                    FileName = "TestCreateSnippets.cs update",
                    Title = "Test Create Snippets update ",
                     SnippetID= snipetHook.Id

                };

                var updated = snipets.Update(toUpdate);

                snipets.All.Count().ShouldBe(1);

                Assert.AreEqual(toUpdate.FileName, updated.FileName);
                Assert.AreEqual(toUpdate.Description, updated.Description);
                Assert.AreEqual(toUpdate.Title, updated.Title);
                snipets.Delete(updated.Id);
                snipets.All.ShouldBeEmpty();
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