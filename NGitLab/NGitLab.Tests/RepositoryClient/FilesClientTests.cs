using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class FilesClientTests
    {
        private readonly IBranchClient _client;

        public FilesClientTests()
        {
            _client = _RepositoryClientTests.RepositoryClient.Branches;
        }

        [Test]
        public void CreateUpdateDelete()
        {
            var branches = _client.All.ToArray();
        }
    }
}