using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    [Timeout(30000)]
    public class PipelineTests
    {
        private IPipelineClient _pipelines;
        private string _ciJobToken;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var triggers = Initialize.GitLabClient.GetTriggers(Initialize.UnitTestProject.Id);
            var trigger = triggers.Create("Test Trigger");
            _ciJobToken = trigger.Token;
            _pipelines = Initialize.GitLabClient.GetPipelines(Initialize.UnitTestProject.Id);
            CommitsTests.EnableCiOnTestProject();

            CreatePipeline("NewTagForPipelineTests");
        }

        private void CreatePipeline(string name)
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = name,
                Ref = "master",
            });

            while (FindPipeline(name) == null)
            {
                Console.WriteLine("Waiting for pipeline to start.");
                Thread.Sleep(1000);
            }
        }

        private void CreatePipelineWithVariables(Dictionary<string, string> variables)
        {
            const string refName = "master";

            var initialNumberOfPipelines = CountPipelines(refName);
            _pipelines.CreatePipelineWithTrigger(_ciJobToken, refName, variables);

            while (CountPipelines(refName) == initialNumberOfPipelines)
            {
                Console.WriteLine("Waiting for pipeline to start.");
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void Test_can_list_the_pipeline_of_the_current_tag()
        {
            var thisTagPipeline = FindPipeline("NewTagForPipelineTests");

            Assert.IsNotNull(thisTagPipeline);
        }

        [Test]
        public void Test_can_list_all_jobs_from_project()
        {
            var allJobs = _pipelines.AllJobs;

            Assert.That(allJobs.Any());
        }

        [Test]
        public void Test_search_for_pipeline()
        {
            var pipeline = _pipelines.All.First();
            var pipelinesFromQuery = _pipelines.Search(new PipelineQuery
            {
                Ref = pipeline.Ref,
            });

            Assert.IsTrue(pipelinesFromQuery.Any());
        }

        [Test]
        public void Test_delete_pipeline()
        {
            CreatePipeline("PipelineToDelete");
            var pipelineToDelete = _pipelines.All.Single(p => string.Equals(p.Ref, "PipelineToDelete", StringComparison.Ordinal));
            _pipelines.Delete(pipelineToDelete.Id);

            while (FindPipeline("PipelineToDelete") != null)
            {
                Console.WriteLine("Waiting for pipeline to be deleted.");
                Thread.Sleep(1000);
            }

            Assert.IsTrue(!_pipelines.All.Any(p => string.Equals(p.Ref, "PipelineToDelete", StringComparison.Ordinal)));
        }

        [Test]
        public void Test_get_pipeline_variables()
        {
            CreatePipelineWithVariables(new Dictionary<string, string>(StringComparer.InvariantCulture) { { "Test", "HelloWorld" } });
            var pipelinesWithVariables = _pipelines.All.Where(p => string.Equals(p.Ref, "master", StringComparison.Ordinal));

            var variables = _pipelines.GetVariables(pipelinesWithVariables.First().Id);

            Assert.IsTrue(variables.Any(v =>
                v.Key.Equals("Test", StringComparison.InvariantCulture) &&
                v.Value.Equals("HelloWorld", StringComparison.InvariantCulture)));
        }

        [Test]
        public void Test_get_pipeline_variables_special_characters()
        {
            CreatePipelineWithVariables(new Dictionary<string, string>(StringComparer.InvariantCulture) { { "EncodedVariable", "+4+" } });
            var pipelinesWithVariables = _pipelines.All.Where(p => string.Equals(p.Ref, "master", StringComparison.Ordinal));

            var variables = _pipelines.GetVariables(pipelinesWithVariables.First().Id);
            Assert.IsTrue(variables.Any(v => v.Key.Equals("EncodedVariable", StringComparison.InvariantCulture) && v.Value.Equals("+4+", StringComparison.InvariantCulture)));
        }

        private PipelineBasic FindPipeline(string refName)
        {
            return _pipelines.All.FirstOrDefault(x => string.Equals(x.Ref, refName, StringComparison.Ordinal));
        }

        private int CountPipelines(string refName)
        {
            return _pipelines.All.Count(x => x.Ref.Equals(refName, StringComparison.InvariantCulture));
        }
    }
}
