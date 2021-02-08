using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class NamespacesTests
    {
        [Test]
        public async Task Test_namespaces_contains_a_group()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var group = context.CreateGroup();
            var namespacesClient = context.Client.Namespaces;

            var groupSearch = namespacesClient.Accessible.FirstOrDefault(g => g.Path.Equals(group.Path, StringComparison.Ordinal));
            Assert.IsNotNull(group);
            Assert.AreEqual(Namespace.Type.Group, groupSearch.GetKind());
        }

        [Test]
        public async Task Test_namespaces_contains_a_user()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var namespacesClient = context.Client.Namespaces;

            var user = namespacesClient.Accessible.FirstOrDefault(g => g.Path.Equals(context.Client.Users.Current.Username, StringComparison.Ordinal));
            Assert.IsNotNull(user);
            Assert.AreEqual(Namespace.Type.User, user.GetKind());
        }

        [Test]
        public async Task Test_namespaces_search_for_user()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var namespacesClient = context.Client.Namespaces;

            var ns = namespacesClient.Search(context.Client.Users.Current.Username).First();
            Assert.AreEqual(Namespace.Type.User, ns.GetKind());
        }

        [Test]
        public async Task Test_namespaces_search_for_group()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var group = context.CreateGroup();
            var namespacesClient = context.Client.Namespaces;

            var user = namespacesClient.Search(group.Name);
            Assert.IsNotNull(user);
        }
    }
}
