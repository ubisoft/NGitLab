using System;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class PipelineTests
    {
        private IPipelineClient _pipelines;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _pipelines = Initialize.GitLabClient.GetPipelines(Initialize.UnitTestProject.Id);
            CommitsTests.EnableCiOnTestProject();
        }

        [Test]
        public void Test_can_list_the_pipeline_of_the_current_tag()
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = "NewTagForBuild",
                Ref = "master"
            });

            // Let the server process the tag.
            Thread.Sleep(TimeSpan.FromSeconds(2));

            var pipelines = _pipelines.All.ToArray();
            var thisTagPipeline = pipelines.FirstOrDefault(x => x.Ref == "NewTagForBuild");

            Assert.IsNotNull(thisTagPipeline);
            Assert.AreEqual(_pipelines[thisTagPipeline.Id].Ref, "NewTagForBuild");

            var jobs = _pipelines.GetJobs(thisTagPipeline.Id);
            Assert.That(jobs.Length > 0);
        }
    }
}