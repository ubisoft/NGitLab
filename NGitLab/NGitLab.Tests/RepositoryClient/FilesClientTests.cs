using NUnit.Framework;
using System.Linq;

namespace NGitLab.Tests.RepositoryClient
{
    public class FilesClientTests
    {
        private readonly IFilesClient _client;

        private IRepositoryClient RepositoryClient;

        [SetUp]
        public void Setup()
        {
            var project = Initialize.GitLabClient.Projects.Owned.First();
            RepositoryClient = Initialize.GitLabClient.GetRepository(project.Id);
        }
    }
}