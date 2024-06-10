using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class EnvironmentsTests
{
    private static string GetSlugNameStart(string name)
    {
        if (name.Length > 17)
            name = name[..17];
        return name.Replace('_', '-');
    }

    [Test]
    [NGitLabRetry]
    public async Task CreateAndGetAll()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var newEnvNameNoUrl = "env_test_name_no_url";
        var newEnvSlugNameNoUrlStart = GetSlugNameStart(newEnvNameNoUrl);
        var newEnvNameWithUrl = "env_test_name_with_url";
        var newEnvSlugNameWithUrlStart = GetSlugNameStart(newEnvNameWithUrl);
        var newEnvNameExternalUrl = "https://www.example.com";

        // Validate environments doesn't exist yet
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameNoUrl, StringComparison.Ordinal) || string.Equals(e.Name, newEnvNameWithUrl, StringComparison.Ordinal)), Is.Null);

        // Create  and check return value
        var env = envClient.Create(newEnvNameNoUrl, externalUrl: null);
        Assert.That(env.Name, Is.EqualTo(newEnvNameNoUrl).IgnoreCase);
        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameNoUrlStart));
        Assert.That(env.Id, Is.Not.Zero);
        Assert.That(env.ExternalUrl, Is.Null);

        // Create newEnvNameWithUrl and check return value
        env = envClient.Create(newEnvNameWithUrl, newEnvNameExternalUrl);
        Assert.That(env.Name, Is.EqualTo(newEnvNameWithUrl).IgnoreCase);
        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameWithUrlStart));
        Assert.That(env.Id, Is.Not.Zero);
        Assert.That(env.ExternalUrl, Is.EqualTo(newEnvNameExternalUrl).IgnoreCase);

        // Validate new environment are present in All
        env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameNoUrl, StringComparison.Ordinal));
        Assert.That(env, Is.Not.Null);
        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameNoUrlStart));
        Assert.That(env.Id, Is.Not.Zero);
        Assert.That(env.ExternalUrl, Is.Null);

        env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameWithUrl, StringComparison.Ordinal));
        Assert.That(env, Is.Not.Null);
        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameWithUrlStart));
        Assert.That(env.Id, Is.Not.Zero);
        Assert.That(env.ExternalUrl, Is.EqualTo(newEnvNameExternalUrl).IgnoreCase);
    }

    [Test]
    [NGitLabRetry]
    public async Task Edit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var newEnvNameToEdit = "env_test_name_to_edit_init";
        var newEnvNameUpdated = "env_test_name_to_edit_updated";
        var newEnvSlugNameUpdatedStart = GetSlugNameStart(newEnvNameUpdated);
        var newEnvNameExternalUrlUpdated = "https://www.example.com/updated";

        // Validate environments doesn't exist yet
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToEdit, StringComparison.Ordinal) || string.Equals(e.Name, newEnvNameUpdated, StringComparison.Ordinal)), Is.Null);

        // Create newEnvNameToEdit
        var env = envClient.Create(newEnvNameToEdit, externalUrl: null);
        var initialEnvId = env.Id;

        // Validate newEnvNameToEdit is present
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToEdit, StringComparison.Ordinal)), Is.Not.Null);

        // Edit and check return value
        env = envClient.Edit(initialEnvId, newEnvNameUpdated, newEnvNameExternalUrlUpdated);

        if (context.IsGitLabMajorVersion(15))
        {
            Assert.That(env.Name, Is.EqualTo(newEnvNameUpdated).IgnoreCase);
        }

        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameUpdatedStart));
        Assert.That(env.Id, Is.EqualTo(initialEnvId), "Environment Id should not change");
        Assert.That(env.ExternalUrl, Is.EqualTo(newEnvNameExternalUrlUpdated).IgnoreCase);

        // Validate update is effective
        // Renaming an environment with the API removed in GitLab 16.0.
        env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, context.IsGitLabMajorVersion(15) ? newEnvNameUpdated : newEnvNameToEdit, StringComparison.Ordinal));
        Assert.That(env, Is.Not.Null);
        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameUpdatedStart));
        Assert.That(env.Id, Is.EqualTo(initialEnvId), "Environment Id should not change");
        Assert.That(env.ExternalUrl, Is.EqualTo(newEnvNameExternalUrlUpdated).IgnoreCase);
    }

    [Test]
    [NGitLabRetry]
    public async Task Delete()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var newEnvNameToDelete = "env_test_name_to_delete";

        // Validate environment doesn't exist yet
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, StringComparison.Ordinal)), Is.Null);

        // Create newEnvNameToDelete
        var env = envClient.Create(newEnvNameToDelete, externalUrl: null);
        var initialEnvId = env.Id;

        // Validate newEnvNameToDelete is present & available
        env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, StringComparison.Ordinal));
        Assert.That(env, Is.Not.Null);
        Assert.That(env.State, Is.EqualTo("available").IgnoreCase);

        // Trying to delete without stopping beforehand will throw...
        Assert.Throws<GitLabException>(() => envClient.Delete(initialEnvId));

        // Stop
        envClient.Stop(initialEnvId);
        env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, StringComparison.Ordinal));
        Assert.That(env.State, Is.EqualTo("stopped").IgnoreCase);

        // Delete
        envClient.Delete(initialEnvId);

        // Validate delete is effective
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, StringComparison.Ordinal)), Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Stop()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var newEnvNameToStop = "env_test_name_to_stop";
        var newEnvSlugNameToStopStart = GetSlugNameStart(newEnvNameToStop);

        // Validate environment doesn't exist yet
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToStop, StringComparison.Ordinal)), Is.Null);

        // Create newEnvNameToStop
        var env = envClient.Create(newEnvNameToStop, externalUrl: null);
        var initialEnvId = env.Id;

        // Validate newEnvNameToStop is present
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToStop, StringComparison.Ordinal)), Is.Not.Null);

        // Stop and check return value
        env = envClient.Stop(initialEnvId);
        Assert.That(env.Name, Is.EqualTo(newEnvNameToStop).IgnoreCase);
        Assert.That(env.Slug, Does.StartWith(newEnvSlugNameToStopStart));
        Assert.That(env.Id, Is.EqualTo(initialEnvId), "Environment Id should not change");
        Assert.That(env.ExternalUrl, Is.Null);

        // Validate environment is still present
        Assert.That(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToStop, StringComparison.Ordinal)), Is.Not.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task QueryByState()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var newEnvNameToStop = "env_test_name_to_stop";
        var stoppedEnv = envClient.Create(newEnvNameToStop, externalUrl: null);
        envClient.Stop(stoppedEnv.Id);

        var newEnvNameAvailable = "env_test_name_available";
        var availableEnv = envClient.Create(newEnvNameAvailable, externalUrl: null);

        // Act
        var availableEnvs = envClient.GetEnvironmentsAsync(new EnvironmentQuery { State = "available" });

        // Assert
        Assert.That(envClient.All.Count(), Is.EqualTo(2));
        var availableEnvResult = availableEnvs.Single();
        Assert.That(availableEnvResult.Name, Is.EqualTo(newEnvNameAvailable));
        Assert.That(availableEnvResult.State, Is.EqualTo("available"));
    }

    [Test]
    [NGitLabRetry]
    public async Task QueryByName()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var devEnvName = "dev";
        var devEnvironment = envClient.Create(devEnvName, externalUrl: null);

        var prodEnvName = "production";
        var prodEnvironment = envClient.Create(prodEnvName, externalUrl: null);

        // Act
        var prodEnvs = envClient.GetEnvironmentsAsync(new EnvironmentQuery { Name = prodEnvName });

        // Assert
        Assert.That(envClient.All.Count(), Is.EqualTo(2));
        var availableEnvResult = prodEnvs.Single();
        Assert.That(availableEnvResult.Name, Is.EqualTo(prodEnvName));
    }

    [Test]
    [NGitLabRetry]
    public async Task QueryBySearch()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var devEnvName = "dev";
        var devEnvironment = envClient.Create(devEnvName, externalUrl: null);

        var dev2EnvName = "dev2";
        var dev2Environment = envClient.Create(dev2EnvName, externalUrl: null);

        var prodEnvName = "production";
        var prodEnvironment = envClient.Create(prodEnvName, externalUrl: null);

        // Act
        var devEnvs = envClient.GetEnvironmentsAsync(new EnvironmentQuery { Search = "dev" });

        // Assert
        Assert.That(envClient.All.Count(), Is.EqualTo(3));
        Assert.That(devEnvs.Count(), Is.EqualTo(2));
        Assert.That(devEnvs, Is.All.Matches<EnvironmentInfo>(e => e.Name.Contains("dev", StringComparison.Ordinal)));
    }

    [Test]
    [NGitLabRetry]
    public async Task GetById()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var envClient = context.Client.GetEnvironmentClient(project.Id);

        var devEnvName = "dev";
        var devEnvironment = envClient.Create(devEnvName, externalUrl: null);

        // Act
        var devEnv = await envClient.GetByIdAsync(devEnvironment.Id);

        // Assert
        Assert.That(devEnv, Is.Not.Null);
        Assert.That(devEnv.Name, Is.EqualTo(devEnvName));
    }
}
