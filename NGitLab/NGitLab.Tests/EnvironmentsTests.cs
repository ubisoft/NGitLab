using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class EnvironmentsTests
    {
        private static string GetSlugNameStart(string name)
        {
            if (name.Length > 17)
                name = name.Substring(0, 17);
            return name.Replace('_', '-');
        }

        [Test]
        public void CreateAndGetAll()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            var newEnvNameNoUrl = "env_test_name_no_url";
            var newEnvSlugNameNoUrlStart = GetSlugNameStart(newEnvNameNoUrl);
            var newEnvNameWithUrl = "env_test_name_with_url";
            var newEnvSlugNameWithUrlStart = GetSlugNameStart(newEnvNameWithUrl);
            var newEnvNameExternalUrl = Initialize.GitLabHookTest;

            // Validate environments doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameNoUrl || e.Name == newEnvNameWithUrl));

            // Create  and check return value
            var env = envClient.Create(newEnvNameNoUrl, externalUrl: null);
            StringAssert.AreEqualIgnoringCase(newEnvNameNoUrl, env.Name);
            StringAssert.StartsWith(newEnvSlugNameNoUrlStart, env.Slug);
            Assert.NotZero(env.Id);
            Assert.IsNull(env.ExternalUrl);

            // Create newEnvNameWithUrl and check return value
            env = envClient.Create(newEnvNameWithUrl, newEnvNameExternalUrl);
            StringAssert.AreEqualIgnoringCase(newEnvNameWithUrl, env.Name);
            StringAssert.StartsWith(newEnvSlugNameWithUrlStart, env.Slug);
            Assert.NotZero(env.Id);
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrl, env.ExternalUrl);

            // Validate new environment are present in All
            env = envClient.All.FirstOrDefault(e => e.Name == newEnvNameNoUrl);
            Assert.IsNotNull(env);
            StringAssert.StartsWith(newEnvSlugNameNoUrlStart, env.Slug);
            Assert.NotZero(env.Id);
            Assert.IsNull(env.ExternalUrl);

            env = envClient.All.FirstOrDefault(e => e.Name == newEnvNameWithUrl);
            Assert.IsNotNull(env);
            StringAssert.StartsWith(newEnvSlugNameWithUrlStart, env.Slug);
            Assert.NotZero(env.Id);
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrl, env.ExternalUrl);
        }

        [Test]
        public void Edit()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            string newEnvNameToEdit = "env_test_name_to_edit_init";
            string newEnvNameUpdated = "env_test_name_to_edit_updated";
            string newEnvSlugNameUpdatedStart = GetSlugNameStart(newEnvNameUpdated);
            string newEnvNameExternalUrlUpdated = Initialize.GitLabHookTest + "/updated";

            // Validate environments doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToEdit || e.Name == newEnvNameUpdated));

            // Create newEnvNameToEdit
            var env = envClient.Create(newEnvNameToEdit, externalUrl: null);
            int initialEnvId = env.Id;

            // Validate newEnvNameToEdit is present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToEdit));

            // Edit and check return value
            env = envClient.Edit(initialEnvId, newEnvNameUpdated, newEnvNameExternalUrlUpdated);
            StringAssert.AreEqualIgnoringCase(newEnvNameUpdated, env.Name);
            StringAssert.StartsWith(newEnvSlugNameUpdatedStart, env.Slug);
            Assert.AreEqual(initialEnvId, env.Id, "Environment Id should not change");
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrlUpdated, env.ExternalUrl);

            // Validate update is effective
            env = envClient.All.FirstOrDefault(e => e.Name == newEnvNameUpdated);
            Assert.IsNotNull(env);
            StringAssert.StartsWith(newEnvSlugNameUpdatedStart, env.Slug);
            Assert.AreEqual(initialEnvId, env.Id, "Environment Id should not change");
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrlUpdated, env.ExternalUrl);
        }

        [Test]
        public void Delete()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            string newEnvNameToDelete = "env_test_name_to_delete";

            // Validate environment doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToDelete));

            // Create newEnvNameToDelete
            var env = envClient.Create(newEnvNameToDelete, externalUrl: null);
            int initialEnvId = env.Id;

            // Validate newEnvNameToDelete is present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToDelete));

            // Delete
            envClient.Delete(initialEnvId);

            // Validate delete is effective
            Assert.IsNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToDelete));
        }

        [Test]
        public void Stop()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            var newEnvNameToStop = "env_test_name_to_stop";
            var newEnvSlugNameToStopStart = GetSlugNameStart(newEnvNameToStop);

            // Validate environment doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToStop));

            // Create newEnvNameToStop
            var env = envClient.Create(newEnvNameToStop, externalUrl: null);
            int initialEnvId = env.Id;

            // Validate newEnvNameToStop is present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToStop));

            // Stop and check return value
            env = envClient.Stop(initialEnvId);
            StringAssert.AreEqualIgnoringCase(newEnvNameToStop, env.Name);
            StringAssert.StartsWith(newEnvSlugNameToStopStart, env.Slug);
            Assert.AreEqual(initialEnvId, env.Id, "Environment Id should not change");
            Assert.IsNull(env.ExternalUrl);

            // Validate environment is still present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => e.Name == newEnvNameToStop));
        }
    }
}
