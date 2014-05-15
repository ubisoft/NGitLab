using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    [SetUpFixture]
// ReSharper disable InconsistentNaming
    public class _RepositoryClientTests
// ReSharper restore InconsistentNaming
    {
        public static IRepositoryClient RepositoryClient;
        private Project _project;

        [SetUp]
        public void SetUp()
        {
            _project = Config.Connect().Projects.Owned.Single();
            RepositoryClient = Config.Connect().GetRepository(_project.Id);
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}