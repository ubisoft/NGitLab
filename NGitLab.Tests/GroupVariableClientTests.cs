using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class GroupVariableClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_group_variables()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var group = context.CreateGroup();
            var groupVariableClient = context.Client.GetGroupVariableClient(group.Id);

            // Create
            var variable = groupVariableClient.Create(new VariableCreate
            {
                Key = "My_Key",
                Value = "My value",
                Protected = true,
            });

            Assert.That(variable.Key, Is.EqualTo("My_Key"));
            Assert.That(variable.Value, Is.EqualTo("My value"));
            Assert.That(variable.Protected, Is.EqualTo(true));

            // Update
            variable = groupVariableClient.Update(variable.Key, new VariableUpdate
            {
                Value = "My value edited",
                Protected = false,
            });

            Assert.That(variable.Key, Is.EqualTo("My_Key"));
            Assert.That(variable.Value, Is.EqualTo("My value edited"));
            Assert.That(variable.Protected, Is.EqualTo(false));

            // Delete
            groupVariableClient.Delete(variable.Key);

            var variables = groupVariableClient.All.ToList();
            Assert.That(variables, Is.Empty);

            // All
            groupVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test" });
            groupVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test" });
            groupVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test" });
            variables = groupVariableClient.All.ToList();
            Assert.That(variables, Has.Count.EqualTo(3));
        }
    }
}
