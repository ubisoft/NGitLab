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

            var reportSummary = pipeline.TestReportsSummary = new Models.TestReportSummary
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
            Assert.AreEqual(job.Trace, await client.GetJobs(project.Id).GetTraceAsync(job.Id));

            var summary = client.GetPipelines(project.Id).GetTestReportsSummary(pipeline.Id);
            Assert.AreEqual(60, summary.Total.Time);
            Assert.AreEqual(1157, summary.Total.Count);
            Assert.AreEqual(1157, summary.Total.Success);
            Assert.AreEqual(0, summary.Total.Skipped);
            Assert.AreEqual(0, summary.Total.Failed);
            Assert.AreEqual(0, summary.Total.Error);
        }
    }
}
