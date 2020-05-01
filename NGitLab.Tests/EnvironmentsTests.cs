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
            Assert.IsNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameNoUrl, System.StringComparison.Ordinal) || string.Equals(e.Name, newEnvNameWithUrl, System.StringComparison.Ordinal)));

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
            env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameNoUrl, System.StringComparison.Ordinal));
            Assert.IsNotNull(env);
            StringAssert.StartsWith(newEnvSlugNameNoUrlStart, env.Slug);
            Assert.NotZero(env.Id);
            Assert.IsNull(env.ExternalUrl);

            env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameWithUrl, System.StringComparison.Ordinal));
            Assert.IsNotNull(env);
            StringAssert.StartsWith(newEnvSlugNameWithUrlStart, env.Slug);
            Assert.NotZero(env.Id);
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrl, env.ExternalUrl);
        }

        [Test]
        public void Edit()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            var newEnvNameToEdit = "env_test_name_to_edit_init";
            var newEnvNameUpdated = "env_test_name_to_edit_updated";
            var newEnvSlugNameUpdatedStart = GetSlugNameStart(newEnvNameUpdated);
            var newEnvNameExternalUrlUpdated = Initialize.GitLabHookTest + "/updated";

            // Validate environments doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToEdit, System.StringComparison.Ordinal) || string.Equals(e.Name, newEnvNameUpdated, System.StringComparison.Ordinal)));

            // Create newEnvNameToEdit
            var env = envClient.Create(newEnvNameToEdit, externalUrl: null);
            var initialEnvId = env.Id;

            // Validate newEnvNameToEdit is present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToEdit, System.StringComparison.Ordinal)));

            // Edit and check return value
            env = envClient.Edit(initialEnvId, newEnvNameUpdated, newEnvNameExternalUrlUpdated);
            StringAssert.AreEqualIgnoringCase(newEnvNameUpdated, env.Name);
            StringAssert.StartsWith(newEnvSlugNameUpdatedStart, env.Slug);
            Assert.AreEqual(initialEnvId, env.Id, "Environment Id should not change");
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrlUpdated, env.ExternalUrl);

            // Validate update is effective
            env = envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameUpdated, System.StringComparison.Ordinal));
            Assert.IsNotNull(env);
            StringAssert.StartsWith(newEnvSlugNameUpdatedStart, env.Slug);
            Assert.AreEqual(initialEnvId, env.Id, "Environment Id should not change");
            StringAssert.AreEqualIgnoringCase(newEnvNameExternalUrlUpdated, env.ExternalUrl);
        }

        [Test]
        public void Delete()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            var newEnvNameToDelete = "env_test_name_to_delete";

            // Validate environment doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, System.StringComparison.Ordinal)));

            // Create newEnvNameToDelete
            var env = envClient.Create(newEnvNameToDelete, externalUrl: null);
            var initialEnvId = env.Id;

            // Validate newEnvNameToDelete is present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, System.StringComparison.Ordinal)));

            // Delete
            envClient.Delete(initialEnvId);

            // Validate delete is effective
            Assert.IsNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToDelete, System.StringComparison.Ordinal)));
        }

        [Test]
        public void Stop()
        {
            var envClient = Initialize.GitLabClient.GetEnvironmentClient(Initialize.UnitTestProject.Id);
            var newEnvNameToStop = "env_test_name_to_stop";
            var newEnvSlugNameToStopStart = GetSlugNameStart(newEnvNameToStop);

            // Validate environment doesn't exist yet
            Assert.IsNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToStop, System.StringComparison.Ordinal)));

            // Create newEnvNameToStop
            var env = envClient.Create(newEnvNameToStop, externalUrl: null);
            var initialEnvId = env.Id;

            // Validate newEnvNameToStop is present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToStop, System.StringComparison.Ordinal)));

            // Stop and check return value
            env = envClient.Stop(initialEnvId);
            StringAssert.AreEqualIgnoringCase(newEnvNameToStop, env.Name);
            StringAssert.StartsWith(newEnvSlugNameToStopStart, env.Slug);
            Assert.AreEqual(initialEnvId, env.Id, "Environment Id should not change");
            Assert.IsNull(env.ExternalUrl);

            // Validate environment is still present
            Assert.IsNotNull(envClient.All.FirstOrDefault(e => string.Equals(e.Name, newEnvNameToStop, System.StringComparison.Ordinal)));
        }
    }
}
