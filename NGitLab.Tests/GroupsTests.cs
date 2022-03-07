using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class GroupsTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_groups_is_not_empty()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            Assert.IsNotEmpty(groupClient.Accessible);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_projects_are_set_in_a_group_by_id()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();
            var project = context.Client.Projects.Create(new ProjectCreate { Name = "test", NamespaceId = group.Id.ToString(CultureInfo.InvariantCulture) });

            group = groupClient[group.Id];
            Assert.IsNotNull(group);
            Assert.IsNotEmpty(group.Projects);
            Assert.AreEqual(project.Id, group.Projects[0].Id);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_group_by_fullpath()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            group = groupClient[group.FullPath];
            Assert.IsNotNull(group);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_create_delete_group()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            // Search
            var searchedGroup = groupClient.Search(group.Name).Single();
            Assert.AreEqual(group.Id, searchedGroup.Id);

            // Delete (operation is asynchronous so we have to retry until the project is deleted)
            // Group can be marked for deletion (https://docs.gitlab.com/ee/user/admin_area/settings/visibility_and_access_controls.html#default-deletion-adjourned-period-premium-only)
            groupClient.Delete(group.Id);
            await GitLabTestContext.RetryUntilAsync(() => TryGetGroup(groupClient, group.Id), group => group == null || group.MarkedForDeletionOn != null, TimeSpan.FromMinutes(2));
        }

        private static Group TryGetGroup(IGroupsClient groupClient, int groupId)
        {
            try
            {
                return groupClient[groupId];
            }
            catch (GitLabException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_nulls_does_not_throws()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            // Arrange
            var groupQueryNull = new GroupQuery();

            // Act & Assert
            Assert.NotNull(groupClient.Get(groupQueryNull).Take(10).ToList());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_SkipGroups_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group1 = context.CreateGroup();
            var group2 = context.CreateGroup();
            var group3 = context.CreateGroup();

            // Arrange
            var skippedGroupIds = new[] { group2.Id };

            // Act
            var resultSkip = groupClient.Get(new GroupQuery { SkipGroups = skippedGroupIds }).ToList();

            // Assert
            foreach (var skippedGroup in skippedGroupIds)
            {
                Assert.False(resultSkip.Any(group => group.Id == skippedGroup), $"Group {skippedGroup} found in results");
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_Search_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group1 = context.CreateGroup();
            var group2 = context.CreateGroup();

            // Arrange
            var groupQueryNull = new GroupQuery
            {
                Search = group1.Name,
            };

            // Act
            var result = groupClient.Get(groupQueryNull).Count(g => string.Equals(g.Name, group1.Name, StringComparison.Ordinal));

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_SearchProjectsQuery_returns_project()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();
            var project = context.Client.Projects.Create(new ProjectCreate { Name = "test", NamespaceId = group.Id.ToString(CultureInfo.InvariantCulture), Path = "testgroup" });

            // Act
            var result = groupClient.SearchProjects(new SearchProjectQuery { GroupId = group.Name, Search = "test", Scope = GroupQueryScope.Projects });

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(project.Name, result.FirstOrDefault().Name);
            Assert.AreEqual(project.Id, result.FirstOrDefault().Id);
            Assert.AreEqual(project.Path, result.FirstOrDefault().Path);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_AllAvailable_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            // Arrange
            var groupQueryAllAvailable = new GroupQuery
            {
                AllAvailable = true,
            };

            // Act
            var result = groupClient.Get(groupQueryAllAvailable);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_OrderBy_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            // Arrange
            var groupQueryOrderByName = new GroupQuery
            {
                OrderBy = "name",
            };
            var groupQueryOrderByPath = new GroupQuery
            {
                OrderBy = "path",
            };
            var groupQueryOrderById = new GroupQuery
            {
                OrderBy = "id",
            };

            // Act
            var resultByName = groupClient.Get(groupQueryOrderByName);
            var resultByPath = groupClient.Get(groupQueryOrderByPath);
            var resultById = groupClient.Get(groupQueryOrderById);

            // Assert
            Assert.IsTrue(resultByName.Any());
            Assert.IsTrue(resultByPath.Any());
            Assert.IsTrue(resultById.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_Sort_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            // Arrange
            var groupQueryAsc = new GroupQuery
            {
                Sort = "asc",
            };
            var groupQueryDesc = new GroupQuery
            {
                Sort = "desc",
            };

            // Act
            var resultAsc = groupClient.Get(groupQueryAsc);
            var resultDesc = groupClient.Get(groupQueryDesc);

            // Assert
            Assert.IsTrue(resultAsc.Any());
            Assert.IsTrue(resultDesc.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_Statistics_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            var groupQueryWithStats = new GroupQuery
            {
                Statistics = true,
            };

            // Act
            var result = groupClient.Get(groupQueryWithStats);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_WithCustomAttributes_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            var groupQueryWithCustomAttributes = new GroupQuery
            {
                WithCustomAttributes = true,
            };

            // Act
            var result = groupClient.Get(groupQueryWithCustomAttributes);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_Owned_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            var groupQueryOwned = new GroupQuery
            {
                Owned = true,
            };

            // Act
            var result = groupClient.Get(groupQueryOwned);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_by_group_query_groupQuery_MinAccessLevel_returns_groups()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var groupClient = context.Client.Groups;
            var group = context.CreateGroup();

            var groupQuery10 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Guest,
            };
            var groupQuery20 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Reporter,
            };
            var groupQuery30 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Developer,
            };
            var groupQuery40 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Maintainer,
            };
            var groupQuery50 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Owner,
            };

            // Act
            var result10 = groupClient.Get(groupQuery10);
            var result20 = groupClient.Get(groupQuery20);
            var result30 = groupClient.Get(groupQuery30);
            var result40 = groupClient.Get(groupQuery40);
            var result50 = groupClient.Get(groupQuery50);

            // Assert
            Assert.IsTrue(result10.Any());
            Assert.IsTrue(result20.Any());
            Assert.IsTrue(result30.Any());
            Assert.IsTrue(result40.Any());
            Assert.IsTrue(result50.Any());
        }
    }
}
