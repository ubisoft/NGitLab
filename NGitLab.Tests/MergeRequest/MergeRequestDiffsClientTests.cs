using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MergeRequestDiffsClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task GetDiffsAsync_returns_changed_files()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mrClient = context.Client.GetMergeRequest(project.Id);

        var diffs = await mrClient.GetDiffsAsync(mergeRequest.Iid, query: null).ToListAsync();

        Assert.That(diffs, Has.Count.GreaterThan(0), "At least one diff should be returned");
        Assert.That(diffs[0].OldPath, Is.Not.Null.And.Not.Empty);
        Assert.That(diffs[0].NewPath, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetDiffsAsync_with_unidiff_returns_changed_files()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mrClient = context.Client.GetMergeRequest(project.Id);

        var query = new MergeRequestDiffQuery { Unidiff = true };
        var diffs = await mrClient.GetDiffsAsync(mergeRequest.Iid, query).ToListAsync();

        Assert.That(diffs, Has.Count.GreaterThan(0), "At least one diff should be returned with unidiff=true");
        Assert.That(diffs[0].OldPath, Is.Not.Null.And.Not.Empty);
        Assert.That(diffs[0].NewPath, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetDiffsAsync_unidiff_format_starts_with_unified_diff_header()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mrClient = context.Client.GetMergeRequest(project.Id);

        var plainDiffs = await mrClient.GetDiffsAsync(mergeRequest.Iid, query: null).ToListAsync();
        var unifiedDiffs = await mrClient.GetDiffsAsync(mergeRequest.Iid, new MergeRequestDiffQuery { Unidiff = true }).ToListAsync();

        Assert.That(plainDiffs, Has.Count.EqualTo(unifiedDiffs.Count), "Both queries should return the same number of files");

        // Unified diff format uses "--- a/..." / "+++ b/..." headers; the plain GitLab format does not.
        var firstUnified = unifiedDiffs[0].Difference;
        Assert.That(firstUnified, Does.StartWith("---").Or.StartWith("diff --git"),
            "Unified diff content should start with standard unified-diff markers");
    }
}
