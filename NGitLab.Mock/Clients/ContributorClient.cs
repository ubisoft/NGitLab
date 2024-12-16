using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ContributorClient : ClientBase, IContributorClient
{
    private readonly long _projectId;

    public ContributorClient(ClientContext context, long projectId)
        : base(context)
    {
        _projectId = projectId;
    }

    public IEnumerable<Contributor> All
    {
        get
        {
            // Note: Counting the number of addition / deletion is too slow.
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                if (project.Repository.IsEmpty)
                    return Enumerable.Empty<Contributor>();

                var result = new ConcurrentDictionary<string, Contributor>(StringComparer.OrdinalIgnoreCase);
                foreach (var commit in project.Repository.GetCommits(project.DefaultBranch))
                {
                    var contributor = result.GetOrAdd(commit.Author.Email, email => new Contributor() { Email = email });

                    contributor.Name ??= commit.Author.Name;
                    contributor.Commits++;
                }

                return result.Values.ToArray();
            }
        }
    }
}
