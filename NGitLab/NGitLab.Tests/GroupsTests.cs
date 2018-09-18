using System;
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

                TimeSpan timeout = TimeSpan.FromSeconds(45);
                if (sw.Elapsed > timeout)
                {
                    CollectionAssert.IsEmpty(groups, $"Group was not deleted in the allotted time of {timeout.TotalSeconds:0}seconds");
                }
            }
        }

        private IGroupsClient Groups => Initialize.GitLabClient.Groups;
    }
}
