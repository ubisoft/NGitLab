using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class GroupHooksClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_create_update_delete_group_hook()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var group = context.CreateGroup();
        var groupHooksClient = context.Client.GetGroupHooksClient(group.Id);

        var toCreateGroupHook = new GroupHookUpsert
        {
            Url = new Uri("https://test-create-group-hook.com"),
            EnableSslVerification = true,
            PushEvents = true,
        };

        // Act
        var createdGroupHook = groupHooksClient.Create(toCreateGroupHook);

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Has.Length.EqualTo(1));

        Assert.That(createdGroupHook.Url, Is.EqualTo(toCreateGroupHook.Url));
        Assert.That(createdGroupHook.EnableSslVerification, Is.EqualTo(toCreateGroupHook.EnableSslVerification));
        Assert.That(createdGroupHook.PushEvents, Is.EqualTo(toCreateGroupHook.PushEvents));

        var groupHookById = groupHooksClient[createdGroupHook.Id];
        Assert.That(groupHookById.Url, Is.EqualTo(toCreateGroupHook.Url));
        Assert.That(groupHookById.EnableSslVerification, Is.EqualTo(toCreateGroupHook.EnableSslVerification));
        Assert.That(groupHookById.PushEvents, Is.EqualTo(toCreateGroupHook.PushEvents));

        // Arrange
        var toUpdateGroupHook = new GroupHookUpsert
        {
            Url = new Uri("https://test-update-group-hook.com"),
            PushEvents = false,
        };

        // Act
        var updatedGroupHook = groupHooksClient.Update(createdGroupHook.Id, toUpdateGroupHook);

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Has.Length.EqualTo(1));

        Assert.That(updatedGroupHook.Url, Is.EqualTo(toUpdateGroupHook.Url));
        Assert.That(updatedGroupHook.PushEvents, Is.EqualTo(toUpdateGroupHook.PushEvents));
        Assert.That(updatedGroupHook.EnableSslVerification, Is.EqualTo(toCreateGroupHook.EnableSslVerification));

        groupHookById = groupHooksClient[updatedGroupHook.Id];
        Assert.That(groupHookById.Url, Is.EqualTo(toUpdateGroupHook.Url));
        Assert.That(groupHookById.PushEvents, Is.EqualTo(toUpdateGroupHook.PushEvents));
        Assert.That(groupHookById.EnableSslVerification, Is.EqualTo(toCreateGroupHook.EnableSslVerification));

        // Act
        groupHooksClient.Delete(updatedGroupHook.Id);

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Is.Empty);
    }
}
