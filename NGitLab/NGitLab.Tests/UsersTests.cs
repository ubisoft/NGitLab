using System;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests {
    public class UsersTests {
        readonly IUserClient _users;

        public UsersTests() {
            _users = Config.Connect().Users;
        }

        [Test]
        [Category("Server_Required")]
        public void Current() {
            var session = _users.Current();

            session.CreatedAt.ShouldNotBe(default(DateTime));
            session.Email.ShouldNotBeNull();
            session.Name.ShouldNotBeNull();
        }

        [Test]
        [Category("Server_Required")]
        public void GetUsers() {
            _users.All().ShouldNotBeEmpty();
        }

        [Test]
        [Category("Server_Required")]
        public void GetUser() {
            var user = _users.Get(1);

            Assert.AreNotEqual(string.Empty, user.Username);
            Assert.AreEqual(true, user.CanCreateGroup);
        }

        [Test]
        [Category("Server_Required")]
        public void CreateUpdateDelete() {
            var u = new UserUpsert {
                Email = "test@test.pl",
                Bio = "bio",
                CanCreateGroup = true,
                IsAdmin = true,
                Name = "sadfasdf",
                Password = "!@#$QWDRQW@",
                ProjectsLimit = 1000,
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