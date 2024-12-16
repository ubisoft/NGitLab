using System;
using System.Collections.Generic;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl;

public class WikiClient : IWikiClient
{
    private readonly API _api;
    private readonly string _projectPath;

    public WikiClient(API api, ProjectId projectId)
    {
        _api = api;
        _projectPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}";
    }

    public IEnumerable<WikiPage> All => _api.Get().GetAll<WikiPage>(_projectPath + "/wikis");

    public WikiPage this[string slug] => _api.Get().To<WikiPage>(_projectPath + "/wikis/" + WebUtility.UrlEncode(slug));

    public WikiPage Create(WikiPageCreate wikiPage)
    {
        if (wikiPage == null)
            throw new ArgumentNullException(nameof(wikiPage));

        return _api
            .Post().With(wikiPage)
            .To<WikiPage>(_projectPath + "/wikis");
    }

    public WikiPage Update(string slug, WikiPageUpdate wikiPage) => _api
        .Put().With(wikiPage)
        .To<WikiPage>(_projectPath + "/wikis/" + WebUtility.UrlEncode(slug));

    public void Delete(string slug) => _api
        .Delete()
        .Execute(_projectPath + "/wikis/" + WebUtility.UrlEncode(slug));
}
