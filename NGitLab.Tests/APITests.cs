using System.Threading.Tasks;
using NGitLab.Impl;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class APITests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_the_exception_does_not_contain_the_password_on_connection_error()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var credentials = new GitLabCredentials(context.DockerContainer.GitLabUrl.ToString(), "invalidUser", "myInvalidPassword");
        var api = new API(credentials);
        var exception = Assert.Throws<GitLabException>(() => api.Get());

        Assert.That(exception.Message, Does.Not.Contain(credentials.Password));
        Assert.That(exception.OriginalCall.ToString(), Does.Not.Contain(credentials.Password));
    }
}
