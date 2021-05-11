using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class CommitStatusTests
    {
        private sealed class CommitStatusTestContext : IDisposable
        {
            public GitLabTestContext Context { get; }

            public Project Project { get; }

            public Commit Commit { get; }

            public ICommitStatusClient CommitStatusClient { get; }

            public CommitStatusTestContext(GitLabTestContext context, Project project, Commit commit, ICommitStatusClient commitStatusClient)
            {
                Context = context;
                Project = project;
                Commit = commit;
                CommitStatusClient = commitStatusClient;
            }

            public void Dispose()
            {
                Context.Dispose();
            }

            public static async Task<CommitStatusTestContext> Create()
            {
                var context = await GitLabTestContext.CreateAsync().ConfigureAwait(false);
                var project = context.CreateProject();

                var upsert = new FileUpsert
                {
                    RawContent = "test",
                    CommitMessage = "Commit for CommitStatusTests",
                    Path = "CommitStatusTests.txt",
                    Branch = project.DefaultBranch,
                };
                context.Client.GetRepository(project.Id).Files.Create(upsert);
                var commit = context.Client.GetCommits(project.Id).GetCommit(upsert.Branch);
                var client = context.Client.GetCommitStatus(project.Id);
                return new CommitStatusTestContext(context, project, commit, client);
            }

            public CommitStatusCreate AddOrUpdateCommitStatus(string state = "success", int? coverage = null)
            {
                var commitStatus = SetupCommitStatus(state, coverage);

                var createdCommitStatus = CommitStatusClient.AddOrUpdate(commitStatus);

                Assert.AreEqual(commitStatus.Ref, createdCommitStatus.Ref);
                Assert.AreEqual(commitStatus.Coverage, createdCommitStatus.Coverage);
                Assert.AreEqual(commitStatus.Description, createdCommitStatus.Description);
                Assert.AreEqual(commitStatus.State, createdCommitStatus.Status);
                Assert.AreEqual(commitStatus.Name, createdCommitStatus.Name);
                Assert.AreEqual(commitStatus.TargetUrl, createdCommitStatus.TargetUrl);
                Assert.IsTrue(string.Equals(commitStatus.CommitSha, createdCommitStatus.CommitSha, StringComparison.OrdinalIgnoreCase));

                return createdCommitStatus;
            }

            private CommitStatusCreate SetupCommitStatus(string state, int? coverage = 100)
            {
                return new CommitStatusCreate
                {
                    Ref = Project.DefaultBranch,
                    CommitSha = Commit.Id.ToString(),
                    Name = "Commit for CommitStatusTests",
                    State = state,
                    ProjectId = Project.Id,
                    Description = "desc",
                    Coverage = coverage,
                    TargetUrl = "https://google.ca/",
                };
            }
        }

        [Test]
        public async Task Test_get_commit_status()
        {
            using var context = await CommitStatusTestContext.Create();
            var createdCommitStatus = context.AddOrUpdateCommitStatus();

            var commitStatus = context.CommitStatusClient.AllBySha(context.Commit.Id.ToString().ToLowerInvariant()).ToList();
            Assert.IsNotNull(commitStatus.FirstOrDefault()?.Status);
        }

        [Test]
        public async Task Test_post_commit_status_with_no_coverage()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(coverage: null);

            Assert.AreEqual(commitStatus.Coverage, null);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_pending_to_running_to_success()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
            Assert.AreEqual("pending", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "running");
            Assert.AreEqual("running", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "success");
            Assert.AreEqual("success", commitStatus.Status);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_pending_to_failed()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
            Assert.AreEqual("pending", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "failed");
            Assert.AreEqual("failed", commitStatus.Status);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_pending_to_canceled()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
            Assert.AreEqual("pending", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "canceled");
            Assert.AreEqual("canceled", commitStatus.Status);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_success_to_pending()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "success");
            Assert.AreEqual("success", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
            Assert.AreEqual("pending", commitStatus.Status);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_success_to_failed()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "success");
            Assert.AreEqual("success", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "failed");
            Assert.AreEqual("failed", commitStatus.Status);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_success_to_canceled()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "success");
            Assert.AreEqual("success", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "canceled");
            Assert.AreEqual("canceled", commitStatus.Status);
        }

        [Test]
        public async Task Test_post_commit_status_and_update_it_from_canceled_to_pending()
        {
            using var context = await CommitStatusTestContext.Create();
            var commitStatus = context.AddOrUpdateCommitStatus(state: "canceled");
            Assert.AreEqual("canceled", commitStatus.Status);

            commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
            Assert.AreEqual("pending", commitStatus.Status);
        }
    }
}
