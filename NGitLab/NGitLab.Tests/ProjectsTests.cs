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
            _projects = GitLabClient.Connect(Config.ServiceUrl, Config.Secret).Projects;
        }

        [Test]
        public void GetAllProjects()
        {
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
        public void GetAccessibleProjects()
        {
            var projects = _projects.Accessible.ToArray();

            CollectionAssert.IsNotEmpty(projects);
        }

        [Explicit, Test]
        public void DeleteAll()
        {
            foreach (var p in _projects.All.ToArray())
            {
                _projects.Delete(p.Id);
            }
        }

        [Test]
        public void CreateDelete()
        {
            var p = new ProjectCreate
            {
                Description = "desc",
                ImportUrl = null,
                IssuesEnabled = true,
                MergeRequestsEnabled = true,
                Name = "test",
                NamespaceId = null,
                SnippetsEnabled = true,
                VisibilityLevel = VisibilityLevel.Public,
                WallEnabled = true,
                WikiEnabled = true
            };

            var created = _projects.Create(p);

            Assert.AreEqual(p.Description, created.Description);
            Assert.AreEqual(p.IssuesEnabled, created.IssuesEnabled);
            Assert.AreEqual(p.MergeRequestsEnabled, created.MergeRequestsEnabled);
            Assert.AreEqual(p.Name, created.Name);

            _projects.Delete(created.Id);
        }
    }
}