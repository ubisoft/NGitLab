using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class UsersTests
    {
        private readonly IUserClient _users;

        public UsersTests()
        {
            _users = GitLabClient.Connect(Config.ServiceUrl, Config.Secret).Users;
        }

        [Test]
        public void Current()
        {
            var session = _users.Current;

            Assert.AreNotEqual(default(DateTime), session.CreatedAt);
            Assert.NotNull(session.Email);
            Assert.NotNull(session.Name);
            Assert.NotNull(session.PrivateToken);
            Assert.NotNull(session.PrivateToken);
        }

        [Test]
        public void GetUsers()
        {
            var users = _users.All.ToArray();

            CollectionAssert.IsNotEmpty(users);
        }

        [Test]
        public void GetUser()
        {
            var user = _users[1];

            Assert.AreEqual("user", user.Username);
            Assert.AreEqual(true, user.CanCreateGroup);
        }

        [Test]
        public void CreateUpdateDelete()
        {
            var u = new UserUpsert
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
                WebsiteURL = "wp.pl"
            };

            var addedUser = _users.Create(u);
            Assert.AreEqual(u.Bio, addedUser.Bio);

            u.Bio = "Bio2";
            u.Email = "test@test.pl";

            var updatedUser = _users.Update(addedUser.Id, u);
            Assert.AreEqual(u.Bio, updatedUser.Bio);

            _users.Delete(addedUser.Id);
        }
    }
}