using System.Linq;
using System.Threading.Tasks;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class GroupsMockTests
{
    [Test]
    public async Task Test_group_get_by_id()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test")
                .WithGroup("G1", 1)
                .WithGroup("G2", 2)
                .WithGroup("G3", 3)
            .BuildServer();

        var client = server.CreateClient("user1");
        var group = await client.Groups.GetByIdAsync(1);

        Assert.That(group.Name, Is.EqualTo("G1"), "Subgroups found are invalid");
    }

    [Test]
    public async Task Test_group_get_by_fullpath()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test")
                .WithGroup("G1", @namespace: "name1")
                .WithGroup("G2", @namespace: "name2")
                .WithGroup("G3", @namespace: "name3")
            .BuildServer();

        var client = server.CreateClient("user1");
        var group = await client.Groups.GetByFullPathAsync("name3");

        Assert.That(group.FullPath, Is.EqualTo("name3"), "Subgroups found are invalid");
    }

    [Test]
    public void Test_get_subgroups_by_id()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithProject("Test", addDefaultUserAsMaintainer: true)
                .WithGroup("parentGroup1", configure: group => group.Id = 12)
                .WithGroup("parentGroup2", configure: group => group.Id = 89)
                .WithGroup("G1", 2, @namespace: "parentGroup1")
                .WithGroup("G2", 3, @namespace: "parentGroup1")
                .WithGroup("G3", 4, @namespace: "parentGroup2")
            .BuildServer();

        var client = server.CreateClient("user1");
        var group = client.Groups.GetSubgroupsByIdAsync(12, new Models.SubgroupQuery { });

        Assert.That(group.Count(), Is.EqualTo(2), "Subgroups found are invalid");
    }

    [Test]
    public void Test_get_subgroups_by_fullpath()
    {
        using var server = new GitLabConfig()
           .WithUser("user1", isDefault: true)
           .WithProject("Test", addDefaultUserAsMaintainer: true)
               .WithGroup("parentGroup1", configure: group => group.Id = 12)
               .WithGroup("parentGroup2", configure: group => group.Id = 89)
               .WithGroup("G1", 2, @namespace: "parentGroup1")
               .WithGroup("G2", 3, @namespace: "parentGroup1")
               .WithGroup("G3", 4, @namespace: "parentGroup2")
           .BuildServer();

        var client = server.CreateClient("user1");
        var group = client.Groups.GetSubgroupsByFullPathAsync("parentgroup1", new Models.SubgroupQuery { });

        Assert.That(group.Count(), Is.EqualTo(2), "Subgroups found are invalid");
    }
}
