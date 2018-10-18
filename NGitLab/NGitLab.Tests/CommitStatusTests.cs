using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class CommitStatusTests
    {
        private Commit _commit;
        private ICommitStatusClient _commitStatus;
        private Project _project;
        private string _sha;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _project = Initialize.UnitTestProject;
            _commitStatus = Initialize.GitLabClient.GetCommitStatus(_project.Id);
            CommitsTests.EnableCiOnTestProject();

            var upsert = new FileUpsert
            {
                RawContent = "test",
                CommitMessage = "Commit for CommitStatusTests",
                Path = "CommitStatusTests.txt",
                Branch = "master",
            };

            Initialize.Repository.Files.Create(upsert);
            _commit = Initialize.Repository.Commits.FirstOrDefault(c => c.Message.Contains("CommitStatusTests"));

            Assert.AreEqual(upsert.CommitMessage, _commit.Message);
        }

        [Test]
        public void Test_get_commit_status()
        {
            Test_post_commit_status();
            var sha1 = _sha;
            var commitStatus = _commitStatus.AllBySha(sha1);
            Assert.IsNotNull(commitStatus.FirstOrDefault()?.Status);
        }

        [Test]
        public void Test_post_commit_status()
        {
            var commitStatus = SetupCommitStatus(state: "success");

            var createdCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            _sha = createdCommitStatus.CommitSha;

            Assert.AreEqual(commitStatus.Ref, createdCommitStatus.Ref);
            Assert.AreEqual(commitStatus.Coverage, createdCommitStatus.Coverage);
            Assert.AreEqual(commitStatus.Description, createdCommitStatus.Description);
            Assert.AreEqual(commitStatus.State, createdCommitStatus.Status);
            Assert.AreEqual(commitStatus.Name, createdCommitStatus.Name);
            Assert.AreEqual(commitStatus.TargetUrl, createdCommitStatus.TargetUrl);
            Assert.IsTrue(string.Equals(commitStatus.CommitSha, createdCommitStatus.CommitSha, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Test_post_commit_status_with_no_coverage()
        {
            var commitStatus = SetupCommitStatus(state: "success", coverage: null);

            var createdCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            _sha = createdCommitStatus.CommitSha;

            Assert.AreEqual(commitStatus.Ref, createdCommitStatus.Ref);
            Assert.AreEqual(commitStatus.Coverage, createdCommitStatus.Coverage);
            Assert.AreEqual(commitStatus.Description, createdCommitStatus.Description);
            Assert.AreEqual(commitStatus.State, createdCommitStatus.Status);
            Assert.AreEqual(commitStatus.Name, createdCommitStatus.Name);
            Assert.AreEqual(commitStatus.TargetUrl, createdCommitStatus.TargetUrl);
            Assert.IsTrue(string.Equals(commitStatus.CommitSha, createdCommitStatus.CommitSha, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_pending_to_running_to_success()
        {
            var commitStatus = SetupCommitStatus(state: "pending");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "running";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "success";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_pending_to_failed()
        {
            var commitStatus = SetupCommitStatus(state: "pending");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "failed";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_pending_to_canceled()
        {
            var commitStatus = SetupCommitStatus(state: "pending");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "canceled";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_success_to_pending()
        {
            var commitStatus = SetupCommitStatus(state: "success");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "pending";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_success_to_failed()
        {
            var commitStatus = SetupCommitStatus(state: "success");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "failed";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_success_to_canceled()
        {
            var commitStatus = SetupCommitStatus(state: "success");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "canceled";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_canceled_to_pending()
        {
            var commitStatus = SetupCommitStatus(state: "canceled");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "pending";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_canceled_to_running_to_failed()
        {
            var commitStatus = SetupCommitStatus(state: "canceled");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "running";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "failed";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_canceled_to_success()
        {
            var commitStatus = SetupCommitStatus(state: "canceled");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "success";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_canceled_to_failed()
        {
            var commitStatus = SetupCommitStatus(state: "canceled");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "failed";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_failed_to_pending()
        {
            var commitStatus = SetupCommitStatus(state: "failed");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "pending";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_failed_to_running_to_canceled()
        {
            var commitStatus = SetupCommitStatus(state: "failed");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "running";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "canceled";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_failed_to_success()
        {
            var commitStatus = SetupCommitStatus(state: "failed");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "success";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        [Test]
        public void Test_post_commit_status_and_update_it_from_failed_to_canceled()
        {
            var commitStatus = SetupCommitStatus(state: "failed");
            var createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);

            commitStatus.State = "canceled";
            createdOrUpdatedCommitStatus = _commitStatus.AddOrUpdate(commitStatus);
            Assert.AreEqual(commitStatus.State, createdOrUpdatedCommitStatus.Status);
        }

        private CommitStatusCreate SetupCommitStatus(string state, int? coverage = 100)
        {
            return new CommitStatusCreate
            {
                Ref = "master",
                CommitSha = _commit.Id.ToString(),
                Name = "Commit for CommitStatusTests",
                State = state,
                ProjectId = _project.Id,
                Description = "desc",
                Coverage = coverage,
                TargetUrl = "https://google.ca/"
            };
        }
    }
}
