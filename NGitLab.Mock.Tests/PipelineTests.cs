using System.Threading.Tasks;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class PipelineTests
    {
        [Test]
        public async Task Test_pipelines()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew(project => project.Visibility = Models.VisibilityLevel.Internal);
            var commit = project.Repository.Commit(user, "test");

            var pipeline = project.Pipelines.Add(commit.Sha, JobStatus.Success, user);
            var job = pipeline.AddNewJob("Test_job", JobStatus.Success);
            job.Trace = "This is a trace\nWith Multiple line";

            var client = server.CreateClient();
            Assert.AreEqual(job.Trace, await client.GetJobs(project.Id).GetTraceAsync(job.Id));
        }

        [Test]
        public void Test_pipelines_testreport_summary()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew(project => project.Visibility = Models.VisibilityLevel.Internal);
            var commit = project.Repository.Commit(user, "test");

            var pipeline = project.Pipelines.Add(commit.Sha, JobStatus.Success, user);
            pipeline.TestReportsSummary = new Models.TestReportSummary
            {
                Total = new Models.TestReportSummaryTotals
                {
                    Time = 60,
                    Count = 1157,
                    Success = 1157,
                    Failed = 0,
                    Skipped = 0,
                    Error = 0,
                },
            };

            var client = server.CreateClient();
            var summary = client.GetPipelines(project.Id).GetTestReportsSummary(pipeline.Id);
            Assert.AreEqual(60, summary.Total.Time);
            Assert.AreEqual(1157, summary.Total.Count);
            Assert.AreEqual(1157, summary.Total.Success);
            Assert.AreEqual(0, summary.Total.Skipped);
            Assert.AreEqual(0, summary.Total.Failed);
            Assert.AreEqual(0, summary.Total.Error);
        }

        [Test]
        public void Test_create_pipeline_with_branch_ref_sets_sha()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew(project => project.Visibility = Models.VisibilityLevel.Internal);
            var commit = project.Repository.Commit(user, "test");

            var branch = "my-branch";
            project.Repository.CreateBranch(branch);

            var pipeline = project.Pipelines.Add(branch, JobStatus.Success, user);

            Assert.AreEqual(new Sha1(commit.Sha), pipeline.Sha);
        }

        [Test]
        public void Test_create_pipeline_with_tag_ref_sets_sha()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew(project => project.Visibility = Models.VisibilityLevel.Internal);
            var commit = project.Repository.Commit(user, "test");

            var tag = "my-tag";
            project.Repository.CreateTag(tag);

            var pipeline = project.Pipelines.Add(tag, JobStatus.Success, user);

            Assert.AreEqual(new Sha1(commit.Sha), pipeline.Sha);
        }

        [Test]
        public void Test_create_pipeline_with_invalid_ref_does_not_set_sha()
        {
            using var server = new GitLabServer();
            var user = server.Users.AddNew();
            var project = user.Namespace.Projects.AddNew(project => project.Visibility = Models.VisibilityLevel.Internal);
            var commit = project.Repository.Commit(user, "test");

            var pipeline = project.Pipelines.Add("invalid_ref", JobStatus.Success, user);

            Assert.AreEqual(default(Sha1), pipeline.Sha);
        }
    }
}
