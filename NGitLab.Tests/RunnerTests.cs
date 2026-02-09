using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NuGet.Versioning;
using NUnit.Framework;
using Polly;
using Polly.Retry;

namespace NGitLab.Tests;

public class RunnerTests
{
    /// <remarks>
    /// <para>Project runner ownership is enforced on v18.</para>
    /// <para>It is not possible to unassign a runner from the owner project. Runner should be deleted instead.</para>
    /// <para>See <see href="https://docs.gitlab.com/ci/runners/runners_scope/#project-runner-ownership"/>.</para>
    /// <para>See <see cref="Test_can_enable_disable_and_delete_a_runner_on_projects"/> for a scenario with the runner deletion on owner project.</para>
    /// <para>See <see cref="Test_cannot_disable_runner_on_owner_project"/> for a scenario that validates the restriction on owner project.</para>
    /// </remarks>
    [Test]
    [NGitLabRetry]
    public async Task Test_can_enable_and_disable_a_runner_on_a_project()
    {
        // We need 2 projects associated to a runner to disable a runner
        using var context = await GitLabTestContext.CreateAsync();
        context.IgnoreTestIfGitLabVersionOutOfRange(VersionRange.Parse("[,18.0)"));

        var project1 = context.CreateProject(initializeWithCommits: true);
        var project2 = context.CreateProject(initializeWithCommits: true);

        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = project1.RunnersToken });
        runnersClient.EnableRunner(project2.Id, new RunnerId(runner.Id));

        runnersClient.DisableRunner(project1.Id, new RunnerId(runner.Id));
        Assert.That(IsEnabled(), Is.False);

        runnersClient.EnableRunner(project1.Id, new RunnerId(runner.Id));
        Assert.That(IsEnabled(), Is.True);

        bool IsEnabled() => runnersClient[runner.Id].Projects.Any(x => x.Id == project1.Id);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_enable_disable_and_delete_a_runner_on_projects()
    {
        using var context = await GitLabTestContext.CreateAsync();

        var project1 = context.CreateProject(initializeWithCommits: true);
        var project2 = context.CreateProject(initializeWithCommits: true);

        var runnersClient = context.Client.Runners;

        // Register a runner on project 1 (owner of the runner)
        var runner = runnersClient.Register(new RunnerRegister { Token = project1.RunnersToken });

        // It Should be enabled by default
        Assert.That(IsEnabledOnProject(project1), Is.True);

        runnersClient.EnableRunner(project2.Id, new RunnerId(runner.Id));
        Assert.That(IsEnabledOnProject(project2), Is.True);

        // Runner can be disabled on projects that does not owns it
        runnersClient.DisableRunner(project2.Id, new RunnerId(runner.Id));
        Assert.That(IsEnabledOnProject(project2), Is.False);

        // And the only way to unregister it from the owner project is to delete it
        runnersClient.Delete(runner.Id);
        var ex = Assert.Throws<GitLabException>(() => IsRegistered(runner.Id));
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        bool IsEnabledOnProject(Project project) => runnersClient[runner.Id].Projects.Any(x => x.Id == project.Id);
        bool IsRegistered(long runnerId) => GetRetryPolicy().Execute(() => runnersClient[runnerId].Projects.Length != 0);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_cannot_disable_runner_on_owner_project()
    {
        using var context = await GitLabTestContext.CreateAsync();
        context.IgnoreTestIfGitLabVersionOutOfRange(VersionRange.Parse("[18.0,)"));

        var project = context.CreateProject(initializeWithCommits: true);

        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = project.RunnersToken });
        Assert.That(IsEnabledOnProject(project), Is.True);

        var exception = Assert.Throws<GitLabException>(() => runnersClient.DisableRunner(project.Id, new RunnerId(runner.Id)));
        Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        bool IsEnabledOnProject(Project project) => runnersClient[runner.Id].Projects.Any(x => x.Id == project.Id);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_register_and_delete_a_runner_on_a_group()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group1 = context.CreateGroup();

        // The runners token of a group is not contained in the API response
        var groupClient = context.Client.Groups;
        var createdGroup1 = groupClient.GetGroup(group1.Id);

        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = createdGroup1.RunnersToken });
        Assert.That(IsRegistered(), Is.True);

        GetRetryPolicy().Execute(() => { runnersClient.Delete(runner.Id); });
        var ex = Assert.Throws<GitLabException>(() => IsRegistered());
        Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        bool IsRegistered() => GetRetryPolicy().Execute(() => runnersClient[runner.Id].Groups.Any(x => x.Id == createdGroup1.Id));
    }

    /// <remarks>
    /// <para>Project runner ownership is enforced on v18.</para>
    /// <para>It is not possible to unassign a runner from the owner project. Delete the runner instead.</para>
    /// <para>See <see href="https://docs.gitlab.com/ci/runners/runners_scope/#project-runner-ownership"/>.</para>
    /// </remarks>
    [Test]
    [NGitLabRetry]
    public async Task Test_can_find_a_runner_on_a_project()
    {
        using var context = await GitLabTestContext.CreateAsync();
        context.IgnoreTestIfGitLabVersionOutOfRange(VersionRange.Parse("[,18.0)"));

        var project = context.CreateProject(initializeWithCommits: true);
        var project2 = context.CreateProject(initializeWithCommits: true);
        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = project.RunnersToken });
        runnersClient.EnableRunner(project2.Id, new RunnerId(runner.Id));

        var result = runnersClient.OfProject(project.Id).ToList();
        Assert.That(result.Exists(r => r.Id == runner.Id), Is.True);

        runnersClient.DisableRunner(project.Id, new RunnerId(runner.Id));
        result = runnersClient.OfProject(project.Id).ToList();
        Assert.That(result.TrueForAll(r => r.Id != runner.Id), Is.True);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_find_a_runner_on_a_group()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group1 = context.CreateGroup();

        // The runners token of a group is not contained in the API response
        var groupClient = context.Client.Groups;
        var createdGroup1 = groupClient.GetGroup(group1.Id);

        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = createdGroup1.RunnersToken });

        var result = runnersClient.OfGroup(createdGroup1.Id).ToArray();
        Assert.That(result.Any(r => r.Id == runner.Id), Is.True);

        GetRetryPolicy().Execute(() => { runnersClient.Delete(runner.Id); });
        result = runnersClient.OfGroup(createdGroup1.Id).ToArray();
        Assert.That(result.All(r => r.Id != runner.Id), Is.True);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_Runner_Can_Be_Locked_And_Unlocked()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = project.RunnersToken, Locked = false });
        Assert.That(runner.Locked, Is.False, "Runner should not be locked.");

        runner = runnersClient.Update(runner.Id, new RunnerUpdate { Locked = true });
        Assert.That(runner.Locked, Is.True, "Runner should be locked.");

        runner = runnersClient.Update(runner.Id, new RunnerUpdate { Locked = false });
        Assert.That(runner.Locked, Is.False, "Runner should not be locked.");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_Runner_Can_Update_RunUntagged_Flag()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var runnersClient = context.Client.Runners;
        var runner = runnersClient.Register(new RunnerRegister { Token = project.RunnersToken, RunUntagged = false, TagList = new[] { "tag" } });
        Assert.That(runner.RunUntagged, Is.False);

        runner = runnersClient.Update(runner.Id, new RunnerUpdate { RunUntagged = true });
        Assert.That(runner.RunUntagged, Is.True);
    }

    private static RetryPolicy GetRetryPolicy()
    {
        return Policy
            .Handle<GitLabException>(ex => ex.StatusCode is HttpStatusCode.Forbidden)
            .WaitAndRetry(3, sleepDurationProvider: attempt => TimeSpan.FromSeconds(2 * attempt));
    }
}
