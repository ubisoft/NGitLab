using System;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class UsersTests
    {
        private readonly GitLabClient _client;

        public UsersTests()
        {
            _client = GitLabClient.Connect(Config.ServiceUrl, Config.Secret);
        }

        [Test]
        public void Current()
        {
            var session = _client.Users.Current;

            Assert.AreNotEqual(default(DateTime), session.CreatedAt);
            Assert.NotNull(session.Email);
            Assert.NotNull(session.Name);
            Assert.NotNull(session.PrivateToken);
            Assert.NotNull(session.PrivateToken);
        }

        [Test]
        public void GetUsers()
        {
            var users = _client.Users.All;

            CollectionAssert.IsNotEmpty(users);
        }

        [Test]
        public void GetUser()
        {
            var user = _client.Users[1];

            Assert.AreEqual("user", user.Username);
            Assert.AreEqual(true, user.CanCreateGroup);
        }
    }
}