using System.Linq;
using System.Threading.Tasks;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class TriggerTests
    {
        [Test]
        [Timeout(10000)]
        public async Task Test_can_get_triggers_for_project()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var triggersClient = context.Client.GetTriggers(project.Id);

            var createdTrigger = triggersClient.Create("Unit_Test_Description");
            var trigger = triggersClient[createdTrigger.Id];

            Assert.AreEqual("Unit_Test_Description", trigger.Description);

            var triggers = triggersClient.All.Take(10).ToArray();

            Assert.IsNotNull(triggers);
            Assert.IsNotEmpty(triggers);
        }
    }
}
