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

        private IMembersClient Members { get; } = Substitute.For<IMembersClient>();

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

            Members.OfProject("dummy").ReturnsForAnyArgs(call => GetMembersOfProject((string)call[0]));
            Members.OfNamespace("dummy").ReturnsForAnyArgs(call => new Membership[0]);
            Client.Members.Returns(call => Members);
        }

        public MockProject GetProject(int projectId)
        {
            return Projects.First(x => x.ClientProject.Id == projectId);
        }

        public MockProject GetProject(string fullName)
        {
            int projectId;
            if (int.TryParse(fullName, out projectId))
            {
                return GetProject(projectId);
            }

            fullName = fullName.Replace('\\', '/');
            var project = Projects.FirstOrDefault(x => x.FullName == fullName);

            if (project == null)
            {
                throw new Exception($"Project {fullName} was not found");
            }

            return project;
        }

        public MockProject CreateProject()
        {
            var project = new MockProject
            {
                Name = "DefaultName",
                Namespace = "DefaultNamespace",
            };

            Projects.Add(project);
            return project;
        }

        private IEnumerable<Membership> GetMembersOfProject(string projectName)
        {
            var project = GetProject(projectName);
            return project.Members;
        }
    }

    public class MockProject
    {
        public IRepositoryClient Repository { get; } = Substitute.For<IRepositoryClient>();

        public Project ClientProject { get; } = new Project
        {
            Id = ProjectIds.Next,
        };

        public List<Tag> Tags { get; } = new List<Tag>();

        public List<Commit> Commits { get; } = new List<Commit>();

        public List<Membership> Members { get; } = new List<Membership>();

        public string HttpUrl => $"https://mygitlabserver.org/{FullName}.git";

        public string FullName => $"{ClientProject.Namespace.Name}/{ClientProject.Name}";

        public string Name
        {
            get { return ClientProject.Name; }
            set
            {
                ClientProject.Name = value;
                ClientProject.HttpUrl = HttpUrl;
            }
        }

        public string Namespace
        {
            get { return ClientProject.Namespace.Name; }
            set
            {
                ClientProject.Namespace.Name = value;
                ClientProject.HttpUrl = HttpUrl;
            }
        }

        public MockProject()
        {
            ClientProject.Namespace = new Namespace { Name = "MockGroup" };
            ClientProject.Name = "MockProject";

            Repository.Commits.Returns(Commits);
            Repository.GetCommits("refname").ReturnsForAnyArgs(call => GetCommits((string)call[0]));
            Repository.Tags.Returns(Tags);
        }

        private IEnumerable<Commit> GetCommits(string refName)
        {
            var commit = Commits.FirstOrDefault(x => x.Id.ToString() == refName);
            if (commit == null)
            {
                throw new Exception($"Cannot find the requested ref {refName}");
            }

            int index = Commits.IndexOf(commit);
            return Commits.Take(index + 1).Reverse();
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
                Name = tagName,
                Commit = ToCommitInfo(commit)
            };

            Tags.Add(tag);
            return tag;
        }

        public void AddMember(string userName, AccessLevel accessLevel = AccessLevel.Developer)
        {
            Members.Add(new Membership
            {
                Id = MemberIds.Next,
                AccessLevel = (int)accessLevel,
                UserName = userName,
                Name = $"FullName of {userName}",
                CreatedAt = DateTime.Now
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
            var random = new Random();
            var result = new StringBuilder();
            for (int i = 0; i < 40; i++)
            {
                var @char = (char)random.Next('0', '9');
                result.Append(@char);
            }
            return new Sha1(result.ToString());
        }
    }

    internal class ProjectIds
    {
        private static int _currentId;
        public static int Next => ++_currentId;
    }

    internal class MemberIds
    {
        private static int _currentId;
        public static int Next => ++_currentId;
    }
}