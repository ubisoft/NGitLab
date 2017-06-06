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
            CreateProject(out _created, "default-project-tests", false);
        }

        public void Dispose()
        {
            _projects.Delete(_created.Id);
        }

        [Test]
        [Category("Server_Required")]
        public void GetStarredProjects()
        {
            Project _starcreated = null;
            try
            {

                CreateProject(out _starcreated, "default-project-starred", true);
                _projects.Starred().ShouldNotBeEmpty();
            }
            finally
            {
                if (_starcreated != null)
                {
                    _projects.Delete(_starcreated.Id);
                }
            }
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
            Project created2 = null;

            try
            {
                var p = CreateProject(out created2, "test2", false);

                created2.Description.ShouldBe(p.Description);
                created2.IssuesEnabled.ShouldBe(p.IssuesEnabled);
                created2.MergeRequestsEnabled.ShouldBe(p.MergeRequestsEnabled);
                created2.Name.ShouldBe(p.Name);
                _projects.Delete(created2.Id).ShouldBeTrue();
            }
            catch (Exception ex)
            {
                if (created2 != null)
                {
                    _projects.Delete(created2.Id);
                }
            }
        }

        private ProjectCreate CreateProject(out Project created, string name, bool star)
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
                WikiEnabled = true,
            };

            created = _projects.Create(p);

            // star is applicable
            if (star)
            {
                created = _projects.Star(created.Id);
            }

            return p;
        }
    }
}