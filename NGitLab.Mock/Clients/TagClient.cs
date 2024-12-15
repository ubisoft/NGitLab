using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;
using Commit = LibGit2Sharp.Commit;

namespace NGitLab.Mock.Clients;

internal sealed class TagClient : ClientBase, ITagClient
{
    private readonly long _projectId;

    public TagClient(ClientContext context, long projectId)
        : base(context)
    {
        _projectId = projectId;
    }

    public IEnumerable<Tag> All
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return GetProject(_projectId, ProjectPermission.View).Repository.GetTags().Select(t => ToTagClient(t)).ToList();
            }
        }
    }

    public Tag Create(TagCreate tag)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var createdTag = project.Repository.CreateTag(Context.User, tag.Name, tag.Ref, tag.Message);

            return ToTagClient(createdTag);
        }
    }

    public Task<Tag> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var mockTag = project.Repository.GetTags().FirstOrDefault(t => t.FriendlyName.Equals(name, StringComparison.Ordinal));
            if (mockTag is null)
                throw new GitLabException() { StatusCode = HttpStatusCode.NotFound };
            return Task.FromResult(ToTagClient(mockTag));
        }
    }

    public void Delete(string name)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            project.Repository.DeleteTag(name);
        }
    }

    public Tag ToTagClient(LibGit2Sharp.Tag tag)
    {
        var project = GetProject(_projectId, ProjectPermission.Contribute);
        var commit = (Commit)tag.PeeledTarget;

        return new Tag
        {
            Commit = commit.ToCommitInfo(),
            Name = tag.FriendlyName,
            Release = new Models.ReleaseInfo
            {
                Description = project.Releases.GetByTagName(tag.FriendlyName)?.Description,
                TagName = tag.FriendlyName,
            },
            Message = tag.Annotation?.Message,
        };
    }

    public GitLabCollectionResponse<Models.Tag> GetAsync(TagQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var result = GetProject(_projectId, ProjectPermission.View).Repository.GetTags();
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Sort))
                    throw new NotImplementedException();

                if (!string.IsNullOrEmpty(query.OrderBy))
                    throw new NotImplementedException();
            }

            return GitLabCollectionResponse.Create(result.Select(tag => ToTagClient(tag)).ToArray());
        }
    }
}
