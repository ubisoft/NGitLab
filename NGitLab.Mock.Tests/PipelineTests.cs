using System.Threading.Tasks;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class PipelineTests
{
    [Test]
    public async Task Test_pipelines()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var commit = project.Repository.Commit(user, "test");

        var pipeline = project.Pipelines.Add(commit.Sha, JobStatus.Success, user);
        var job = pipeline.AddNewJob("Test_job", JobStatus.Success);
        job.Trace = "This is a trace\nWith Multiple line";

        var client = server.CreateClient();
        Assert.That(await client.GetJobs(project.Id).GetTraceAsync(job.Id), Is.EqualTo(job.Trace));
    }

    [Test]
    public void Test_pipelines_testreport_summary()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var commit = project.Repository.Commit(user, "test");

        var pipeline = project.Pipelines.Add(commit.Sha, JobStatus.Success, user);
        pipeline.TestReportsSummary = new TestReportSummary
        {
            Total = new TestReportSummaryTotals
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
        Assert.That(summary.Total.Time, Is.EqualTo(60));
        Assert.That(summary.Total.Count, Is.EqualTo(1157));
        Assert.That(summary.Total.Success, Is.EqualTo(1157));
        Assert.That(summary.Total.Skipped, Is.EqualTo(0));
        Assert.That(summary.Total.Failed, Is.EqualTo(0));
        Assert.That(summary.Total.Error, Is.EqualTo(0));
    }

    [TestCase(false)]
    [TestCase(true)]
    public void Test_create_pipeline_with_branch_ref_sets_sha(bool addCommitAfterBranching)
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var commit = project.Repository.Commit(user, "test");

        var branch = "my-branch";
        if (addCommitAfterBranching)
        {
            project.Repository.CreateAndCheckoutBranch(branch);
            var commit2 = project.Repository.Commit(user, "another test");
            Assert.That(commit2.Sha, Is.Not.EqualTo(commit.Sha));
            commit = commit2;
        }
        else
        {
            project.Repository.CreateBranch(branch);
        }

        var pipeline = project.Pipelines.Add(branch, JobStatus.Success, user);

        Assert.That(pipeline.Sha, Is.EqualTo(new Sha1(commit.Sha)));
    }

    [Test]
    public void Test_create_pipeline_with_tag_ref_sets_sha()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var commit = project.Repository.Commit(user, "test");

        var tag = "my-tag";
        project.Repository.CreateTag(tag);

        var pipeline = project.Pipelines.Add(tag, JobStatus.Success, user);

        Assert.That(pipeline.Sha, Is.EqualTo(new Sha1(commit.Sha)));
    }

    [Test]
    public void Test_create_pipeline_with_invalid_ref_does_not_set_sha()
    {
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var commit = project.Repository.Commit(user, "test");

        var pipeline = project.Pipelines.Add("invalid_ref", JobStatus.Success, user);

        Assert.That(pipeline.Sha, Is.EqualTo(default(Sha1)));
    }
}
