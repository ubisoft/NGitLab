using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class RunnerTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_can_enable_and_disable_a_runner_on_a_project()
        {
            // We need 2 projects associated to a runner to disable a runner
            using var context = await GitLabTestContext.CreateAsync();
            var project1 = context.CreateProject(initializeWithCommits: true);
            var project2 = context.CreateProject(initializeWithCommits: true);

            var runnersClient = context.Client.Runners;
            var runner = runnersClient.Register(new RunnerRegister() { Token = project1.RunnersToken });
            runnersClient.EnableRunner(project2.Id, new RunnerId(runner.Id));

            runnersClient.DisableRunner(project1.Id, new RunnerId(runner.Id));
            Assert.IsFalse(IsEnabled());

            runnersClient.EnableRunner(project1.Id, new RunnerId(runner.Id));
            Assert.IsTrue(IsEnabled());

            bool IsEnabled() => runnersClient[runner.Id].Projects.Any(x => x.Id == project1.Id);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_can_find_a_runner_on_a_project()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var project2 = context.CreateProject(initializeWithCommits: true);
            var runnersClient = context.Client.Runners;
            var runner = runnersClient.Register(new RunnerRegister() { Token = project.RunnersToken });
            runnersClient.EnableRunner(project2.Id, new RunnerId(runner.Id));

            var result = runnersClient.OfProject(project.Id).ToList();
            Assert.IsTrue(result.Any(r => r.Id == runner.Id));

            runnersClient.DisableRunner(project.Id, new RunnerId(runner.Id));
            result = runnersClient.OfProject(project.Id).ToList();
            Assert.IsTrue(result.All(r => r.Id != runner.Id));
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_some_runner_fields_are_not_null()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var runnersClient = context.Client.Runners;
            var runner = runnersClient.Register(new RunnerRegister() { Token = project.RunnersToken });

            using (await context.StartRunnerForOneJobAsync(project.Id))
            {
                var runnerDetails = await GitLabTestContext.RetryUntilAsync(() => runnersClient[runner.Id], runner => runner.IpAddress != null, TimeSpan.FromMinutes(2));

                Assert.IsNotNull(runnerDetails.Id);
                Assert.IsNotNull(runnerDetails.Active);
                Assert.IsNotNull(runnerDetails.Locked);
                Assert.IsNotNull(runnerDetails.RunUntagged);
                Assert.IsNotNull(runnerDetails.IpAddress);
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_Runner_Can_Be_Locked_And_Unlocked()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var runnersClient = context.Client.Runners;
            var runner = runnersClient.Register(new RunnerRegister() { Token = project.RunnersToken, Locked = false });
            Assert.IsFalse(runner.Locked, "Runner should not be locked.");

            runner = runnersClient.Update(runner.Id, new RunnerUpdate { Locked = true });
            Assert.IsTrue(runner.Locked, "Runner should be locked.");

            runner = runnersClient.Update(runner.Id, new RunnerUpdate { Locked = false });
            Assert.IsFalse(runner.Locked, "Runner should not be locked.");
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_Runner_Can_Update_RunUntagged_Flag()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject(initializeWithCommits: true);
            var runnersClient = context.Client.Runners;
            var runner = runnersClient.Register(new RunnerRegister() { Token = project.RunnersToken, RunUntagged = false, TagList = new[] { "tag" } });
            Assert.False(runner.RunUntagged);

            runner = runnersClient.Update(runner.Id, new RunnerUpdate { RunUntagged = true });
            Assert.AreEqual(true, runner.RunUntagged);
        }
    }
}
