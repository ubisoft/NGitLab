using System;
using System.Collections.Generic;
using System.Text;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class RepositoryMockTests
    {
        [Test]
        public void Test_project_can_be_mocked()
        {
            using var gitLabServer = new GitLabServer();
            var group = new Group("TestGroup");
            gitLabServer.Groups.Add(group);
            var project = new Project("Test") { Visibility = VisibilityLevel.Internal };
            group.Projects.Add(project);

            var modelProject = project.ToClientProject();
            Assert.IsNotNull(modelProject);
            Assert.AreEqual("Test", modelProject.Name);
            Assert.AreEqual("testgroup", modelProject.Namespace.FullPath);
        }
    }
}
