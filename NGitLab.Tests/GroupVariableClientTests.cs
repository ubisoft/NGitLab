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

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value", variable.Value);
            Assert.AreEqual(true, variable.Protected);

            // Update
            variable = groupVariableClient.Update(variable.Key, new VariableUpdate
            {
                Value = "My value edited",
                Protected = false,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value edited", variable.Value);
            Assert.AreEqual(false, variable.Protected);

            // Delete
            groupVariableClient.Delete(variable.Key);

            var variables = groupVariableClient.All.ToList();
            Assert.IsEmpty(variables);

            // All
            groupVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test" });
            groupVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test" });
            groupVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test" });
            variables = groupVariableClient.All.ToList();
            Assert.AreEqual(3, variables.Count);
        }
    }
}
