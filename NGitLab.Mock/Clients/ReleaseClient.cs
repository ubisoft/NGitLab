using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<ReleaseInfo> All
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId, ProjectPermission.Contribute);
                    return project.Repository.GetReleases();
                }
            }
        }

        public ReleaseInfo Create(ReleaseCreate data)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                return project.Repository.CreateRelease(data.Name, data.Description);
            }
        }

        public void Delete(string name)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                project.Repository.DeleteRelease(name);
            }
        }

        public ReleaseInfo Update(ReleaseUpdate data)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                return project.Repository.UpdateRelease(data.Name, data.Description);
            }
        }

        public Tag ToReleaseClient(LibGit2Sharp.Tag tag)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commit = (LibGit2Sharp.Commit)tag.PeeledTarget;

            return new Models.Tag
            {
                Commit = commit.ToCommitInfo(),
                Name = tag.FriendlyName,
                Release = new Models.ReleaseInfo
                {
                    Description = project.Repository.GetRelease(tag.FriendlyName)?.Description,
                    TagName = tag.FriendlyName,
                },
                Message = tag.Annotation?.Message,
            };
        }
    }
}
