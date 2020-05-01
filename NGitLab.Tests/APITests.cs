using NGitLab.Impl;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class APITests
    {
        [Test]
        public void Test_the_exception_does_not_contain_the_password_on_connection_error()
        {
            var credentials = new GitLabCredentials(Initialize.GitLabHost, "invalidUser", "myInvalidPassword");
            var api = new API(credentials);
            var exception = Assert.Throws<GitLabException>(() => api.Get());

            Assert.That(exception.Message, Does.Not.Contain(credentials.Password));
            Assert.That(exception.OriginalCall.ToString(), Does.Not.Contain(credentials.Password));
        }
    }
}