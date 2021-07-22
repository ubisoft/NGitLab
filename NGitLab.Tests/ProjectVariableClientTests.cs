using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectVariableClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_project_variables()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectVariableClient = context.Client.GetProjectVariableClient(project.Id);

            // Create
            var variable = projectVariableClient.Create(new VariableCreate
            {
                Key = "My_Key",
                Value = "My value",
                Protected = true,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value", variable.Value);
            Assert.AreEqual(true, variable.Protected);

            // Update
            variable = projectVariableClient.Update(variable.Key, new VariableUpdate
            {
                Value = "My value edited",
                Protected = false,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value edited", variable.Value);
            Assert.AreEqual(false, variable.Protected);

            // Delete
            projectVariableClient.Delete(variable.Key);

            var variables = projectVariableClient.All.ToList();
            Assert.IsEmpty(variables);

            // All
            projectVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test" });
            projectVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test" });
            projectVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test" });
            variables = projectVariableClient.All.ToList();
            Assert.AreEqual(3, variables.Count);
        }
    }
}
