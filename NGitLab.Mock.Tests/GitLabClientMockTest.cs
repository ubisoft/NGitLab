using System;
using System.Collections;
using System.Linq;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;


public class GitLabClientMockTest
{
    public static IEnumerable TestCases
    {
        get
        {
            static TestCaseData TestCase(string name, Func<IGitLabClient, ProjectId, object> getClient) => new TestCaseData(getClient).SetArgDisplayNames(name);

            // Enumerate all methods on IGitLabClient that accept a single ProjectId parameter

            var methods = typeof(IGitLabClient).GetMethods()
                .Where(method => method.IsPublic)
                .Where(method => method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(ProjectId));

            foreach (var method in methods)
            {
                yield return TestCase(method.Name, (client, id) => method.Invoke(client, new object[] { id }));
            }

        }
    }

    [TestCaseSource(nameof(TestCases))]
    public void Test_can_get_project_client(Func<IGitLabClient, ProjectId, object> getClient)
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("test-group")
            .WithProject("test-project", id: 1, @namespace: "test-group")
            .BuildServer();

        var client = server.CreateClient();

        Assert.Multiple(() =>
        {
            Assert.That(getClient(client, 1), Is.Not.Null);
            Assert.That(getClient(client, "test-group/test-project"), Is.Not.Null);
        });
    }
}
