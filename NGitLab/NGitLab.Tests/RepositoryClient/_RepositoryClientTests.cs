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
            _project = Config.Connect().Projects.Create(
                new ProjectCreate
                {
                    Name = "RepositoryClientTests"
                }
                );

            RepositoryClient = Config.Connect().GetRepository(_project.Id);
        }

        [TearDown]
        public void TearDown()
        {
            Config.Connect().Projects.Delete(_project.Id);
        }
    }
}