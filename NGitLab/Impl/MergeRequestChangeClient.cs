using System.Globalization;
using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab;

/// <summary>
/// Retrieves changes on a specific Merge Request
/// </summary>
/// <see cref="https://docs.gitlab.com/ee/api/merge_requests.html#get-single-mr-changes"/>
public class MergeRequestChangeClient : IMergeRequestChangeClient
{
    private readonly API _api;
    private readonly string _changesPath;

    public MergeRequestChangeClient(API api, string projectPath, long mergeRequestIid)
    {
        var iid = mergeRequestIid.ToString(CultureInfo.InvariantCulture);
        _api = api;
        _changesPath = projectPath + "/merge_requests/" + iid + "/changes";
    }

    public MergeRequestChange MergeRequestChange => _api.Get().To<MergeRequestChange>(_changesPath);
}
