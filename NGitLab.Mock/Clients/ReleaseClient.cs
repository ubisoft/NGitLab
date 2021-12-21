using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ReleaseClient : ClientBase, IReleaseClient
    {
        private readonly int _projectId;

        public ReleaseClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public IEnumerable<Models.ReleaseInfo> All
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId, ProjectPermission.View);
                    return project.Releases.Select(r => r.ToReleaseClient());
                }
            }
        }

        public Models.ReleaseInfo this[string tagName]
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId, ProjectPermission.View);
                    var release = project.Releases.FirstOrDefault(r => r.TagName.Equals(tagName, StringComparison.Ordinal));

                    return release.ToReleaseClient();
                }
            }
        }

        public Models.ReleaseInfo Create(Models.ReleaseCreate data)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var release = project.Releases.Add(data.TagName, data.Name, data.Ref, data.Description, Context.User);
                return release.ToReleaseClient();
            }
        }

        public Models.ReleaseInfo Update(ReleaseUpdate data)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var release = project.Releases.GetByTagName(data.TagName);
                if (release == null)
                {
                    throw new GitLabNotFoundException();
                }

                if (data.Name != null)
                {
                    release.Name = data.Name;
                }

                if (data.Description != null)
                {
                    release.Description = data.Description;
                }

                if (data.ReleasedAt.HasValue)
                {
                    release.ReleasedAt = data.ReleasedAt.Value;
                }

                return release.ToReleaseClient();
            }
        }

        public void Delete(string tagName)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var release = project.Releases.FirstOrDefault(r => r.TagName.Equals(tagName, StringComparison.Ordinal));
                if (release == null)
                    throw new GitLabNotFoundException();

                project.Releases.Remove(release);
            }
        }

        public IReleaseLinkClient ReleaseLinks(string tagName)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
        public async Task<Models.ReleaseInfo> CreateAsync(ReleaseCreate data, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return Create(data);
        }
    }
}
