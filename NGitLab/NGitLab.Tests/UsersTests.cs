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
            _users = Initialize.GitLabClient.Users;
        }

        [Test]
        public void Current()
        {
            var session = _users.Current;
            Assert.That(session, Is.Not.Null);
            Assert.That(session.CreatedAt.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(session.Email, Is.EqualTo("admin@example.com"));
            Assert.That(session.Name, Is.EqualTo("Administrator"));
            Assert.That(session.PrivateToken, Is.Not.Null);
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
            Assert.That(user.Username, Is.EqualTo("root"));
            Assert.That(user.CanCreateGroup, Is.True);
        }

        [Test, Ignore("Needs admin rights")]
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

        [Test]
        [Ignore("Do not run automatically to not modify the current user.")")]
        public void Test_can_add_an_ssh_key_to_the_gitlab_profile()
        {
            var users = _users;
            var keys = users.CurrentUserSShKeys;
            var keysBefore = keys.All.ToArray();

            var result = keys.Add(new SshKeyCreate
            {
                Key = "ssh-rsa dummy mytestkey@mycurrentpc",
                Title = "test key",
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(keysBefore.Length + 1, keys.All.ToArray().Length);

            keys.Remove(result.Id);
            Assert.AreEqual(keysBefore.Length, keys.All.ToArray().Length);
        }
    }
}