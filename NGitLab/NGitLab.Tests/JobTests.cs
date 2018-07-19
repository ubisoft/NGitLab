using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class JobTests
    {
        private IJobClient _jobs;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _jobs = Initialize.GitLabClient.GetJobs(Initialize.UnitTestProject.Id);
            CommitsTests.EnableCiOnTestProject();
        }

        private Job GetTestJob(JobScopeMask jobMask)
        {
            while (true)
            {
                Job job = _jobs.GetJobs(jobMask).FirstOrDefault();
                if (job != null)
                    return job;
            }
        }

        [Test, Timeout(5000)]
        public void Test_getjobs_all()
        {
            // Rely on test timeout if no job are found
            GetTestJob(JobScopeMask.All);
        }

        [Test, Timeout(5000)]
        public void Test_getjobs_scope()
        {
            // Rely on test timeout if no job are found
            GetTestJob(JobScopeMask.Manual);
        }

        [Test, Timeout(5000)]
        public void Test_run_action_play()
        {
            // Rely on test timeout if no job are found
            Job job = GetTestJob(JobScopeMask.Manual);

            Job job2 = _jobs.RunAction(job.Id, JobAction.Play);

            Assert.AreEqual(job2.Id, job.Id); // Same Job
            Assert.AreEqual(job2.Pipeline.Id, job.Pipeline.Id); // Same Pipeline
            Assert.AreEqual(job2.Commit.Id, job.Commit.Id); // Same Commit
        }

        [Test, Timeout(5000)]
        public void Test_run_action_retry()
        {
            // Rely on test timeout if no job are found
            Job job = GetTestJob(JobScopeMask.Success);

            Job job2 = _jobs.RunAction(job.Id, JobAction.Retry);

            Assert.AreNotEqual(job2.Id, job.Id); // New job is created
            Assert.AreEqual(job2.Pipeline.Id, job.Pipeline.Id); // Same Pipeline
            Assert.AreEqual(job2.Commit.Id, job.Commit.Id); // Same Commit
        }

        [Test, Timeout(5000 * 60)] // The job must be took by the runner and completed, sometime it takes multiples minutes
        public void Test_get_job_from_id()
        {
            // Rely on test timeout if no job are found
            Job job = GetTestJob(JobScopeMask.Manual);

            Job job2 = _jobs.Get(job.Id);

            Assert.AreEqual(job2.Id, job.Id); // Same Job
            Assert.AreEqual(job2.Pipeline.Id, job.Pipeline.Id); // Same Pipeline
            Assert.AreEqual(job2.Commit.Id, job.Commit.Id); // Same Commit
        }

        [Test, Timeout(5000 * 60)] // The job must be took by the runner and completed, sometime it takes multiples minutes
        public void Test_get_job_trace()
        {
            // Rely on test timeout if no job are found
            Job job = GetTestJob(JobScopeMask.Success);

            string trace = _jobs.GetTrace(job.Id);

            Assert.IsTrue(trace.Contains("Running with gitlab-runner"));
            Assert.IsTrue(trace.Contains("Job succeeded"));
        }

        [Test, Timeout(5000 * 60)] // The job must be took by the runner and completed, sometime it takes multiples minutes
        public void Test_get_job_artifacts()
        {
            // Rely on test timeout if no job are found
            Job job = GetTestJob(JobScopeMask.Success);

            byte[] artifacts = _jobs.GetJobArtifacts(job.Id);

            Assert.IsNotEmpty(artifacts);
        }
    }
}