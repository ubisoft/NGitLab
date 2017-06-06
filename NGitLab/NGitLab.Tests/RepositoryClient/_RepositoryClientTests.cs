using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    [SetUpFixture]
    public class _RepositoryClientTests
    {
        public static IRepositoryClient RepositoryClient;
        private Project _project;

        [OneTimeSetUp]
        public void SetUp()
        {
            _project = Config.Connect().Projects.Owned().FirstOrDefault();
            RepositoryClient = Config.Connect().GetRepository(_project.Id);
        }
    }
}