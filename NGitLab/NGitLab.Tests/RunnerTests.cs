using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class RunnerTests
    {
        [Test]
        public void Test()
        {
            var runnerToEnable = GetDefaultRunner();
            var runners = Initialize.GitLabClient.Runners;

            var jobs = runners.GetJobs(runnerToEnable.Id, JobScope.All)
                .Take(1);

            Assert.That(jobs, Is.Not.Empty);
        }

        [Test]
        public void Test_can_enable_and_disable_a_runner_on_a_project()
        {
            var projectId = Initialize.UnitTestProject.Id;

            var runnerToEnable = GetDefaultRunner();
            var runners = Initialize.GitLabClient.Runners;

            if (IsEnabled(runnerToEnable, projectId))
            {
                runners.DisableRunner(projectId, new RunnerId(runnerToEnable.Id));
            }

            runners.EnableRunner(projectId, new RunnerId(runnerToEnable.Id));
            Assert.IsTrue(IsEnabled(runnerToEnable, projectId));

            runners.DisableRunner(projectId, new RunnerId(runnerToEnable.Id));
            Assert.IsFalse(IsEnabled(runnerToEnable, projectId));
        }

        [Test]
        public void Test_can_find_a_runner_on_a_project()
        {
            var projectId = Initialize.UnitTestProject.Id;
            var runnerToEnable = GetDefaultRunner();
            var runners = Initialize.GitLabClient.Runners;
            runners.EnableRunner(projectId, new RunnerId(runnerToEnable.Id));

            var result = Initialize.GitLabClient.Runners.OfProject(projectId).ToList();
            Assert.IsTrue(result[0].Id == runnerToEnable.Id);

            runners.DisableRunner(projectId, new RunnerId(runnerToEnable.Id));
            result = Initialize.GitLabClient.Runners.OfProject(projectId).ToList();
            Assert.IsEmpty(result);
        }

        private static bool IsEnabled(Runner runner, int projectId)
        {
            return Initialize.GitLabClient.Runners[runner.Id].Projects.Any(x => x.Id == projectId);
        }

        public static Runner GetDefaultRunner()
        {
            var allRunners = Initialize.GitLabClient.Runners.Accessible.ToArray();
            var runner = allRunners.FirstOrDefault(x => x.Active);

            if (runner == null)
            {
                Assert.Inconclusive("Will not be able to test the builds as no runner is setup for this project");
            }

            return runner;
        }
    }
}