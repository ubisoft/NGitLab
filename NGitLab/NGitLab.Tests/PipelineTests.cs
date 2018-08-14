using System.Linq;
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

        [Test, Timeout(5000)]
        public void Test_can_list_the_pipeline_of_the_current_tag()
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = "NewTagForBuild",
                Ref = "master"
            });

            PipelineBasic thisTagPipeline;

            while (true)
            {
                var pipelines = _pipelines.All.ToArray();
                thisTagPipeline = pipelines.FirstOrDefault(x => x.Ref == "NewTagForBuild");

                if (thisTagPipeline != null && _pipelines[thisTagPipeline.Id].Ref == "NewTagForBuild")
                {
                    break;
                }
            }

            while (true)
            {
                var jobs = _pipelines.GetJobs(thisTagPipeline.Id);

                if (jobs.Length > 0)
                    return;
            }
        }

        [Test, Timeout(5000)]
        public void Test_can_list_pipelines_with_scope_all()
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = "NewTagForBuildScopeAll",
                Ref = "master"
            });

            while (true)
            {
#pragma warning disable 618 // Obsolete
                if (_pipelines.GetJobsInProject(JobScope.All).Any())
                    return;
#pragma warning restore 618
            }
        }

        [Test, Timeout(5000)]
        public void Test_can_list_pipelines_with_scope_pending()
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = "NewTagForBuildScopePending",
                Ref = "master"
            });

            while (true)
            {
#pragma warning disable 618 // Obsolete
                if (_pipelines.GetJobsInProject(JobScope.Pending).Any())
                    return;
#pragma warning restore 618
            }
        }

        [Test, Timeout(10000)]
        public void Test_can_list_all_jobs_from_project()
        {
            Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags.Create(new TagCreate
            {
                Name = "NewTagForBuildAllJobs",
                Ref = "master"
            });

            while (true)
            {
                if (_pipelines.AllJobs.Any())
                    return;
            }
        }
    }
}
