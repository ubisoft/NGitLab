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
    [Timeout(30000)]
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
