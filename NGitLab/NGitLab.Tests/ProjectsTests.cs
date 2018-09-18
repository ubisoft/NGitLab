using System;
using System.Linq;
using Castle.Core.Internal;
using NGitLab.Models;
using NUnit.Framework;
using System.Collections.Generic;

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
            var projects = _projects.Owned.Take(10).ToArray();
            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetVisibleProjects()
        {
            var projects = _projects.Visible.Take(10).ToArray();
            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetAccessibleProjects()
        {
            var projects = _projects.Accessible.Take(10).ToArray();

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

            var projects = GetProjects(query);
            Assert.AreEqual(1, projects.Length);

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetProjectsStatistics()
        {
            var projects = GetProjects(new ProjectQuery { Statistics = true }).ToList();

            if (!projects.Any())
            {
                Assert.Fail("No projects found.");
            }

            projects.ForEach(p => Assert.IsNotNull(p.Statistics));
        }

        [Test]
        public void GetProjectsLinks()
        {
            var projects = GetProjects(new ProjectQuery()).ToList();

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

            var projects = GetProjects(query);

            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetProjectsCanSpecifyTheProjectPerPageCount()
        {
            var query = new ProjectQuery
            {
                Simple = true,
                Visibility = VisibilityLevel.Internal,
                PerPage = 5,
            };

            var projects = GetProjects(query);

            CollectionAssert.IsNotEmpty(projects);
            Assert.That(Initialize.LastRequest.RequestUri.ToString(), Contains.Substring("per_page=5"));
        }

        private Project[] GetProjects(ProjectQuery query, int takeCount = 10)
        {
            return _projects.Get(query).Take(takeCount).ToArray();
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
                WikiEnabled = true,
                Tags = new List<string> { "Tag-1", "Tag-2" }
            };

            var createdProject = _projects.Create(project);

            Assert.AreEqual(project.Description, createdProject.Description);
            Assert.AreEqual(project.IssuesEnabled, createdProject.IssuesEnabled);
            Assert.AreEqual(project.MergeRequestsEnabled, createdProject.MergeRequestsEnabled);
            Assert.AreEqual(project.Name, createdProject.Name);
            CollectionAssert.AreEquivalent(project.Tags, createdProject.TagList);

            _projects.Delete(createdProject.Id);
        }
    }
}
