using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public sealed class BotUserTests
{
    [Test]
    public void Test_project_bot_user()
    {
        using var server = new GitLabServer();
        var group = new Group("test");
        var project = new Project("test-project");
        server.Groups.Add(group);
        group.Projects.Add(project);

        var bot = project.CreateBotUser("token_name", AccessLevel.Maintainer);

        Assert.That(bot.Bot, Is.True);
        Assert.That(bot.Name, Is.EqualTo("token_name"));
        var permissions = project.GetEffectivePermissions();
        var botPermission = permissions.GetEffectivePermission(bot);
        Assert.That(botPermission.AccessLevel, Is.EqualTo(AccessLevel.Maintainer));
    }

    [Test]
    public void Test_group_bot_user()
    {
        using var server = new GitLabServer();
        var group = new Group("test");
        server.Groups.Add(group);

        var bot = group.CreateBotUser(AccessLevel.Maintainer);

        Assert.That(bot.Bot, Is.True);
        var permissions = group.GetEffectivePermissions();
        var botPermission = permissions.GetEffectivePermission(bot);
        Assert.That(botPermission.AccessLevel, Is.EqualTo(AccessLevel.Maintainer));
    }
}
