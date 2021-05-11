using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class RepositoryClientTests
    {
        private sealed class RepositoryClientTestsContext : IDisposable
        {
            public RepositoryClientTestsContext(GitLabTestContext context, Project project, Commit[] commits, IRepositoryClient repositoryClient)
            {
                Context = context;
                Project = project;
                Commits = commits;
                RepositoryClient = repositoryClient;
            }

            public GitLabTestContext Context { get; }

            public Project Project { get; }

            public Commit[] Commits { get; }

            public IRepositoryClient RepositoryClient { get; }

            public void Dispose()
            {
                Context.Dispose();
            }

            public static async Task<RepositoryClientTestsContext> CreateAsync(int commitCount)
            {
                var context = await GitLabTestContext.CreateAsync().ConfigureAwait(false);
                var project = context.CreateProject();
                var repositoryClient = context.Client.GetRepository(project.Id);

                var commits = new Commit[commitCount];
                for (var i = 0; i < commits.Length; i++)
                {
                    commits[i] = context.Client.GetCommits(project.Id).Create(new CommitCreate
                    {
                        Branch = project.DefaultBranch,
                        CommitMessage = context.GetUniqueRandomString(),
                        AuthorEmail = "a@example.com",
                        AuthorName = "a",
                        ProjectId = project.Id,
                        Actions =
                        {
                            new CreateCommitAction
                            {
                                Action = "create",
                                Content = $"test",
                                FilePath = $"test{i}.md",
                            },
                        },
                    });
                }

                return new RepositoryClientTestsContext(context, project, commits, repositoryClient);
            }
        }

        [Test]
        public async Task GetAllCommits()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);
            var commits = context.RepositoryClient.Commits.ToArray();
            Assert.AreEqual(2, commits.Length);
            Assert.AreEqual(context.Commits[1].Message, commits[0].Message);
            Assert.AreEqual(context.Commits[0].Message, commits[1].Message);
        }

        [Test]
        public async Task GetCommitByBranchName()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);
            var defaultBranch = context.Project.DefaultBranch;

            CollectionAssert.IsNotEmpty(context.RepositoryClient.GetCommits(defaultBranch));
            CollectionAssert.IsNotEmpty(context.RepositoryClient.GetCommits(defaultBranch, -1));

            var commits = context.RepositoryClient.GetCommits(defaultBranch, 1).ToArray();
            Assert.AreEqual(1, commits.Length);
            Assert.AreEqual(context.Commits[1].Message, commits[0].Message);
        }

        [Test]
        public async Task GetCommitBySha1()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var sha1 = context.Commits[0].Id;
            var commit = context.RepositoryClient.GetCommit(sha1);
            Assert.AreEqual(sha1, commit.Id);
            Assert.AreEqual(context.Commits[0].Message, commit.Message);
        }

        [Test]
        public async Task GetCommitBySha1Range()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 5);

            var allCommits = context.RepositoryClient.Commits.Reverse().ToArray();
            var commitRequest = new GetCommitsRequest
            {
                RefName = $"{allCommits[1].Id}..{allCommits[3].Id}",
                FirstParent = true,
            };

            var commits = context.RepositoryClient.GetCommits(commitRequest).Reverse().ToArray();
            Assert.AreEqual(allCommits[2].Id, commits[0].Id);
            Assert.AreEqual(allCommits[3].Id, commits[1].Id);
        }

        [Test]
        public async Task GetCommitDiff()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            CollectionAssert.IsNotEmpty(context.RepositoryClient.GetCommitDiff(context.RepositoryClient.Commits.First().Id).ToArray());
        }

        [Test]
        public async Task GetAllTreeInPath()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var tree = context.RepositoryClient.GetTree(string.Empty);
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public async Task GetAllTreeInPathRecursively()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var tree = context.RepositoryClient.GetTree(string.Empty, @ref: null, recursive: true);
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public async Task GetAllTreeInPathOnRef()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var tree = context.RepositoryClient.GetTree(string.Empty, context.Project.DefaultBranch, recursive: false);
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public async Task GetAllTreeInPathWith100ElementsByPage()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var tree = context.RepositoryClient.GetTree(new RepositoryGetTreeOptions { Path = string.Empty, PerPage = 100 });
            Assert.IsNotEmpty(tree);
        }

        [Test]
        public async Task GetAllTreeInNotGoodPath()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var tree = context.RepositoryClient.GetTree("Fakepath");
            Assert.IsEmpty(tree);
        }

        [TestCase(CommitRefType.All)]
        [TestCase(CommitRefType.Branch)]
        [TestCase(CommitRefType.Tag)]
        public async Task GetCommitRefs(CommitRefType type)
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var commitRefs = context.RepositoryClient.GetCommitRefs(context.RepositoryClient.Commits.First().Id, type).ToArray();

            if (type == CommitRefType.Tag)
            {
                CollectionAssert.IsEmpty(commitRefs);
            }
            else
            {
                CollectionAssert.IsNotEmpty(commitRefs);
            }
        }
    }
}
