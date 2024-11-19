using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

public class TagClient : ITagClient
{
    private readonly API _api;
    private readonly string _tagsPath;

    public TagClient(API api, string repoPath)
    {
        _api = api;
        _tagsPath = repoPath + "/tags";
    }

    public Tag Create(TagCreate tag)
    {
        return _api.Post().With(tag).To<Tag>(_tagsPath);
    }

    public Task<Tag> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<Tag>(_tagsPath + '/' + Uri.EscapeDataString(name), cancellationToken);
    }

    public void Delete(string name)
    {
        _api.Delete().Stream($"{_tagsPath}/{WebUtility.UrlEncode(name)}", _ => { });
    }

    public IEnumerable<Tag> All => _api.Get().GetAll<Tag>(_tagsPath + "?per_page=50");

    public GitLabCollectionResponse<Tag> GetAsync(TagQuery query)
    {
        var url = _tagsPath;
        if (query != null)
        {
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);
            url = Utils.AddParameter(url, "per_page", query.PerPage);
            url = Utils.AddParameter(url, "search", query.Search);
        }

        return _api.Get().GetAllAsync<Tag>(url);
    }
}
