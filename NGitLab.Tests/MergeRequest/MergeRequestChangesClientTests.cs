using System;
using System.Threading.Tasks;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MergeRequestChangesClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task GetChangesOnMergeRequest()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = await context.CreateMergeRequestAsync();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var mergeRequestChanges = mergeRequestClient.Changes(mergeRequest.Iid);

        var changes = await GitLabTestContext.RetryUntilAsync(
            () => mergeRequestChanges.MergeRequestChange.Changes,
            changes => changes.Length != 0,
            TimeSpan.FromSeconds(10));

        Assert.That(changes, Has.Length.EqualTo(1));
        Assert.That(changes[0].AMode, Is.EqualTo(100644));
        Assert.That(changes[0].BMode, Is.EqualTo(100644));
        Assert.That(changes[0].DeletedFile, Is.False);
        Assert.That(changes[0].NewFile, Is.False);
        Assert.That(changes[0].RenamedFile, Is.False);
        Assert.That(changes[0].Diff, Is.EqualTo("@@ -1 +1 @@\n-this project should only live during the unit tests, you can delete if you find some\n\\ No newline at end of file\n+test2\n\\ No newline at end of file\n"));
    }
}
