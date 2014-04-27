using System;
using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class APITests
    {
        [Test]
        public void GetCurrentSession()
        {
            var api = CreateAPI();

            var session = api.GetCurrentSession();

            Assert.AreNotEqual(default(DateTime), session.CreatedAt);
            Assert.NotNull(session.Email);
            Assert.NotNull(session.Name);
            Assert.NotNull(session.PrivateToken);
            Assert.NotNull(session.PrivateToken);
        }

        [Test]
        public void GetUsers()
        {
            var api = CreateAPI();

            var users = api.GetUsers();

            CollectionAssert.IsNotEmpty(users);
        }

        [Test]
        public void GetProjects()
        {
            var api = CreateAPI();

            var projects = api.GetProjects();

            Assert.GreaterOrEqual(projects.Count(), 21);
        }


        private static API CreateAPI()
        {
            return API.Connect(Config.ServiceUrl, Config.Secret);
        }
    }
}