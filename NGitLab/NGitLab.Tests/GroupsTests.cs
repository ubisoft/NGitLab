using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    internal class GroupsTests
    {
        [Test]
        public void Test_groups_is_not_empty()
        {
            Assert.IsNotEmpty(Groups.Accessible);
        }

        [Test]
        public void Test_projects_are_set_in_a_group_by_id()
        {
            var group = Groups[Initialize.UnitTestGroup.Id];
            Assert.IsNotNull(group);
            Assert.IsNotEmpty(group.Projects);
        }

        [Test]
        public void Test_projects_are_set_in_a_group_by_fullpath()
        {
            var group = Groups[Initialize.UnitTestGroup.FullPath];
            Assert.IsNotNull(group);
            Assert.IsNotEmpty(group.Projects);
        }

        [Test]
        public void Test_create_delete_group()
        {
            var randomNumber = new Random().Next();
            var name = "NewGroup" + randomNumber;
            var path = "NewGroupPath" + randomNumber;
            // Create
            var group = Groups.Create(new GroupCreate()
            {
                Name = name,
                Path = path,
                Visibility = VisibilityLevel.Internal
            });
            Assert.IsNotNull(group);
            Assert.AreEqual(name, group.Name);
            Assert.AreEqual(path, group.Path);
            Assert.AreEqual(VisibilityLevel.Internal, group.Visibility);

            // Search
            group = Groups.Search(name).Single();
            Assert.AreEqual(name, group.Name);
            Assert.AreEqual(path, group.Path);
            Assert.AreEqual(VisibilityLevel.Internal, group.Visibility);

            // Delete (operation is asynchronous so we have to retry until the project is deleted)
            Groups.Delete(group.Id);
            var sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                var groups = Groups.Search(name).ToList();
                if (groups.Count == 0)
                    return;

                var timeout = TimeSpan.FromSeconds(45);
                if (sw.Elapsed > timeout)
                {
                    CollectionAssert.IsEmpty(groups, $"Group was not deleted in the allotted time of {timeout.TotalSeconds:0}seconds");
                }
            }
        }

        [Test]
        public void Test_get_by_group_query_nulls_does_not_throws()
        {
            //Arrange
            var groupQueryNull = new GroupQuery();

            //Act & Assert
            Assert.DoesNotThrow(() => Groups.Get(groupQueryNull));
        }

        [Test]
        public void Test_get_by_group_query_nulls_returns_groups()
        {
            //Arrange
            var groupQueryNull = new GroupQuery();

            //Act
            var result = Groups.Get(groupQueryNull);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        [NonParallelizable]
        public void Test_get_by_group_query_groupQuery_SkipGroups_returns_groups()
        {
            //Arrange
            var skippedGroupIds = new[] { 7161, 1083 }; // Square & TeaBox IDs

            // Ensure the groups exist
            foreach (var groupId in skippedGroupIds)
            {
                Assert.IsNotNull(Groups[groupId]);
            }

            //Act
            var resultSkip = Groups.Get(new GroupQuery { SkipGroups = skippedGroupIds }).ToList();

            // Assert
            foreach (var skippedGroup in skippedGroupIds)
            {
                Assert.False(resultSkip.Any(group => group.Id == skippedGroup), $"Group {skippedGroup} found in results");
            }
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_Search_returns_groups()
        {
            //Arrange
            var groupQueryNull = new GroupQuery
            {
                Search = "Square",
            };

            //Act
            var result = Groups.Get(groupQueryNull).Count(g => string.Equals(g.Name, "Square", StringComparison.InvariantCultureIgnoreCase));

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_AllAvailable_returns_groups()
        {
            //Arrange
            var groupQueryAllAvailable = new GroupQuery
            {
                AllAvailable = true
            };

            //Act
            var result = Groups.Get(groupQueryAllAvailable);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_OrderBy_returns_groups()
        {
            //Arrange
            var groupQueryOrderByName = new GroupQuery
            {
                OrderBy = "name"
            };
            var groupQueryOrderByPath = new GroupQuery
            {
                OrderBy = "path"
            };
            var groupQueryOrderById = new GroupQuery
            {
                OrderBy = "id"
            };

            //Act
            var resultByName = Groups.Get(groupQueryOrderByName);
            var resultByPath = Groups.Get(groupQueryOrderByPath);
            var resultById = Groups.Get(groupQueryOrderById);

            // Assert
            Assert.IsTrue(resultByName.Any());
            Assert.IsTrue(resultByPath.Any());
            Assert.IsTrue(resultById.Any());
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_Sort_returns_groups()
        {
            //Arrange
            var groupQueryAsc = new GroupQuery
            {
                Sort = "asc"
            };
            var groupQueryDesc = new GroupQuery
            {
                Sort = "desc"
            };

            //Act
            var resultAsc = Groups.Get(groupQueryAsc);
            var resultDesc = Groups.Get(groupQueryDesc);

            // Assert
            Assert.IsTrue(resultAsc.Any());
            Assert.IsTrue(resultDesc.Any());
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_Statistics_returns_groups()
        {
            //Arrange
            var groupQueryWithStats = new GroupQuery
            {
                Statistics = true
            };

            //Act
            var result = Groups.Get(groupQueryWithStats);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_WithCustomAttributes_returns_groups()
        {
            //Arrange
            var groupQueryWithCustomAttributes = new GroupQuery
            {
                WithCustomAttributes = true
            };

            //Act
            var result = Groups.Get(groupQueryWithCustomAttributes);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_Owned_returns_groups()
        {
            //Arrange
            var groupQueryOwned = new GroupQuery
            {
                Owned = true
            };

            //Act
            var result = Groups.Get(groupQueryOwned);

            // Assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void Test_get_by_group_query_groupQuery_MinAccessLevel_returns_groups()
        {
            //Arrange
            var groupQuery10 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Guest
            };
            var groupQuery20 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Reporter
            };
            var groupQuery30 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Developer
            };
            var groupQuery40 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Master
            };
            var groupQuery50 = new GroupQuery
            {
                MinAccessLevel = AccessLevel.Owner
            };

            //Act
            var result10 = Groups.Get(groupQuery10);
            var result20 = Groups.Get(groupQuery20);
            var result30 = Groups.Get(groupQuery30);
            var result40 = Groups.Get(groupQuery40);
            var result50 = Groups.Get(groupQuery50);

            // Assert
            Assert.IsTrue(result10.Any());
            Assert.IsTrue(result20.Any());
            Assert.IsTrue(result30.Any());
            Assert.IsTrue(result40.Any());
            Assert.IsTrue(result50.Any());
        }

        private IGroupsClient Groups => Initialize.GitLabClient.Groups;

        private class GroupComparer : IEqualityComparer<Group>
        {
            public bool Equals(Group x, Group y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;

                return x.FullPath.Equals(y.FullPath, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Group group) => StringComparer.OrdinalIgnoreCase.GetHashCode(group.FullPath);
        }
    }
}
