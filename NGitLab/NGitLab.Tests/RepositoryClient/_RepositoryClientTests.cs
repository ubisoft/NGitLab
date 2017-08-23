using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient {
    [SetUpFixture]
    public class _RepositoryClientTests {
        public static IRepositoryClient RepositoryClient;
        Project project;

        [OneTimeSetUp]
        public void SetUp() {
            project = Config.Connect().Projects.Owned().FirstOrDefault();
            RepositoryClient = Config.Connect().GetRepository(project.Id);
        }
    }
}