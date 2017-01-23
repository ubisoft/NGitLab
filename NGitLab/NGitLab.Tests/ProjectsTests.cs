using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectsTests
    {
        private readonly IProjectClient _projects;

        public ProjectsTests()
        {
            _projects = Initialize.GitLabClient.Projects;
        }

        [Test]
        public void GetAllProjects()
        {
            if (!Initialize.IsAdmin)
            {
                Assert.Inconclusive("Cannot test getting all projects since the current user is not admin");
            }

            var projects = _projects.All.ToArray();
            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetOwnedProjects()
        {
            var projects = _projects.Owned.ToArray();
            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetVisibleProjects()
        {
            var projects = _projects.Visible.ToArray();
            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetAccessibleProjects()
        {
            var projects = _projects.Accessible.ToArray();

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetProjectsByQuery()
        {
            var query = new ProjectQuery
            {
                Simple = true,
                Search = Initialize.UnitTestProject.Name
            };

            var projects = _projects.Get(query).ToArray();
            Assert.AreEqual(1, projects.Length);

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void CreateDelete()
        {
            var project = new ProjectCreate
            {
                Description = "desc",
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = "test 2",
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Public,
                WikiEnabled = true
            };

            var createdProject = _projects.Create(project);

            Assert.AreEqual(project.Description, createdProject.Description);
            Assert.AreEqual(project.IssuesEnabled, createdProject.IssuesEnabled);
            Assert.AreEqual(project.MergeRequestsEnabled, createdProject.MergeRequestsEnabled);
            Assert.AreEqual(project.Name, createdProject.Name);

            Assert.AreEqual(_projects.Delete(createdProject.Id), true);
        }
    }
}