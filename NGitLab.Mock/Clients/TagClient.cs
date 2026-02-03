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
            IEnumerable<LibGit2Sharp.Tag> result = GetProject(_projectId, ProjectPermission.View).Repository.GetTags();
            if (query != null)
            {
                result = ApplyQuery(result, query.OrderBy, query.Sort);
            }

            return GitLabCollectionResponse.Create(result.Select(ToTagClient).ToArray());
        }

        static IEnumerable<LibGit2Sharp.Tag> ApplyQuery(IEnumerable<LibGit2Sharp.Tag> tags, string orderBy, string direction)
        {
            tags = orderBy switch
            {
                "name" => tags.OrderBy(t => t.FriendlyName, StringComparer.Ordinal),
                "version" => tags.OrderBy(t => t.FriendlyName, SemanticVersionComparer.Instance),
                null => tags,

                // LibGitSharp does not really expose tag creation time, so hard to sort using that annotation,
                "updated" => throw new NotSupportedException("Sorting by 'updated' is not supported since the info is not available in LibGit2Sharp."),
                _ => throw new NotSupportedException($"Sorting by '{orderBy}' is not supported."),
            };

            if (string.IsNullOrEmpty(direction))
                direction = "desc";

            return direction switch
            {
                "desc" => tags.Reverse(),
                "asc" => tags,
                _ => throw new NotSupportedException($"Sort direction must be 'asc' or 'desc', got '{direction}' instead"),
            };
        }
    }

    private sealed class SemanticVersionComparer : IComparer<string>
    {
        public static SemanticVersionComparer Instance { get; } = new();

        public int Compare(string x, string y)
        {
            var versionX = ParseVersion(x);
            var versionY = ParseVersion(y);

            var majorCmp = versionX.Major.CompareTo(versionY.Major);
            if (majorCmp != 0)
                return majorCmp;

            var minorCmp = versionX.Minor.CompareTo(versionY.Minor);
            if (minorCmp != 0)
                return minorCmp;

            return versionX.Patch.CompareTo(versionY.Patch);
        }

        private static (int Major, int Minor, int Patch) ParseVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return (0, 0, 0);

            // Strip leading 'v' or 'V' if present
            if (version.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                version = version[1..];

            if (version.IndexOf('-') is int dashIndex and not -1)
                version = version[..dashIndex];

            var parts = version.Split('.');
            var major = parts.Length > 0 && int.TryParse(parts[0], out var m) ? m : 0;
            var minor = parts.Length > 1 && int.TryParse(parts[1], out var n) ? n : 0;
            var patch = parts.Length > 2 && int.TryParse(parts[2], out var p) ? p : 0;

            return (major, minor, patch);
        }
    }
}
