using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    class GroupsTests
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
            // Create
            var group = Groups.Create(new GroupCreate()
            {
                Name = "NewGroup",
                Path = "NewGroupPath",
                Visibility = VisibilityLevel.Internal
            });
            Assert.IsNotNull(group);
            Assert.AreEqual("NewGroup", group.Name);
            Assert.AreEqual("NewGroupPath", group.Path);
            Assert.AreEqual(VisibilityLevel.Internal, group.Visibility);

            // Search
            group = Groups.Search("NewGroup").FirstOrDefault();
            Assert.AreEqual("NewGroup", group.Name);
            Assert.AreEqual("NewGroupPath", group.Path);
            Assert.AreEqual(VisibilityLevel.Internal, group.Visibility);

            // Delete
            Groups.Delete(group.Id);
            group = Groups.Search("NewGroup").FirstOrDefault();
            Assert.IsNull(group);
        }

        private IGroupsClient Groups => Initialize.GitLabClient.Groups;
    }
}
