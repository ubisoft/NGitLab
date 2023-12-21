using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IWikiClient
{
    WikiPage this[string slug] { get; }

    IEnumerable<WikiPage> All { get; }

    WikiPage Create(WikiPageCreate wikiPage);

    void Delete(string slug);

    WikiPage Update(string slug, WikiPageUpdate wikiPage);
}
