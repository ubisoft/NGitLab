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
    }
}
