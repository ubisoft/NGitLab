using System.Linq;
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

        //[Test]
        //public void CreateUpdateDelete()
        //{
        //    new ProjectCreate
        //    {
        //        Description = "desc",
        //        ImportUrl = null,
        //        IssuesEnabled = true,
        //        MergeRequestsEnabled= true,
        //        Name = "test",
        //        NamespaceId = 

        //    }

        //    _projects.Create()
        //}
    }
}