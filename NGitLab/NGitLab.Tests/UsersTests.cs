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
            _users = Config.Connect().Users;
        }

        [Test]
        public void Current()
        {
            var session = _users.Current;
            Assert.That(session, Is.Not.Null);
            Assert.That(session.CreatedAt, Is.EqualTo(new DateTime(2012, 05, 23, 08, 0, 58)));
            Assert.That(session.Email, Is.EqualTo("john@example.com"));
            Assert.That(session.Name, Is.EqualTo("John Smith"));
            Assert.That(session.PrivateToken, Is.Null);
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
            Assert.IsNotNull(user);
            Assert.That(user.Username, Is.EqualTo("john_smith"));
            Assert.That(user.CanCreateGroup, Is.True);
        }

        [Test]
        [Ignore(reason: "WIP")]
        public void CreateUpdateDelete()
        {
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
                WebsiteURL = "wp.pl"
            };

            var addedUser = _users.Create(userUpsert);
            Assert.That(addedUser.Bio, Is.EqualTo(userUpsert.Bio));

            userUpsert.Bio = "Bio2";
            userUpsert.Email = "test@test.pl";

            var updatedUser = _users.Update(addedUser.Id, userUpsert);
            Assert.That(updatedUser.Bio, Is.EqualTo(userUpsert.Bio));

            _users.Delete(addedUser.Id);
        }
    }
}