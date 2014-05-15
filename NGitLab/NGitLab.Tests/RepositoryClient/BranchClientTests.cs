using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests.RepositoryClient
{
    public class BranchClientTests
    {
        private readonly IBranchClient _branches;

        public BranchClientTests()
        {
            _branches = _RepositoryClientTests.RepositoryClient.Branches;
        }

        [Test]
        public void GetAll()
        {
            CollectionAssert.IsNotEmpty(_branches.All.ToArray());
        }

        [Test]
        public void GetByName()
        {
            var branch = _branches["master"];
            Assert.IsNotNull(branch);
            Assert.IsNotNull(branch.Name);
        }
    }
}