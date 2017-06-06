using System;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests
{
    public class ProjectsTests : IDisposable
    {
        private readonly IProjectClient _projects;
        private readonly Project _created;

        public ProjectsTests()
        {
            _projects = Config.Connect().Projects;
            CreateProject(out _created, "default");
        }

        public void Dispose()
        {
            _projects.Delete(_created.Id);
        }

        [Test]
        [Category("Server_Required")]
        public void GetStarredProjects()
        {
            _projects.Starred().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetOwnedProjects()
        {
            _projects.Owned().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetAccessibleProjects()
        {
            _projects.Accessible().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void CreateDelete()
        {
            Project created;
            var p = CreateProject(out created, "test2");

            created.Description.ShouldBe(p.Description);
            created.IssuesEnabled.ShouldBe(p.IssuesEnabled);
            created.MergeRequestsEnabled.ShouldBe(p.MergeRequestsEnabled);
            created.Name.ShouldBe(p.Name);

            _projects.Delete(created.Id).ShouldBeTrue();
        }

        private ProjectCreate CreateProject(out Project created, string name)
        {
            var p = new ProjectCreate
            {
                Description = "desc",
                ImportUrl = null,
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = name,
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Public,
                WallEnabled = true,
                WikiEnabled = true
            };

            created = _projects.Create(p);
            return p;
        }
    }
}