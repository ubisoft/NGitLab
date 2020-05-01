using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class GroupVariableClientTests
    {
        private IGroupVariableClient _groupVariableClient;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _groupVariableClient = Initialize.GitLabClient.GetGroupVariableClient(Initialize.UnitTestGroup.Id);
        }

        [Test]
        public void Test_group_variables()
        {
            // Clear variables
            var variables = _groupVariableClient.All.ToList();
            variables.ForEach(b => _groupVariableClient.Delete(b.Key));
            variables = _groupVariableClient.All.ToList();
            Assert.AreEqual(0, variables.Count);

            // Create
            var variable = _groupVariableClient.Create(new VariableCreate
            {
                Key = "My_Key",
                Value = "My value",
                Protected = true,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value", variable.Value);
            Assert.AreEqual(true, variable.Protected);

            // Update
            variable = _groupVariableClient.Update(variable.Key, new VariableUpdate
            {
                Value = "My value edited",
                Protected = false,
            });

            Assert.AreEqual("My_Key", variable.Key);
            Assert.AreEqual("My value edited", variable.Value);
            Assert.AreEqual(false, variable.Protected);

            // Delete
            _groupVariableClient.Delete(variable.Key);

            variables = _groupVariableClient.All.ToList();
            Assert.IsEmpty(variables);

            // All
            _groupVariableClient.Create(new VariableCreate { Key = "Variable1", Value = "test" });
            _groupVariableClient.Create(new VariableCreate { Key = "Variable2", Value = "test" });
            _groupVariableClient.Create(new VariableCreate { Key = "Variable3", Value = "test" });
            variables = _groupVariableClient.All.ToList();
            Assert.AreEqual(3, variables.Count);
        }
    }
}
