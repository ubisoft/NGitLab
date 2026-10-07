using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class BadgesMockTests
{
    [Test]
    public void Test_project_badge_name_is_set_on_create_and_update()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", id: 1, addDefaultUserAsMaintainer: true)
            .BuildServer();
        var badgeClient = server.CreateClient().GetProjectBadgeClient(1);

        var badge = badgeClient.Create(
            new BadgeCreate
            {
                Name = "created",
                LinkUrl = "http://dummy/a.html",
                ImageUrl = "http://dummy/a.png",
            }
        );
        Assert.That(badge.Name, Is.EqualTo("created"));

        badge = badgeClient.Update(
            badge.Id,
            new BadgeUpdate
            {
                Name = "updated",
                LinkUrl = "http://dummy/b.html",
                ImageUrl = "http://dummy/b.png",
            }
        );
        Assert.That(badge.Name, Is.EqualTo("updated"));
        Assert.That(badgeClient[badge.Id].Name, Is.EqualTo("updated"));
    }

    [Test]
    public void Test_group_badge_name_is_set_on_create_and_update()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("group1", id: 1, addDefaultUserAsMaintainer: true)
            .BuildServer();
        var badgeClient = server.CreateClient().GetGroupBadgeClient(1);

        var badge = badgeClient.Create(
            new BadgeCreate
            {
                Name = "created",
                LinkUrl = "http://dummy/a.html",
                ImageUrl = "http://dummy/a.png",
            }
        );
        Assert.That(badge.Name, Is.EqualTo("created"));

        badge = badgeClient.Update(
            badge.Id,
            new BadgeUpdate
            {
                Name = "updated",
                LinkUrl = "http://dummy/b.html",
                ImageUrl = "http://dummy/b.png",
            }
        );
        Assert.That(badge.Name, Is.EqualTo("updated"));
        Assert.That(badgeClient[badge.Id].Name, Is.EqualTo("updated"));
    }
}
