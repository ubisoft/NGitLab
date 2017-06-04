using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

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
        public void GetAllProjects()
        {
            var projects = _projects.All().ToArray();

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        [Category("Server_Required")]
        public void GetOwnedProjects()
        {
            var projects = _projects.Owned().ToArray();

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        [Category("Server_Required")]
        public void GetAccessibleProjects()
        {
            var projects = _projects.Accessible().ToArray();

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        [Category("Server_Required")]
        public void CreateDelete()
        {
            Project created;
            var p = CreateProject(out created, "test2");

            Assert.AreEqual(p.Description, created.Description);
            Assert.AreEqual(p.IssuesEnabled, created.IssuesEnabled);
            Assert.AreEqual(p.MergeRequestsEnabled, created.MergeRequestsEnabled);
            Assert.AreEqual(p.Name, created.Name);

            Assert.AreEqual(_projects.Delete(created.Id), true);
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