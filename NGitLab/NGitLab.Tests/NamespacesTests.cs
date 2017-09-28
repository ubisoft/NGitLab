using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    class NamespacesTests
    {
        [Test]
        public void Test_namespaces_contains_a_group()
        {
            var group = Namespaces.Accessible.FirstOrDefault(g => g.Path.Equals(Initialize.UnitTestGroup.Path, StringComparison.OrdinalIgnoreCase));
            Assert.IsNotNull(group);
            Assert.AreEqual(Namespace.Type.Group, group.GetKind());
        }

        [Test]
        public void Test_namespaces_contains_a_user()
        {
            var user = Namespaces.Accessible.FirstOrDefault(g => g.Path.Equals("robot", StringComparison.OrdinalIgnoreCase));
            Assert.IsNotNull(user);
            Assert.AreEqual(Namespace.Type.User, user.GetKind());
        }

        [Test]
        public void Test_namespaces_search_for_user()
        {
            var user = Namespaces.Search("robot");
            Assert.IsNotNull(user);
        }

        [Test]
        public void Test_namespaces_search_for_group()
        {
            var user = Namespaces.Search("TestGroup");
            Assert.IsNotNull(user);
        }

        private INamespacesClient Namespaces => Initialize.GitLabClient.Namespaces;
    }
}