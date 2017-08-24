using System.Linq;
using NGitLab.Models;
using NUnit.Framework;
using Shouldly;

namespace NGitLab.Tests.MergeRequest {
    public class MergeRequestChangesClientTests {
        readonly IMergeRequestChangesClient mergeRequestChangesClient;

        public MergeRequestChangesClientTests() {
            var mergeRequestClient = _MergeRequestClientTests.MergeRequestClient;
            var mergeRequest = mergeRequestClient.AllInState(MergeRequestState.opened).FirstOrDefault(x => x.Title == "mergeme");
            mergeRequestChangesClient = _MergeRequestClientTests.MergeRequestClient.Changes(mergeRequest.Iid);
        }

        [Test]
        [Category("Server_Required")]
        public void Changes() {
            var changes = mergeRequestChangesClient.Changes;
            changes.ShouldNotBeNull();
            changes.Files.ShouldNotBeNull();
        }
    }
}