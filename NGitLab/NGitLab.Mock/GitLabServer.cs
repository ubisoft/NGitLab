using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGitLab.Models;
using NSubstitute;

namespace NGitLab.Mock
{
    /// <summary>
    /// Starting point of your mock, instantiate GitLab client
    /// and add fake content to it.
    /// </summary>
    public class GitLabServer
    {
        /// <summary>
        /// Use this client to run queries on your mock server.
        /// </summary>
        public IGitLabClient Client { get; } = Substitute.For<IGitLabClient>();

        private IProjectClient ProjectClient { get; } = Substitute.For<IProjectClient>();

        public List<MockProject> Projects { get; } = new List<MockProject>();
        
        public GitLabServer()
        {
            Client.Projects.Returns(ProjectClient);
            ProjectClient.Accessible.Returns(call => Projects.Select(x => x.ClientProject));
            ProjectClient.Owned.Returns(call => Projects.Select(x => x.ClientProject));
            ProjectClient.All.Returns(call => Projects.Select(x => x.ClientProject));
            ProjectClient[-1].ReturnsForAnyArgs(call => GetProject((int)call[0]).ClientProject);
            ProjectClient["anyName"].ReturnsForAnyArgs(call => GetProject((string)call[0]).ClientProject);

            Client.GetRepository(-1).ReturnsForAnyArgs(call => GetProject((int)call[0]).Repository);
        }

        public MockProject GetProject(int projectId)
        {
            return Projects.First(x => x.ClientProject.Id == projectId);
        }

        public MockProject GetProject(string fullName)
        {
            fullName = fullName.Replace('\\', '/');
            return Projects.First(x => x.FullName == fullName);
        }

        public MockProject CreateProject()
        {
            var project = new MockProject();
            Projects.Add(project);
            return project;
        }
    }

    public class MockProject
    {
        public IRepositoryClient Repository { get; } = Substitute.For<IRepositoryClient>();

        public IMembersClient MembersClient { get; } = Substitute.For<IMembersClient>();

        public Project ClientProject { get; } = new Project();

        public List<Tag> Tags { get; } = new List<Tag>();

        public List<Commit> Commits { get; } = new List<Commit>();

        public List<Membership> Members { get; } = new List<Membership>();

        public string FullName => $"{ClientProject.Namespace.Name}/{ClientProject.Name}";

        public string Name
        {
            get { return ClientProject.Name; }
            set { ClientProject.Name = value; }
        }

        public string Namespace
        {
            get { return ClientProject.Namespace.Name; }
            set { ClientProject.Namespace.Name = value; }
        }

        public MockProject()
        {
            ClientProject.Namespace = new Namespace {Name = "MockGroup"};
            ClientProject.Name = "MockProject";

            Repository.Commits.Returns(Commits);
            Repository.Tags.Returns(Tags);
        }

        public Commit Commit(string author = "John Doe")
        {
            var commit = new Commit
            {
                Id = RandomSha1(),
                AuthorName = author,
                CreatedAt = DateTime.Now
            };

            Commits.Add(commit);
            return commit;
        }

        /// <summary>
        /// Tag a commit (or the latest if not specified)
        /// </summary>
        public Tag Tag(string tagName = "v1.1", Commit commit = null)
        {
            commit = commit ?? Commits.Last();

            var tag = new Tag
            {
                Commit = ToCommitInfo(commit)
            };

            Tags.Add(tag);
            return tag;
        }

        public void AddMember(string userName, AccessLevel accessLevel = AccessLevel.Developer)
        {
            Members.Add(new Membership
            {
                AccessLevel = (int)accessLevel,
                UserName = userName,
            });
        }

        private CommitInfo ToCommitInfo(Commit commit)
        {
            return new CommitInfo
            {
                Id = commit.Id,
                Message = commit.Message,
                CommittedDate = commit.CreatedAt
            };
        }

        private Sha1 RandomSha1()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 40; i++)
            {
                var @char = (char)new Random().Next('0', '9');
                result.Append(@char);
            }
            return new Sha1(result.ToString());
        }
    }
}
