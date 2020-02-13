using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class GroupClient : ClientBase, IGroupsClient
    {
        public GroupClient(ClientContext context)
            : base(context)
        {
        }

        public Models.Group this[int id]
        {
            get
            {
                var group = Server.AllGroups.FirstOrDefault(g => g.Id == id);
                if (group == null || !group.CanUserViewGroup(Context.User))
                    throw new GitLabNotFoundException();

                return group.ToClientGroup();
            }
        }

        public Models.Group this[string fullPath]
        {
            get
            {
                var group = Server.AllGroups.FindGroup(fullPath);
                if (group == null || !group.CanUserViewGroup(Context.User))
                    throw new GitLabNotFoundException();

                return group.ToClientGroup();
            }
        }

        public IEnumerable<Models.Group> Accessible
        {
            get
            {
                return Server.AllGroups.Where(group => group.CanUserViewGroup(Context.User)).Select(group => group.ToClientGroup());
            }
        }

        public Models.Group Create(GroupCreate group)
        {
            Group parentGroup = null;
            if (group.ParentId != null)
            {
                parentGroup = Server.AllGroups.FirstOrDefault(g => g.Id == group.ParentId.Value);
                if (parentGroup == null || !parentGroup.CanUserViewGroup(Context.User))
                    throw new GitLabNotFoundException();

                if (!parentGroup.CanUserAddGroup(Context.User))
                    throw new GitLabForbiddenException();
            }

            var newGroup = new Group
            {
                Name = group.Name,
                Description = group.Description,
                Visibility = group.Visibility,
                Permissions =
                {
                    new Permission(Context.User, AccessLevel.Owner),
                },
            };

            if (parentGroup != null)
            {
                parentGroup.Groups.Add(newGroup);
            }
            else
            {
                Server.Groups.Add(newGroup);
            }

            return newGroup.ToClientGroup();
        }

        public void Delete(int id)
        {
            var group = Server.AllGroups.FirstOrDefault(g => g.Id == id);
            if (group == null || !group.CanUserViewGroup(Context.User))
                throw new GitLabNotFoundException();

            if (!group.CanUserDeleteGroup(Context.User))
                throw new GitLabForbiddenException();

            group.ToClientGroup();
        }

        public IEnumerable<Models.Group> Get(GroupQuery query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Group> Search(string search)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Project> SearchProjects(int groupId, string search)
        {
            throw new NotImplementedException();
        }

        public Models.Group Update(int id, GroupUpdate groupUpdate)
        {
            var group = Server.AllGroups.FindGroupById(id);
            if (group == null || !group.CanUserViewGroup(Context.User))
                throw new GitLabNotFoundException();

            if (!group.CanUserEditGroup(Context.User))
                throw new GitLabForbiddenException();

            if (groupUpdate.Description != null)
            {
                group.Description = groupUpdate.Description;
            }

            if (groupUpdate.ExtraSharedRunnersMinutesLimit != null)
            {
                group.ExtraSharedRunnersLimit = TimeSpan.FromMinutes(groupUpdate.ExtraSharedRunnersMinutesLimit.Value);
            }

            if (groupUpdate.LfsEnabled != null)
            {
                group.LfsEnabled = groupUpdate.LfsEnabled.Value;
            }

            if (groupUpdate.Name != null)
            {
                group.Name = groupUpdate.Name;
            }

            if (groupUpdate.Path != null)
            {
                group.Path = groupUpdate.Path;
            }

            if (groupUpdate.RequestAccessEnabled != null)
            {
                group.RequestAccessEnabled = groupUpdate.RequestAccessEnabled.Value;
            }

            if (groupUpdate.SharedRunnersMinutesLimit != null)
            {
                group.SharedRunnersLimit = TimeSpan.FromMinutes(groupUpdate.SharedRunnersMinutesLimit.Value);
            }

            if (groupUpdate.Visibility != null)
            {
                group.Visibility = groupUpdate.Visibility.Value;
            }

            return group.ToClientGroup();
        }
    }
}
