using System.Collections.Generic;
using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Impl;

public class MergeRequestCommentClient : IMergeRequestCommentClient
{
    private readonly API _api;
    private readonly string _notesPath;
    private readonly string _discussionsPath;

    public MergeRequestCommentClient(API api, string projectPath, long mergeRequestIid)
    {
        _api = api;
        _notesPath = projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/notes";
        _discussionsPath = projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/discussions";
    }

    public IEnumerable<MergeRequestComment> All => _api.Get().GetAll<MergeRequestComment>(_notesPath);

    public IEnumerable<MergeRequestDiscussion> Discussions => _api.Get().GetAll<MergeRequestDiscussion>(_discussionsPath);

    public MergeRequestComment Add(MergeRequestComment comment) => _api.Post().With(comment).To<MergeRequestComment>(_notesPath);

    public MergeRequestComment Add(MergeRequestCommentCreate comment) => _api.Post().With(comment).To<MergeRequestComment>(_notesPath);

    public MergeRequestComment Add(string discussionId, MergeRequestCommentCreate comment) => _api.Post().With(comment).To<MergeRequestComment>(_discussionsPath + "/" + discussionId + "/notes");

    public MergeRequestComment Edit(long id, MergeRequestCommentEdit comment) => _api.Put().With(comment).To<MergeRequestComment>(_notesPath + "/" + id.ToString(CultureInfo.InvariantCulture));

    public void Delete(long id) => _api.Delete().Execute(_notesPath + "/" + id.ToString(CultureInfo.InvariantCulture));

    public IEnumerable<MergeRequestComment> Get(MergeRequestCommentQuery query)
    {
        var url = _notesPath;
        url = Utils.AddOrderBy(url, query.OrderBy, supportKeysetPagination: false);
        url = Utils.AddParameter(url, "sort", query.Sort);
        url = Utils.AddParameter(url, "page", query.Page);
        url = Utils.AddParameter(url, "per_page", query.PerPage);
        return _api.Get().To<IEnumerable<MergeRequestComment>>(url);
    }
}
