using NUnit.Framework;

namespace NGitLab.Tests
{
    public class TriggerTests
    {
        private ITriggerClient _triggers;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _triggers = Initialize.GitLabClient.GetTriggers(Initialize.UnitTestProject.Id);
        }

        [Test, Timeout(10000)]
        public void Test_can_get_triggers_for_project()
        {
            var triggers = _triggers.All;

            Assert.IsNotNull(triggers);
            Assert.IsNotEmpty(triggers);
        }

        [Test, Timeout(10000)]
        public void Test_can_get_specific_trigger()
        {
            var trigger = _triggers[Initialize.UnitTestTrigger.Id];

            Assert.IsNotNull(trigger);
        }
    }
}
