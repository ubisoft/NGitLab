#nullable enable

using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MembersClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task AddMemberToProject()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        context.CreateNewUser(out var user);
        var projectId = project.Id.ToString(CultureInfo.InvariantCulture);

        var expiresAt = DateTimeOffset.UtcNow.AddDays(30).ToString("yyyy-MM-dd");
        context.Client.Members.AddMemberToProject(projectId, new ProjectMemberCreate
        {
            AccessLevel = AccessLevel.Developer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            ExpiresAt = expiresAt,
        });

        var projectUser = context.Client.Members.OfProject(projectId).Single(u => u.Id == user.Id);
        Assert.That((AccessLevel)projectUser.AccessLevel, Is.EqualTo(AccessLevel.Developer));
        Assert.That(projectUser.ExpiresAt?.ToString("yyyy-MM-dd"), Is.EqualTo(expiresAt));
    }

    [Test]
    [NGitLabRetry]
    public async Task UpsertAccessLevelMemberOfProject()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        context.CreateNewUser(out var user);
        var projectId = project.Id.ToString(CultureInfo.InvariantCulture);

        // Add
        context.Client.Members.AddMemberToProject(projectId, new ProjectMemberCreate
        {
            AccessLevel = AccessLevel.Developer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
        });
        var projectUser = context.Client.Members.OfProject(projectId).Single(u => u.Id == user.Id);
        Assert.That((AccessLevel)projectUser.AccessLevel, Is.EqualTo(AccessLevel.Developer));

        // Update
        context.Client.Members.UpdateMemberOfProject(projectId, new ProjectMemberUpdate
        {
            AccessLevel = AccessLevel.Maintainer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
        });
        projectUser = context.Client.Members.OfProject(projectId).Single(u => u.Id == user.Id);
        Assert.That((AccessLevel)projectUser.AccessLevel, Is.EqualTo(AccessLevel.Maintainer));
    }

    [Test]
    [NGitLabRetry]
    public async Task GetAccessLevelMemberOfProject()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        context.CreateNewUser(out var user);
        var projectId = project.Id.ToString(CultureInfo.InvariantCulture);

        context.Client.Members.AddMemberToProject(projectId, new ProjectMemberCreate
        {
            AccessLevel = AccessLevel.Developer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
        });

        // Get
        var projectUser = context.Client.Members.GetMemberOfProject(projectId, user.Id.ToString(CultureInfo.InvariantCulture));
        Assert.That((AccessLevel)projectUser.AccessLevel, Is.EqualTo(AccessLevel.Developer));
    }

    [Test]
    [NGitLabRetry]
    public async Task AddMemberToGroup()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        context.CreateNewUser(out var user);
        var groupId = group.Id.ToString(CultureInfo.InvariantCulture);

        var expiresAt = DateTimeOffset.UtcNow.AddDays(30).ToString("yyyy-MM-dd");
        context.Client.Members.AddMemberToGroup(groupId, new GroupMemberCreate
        {
            AccessLevel = AccessLevel.Developer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            ExpiresAt = expiresAt,
        });

        var groupUser = context.Client.Members.OfGroup(groupId).Single(u => u.Id == user.Id);
        Assert.That((AccessLevel)groupUser.AccessLevel, Is.EqualTo(AccessLevel.Developer));
        Assert.That(groupUser.ExpiresAt?.ToString("yyyy-MM-dd"), Is.EqualTo(expiresAt));
    }

    [Test]
    [NGitLabRetry]
    public async Task UpsertAccessLevelMemberOfGroup()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        context.CreateNewUser(out var user);
        var groupId = group.Id.ToString(CultureInfo.InvariantCulture);

        // Add
        context.Client.Members.AddMemberToGroup(groupId, new GroupMemberCreate
        {
            AccessLevel = AccessLevel.Developer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
        });
        var groupUser = context.Client.Members.OfGroup(groupId).Single(u => u.Id == user.Id);
        Assert.That((AccessLevel)groupUser.AccessLevel, Is.EqualTo(AccessLevel.Developer));

        // Update
        context.Client.Members.UpdateMemberOfGroup(groupId, new GroupMemberUpdate
        {
            AccessLevel = AccessLevel.Maintainer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
        });
        groupUser = context.Client.Members.OfGroup(groupId).Single(u => u.Id == user.Id);
        Assert.That((AccessLevel)groupUser.AccessLevel, Is.EqualTo(AccessLevel.Maintainer));
    }

    [Test]
    [NGitLabRetry]
    public async Task GetAccessLevelMemberOfGroup()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        context.CreateNewUser(out var user);
        var groupId = group.Id.ToString(CultureInfo.InvariantCulture);

        context.Client.Members.AddMemberToGroup(groupId, new GroupMemberCreate
        {
            AccessLevel = AccessLevel.Developer,
            UserId = user.Id.ToString(CultureInfo.InvariantCulture),
        });

        // Get
        var groupUser = context.Client.Members.GetMemberOfGroup(groupId, user.Id.ToString(CultureInfo.InvariantCulture));
        Assert.That((AccessLevel)groupUser.AccessLevel, Is.EqualTo(AccessLevel.Developer));
    }

    [Test]
    public async Task AsyncMethodsBehaveAsExpected()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var client = context.Client;
        var ownerName = client.Users.Current.Username;

        // Create a user, a top-level group, a group project, and a subgroup...
        context.CreateNewUser(out var user1);
        var group1 = context.CreateGroup();
        var group2 = context.CreateSubgroup(group1.Id);
        var project = context.CreateProject(group1.Id);

        var user1Name = user1.Username;
        var user1Id = user1.Id.ToString();

        // Add the user to the top-level group as a Maintainer...
        await client.Members.AddMemberToGroupAsync(group1.Id, new()
        {
            AccessLevel = AccessLevel.Maintainer,
            UserId = user1Id,
        });

        // Act
        // Assert
        var projectId = project.PathWithNamespace!;
        var groupId = group2.FullPath!;

        // Does NOT search inherited permission by default...
        AssertThrowsGitLabException(() => client.Members.GetMemberOfProjectAsync(projectId, user1.Id), System.Net.HttpStatusCode.NotFound);
        AssertThrowsGitLabException(() => client.Members.GetMemberOfGroupAsync(groupId, user1.Id), System.Net.HttpStatusCode.NotFound);
        Assert.That(client.Members.OfProjectAsync(projectId).Select(m => m.UserName), Is.Empty);
        Assert.That(client.Members.OfGroupAsync(groupId).Select(m => m.UserName), Is.EquivalentTo(new[] { ownerName }));

        // Does search inherited permission when asked...
        await client.Members.GetMemberOfProjectAsync(projectId, user1.Id, includeInheritedMembers: true);
        await client.Members.GetMemberOfGroupAsync(groupId, user1.Id, includeInheritedMembers: true);
        Assert.That(client.Members.OfProjectAsync(projectId, includeInheritedMembers: true).Select(m => m.UserName), Is.EquivalentTo(new[] { ownerName, user1Name }));
        Assert.That(client.Members.OfGroupAsync(groupId, includeInheritedMembers: true).Select(m => m.UserName), Is.EquivalentTo(new[] { ownerName, user1Name }));

        // Cannot update non-existent membership...
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Owner }), System.Net.HttpStatusCode.NotFound);
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Owner }), System.Net.HttpStatusCode.NotFound);

        // Cannot add membership with an access-level lower than inherited...
        AssertThrowsGitLabException(() => client.Members.AddMemberToProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);
        AssertThrowsGitLabException(() => client.Members.AddMemberToGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);

        // Can add membership with greater than or equal access-level...
        await AssertReturnsMembership(() => client.Members.AddMemberToProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Maintainer }), AccessLevel.Maintainer);
        await AssertReturnsMembership(() => client.Members.AddMemberToGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Maintainer }), AccessLevel.Maintainer);

        // Cannot add duplicate membership...
        AssertThrowsGitLabException(() => client.Members.AddMemberToProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Owner }), System.Net.HttpStatusCode.Conflict);
        AssertThrowsGitLabException(() => client.Members.AddMemberToGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Owner }), System.Net.HttpStatusCode.Conflict);

        // Can raise access-level above inherited...
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Owner }), AccessLevel.Owner);
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Owner }), AccessLevel.Owner);

        // Can decrease access-level to inherited...
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Maintainer }), AccessLevel.Maintainer);
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Maintainer }), AccessLevel.Maintainer);

        // Cannot decrease access-level lower than inherited...
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);

        // Can delete...
        await client.Members.RemoveMemberFromProjectAsync(projectId, user1.Id);
        await client.Members.RemoveMemberFromGroupAsync(groupId, user1.Id);

        // Delete fails when not exist...
        AssertThrowsGitLabException(() => client.Members.RemoveMemberFromProjectAsync(projectId, user1.Id), System.Net.HttpStatusCode.NotFound);
        AssertThrowsGitLabException(() => client.Members.RemoveMemberFromGroupAsync(groupId, user1.Id), System.Net.HttpStatusCode.NotFound);
    }

    private static async Task AssertReturnsMembership(Func<Task<Membership>> code, AccessLevel expectedAccessLevel)
    {
        var membership = await code.Invoke().ConfigureAwait(false);
        Assert.That(membership, Is.Not.Null);
        Assert.That(membership.AccessLevel, Is.EqualTo((int)expectedAccessLevel));
    }

    private static void AssertThrowsGitLabException(AsyncTestDelegate code, System.Net.HttpStatusCode expectedStatusCode)
    {
        var ex = Assert.CatchAsync(typeof(GitLabException), code) as GitLabException;
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.StatusCode, Is.EqualTo(expectedStatusCode));
    }
}
