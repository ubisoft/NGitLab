using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectVariableClientTests
    {
        private IProjectVariableClient _projectVariableClient;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _projectVariableClient = Initialize.GitLabClient.GetProjectVariableClient(Initialize.UnitTestProject.Id);
        }

        [Test]
        public void Test_project_variables()
        {
            // Clear variables
            var variables = _projectVariableClient.All.ToList();
            variables.ForEach(b => _projectVariableClient.Delete(b.Key));
            variables = _projectVariableClient.All.ToList();
            Assert.AreEqual(0, variables.Count);

            // Create
            var variable = _projectVariableClient.Create(new VariableCreate
            {
                Key = "My_Key",
                Value = "My value",
                Protected = true,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value", variable.Value);
            Assert.AreEqual(true, variable.Protected);

            // Update
            variable = _projectVariableClient.Update(variable.Key, new VariableUpdate
            {
                Value = "My value edited",
                Protected = false,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value edited", variable.Value);
            Assert.AreEqual(false, variable.Protected);

            // Delete
            _projectVariableClient.Delete(variable.Key);

            variables = _projectVariableClient.All.ToList();
            Assert.IsEmpty(variables);

            // All
            _projectVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test" });
            _projectVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test" });
            _projectVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test" });
            variables = _projectVariableClient.All.ToList();
            Assert.AreEqual(3, variables.Count);
        }
    }
}
