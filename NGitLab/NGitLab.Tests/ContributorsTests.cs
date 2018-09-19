
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    internal class ContributorsTests
    {
        private ICommitClient _commitClient;
        private IUserClient _users;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _users = Initialize.GitLabClient.Users;
            _commitClient = Initialize.GitLabClient.GetCommits(Initialize.UnitTestProject.Id);
            _commitClient.Create(new CommitCreate()
            {
                AuthorName = _users.Current.Name,
                AuthorEmail = _users.Current.Email,
                Branch = "master",
                StartBranch = "master",
                ProjectId = Initialize.UnitTestProject.Id,
                CommitMessage = "test",
            });
        }

        [Test]
        public void Test_can_get_contributors()
        {
            var contributor = Contributors.All;
            Assert.IsNotNull(contributor);
            Assert.IsTrue(contributor.Any(x => x.Email== _users.Current.Email));
        }

        [Test]
        public void Test_can_get_MultipleContributors()
        {
            if (!Initialize.IsAdmin)
            {
                Assert.Inconclusive("Cannot test the creation of users since the current user is not admin");
            }

            var userUpsert = new UserUpsert
            {
                Email = "test@test.pl",
                Bio = "bio",
                CanCreateGroup = true,
                IsAdmin = true,
                Linkedin = null,
                Name = "sadfasdf",
                Password = "!@#$QWDRQW@",
                ProjectsLimit = 1000,
                Provider = "provider",
                Skype = "skype",
                Twitter = "twitter",
                Username = "username",
                WebsiteURL = "wp.pl",
            };

            var addedUser =_users.Create(userUpsert);
            _commitClient.Create(new CommitCreate()
            {
                AuthorName = userUpsert.Name,
                AuthorEmail = userUpsert.Email,
                Branch = "master",
                StartBranch = "master",
                ProjectId = Initialize.UnitTestProject.Id,
                CommitMessage = "test",
            });

            var contributor = Contributors.All;

            Assert.IsNotNull(contributor);
            Assert.IsTrue(contributor.Any(x => x.Email == _users.Current.Email));
            Assert.IsTrue(contributor.Any(x => x.Email == userUpsert.Email));

            _users.Delete(addedUser.Id);
        }

        private IContributorClient Contributors
        {
            get
            {
                Assert.IsNotNull(Initialize.UnitTestProject);
                return Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Contributors;
            }
        }
    }
}
