using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class NamespacesTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_namespaces_contains_a_group()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        var namespacesClient = context.Client.Namespaces;

        var groupSearch = namespacesClient.Accessible.FirstOrDefault(g => g.Path.Equals(group.Path, StringComparison.Ordinal));
        Assert.That(group, Is.Not.Null);
        Assert.That(groupSearch.GetKind(), Is.EqualTo(Namespace.Type.Group));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_namespaces_contains_a_user()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var namespacesClient = context.Client.Namespaces;

        var user = namespacesClient.Accessible.FirstOrDefault(g => g.Path.Equals(context.Client.Users.Current.Username, StringComparison.Ordinal));
        Assert.That(user, Is.Not.Null);
        Assert.That(user.GetKind(), Is.EqualTo(Namespace.Type.User));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_namespaces_search_for_user()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var namespacesClient = context.Client.Namespaces;

        var ns = namespacesClient.Search(context.Client.Users.Current.Username).First();
        Assert.That(ns.GetKind(), Is.EqualTo(Namespace.Type.User));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_namespaces_search_for_group()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        var namespacesClient = context.Client.Namespaces;

        var user = namespacesClient.Search(group.Name);
        Assert.That(user, Is.Not.Null);
    }
}
