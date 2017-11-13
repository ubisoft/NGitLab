using System;
using System.Linq;
using Castle.Core.Internal;
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
        public void GetProjectsStatistics()
        {
            var projects = _projects.Get(new ProjectQuery{Statistics = true});

            if(!projects.Any())
                Assert.Fail("No projects found.");

            projects.ForEach(p => Assert.IsNotNull(p.Statistics));
        }

        [Test]
        public void GetProjectsLinks()
        {
            var projects = _projects.Get(new ProjectQuery());

            if (!projects.Any())
                Assert.Fail("No projects found.");

            projects.ForEach(p => Assert.IsNotNull(p.Links)); 
        }

        [Test]
        public void GetProjectsByQuery_VisibilityInternal()
        {
            var query = new ProjectQuery
            {
                Simple = true,
                Visibility = VisibilityLevel.Internal
            };

            var projects = _projects.Get(query).ToArray();

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
                Name = "CreateDelete_Test_" + new Random().Next(),
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Internal,
                WikiEnabled = true
            };

            var createdProject = _projects.Create(project);

            Assert.AreEqual(project.Description, createdProject.Description);
            Assert.AreEqual(project.IssuesEnabled, createdProject.IssuesEnabled);
            Assert.AreEqual(project.MergeRequestsEnabled, createdProject.MergeRequestsEnabled);
            Assert.AreEqual(project.Name, createdProject.Name);

            _projects.Delete(createdProject.Id);
        }
    }
}