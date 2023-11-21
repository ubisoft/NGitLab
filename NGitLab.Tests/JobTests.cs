using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class JobTests
    {
        internal static void AddGitLabCiFile(IGitLabClient client, Project project, int jobCount = 1, bool manualAction = false, string branch = null, bool pipelineSucceeds = true)
        {
            var content = @"
variables:
  CI_DEBUG_TRACE: ""true""
";

            for (var i = 0; i < jobCount; i++)
            {
                content += $@"
build{i.ToString(CultureInfo.InvariantCulture)}:
  script:
    - echo test
    - echo test > file{i.ToString(CultureInfo.InvariantCulture)}.txt
    - exit {(pipelineSucceeds ? "0" : "1")}
  artifacts:
    paths:
      - '*.txt'
";

                if (manualAction)
                {
                    content += @"
  when: manual
";
                }
            }

            client.GetRepository(project.Id).Files.Create(new FileUpsert
            {
                Branch = branch ?? project.DefaultBranch,
                CommitMessage = "test",
                Path = ".gitlab-ci.yml",
                Content = content,
            });
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_getjobs_all()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            AddGitLabCiFile(context.Client, project);
            var jobs = await GitLabTestContext.RetryUntilAsync(() => context.Client.GetJobs(project.Id).GetJobs(JobScopeMask.Pending), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
            Assert.AreEqual(JobStatus.Pending, jobs.First().Status);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_getjobs_scope()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            AddGitLabCiFile(context.Client, project, manualAction: true);
            var jobs = await GitLabTestContext.RetryUntilAsync(() => context.Client.GetJobs(project.Id).GetJobs(JobScopeMask.Manual), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
            Assert.AreEqual(JobStatus.Manual, jobs.First().Status);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_getjobs_multiple_scopes()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);

            AddGitLabCiFile(context.Client, project, 2);
            var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Pending), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
            var job = jobs.First();

            jobsClient.RunAction(job.Id, JobAction.Cancel);
            jobs = await GitLabTestContext.RetryUntilAsync(() => context.Client.GetJobs(project.Id).GetJobs(
                new JobQuery
                {
                    Scope = JobScopeMask.Pending |
                            JobScopeMask.Canceled,
                }),
                jobs => jobs.Any(), TimeSpan.FromMinutes(2));

            Assert.IsTrue(jobs.First().Status == JobStatus.Canceled);
            Assert.IsTrue(jobs.Last().Status == JobStatus.Pending);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_run_action_play()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);
            AddGitLabCiFile(context.Client, project, manualAction: true);
            var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Manual), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
            var job = jobs.Single();
            Assert.AreEqual(JobStatus.Manual, job.Status);

            var playedJob = jobsClient.RunAction(job.Id, JobAction.Play);

            Assert.AreEqual(job.Id, playedJob.Id);
            Assert.AreEqual(job.Pipeline.Id, playedJob.Pipeline.Id);
            Assert.AreEqual(job.Commit.Id, playedJob.Commit.Id);
            Assert.AreEqual(JobStatus.Pending, playedJob.Status);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_run_action_retry()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);

            AddGitLabCiFile(context.Client, project);
            var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Pending), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
            var job = jobs.Single();

            jobsClient.RunAction(job.Id, JobAction.Cancel);
            await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Canceled), jobs => jobs.Any(), TimeSpan.FromMinutes(2));

            var retriedJob = jobsClient.RunAction(job.Id, JobAction.Retry);

            Assert.AreNotEqual(job.Id, retriedJob.Id);
            Assert.AreEqual(job.Pipeline.Id, retriedJob.Pipeline.Id);
            Assert.AreEqual(job.Commit.Id, retriedJob.Commit.Id);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_job_from_id()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);
            AddGitLabCiFile(context.Client, project);
            var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.All), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
            var job = jobs.Single();

            var job2 = jobsClient.Get(job.Id);

            Assert.AreEqual(job.Id, job2.Id); // Same Job
            Assert.AreEqual(job.Pipeline.Id, job2.Pipeline.Id); // Same Pipeline
            Assert.AreEqual(job.Commit.Id, job2.Commit.Id); // Same Commit
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_job_trace()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);
            using (await context.StartRunnerForOneJobAsync(project.Id))
            {
                AddGitLabCiFile(context.Client, project);
                var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.All), jobs =>
                {
                    var job = jobs.FirstOrDefault();
                    if (jobs.Any())
                    {
                        TestContext.WriteLine("Job status: " + job.Status);
                        return job.Status == JobStatus.Success || job.Status == JobStatus.Failed;
                    }

                    return false;
                }, TimeSpan.FromMinutes(2));

                var job = jobs.Single();
                var trace = jobsClient.GetTrace(job.Id);

                Assert.That(trace, Does.Contain("Running with gitlab-runner"));
                Assert.That(trace, Does.Contain("Job succeeded"));
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_job_artifacts()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);
            using (await context.StartRunnerForOneJobAsync(project.Id))
            {
                AddGitLabCiFile(context.Client, project);
                var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Success), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
                var job = jobs.Single();
                Assert.AreEqual(JobStatus.Success, job.Status);

                var artifacts = jobsClient.GetJobArtifacts(job.Id);

                Assert.IsNotEmpty(artifacts);
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_job_artifact()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);
            using (await context.StartRunnerForOneJobAsync(project.Id))
            {
                AddGitLabCiFile(context.Client, project);
                var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Success), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
                var job = jobs.Single();
                Assert.AreEqual(JobStatus.Success, job.Status);

                var artifact = jobsClient.GetJobArtifact(job.Id, "file0.txt");
                Assert.IsNotEmpty(artifact);

                var content = Encoding.ASCII.GetString(artifact).Trim();
                Assert.AreEqual("test", content);
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_job_artifact_query()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var jobsClient = context.Client.GetJobs(project.Id);
            using (await context.StartRunnerForOneJobAsync(project.Id))
            {
                AddGitLabCiFile(context.Client, project);
                var jobs = await GitLabTestContext.RetryUntilAsync(() => jobsClient.GetJobs(JobScopeMask.Success), jobs => jobs.Any(), TimeSpan.FromMinutes(2));
                var job = jobs.Single();
                Assert.AreEqual(JobStatus.Success, job.Status);

                var query = new JobArtifactQuery();
                query.RefName = project.DefaultBranch;
                query.JobName = job.Name;
                query.ArtifactPath = "file0.txt";

                var artifact = jobsClient.GetJobArtifact(query);
                Assert.IsNotEmpty(artifact);

                var content = Encoding.ASCII.GetString(artifact).Trim();
                Assert.AreEqual("test", content);
            }
        }
    }
}
