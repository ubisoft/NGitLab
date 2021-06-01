using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    [Timeout(240_000)]
    public class PipelineTests
    {
        [Test]
        public async Task Test_can_list_the_pipelines()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);

            var pipelines = await GitLabTestContext.RetryUntilAsync(() => pipelineClient.All, p => p.Any(), TimeSpan.FromSeconds(120));
            Assert.IsNotEmpty(pipelines);
        }

        [Test]
        public async Task Test_can_list_all_jobs_from_project()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);

            var allJobs = await GitLabTestContext.RetryUntilAsync(() => pipelineClient.AllJobs.ToList(), p => p.Any(), TimeSpan.FromSeconds(120));
            Assert.That(allJobs.Any());
        }

        [Test]
        public async Task Test_search_for_pipeline()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);

            var query = new PipelineQuery
            {
                Ref = project.DefaultBranch,
            };
            var pipelinesFromQuery = await GitLabTestContext.RetryUntilAsync(() => pipelineClient.Search(query).ToList(), p => p.Any(), TimeSpan.FromSeconds(120));

            Assert.IsTrue(pipelinesFromQuery.Any());
        }

        [Test]
        public async Task Test_delete_pipeline()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);
            var pipeline = await GitLabTestContext.RetryUntilAsync(() => pipelineClient.All.FirstOrDefault(), p => p != null, TimeSpan.FromSeconds(120));

            pipelineClient.Delete(pipeline.Id);
            await GitLabTestContext.RetryUntilAsync(() => pipelineClient.All.FirstOrDefault(), p => p == null, TimeSpan.FromSeconds(120));
        }

        [Test]
        public async Task Test_create_pipeline_with_variables()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);

            // Arrange/Act
            var pipeline = pipelineClient.Create(new PipelineCreate()
            {
                Ref = project.DefaultBranch,
                Variables =
                {
                    { "Var1", "Value1" },
                    { "Var2", "+4+" },
                },
            });

            // Assert
            var variables = pipelineClient.GetVariables(pipeline.Id);

            var var1 = variables.SingleOrDefault(v => v.Key.Equals("Var1", StringComparison.Ordinal));
            Assert.NotNull(var1);

            var var2 = variables.SingleOrDefault(v => v.Key.Equals("Var2", StringComparison.Ordinal));
            Assert.NotNull(var2);

            Assert.AreEqual("Value1", var1.Value);
            Assert.AreEqual("+4+", var2.Value);
        }

        [Test]
        public async Task Test_create_pipeline_with_testreports()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);

            // Arrange/Act
            var pipeline = pipelineClient.Create(new PipelineCreate()
            {
                Ref = project.DefaultBranch,
                TestReports =
                {
                    Total_time = 1,
                    Total_count = 1,
                    Success_count = 1,
                    Failed_count = 1,
                    Skipped_count = 0,
                    Error_count = 0,
                    Test_suites = null,
                },
            });

            // Assert
            var testReports = pipelineClient.GetTestReports(pipeline.Id);
            Assert.IsNotNull(testReports);

            var totalTime = testReports.Select(x => x.Total_time).First();
            var totalCount = testReports.Select(x => x.Total_count).First();
            var successCount = testReports.Select(x => x.Success_count).First();
            var failedCount = testReports.Select(x => x.Failed_count).First();
            var skippedCount = testReports.Select(x => x.Skipped_count).First();
            var errorCount = testReports.Select(x => x.Error_count).First();
            var testSuites = testReports.Select(x => x.Test_suites).First();

            Assert.AreEqual(1, totalTime);
            Assert.AreEqual(1, totalCount);
            Assert.AreEqual(1, successCount);
            Assert.AreEqual(1, failedCount);
            Assert.AreEqual(0, skippedCount);
            Assert.AreEqual(0, errorCount);
            Assert.IsNull(testSuites);
        }

        [Test]
        public async Task Test_get_triggered_pipeline_variables()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var pipelineClient = context.Client.GetPipelines(project.Id);
            JobTests.AddGitLabCiFile(context.Client, project);

            var triggers = context.Client.GetTriggers(project.Id);
            var trigger = triggers.Create("Test Trigger");
            var ciJobToken = trigger.Token;

            var pipeline = pipelineClient.CreatePipelineWithTrigger(ciJobToken, project.DefaultBranch, new Dictionary<string, string>(StringComparer.InvariantCulture) { { "Test", "HelloWorld" } });

            var variables = pipelineClient.GetVariables(pipeline.Id);

            Assert.IsTrue(variables.Any(v =>
                v.Key.Equals("Test", StringComparison.InvariantCulture) &&
                v.Value.Equals("HelloWorld", StringComparison.InvariantCulture)));
        }
    }
}
