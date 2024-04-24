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

        var groupHookUrl = new Uri("https://test-create-group-hook.com");

        // Act
        var createdGroupHook = groupHooksClient.Create(new GroupHookUpsert { Url = groupHookUrl });

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Has.Length.EqualTo(1));
        Assert.That(groupHooksClient[createdGroupHook.Id].Url, Is.EqualTo(groupHookUrl));
        Assert.That(createdGroupHook.Url, Is.EqualTo(groupHookUrl));

        // Act
        groupHookUrl = new Uri("https://test-update-group-hook.com");
        var updatedGroupHook = groupHooksClient.Update(createdGroupHook.Id, new GroupHookUpsert { Url = groupHookUrl });

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Has.Length.EqualTo(1));
        Assert.That(updatedGroupHook.Url, Is.EqualTo(groupHookUrl));

        // Act
        groupHooksClient.Delete(updatedGroupHook.Id);

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Is.Empty);
    }
}
