using System;
using System.Collections.Generic;
using System.Globalization;
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
                                FilePath =  // Spread files among the root directory and its 'subfolder'
                                    i % 2 == 0 ?
                                    $"test{i.ToString(CultureInfo.InvariantCulture)}.md" :
                                    $"subfolder/test{i.ToString(CultureInfo.InvariantCulture)}.md",
                            },
                        },
                    });
                }

                return new RepositoryClientTestsContext(context, project, commits, repositoryClient);
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task GetAllCommits()
        {
            const int commitCount = 5;
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount);

            var commits = context.RepositoryClient.Commits.ToArray();

            Assert.AreEqual(commitCount, commits.Length);
            CollectionAssert.AreEqual(context.Commits.Select(c => c.Message).Reverse(), commits.Select(c => c.Message));
        }

        [Test]
        [NGitLabRetry]
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
        [NGitLabRetry]
        public async Task GetCommitBySha1()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var sha1 = context.Commits[0].Id;
            var commit = context.RepositoryClient.GetCommit(sha1);
            Assert.AreEqual(sha1, commit.Id);
            Assert.AreEqual(context.Commits[0].Message, commit.Message);
            Assert.AreEqual(context.Commits[0].WebUrl, commit.WebUrl);
            Assert.NotNull(commit.WebUrl);
        }

        [Test]
        [NGitLabRetry]
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
        [NGitLabRetry]
        public async Task GetCommitDiff()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            CollectionAssert.IsNotEmpty(context.RepositoryClient.GetCommitDiff(context.RepositoryClient.Commits.First().Id).ToArray());
        }

        [TestCase(4)]
        [TestCase(11)]
        [NGitLabRetry]
        public async Task GetAllTreeObjectsAtRoot(int commitCount)
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount);

            var treeObjects = context.RepositoryClient.GetTree(string.Empty).ToList();

            var expectedFileCount = (int)Math.Ceiling(commitCount / 2.0);
            var expectedDirCount = 1;

            Assert.AreEqual(expectedFileCount, treeObjects.Count(t => t.Type == ObjectType.blob));
            Assert.AreEqual(expectedDirCount, treeObjects.Count(t => t.Type == ObjectType.tree));
        }

        [TestCase(4)]
        [TestCase(11)]
        [NGitLabRetry]
        public async Task GetAllTreeObjectsRecursivelyFromRoot(int commitCount)
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount);

            var treeObjects = context.RepositoryClient.GetTree(string.Empty, @ref: null, recursive: true).ToList();

            var expectedFileCount = commitCount;
            var expectedDirCount = 1;

            Assert.AreEqual(expectedFileCount, treeObjects.Count(t => t.Type == ObjectType.blob));
            Assert.AreEqual(expectedDirCount, treeObjects.Count(t => t.Type == ObjectType.tree));
        }

        [TestCase(4)]
        [TestCase(11)]
        [NGitLabRetry]
        public async Task GetAllTreeObjectsRecursivelyFromRootAsync(int commitCount)
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount);

            var treeObjects = new List<Tree>();
            await foreach (var item in context.RepositoryClient.GetTreeAsync(new RepositoryGetTreeOptions { Path = string.Empty, Ref = null, Recursive = true }))
            {
                treeObjects.Add(item);
            }

            var expectedFileCount = commitCount;
            var expectedDirCount = 1;

            Assert.AreEqual(expectedFileCount, treeObjects.Count(t => t.Type == ObjectType.blob));
            Assert.AreEqual(expectedDirCount, treeObjects.Count(t => t.Type == ObjectType.tree));
        }

        [Test]
        [NGitLabRetry]
        public async Task GetAllTreeObjectsInPathOnRef()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var treeObjects = context.RepositoryClient.GetTree(string.Empty, context.Project.DefaultBranch, recursive: false);
            Assert.IsNotEmpty(treeObjects);
        }

        [Test]
        [NGitLabRetry]
        public async Task GetAllTreeObjectsInPathWith100ElementsByPage()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var treeObjects = context.RepositoryClient.GetTree(new RepositoryGetTreeOptions { Path = string.Empty, PerPage = 100 });
            Assert.IsNotEmpty(treeObjects);
        }

        [Test]
        [NGitLabRetry]
        public async Task GetAllTreeObjectsAtInvalidPath()
        {
            using var context = await RepositoryClientTestsContext.CreateAsync(commitCount: 2);

            var treeObjects = context.RepositoryClient.GetTree("Fakepath");
            Assert.IsEmpty(treeObjects);
        }

        [TestCase(CommitRefType.All)]
        [TestCase(CommitRefType.Branch)]
        [TestCase(CommitRefType.Tag)]
        [NGitLabRetry]
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
