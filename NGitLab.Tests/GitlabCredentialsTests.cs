using System;
using NGitLab.Impl;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class GitlabCredentialsTests
    {
        [Test]
        [TestCase("https://test/api/v3")]
        [TestCase("https://test/api/v3/")]
        public void Constructor_should_reject_apiv3(string url)
        {
            Assert.Throws<ArgumentException>(() => new GitLabCredentials(url, "my_token"));
        }

        [Test]
        [TestCase("https://test", "https://test/api/v4")]
        [TestCase("https://test/", "https://test/api/v4")]
        [TestCase("https://test/api/v4", "https://test/api/v4")]
        [TestCase("https://test/api/v4/", "https://test/api/v4")]
        public void Constructor_should_complete_api_version_when_not_set(string url, string expectedUrl)
        {
            var gitLabCredentials = new GitLabCredentials(url, "my_token");
            Assert.AreEqual(expectedUrl, gitLabCredentials.HostUrl);
        }
    }
}
