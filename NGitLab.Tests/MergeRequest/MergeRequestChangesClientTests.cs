using System.Threading.Tasks;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class MergeRequestChangesClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task GetChangesOnMergeRequest()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var (project, mergeRequest) = context.CreateMergeRequest();
            var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
            var mergeRequestChanges = mergeRequestClient.Changes(mergeRequest.Iid);
            var changes = mergeRequestChanges.MergeRequestChange.Changes;
            Assert.AreEqual(1, changes.Length);
            Assert.AreEqual(100644, changes[0].AMode);
            Assert.AreEqual(100644, changes[0].BMode);
            Assert.IsFalse(changes[0].DeletedFile);
            Assert.IsFalse(changes[0].NewFile);
            Assert.IsFalse(changes[0].RenamedFile);
            Assert.AreEqual("@@ -1 +1 @@\n-test\n\\ No newline at end of file\n+test2\n\\ No newline at end of file\n", changes[0].Diff);
        }
    }
}
