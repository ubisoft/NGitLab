using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.RepositoryClient {
    public class JobsClientTests {
        readonly IRepositoryClient repo;

        public JobsClientTests() {
            var project = Config.Connect().Projects.Owned().FirstOrDefault(x => x.Name == "pipelines");
            repo = Config.Connect().GetRepository(project.Id);
        }

        [Test]
        [Category("Server_Required")]
        public void GetAll() {
            var pipelines = repo.Jobs.All();
            pipelines.ShouldNotBeEmpty();
        }
    }
}
