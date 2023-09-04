using System.Linq;
using NGitLab.Mock.Config;
using NUnit.Framework;

namespace NGitLab.Mock.Tests
{
    public class MembersMockTests
    {
        [Test]
        public void Test_members_group_all_direct([Values] bool isDefault)
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                    .WithGroup("G1", 1, addDefaultUserAsMaintainer: true)
                    .WithGroup("G2", 2, @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
                .BuildServer();

            var client = server.CreateClient("user1");
            var members = isDefault
                ? client.Members.OfGroup("2")
                : client.Members.OfGroup("2", includeInheritedMembers: false);

            Assert.AreEqual(1, members.Count(), "Membership found are invalid");
        }

        [Test]
        public void Test_members_group_all_inherited()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithProject("Test")
                    .WithGroup("G1", 1, configure: g => g.WithUserPermission("user1", Models.AccessLevel.Maintainer))
                    .WithGroup("G2", 2, @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
                .BuildServer();

            var client = server.CreateClient("user1");
            var members = client.Members.OfGroup("2", includeInheritedMembers: true);

            Assert.AreEqual(2, members.Count(), "Membership found are invalid");
        }

        [Test]
        public void Test_members_project_all_direct([Values] bool isDefault)
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithUser("user3")
                    .WithGroup("G1", 1, addDefaultUserAsMaintainer: true)
                    .WithGroup("G2", 2, @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
                    .WithProject("Project", @namespace: "G1", configure: g =>
                        g.WithUserPermission("user3", Models.AccessLevel.Maintainer)
                         .WithGroupPermission("G2", Models.AccessLevel.Developer))
                .BuildServer();

            var client = server.CreateClient("user1");
            var members = isDefault
                ? client.Members.OfProject("1")
                : client.Members.OfProject("1", includeInheritedMembers: false);

            Assert.AreEqual(1, members.Count(), "Membership found are invalid");
        }

        [Test]
        public void Test_members_project_all_inherited()
        {
            using var server = new GitLabConfig()
                .WithUser("user1", isDefault: true)
                .WithUser("user2")
                .WithUser("user3")
                    .WithGroup("G1", addDefaultUserAsMaintainer: true)
                    .WithGroup("G2", @namespace: "G1", configure: g => g.WithUserPermission("user2", Models.AccessLevel.Maintainer))
                    .WithProject("Project", 1, @namespace: "G1", configure: g =>
                        g.WithUserPermission("user3", Models.AccessLevel.Maintainer)
                         .WithGroupPermission("G1/G2", Models.AccessLevel.Developer))
                .BuildServer();

            var client = server.CreateClient("user1");
            var members = client.Members.OfProject("1", includeInheritedMembers: true);

            Assert.AreEqual(3, members.Count(), "Membership found are invalid");
        }
    }
}
