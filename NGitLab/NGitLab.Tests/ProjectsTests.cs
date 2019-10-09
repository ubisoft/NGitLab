using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NGitLab.Models;
using NGitLab.Tests.Extensions;
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
            var projects = _projects.Owned.Take(10).ToArray();
            CollectionAssert.IsNotEmpty(projects);
        }

        [Test]
        public void GetVisibleProjects()
        {
            var projects = this.ExecuteWithFallbacks(client => client.Projects.Visible.Take(10).ToArray());

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

            if (projects.Count == 0)
            {
                Assert.Fail("No projects found.");
            }

            projects.ForEach(p => Assert.IsNotNull(p.Statistics));
        }

        [Test]
        public void GetProjectsLinks()
        {
            var projects = GetProjects(new ProjectQuery()).ToList();

            if (projects.Count == 0)
            {
                Assert.Fail("No projects found.");
            }

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
        public void GetProjectByIdByQuery_Statistics()
        {
            var query = new SingleProjectQuery()
            {
                Statistics = true
            };

            var project = _projects.GetById(Initialize.UnitTestProject.Id, query);

            Assert.IsNotNull(project);
            Assert.IsNotNull(project.Statistics);
        }

        [Test]
        public void GetProjectLanguages()
        {
            var project = Initialize.UnitTestProject;

            var file = new FileUpsert
            {
                Branch = "master",
                CommitMessage = "add javascript file",
                Content = "var test = 0;",
                Path = "test.js",
            };

            Initialize.GitLabClient.GetRepository(project.Id).Files.Create(file);
            var languages = Initialize.GitLabClient.Projects.GetLanguages(project.Id.ToString());
            Assert.That(languages.Count, Is.EqualTo(1));
            StringAssert.AreEqualIgnoringCase("javascript", languages.First().Key);
            Assert.That(languages.First().Value, Is.EqualTo(100));
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
                IssuesAccessLevel = IssuesAccessLevel.Enabled,
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
            Assert.AreEqual(project.MergeRequestsEnabled, createdProject.MergeRequestsEnabled);
            Assert.AreEqual(project.Name, createdProject.Name);
            CollectionAssert.AreEquivalent(project.Tags, createdProject.TagList);

            _projects.Delete(createdProject.Id);
        }

        [Test]
        public void Test_get_by_project_query_projectQuery_MinAccessLevel_returns_projects()
        {
            //Arrange
            var projectQuery10 = new ProjectQuery
            {
                MinAccessLevel = AccessLevel.Guest
            };
            var projectQuery20 = new ProjectQuery
            {
                MinAccessLevel = AccessLevel.Reporter
            };
            var projectQuery30 = new ProjectQuery
            {
                MinAccessLevel = AccessLevel.Developer
            };
            var projectQuery40 = new ProjectQuery
            {
                MinAccessLevel = AccessLevel.Master
            };

            //Act
            var result10 = _projects.Get(projectQuery10);
            var result20 = _projects.Get(projectQuery20);
            var result30 = _projects.Get(projectQuery30);
            var result40 = _projects.Get(projectQuery40);
            // No owner level (50) for project! See https://gitlab-ncsa.ubisoft.org/help/api/members.md

            // Assert
            Assert.IsTrue(result10.Any());
            Assert.IsTrue(result20.Any());
            Assert.IsTrue(result30.Any());
            Assert.IsTrue(result40.Any());
        }

        [Test]
        [Timeout(30000)]
        public void ForkProject()
        {
            var project = new ProjectCreate
            {
                Description = "desc",
                IssuesAccessLevel = IssuesAccessLevel.Enabled,
                MergeRequestsEnabled = true,
                Name = "ForkProject_Test_" + new Random().Next(),
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Internal,
                WikiEnabled = true,
                Tags = new List<string> { "Tag-1", "Tag-2" }
            };

            var createdProject = _projects.Create(project);
            var forkedProject = _projects.Fork(createdProject.Id.ToString(CultureInfo.InvariantCulture), new ForkProject()
            {
                Path = createdProject.Path + "-fork",
                Name = createdProject.Name + "Fork",
            });

            var forks = _projects.GetForks(createdProject.Id.ToString(CultureInfo.InvariantCulture), new ForkedProjectQuery());

            Assert.That(forks.Single().Id, Is.EqualTo(forkedProject.Id));

            _projects.Delete(forkedProject.Id);
            _projects.Delete(createdProject.Id);
        }

    }
}
