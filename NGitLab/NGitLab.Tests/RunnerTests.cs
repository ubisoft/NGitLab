using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class RunnerTests
    {
        [Test]
        public void Test_can_enable_and_disable_a_runner_on_a_project()
        {
            var projectId = Initialize.UnitTestProject.Id;

            var runnerToEnable = GetDefaultRunner();

            Initialize.GitLabClient.Runners.EnableRunner(projectId, new RunnerId(runnerToEnable.Id));
            Assert.IsTrue(Initialize.GitLabClient.Runners[runnerToEnable.Id].Projects.Any(x => x.Id == projectId));

            Initialize.GitLabClient.Runners.DisableRunner(projectId, new RunnerId(runnerToEnable.Id));
            Assert.IsFalse(Initialize.GitLabClient.Runners[runnerToEnable.Id].Projects.Any(x => x.Id == projectId));
        }

        public static Runner GetDefaultRunner()
        {
            var allRunners = Initialize.GitLabClient.Runners.Accessible.ToArray();
            var runner = allRunners.FirstOrDefault(x => x.Active);

            Assert.IsNotNull(runner, "Will not be able to test the builds as no runner is setup for this project");
            return runner;
        }
    }
}