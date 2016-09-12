using MethodTimer;
using NUnit.Framework;
using System;

namespace NGitLab.Tests
{
    public class IntegrationTests
    {
        GitLabClient _gitlabClient;

        [OneTimeSetUp]
        public void Initialize()
        {
            var host = Environment.GetEnvironmentVariable("GITLAB_HOST");
            var token = Environment.GetEnvironmentVariable("GITLAB_TOKEN");

            Assert.That(host, Is.Not.Null);
            Assert.That(host, Is.Not.Empty);

            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);

            _gitlabClient = GitLabClient.Connect(host, token);
        }

        [Test]
        [Time]
        public void Integration_Current()
        {
            var session = _gitlabClient.Users.Current;
            Assert.That(session, Is.Not.Null);
            Assert.That(session.CreatedAt.Date, Is.EqualTo(new DateTime(2016, 08, 24)));
        }
    }
}
