using System;
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

        private static API CreateAPI()
        {
            return API.Connect(Config.ServiceUrl, Config.Secret);
        }
    }
}