using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.MergeRequest {
    [SetUpFixture]
    public class _MergeRequestClientTests {
        public static IMergeRequestClient MergeRequestClient;
        public static Project Project;

        [OneTimeSetUp]
        public void SetUp() {
            Project = Config.Connect().Projects.Owned().First(project => project.Name == "mergeme");
            MergeRequestClient = Config.Connect().GetMergeRequest(Project.Id);
        }
    }
}