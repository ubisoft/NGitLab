using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Mock.Config;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class GroupsMockTests
{
    private static GitLabServer CreateGroupHierarchy() =>
        new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("tlg", 1)
                .WithGroup("sg1", 2, @namespace: "tlg")
                    .WithGroup("sg1_1", 3, @namespace: "tlg/sg1")
                .WithGroup("sg2", 4, @namespace: "tlg")
                    .WithGroup("sg2_1", 5, @namespace: "tlg/sg2")
            .BuildServer();

    private static GitLabServer CreateProjectHierarchy() =>
        new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .WithGroup("tlg", 1)
                .WithProject("p1", 2, @namespace: "tlg")
                .WithGroup("sg1", 3, @namespace: "tlg")
                    .WithProject("p2", 4, @namespace: "tlg/sg1")
                    .WithProject("p3", 5, @namespace: "tlg/sg1")
                .WithGroup("sg2", 6, @namespace: "tlg")
            .BuildServer();

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
    public void Test_get_groups_with_top_level_only_ignores_subgroups()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        var groups = client.Groups.GetAsync(new Models.GroupQuery { TopLevelOnly = true });

        var expected = new string[] { "user1", "tlg" };
        Assert.That(groups.Select(g => g.FullPath), Is.EquivalentTo(expected));
    }

    [Test]
    public async Task Test_page_groups_first_page()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageAsync(new(page: 1, perPage: 3));

        var expected = new string[] { "user1", "tlg", "tlg/sg1" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(g => g.FullPath), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(6));
        });
    }

    [Test]
    public async Task Test_page_groups_last_page()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageAsync(new(page: 2, perPage: 4));

        var expected = new string[] { "tlg/sg2", "tlg/sg2/sg2_1" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(g => g.FullPath), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(6));
        });
    }

    [Test]
    public async Task Test_page_groups_with_page_0_returns_page_1()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageAsync(new(page: 0));

        Assert.That(total, Is.EqualTo(6));
    }

    [Test]
    public void Test_page_groups_with_invalid_perpage_throws()
    {
        using var server = CreateGroupHierarchy();
        var client = server.CreateClient("user1");
        Assert.ThrowsAsync<Clients.GitLabBadRequestException>(() => client.Groups.PageAsync(new(perPage: 0)));
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

    [Test]
    public void Test_get_subgroups_descendants_by_fullpath()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        var group = client.Groups.GetSubgroupsByFullPathAsync("tlg", new Models.SubgroupQuery { IncludeDescendants = true });

        var expected = new string[] { "tlg/sg1", "tlg/sg1/sg1_1", "tlg/sg2", "tlg/sg2/sg2_1" };
        Assert.That(group.Select(g => g.FullPath), Is.EquivalentTo(expected));
    }

    [Test]
    public void Test_get_subgroups_descendants_by_id()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        var group = client.Groups.GetSubgroupsByIdAsync(1, new Models.SubgroupQuery { IncludeDescendants = true });

        var expected = new string[] { "tlg/sg1", "tlg/sg1/sg1_1", "tlg/sg2", "tlg/sg2/sg2_1" };
        Assert.That(group.Select(g => g.FullPath), Is.EquivalentTo(expected));
    }

    [Test]
    public void Test_get_subgroups_descendants_of_subgroup_by_fullpath()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        var group = client.Groups.GetSubgroupsAsync("tlg/sg2", new Models.SubgroupQuery { IncludeDescendants = true });

        var expected = new string[] { "tlg/sg2/sg2_1" };
        Assert.That(group.Select(g => g.FullPath), Is.EquivalentTo(expected));
    }

    [Test]
    public void Test_get_subgroups_descendants_of_subgroup_by_id()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        var group = client.Groups.GetSubgroupsAsync(2, new Models.SubgroupQuery { IncludeDescendants = true });

        var expected = new string[] { "tlg/sg1/sg1_1" };
        Assert.That(group.Select(g => g.FullPath), Is.EquivalentTo(expected));
    }

    [Test]
    public async Task Test_page_subgroups_with_descendants_first_page()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageSubgroupsAsync("tlg", new(page: 1, perPage: 3, new Models.SubgroupQuery { IncludeDescendants = true }));

        var expected = new string[] { "tlg/sg1", "tlg/sg1/sg1_1", "tlg/sg2" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(g => g.FullPath), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(4));
        });
    }

    [Test]
    public async Task Test_page_subgroups_with_descendants_last_page()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageSubgroupsAsync(1, new(page: 2, perPage: 3, new Models.SubgroupQuery { IncludeDescendants = true }));

        var expected = new string[] { "tlg/sg2/sg2_1" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(g => g.FullPath), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(4));
        });
    }

    [Test]
    public async Task Test_page_subgroups_with_descendants_after_last_page()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageSubgroupsAsync(1, new(page: 100, perPage: 3, new Models.SubgroupQuery { IncludeDescendants = true }));

        Assert.Multiple(() =>
        {
            Assert.That(page.Select(g => g.FullPath), Is.Empty);
            Assert.That(total, Is.EqualTo(4));
        });
    }

    [Test]
    public async Task Test_page_subgroups_with_page_0_returns_page_1()
    {
        using var server = CreateGroupHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageSubgroupsAsync("tlg", new(page: 0));

        Assert.That(total, Is.EqualTo(2));
    }

    [Test]
    public void Test_page_subgroups_with_invalid_perpage_throws()
    {
        using var server = CreateGroupHierarchy();
        var client = server.CreateClient("user1");
        Assert.ThrowsAsync<Clients.GitLabBadRequestException>(() => client.Groups.PageSubgroupsAsync(1, new(page: 1, perPage: 0)));
    }

    [Test]
    public async Task Test_page_projects_first_page()
    {
        using var server = CreateProjectHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageProjectsAsync("tlg", new(page: 1, perPage: 1000));

        var expected = new string[] { "tlg/p1" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(p => p.PathWithNamespace), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task Test_page_projects_in_subgroup()
    {
        using var server = CreateProjectHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageProjectsAsync("tlg/sg1", new(page: 2, perPage: 1));

        var expected = new string[] { "tlg/sg1/p3" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(p => p.PathWithNamespace), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task Test_page_projects_in_subgroup_with_no_projects()
    {
        using var server = CreateProjectHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageProjectsAsync("tlg/sg2", new());

        Assert.Multiple(() =>
        {
            Assert.That(page.Select(p => p.PathWithNamespace), Is.Empty);
            Assert.That(total, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task Test_page_projects_in_subgroup_with_descendants()
    {
        using var server = CreateProjectHierarchy();

        var client = server.CreateClient("user1");
        (var page, var total) = await client.Groups.PageProjectsAsync("tlg", new(query: new() { IncludeSubGroups = true }));

        var expected = new string[] { "p1", "p2", "p3" };
        Assert.Multiple(() =>
        {
            Assert.That(page.Select(p => p.Name), Is.EquivalentTo(expected));
            Assert.That(total, Is.EqualTo(3));
        });
    }

    [Test]
    public void Test_create_update_delete_group_hooks()
    {
        // Arrange
        var groupId = 1;

        using var server = new GitLabConfig()
            .WithUser("user1", isAdmin: true)
            .WithGroup("group1", groupId)
            .BuildServer();

        var client = server.CreateClient("user1");
        var groupHooksClient = client.GetGroupHooksClient(groupId);

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
        Assert.That(updatedGroupHook.EnableSslVerification, Is.False);

        groupHookById = groupHooksClient[updatedGroupHook.Id];
        Assert.That(groupHookById.Url, Is.EqualTo(toUpdateGroupHook.Url));
        Assert.That(groupHookById.PushEvents, Is.EqualTo(toUpdateGroupHook.PushEvents));
        Assert.That(groupHookById.EnableSslVerification, Is.False);

        // Act
        groupHooksClient.Delete(updatedGroupHook.Id);

        // Assert
        Assert.That(groupHooksClient.All.ToArray(), Is.Empty);
    }

    [Test]
    public async Task Test_group_created_at_date()
    {
        using var server = new GitLabConfig()
            .WithUser("user1", isDefault: true)
            .BuildServer();

        var client = server.CreateClient("user1");

        var t1 = DateTime.UtcNow;
        var group = await client.Groups.CreateAsync(new Models.GroupCreate
        {
            Name = "Foo",
            Path = "foo",
        });
        var t2 = DateTime.UtcNow;

        Assert.That(group.CreatedAt, Is.GreaterThanOrEqualTo(t1));
        Assert.That(group.CreatedAt, Is.LessThanOrEqualTo(t2));
    }
}
