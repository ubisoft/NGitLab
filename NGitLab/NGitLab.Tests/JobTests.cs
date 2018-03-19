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

        [Test, Timeout(5000)]
        public void Test_getjobs_all()
        {

        }

        [Test, Timeout(5000)]
        public void Test_getjobs_scope()
        {

        }

        [Test, Timeout(5000)]
        public void Test_run_action_play()
        {

        }

        [Test, Timeout(5000)]
        public void Test_run_action_retry()
        {

        }

        [Test, Timeout(5000)]
        public void Test_get_specific_jobid()
        {

        }

        [Test, Timeout(5000)]
        public void Test_get_job_trace()
        {

        }
    }
}