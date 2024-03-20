using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class MembersMockTests
{
    [Test]
    public void Test_members_group_all_direct([Values] bool isDefault)
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
                .WithGroup("G1", 1, addDefaultUserAsMaintainer: true)
                .WithGroup("G2", 2, @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
            .BuildServer();

        var client = server.CreateClient("user1");
        var members = isDefault
            ? client.Members.OfGroup("2")
            : client.Members.OfGroup("2", includeInheritedMembers: false);

        Assert.That(members.Count(), Is.EqualTo(1), "Membership found are invalid");
    }

    [Test]
    public void Test_members_group_all_inherited()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithProject("Test")
                .WithGroup("G1", 1, configure: g => g.WithUserPermission("user1", Models.AccessLevel.Maintainer))
                .WithGroup("G2", 2, @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
            .BuildServer();

        var client = server.CreateClient("user1");
        var members = client.Members.OfGroup("2", includeInheritedMembers: true);

        Assert.That(members.Count(), Is.EqualTo(2), "Membership found are invalid");
    }

    [Test]
    public void Test_members_project_all_direct([Values] bool isDefault)
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithUser("user3")
                .WithGroup("G1", 1, addDefaultUserAsMaintainer: true)
                .WithGroup("G2", 2, @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
                .WithProject("Project", @namespace: "G1", configure: g =>
                    g.WithUserPermission("user3", Models.AccessLevel.Maintainer)
                     .WithGroupPermission("G2", Models.AccessLevel.Developer))
            .BuildServer();

        var client = server.CreateClient("user1");
        var members = isDefault
            ? client.Members.OfProject("1")
            : client.Members.OfProject("1", includeInheritedMembers: false);

        Assert.That(members.Count(), Is.EqualTo(1), "Membership found are invalid");
    }

    [Test]
    public void Test_members_project_all_inherited()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithUser("user2")
            .WithUser("user3")
                .WithGroup("G1", addDefaultUserAsMaintainer: true)
                .WithGroup("G2", @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
                .WithProject("Project", 1, @namespace: "G1", configure: g =>
                    g.WithUserPermission("user3", Models.AccessLevel.Maintainer)
                     .WithGroupPermission("G1/G2", Models.AccessLevel.Developer))
            .BuildServer();

        var client = server.CreateClient("user1");
        var members = client.Members.OfProject("1", includeInheritedMembers: true);

        Assert.That(members.Count(), Is.EqualTo(3), "Membership found are invalid");
    }

    [Test]
    public async Task Test_members_async_methods_simulate_gitlab_behavior()
    {
        // This test emulates GitLab's behavior. See `MembersClientTests.AsyncMethodsBehaveAsExpected()`

        // Arrange
        var user1Name = "user1";
        var ownerName = "owner";

        using var server = new GitLabConfig()
            .WithUser(ownerName, isDefault: true)
            .WithUser(user1Name)
            .WithGroupOfFullPath("G1", configure: g =>
                g.WithUserPermission(ownerName, Models.AccessLevel.Owner)
                 .WithUserPermission(user1Name, Models.AccessLevel.Maintainer))
            .WithGroupOfFullPath("G1/G2", configure: g =>
                g.WithUserPermission(ownerName, Models.AccessLevel.Owner))
            .WithProject("Project", 1, @namespace: "G1")
            .BuildServer();

        var client = server.CreateClient(ownerName);

        var user1 = await client.Users.GetByUserNameAsync(user1Name);
        var user1Id = user1.Id.ToString();

        // Act
        // Assert
        const string projectId = "G1/Project";
        const string groupId = "G1/G2";

        // Does NOT search inherited permission by default...
        AssertThrowsGitLabException(() => client.Members.GetMemberOfProjectAsync(projectId, user1.Id), System.Net.HttpStatusCode.NotFound);
        AssertThrowsGitLabException(() => client.Members.GetMemberOfGroupAsync(groupId, user1.Id), System.Net.HttpStatusCode.NotFound);
        client.Members.OfProjectAsync(projectId).Select(m => m.UserName).Should().BeEmpty();
        client.Members.OfGroupAsync(groupId).Select(m => m.UserName).Should().BeEquivalentTo(new[] { ownerName });

        // Does search inherited permission when asked...
        (await client.Members.GetMemberOfProjectAsync(projectId, user1.Id, includeInheritedMembers: true)).UserName.Should().Be(user1Name);
        (await client.Members.GetMemberOfGroupAsync(groupId, user1.Id, includeInheritedMembers: true)).UserName.Should().Be(user1Name);
        client.Members.OfProjectAsync(projectId, includeInheritedMembers: true).Select(m => m.UserName).Should().BeEquivalentTo(new[] { ownerName, user1Name });
        client.Members.OfGroupAsync(groupId, includeInheritedMembers: true).Select(m => m.UserName).Should().BeEquivalentTo(new[] { ownerName, user1Name });

        // Cannot update non-existent membership...
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Owner }), System.Net.HttpStatusCode.NotFound);
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Owner }), System.Net.HttpStatusCode.NotFound);

        // Cannot add membership with an access-level lower than inherited...
        AssertThrowsGitLabException(() => client.Members.AddMemberToProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);
        AssertThrowsGitLabException(() => client.Members.AddMemberToGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);

        // Can add membership with greater than or equal access-level...
        await AssertReturnsMembership(() => client.Members.AddMemberToProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Maintainer }), Models.AccessLevel.Maintainer);
        await AssertReturnsMembership(() => client.Members.AddMemberToGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Maintainer }), Models.AccessLevel.Maintainer);

        // Cannot add duplicate membership...
        AssertThrowsGitLabException(() => client.Members.AddMemberToProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Owner }), System.Net.HttpStatusCode.Conflict);
        AssertThrowsGitLabException(() => client.Members.AddMemberToGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Owner }), System.Net.HttpStatusCode.Conflict);

        // Can raise access-level above inherited...
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Owner }), Models.AccessLevel.Owner);
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Owner }), Models.AccessLevel.Owner);

        // Can decrease access-level to inherited...
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Maintainer }), Models.AccessLevel.Maintainer);
        await AssertReturnsMembership(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Maintainer }), Models.AccessLevel.Maintainer);

        // Cannot decrease access-level lower than inherited...
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfProjectAsync(projectId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);
        AssertThrowsGitLabException(() => client.Members.UpdateMemberOfGroupAsync(groupId, new() { UserId = user1Id, AccessLevel = Models.AccessLevel.Reporter }), System.Net.HttpStatusCode.BadRequest);

        // Can delete...
        await client.Members.RemoveMemberFromProjectAsync(projectId, user1.Id);
        await client.Members.RemoveMemberFromGroupAsync(groupId, user1.Id);

        // Delete fails when not exist...
        AssertThrowsGitLabException(() => client.Members.RemoveMemberFromProjectAsync(projectId, user1.Id), System.Net.HttpStatusCode.NotFound);
        AssertThrowsGitLabException(() => client.Members.RemoveMemberFromGroupAsync(groupId, user1.Id), System.Net.HttpStatusCode.NotFound);
    }

    private static async Task AssertReturnsMembership(Func<Task<Models.Membership>> code, Models.AccessLevel expectedAccessLevel)
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
