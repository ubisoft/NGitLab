using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class FilesClientTests
    {
        private IRepositoryClient _repositoryClient;

        [SetUp]
        public void Setup()
        {
            var project = Initialize.GitLabClient.Projects.Owned.First();
            _repositoryClient = Initialize.GitLabClient.GetRepository(project.Id);
        }
    }
}