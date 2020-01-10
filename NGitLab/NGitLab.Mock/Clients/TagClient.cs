using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class TagClient : ClientBase, ITagClient
    {
        private readonly int _projectId;

        public TagClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public IEnumerable<Tag> All => GetProject(_projectId, ProjectPermission.View).Repository.GetTags().Select(t => ToTagClient(t));

        public Tag Create(TagCreate tag)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var createdTag = project.Repository.CreateTag(Context.User, tag.Name, tag.Ref, tag.Message, tag.ReleaseDescription);

            return ToTagClient(createdTag);
        }

        public RealeaseInfo CreateRelease(string name, ReleaseCreate data)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var tag = project.Repository.CreateReleaseTag(name, data.Description);
            return new RealeaseInfo
            {
                TagName = tag.Name,
                Description = tag.ReleaseNotes,
            };
        }

        public void Delete(string name)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            project.Repository.DeleteTag(name);
        }

        public RealeaseInfo UpdateRelease(string name, ReleaseUpdate data)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var tag = project.Repository.UpdateReleaseTag(name, data.Description);

            return new RealeaseInfo
            {
                TagName = tag.Name,
                Description = tag.ReleaseNotes,
            };
        }

        public Tag ToTagClient(LibGit2Sharp.Tag tag)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commit = (LibGit2Sharp.Commit)tag.PeeledTarget;

            return new Models.Tag
            {
                Commit = commit.ToCommitInfo(),
                Name = tag.FriendlyName,
                Release = new Models.RealeaseInfo
                {
                    Description = project.Repository.GetReleaseTag(tag.FriendlyName)?.ReleaseNotes,
                    TagName = tag.FriendlyName,
                },
                Message = tag.Annotation?.Message,
            };
        }
    }
}
