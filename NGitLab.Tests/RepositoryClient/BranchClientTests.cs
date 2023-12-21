using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient;

public class BranchClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task GetAll()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branches = context.Client.GetRepository(project.Id).Branches;

        Assert.That(branches.All.ToArray(), Is.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetByName()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branches = context.Client.GetRepository(project.Id).Branches;

        var branch = branches[project.DefaultBranch];

        Assert.That(branch, Is.Not.Null);
        Assert.That(branch.Name, Is.Not.Null);
        Assert.That(branch.Default, Is.True);
    }

    [Test]
    [NGitLabRetry]
    public async Task AddDelete()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branches = context.Client.GetRepository(project.Id).Branches;

        var branchName = $"merge-me-to-{project.DefaultBranch}_{Path.GetRandomFileName()}";

        branches.Create(new BranchCreate
        {
            Name = branchName,
            Ref = project.DefaultBranch,
        });

        var branch = branches[branchName];
        Assert.That(branch, Is.Not.Null);
        Assert.That(branch.Default, Is.False);

        branches.Protect(branchName);

        branches.Unprotect(branchName);

        branches.Delete(branchName);

        AssertCannotFind(() => branches[branchName]);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_that_branch_names_containing_slashes_are_supported()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branches = context.Client.GetRepository(project.Id).Branches;

        var branchName = "feature/addNewStuff/toto";

        branches.Create(new BranchCreate
        {
            Name = branchName,
            Ref = project.DefaultBranch,
        });

        Assert.That(branches[branchName], Is.Not.Null);

        branches.Protect(branchName);

        branches.Unprotect(branchName);

        branches.Delete(branchName);

        AssertCannotFind(() => branches[branchName]);
    }

    private static void AssertCannotFind<T>(Func<T> get)
    {
        try
        {
            var dummyResult = get();
            Assert.Fail($"Not supposed to return {dummyResult}, should throw a 404 exception instead.");
        }
        catch (GitLabException ex)
        {
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
