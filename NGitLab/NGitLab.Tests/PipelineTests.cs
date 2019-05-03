using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    [Timeout(30000)]
    public class PipelineTests
    {
        private IPipelineClient _pipelines;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _pipelines = Initialize.GitLabClient.GetPipelines(Initialize.UnitTestProject.Id);
            CommitsTests.EnableCiOnTestProject();

            CreatePipeline();
        }

        private void CreatePipeline()
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = "NewTagForPipelineTests",
                Ref = "master"
            });

            while (FindPipeline("NewTagForPipelineTests") == null)
            {
                Console.WriteLine("Waiting for pipeline to start.");
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void Test_can_list_the_pipeline_of_the_current_tag()
        {
            PipelineBasic thisTagPipeline = FindPipeline("NewTagForPipelineTests");

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
            PipelineBasic pipeline = _pipelines.All.First();
            IEnumerable<PipelineBasic> pipelinesFromQuery = _pipelines.Search(new PipelineQuery
            {
                    Ref = pipeline.Ref
            });

            Assert.IsTrue(pipelinesFromQuery.Any());
        }

        private PipelineBasic FindPipeline(string refName)
        {
            return _pipelines.All.FirstOrDefault(x => x.Ref == refName);
        }
    }
}
