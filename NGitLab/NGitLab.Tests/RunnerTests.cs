using System;
using System.Linq;
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

#pragma warning disable 618 // Obsolete
            var jobsOld = runners.GetJobs(runnerToEnable.Id, JobScope.All).Take(1);
#pragma warning restore 618
            var jobs = runners.GetJobs(runnerToEnable.Id).Take(1);

            Assert.That(jobsOld, Is.Not.Empty);
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

        [Test]
        public void Test_some_runner_fields_are_not_null()
        {
            var projectId = Initialize.UnitTestProject.Id;
            var runnerToEnable = GetDefaultRunner();
            var runners = Initialize.GitLabClient.Runners;
            runners.EnableRunner(projectId, new RunnerId(runnerToEnable.Id));

            var result = Initialize.GitLabClient.Runners.OfProject(projectId).ToList();
            var runnerId = result[0].Id;
            var runnerDetails = runners[runnerId];

            Assert.IsNotNull(runnerDetails.Id);
            Assert.IsNotNull(runnerDetails.Active);
            Assert.IsNotNull(runnerDetails.Locked);
            Assert.IsNotNull(runnerDetails.RunUntagged);
            Assert.IsNotNull(runnerDetails.IpAddress);

            runners.DisableRunner(projectId, new RunnerId(runnerToEnable.Id));
            result = Initialize.GitLabClient.Runners.OfProject(projectId).ToList();
            Assert.IsEmpty(result);
        }

        [Test]
        public void Test_Runner_Can_Be_Locked_And_Unlocked()
        {
            var runner = GetLockingRunner();
            var runners = Initialize.GitLabClient.Runners;

            // assert runner is not locked
            Assert.IsFalse(runner.Locked, "Runner should not be locked.");

            // lock runner
            var lockedRunner = new RunnerUpdate
            {
                Locked = true
            };
            var updatedRunner = runners.Update(runner.Id, lockedRunner);

            // assert runner is locked
            Assert.IsTrue(updatedRunner.Locked, "Runner should be locked.");

            // unlock runner
            var unlockedRunner = new RunnerUpdate
            {
                Locked = false
            };
            updatedRunner = runners.Update(runner.Id, unlockedRunner);

            Assert.False(updatedRunner.Locked, "Runner should not be locked.");
        }

        [Test]
        public void Test_Runner_Can_Update_RunUntagged_Flag()
        {
            var runner = GetLockingRunner();
            var runners = Initialize.GitLabClient.Runners;

            Assert.IsFalse(runner.RunUntagged, "Runner should not run untagged.");

            // update runner
            var update = new RunnerUpdate
            {
                RunUntagged = true
            };
            var updatedRunner = runners.Update(runner.Id, update);

            Assert.IsTrue(updatedRunner.RunUntagged, "Runner should run untagged.");

            // update runner
            update = new RunnerUpdate
            {
                RunUntagged = false
            };
            updatedRunner = runners.Update(runner.Id, update);

            Assert.False(updatedRunner.RunUntagged, "Runner should not run untagged.");
        }

        private static bool IsEnabled(Runner runner, int projectId)
        {
            return Initialize.GitLabClient.Runners[runner.Id].Projects.Any(x => x.Id == projectId);
        }

        public static Runner GetDefaultRunner()
        {
            var allRunners = Initialize.GitLabClient.Runners.Accessible.ToArray();
            var runner = Array.Find(allRunners, x => x.Active && string.Equals(x.Description, "ToolSquare_Test", StringComparison.Ordinal));

            if (runner == null)
            {
                Assert.Inconclusive("Will not be able to test the builds as no runner is setup for this project");
            }

            return runner;
        }

        public static Runner GetLockingRunner()
        {
            https://pdc-tst-gitlab01.ubisoft.org/ToolSquare/Runners/SharedRunners_Staging/settings/ci_cd

            var runnerName = "TestLockRunner";

            var allRunners = Initialize.GitLabClient.Runners.Accessible.ToArray();
            var runner = Array.Find(allRunners, x => string.Equals(x.Description, runnerName, StringComparison.Ordinal));

            if (runner == null)
            {
                Assert.Inconclusive("Will not be able to test the locking mechanism as {runnerName} is unavailable.", runnerName);
            }

            return runner;
        }
    }
}
