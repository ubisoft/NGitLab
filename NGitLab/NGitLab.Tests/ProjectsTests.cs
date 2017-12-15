using System;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests {
    public class ProjectsTests : IDisposable {
        readonly Project created;
        readonly IProjectClient projects;

        public ProjectsTests() {
            projects = Config.Connect().Projects;
            CreateProject(out created, "default-project-tests", false);
        }

        public void Dispose() {
            projects.Delete(created.Id);
        }

        [Test]
        [Category("Server_Required")]
        public void GetStarredProjects() {
            Project starcreated = null;
            try {
                CreateProject(out starcreated, "default-project-starred", true);
                projects.Starred().ShouldNotBeEmpty();
            }
            finally {
                if (starcreated != null)
                    projects.Delete(starcreated.Id);
            }
        }

        [Test]
        [Category("Server_Required")]
        public void GetOwnedProjects() {
            projects.Owned().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetAccessibleProjects() {
            projects.Accessible().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void CreateDelete() {
            Project created2 = null;

            try {
                var p = CreateProject(out created2, "test2", false);

                created2.Description.ShouldBe(p.Description);
                created2.IssuesEnabled.ShouldBe(p.IssuesEnabled);
                created2.MergeRequestsEnabled.ShouldBe(p.MergeRequestsEnabled);
                created2.Name.ShouldBe(p.Name);
                projects.Delete(created2.Id).ShouldBeTrue();
            }
            catch (Exception) {
                if (created2 != null)
                    projects.Delete(created2.Id);
            }
        }
        [Test]
        [Category("Server_Required")]
        public void CreateError()
        {
            Project created2 = null;

            try
            {
                var p = CreateProject1(out created2, "test2", false);

                created2.Description.ShouldBe(p.Description);
                created2.IssuesEnabled.ShouldBe(p.IssuesEnabled);
                created2.MergeRequestsEnabled.ShouldBe(p.MergeRequestsEnabled);
                created2.Name.ShouldBe(p.Name);
                projects.Delete(created2.Id).ShouldBeTrue();
            }
            catch (Exception)
            {
                if (created2 != null)
                    projects.Delete(created2.Id);
            }
        }
        ProjectCreate CreateProject1(out Project created, string name, bool star)
        {
            var p = new ProjectCreate
            {
                Description = "desc",
                ImportUrl = null,
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = name,
                NamespaceId = -1,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Public,
                WallEnabled = true,
                WikiEnabled = true
            };

            created = projects.Create(p);

            // star is applicable
            if (star)
                created = projects.Star(created.Id);

            return p;
        }
        ProjectCreate CreateProject(out Project created, string name, bool star) {
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

            // star is applicable
            if (star)
                created = projects.Star(created.Id);

            return p;
        }
    }
}