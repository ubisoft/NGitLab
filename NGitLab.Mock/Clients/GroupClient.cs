using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

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
            using (Context.BeginOperationScope())
            {
                var group = Server.AllGroups.FirstOrDefault(g => g.Id == id);
                if (group == null || !group.CanUserViewGroup(Context.User))
                    throw new GitLabNotFoundException();

                return group.ToClientGroup(Context.User);
            }
        }
    }

    public Models.Group this[string fullPath]
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                var group = Server.AllGroups.FindGroup(fullPath);
                if (group == null || !group.CanUserViewGroup(Context.User))
                    throw new GitLabNotFoundException();

                return group.ToClientGroup(Context.User);
            }
        }
    }

    public IEnumerable<Models.Group> Accessible
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return Server.AllGroups.Where(group => group.CanUserViewGroup(Context.User)).Select(group => group.ToClientGroup(Context.User)).ToList();
            }
        }
    }

    public Models.Group Create(GroupCreate group)
    {
        using (Context.BeginOperationScope())
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

            return newGroup.ToClientGroup(Context.User);
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Group> CreateAsync(GroupCreate group, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Create(group);
    }

    public void Delete(int id)
    {
        using (Context.BeginOperationScope())
        {
            var group = Server.AllGroups.FirstOrDefault(g => g.Id == id);
            if (group == null || !group.CanUserViewGroup(Context.User))
                throw new GitLabNotFoundException();

            if (!group.CanUserDeleteGroup(Context.User))
                throw new GitLabForbiddenException();

            group.ToClientGroup(Context.User);
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        Delete(id);
    }

    public IEnumerable<Models.Group> Get(GroupQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var groups = Server.AllGroups;
            if (query != null)
            {
                if (query.SkipGroups != null && query.SkipGroups.Length > 0)
                {
                    groups = groups.Where(g => !query.SkipGroups.Contains(g.Id));
                }

                if (query.Owned is true)
                {
                    groups = groups.Where(g => g.IsUserOwner(Context.User));
                }

                if (query.MinAccessLevel != null)
                {
                    groups = groups.Where(g => g.GetEffectivePermissions().GetAccessLevel(Context.User) >= query.MinAccessLevel);
                }

                if (!string.IsNullOrEmpty(query.Search))
                    throw new NotImplementedException();
            }

            return groups.Select(g => g.ToClientGroup(Context.User)).ToArray();
        }
    }

    public GitLabCollectionResponse<Models.Group> GetAsync(GroupQuery query)
    {
        return GitLabCollectionResponse.Create(Get(query));
    }

    public async Task<Models.Group> GetByFullPathAsync(string fullPath, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return this[fullPath];
    }

    public async Task<Models.Group> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return this[id];
    }

    public void Restore(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Models.Group> Search(string search)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<Models.Group> SearchAsync(string search)
    {
        return GitLabCollectionResponse.Create(Search(search));
    }

    public IEnumerable<Models.Project> SearchProjects(int groupId, string search)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<Models.Project> GetProjectsAsync(int groupId, GroupProjectsQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var group = Server.AllGroups.FirstOrDefault(g => g.Id == groupId);
            if (group == null || !group.CanUserViewGroup(Context.User))
                throw new GitLabNotFoundException();

            var projects = query?.IncludeSubGroups is true ? group.AllProjects : group.Projects;

            if (query != null)
            {
                if (query.Archived != null)
                {
                    projects = projects.Where(project => project.Archived == query.Archived);
                }

                if (query.Owned != null)
                {
                    projects = projects.Where(project => project.IsUserOwner(Context.User));
                }

                if (query.Visibility != null)
                {
                    projects = projects.Where(project => project.Visibility >= query.Visibility.Value);
                }

                if (!string.IsNullOrEmpty(query.Search))
                    throw new NotImplementedException();

                if (query.MinAccessLevel != null)
                    throw new NotImplementedException();
            }

            projects = projects.Where(project => project.CanUserViewProject(Context.User));
            return GitLabCollectionResponse.Create(projects.Select(project => project.ToClientProject(Context.User)).ToArray());
        }
    }

    public Models.Group Update(int id, GroupUpdate groupUpdate)
    {
        using (Context.BeginOperationScope())
        {
            var group = Server.AllGroups.FindById(id);
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

            return group.ToClientGroup(Context.User);
        }
    }

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Group> UpdateAsync(int id, GroupUpdate groupUpdate, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Update(id, groupUpdate);
    }

    public GitLabCollectionResponse<Models.Group> GetSubgroupsByIdAsync(int id, SubgroupQuery query = null)
    {
        using (Context.BeginOperationScope())
        {
            var parentGroup = this[id];
            var groups = Server.AllGroups;
            if (query != null)
            {
                if (query.SkipGroups != null && query.SkipGroups.Length > 0)
                {
                    groups = groups.Where(g => !query.SkipGroups.Contains(g.Id));
                }

                if (query.Owned is true)
                {
                    groups = groups.Where(g => g.IsUserOwner(Context.User));
                }

                if (query.MinAccessLevel != null)
                {
                    groups = groups.Where(g => g.GetEffectivePermissions().GetAccessLevel(Context.User) >= query.MinAccessLevel);
                }

                if (!string.IsNullOrEmpty(query.Search))
                    throw new NotImplementedException();
            }

            var clientGroups = groups.Select(g => g.ToClientGroup(Context.User));

            return GitLabCollectionResponse.Create(clientGroups.Where(g => g.ParentId == parentGroup.Id));
        }
    }

    public GitLabCollectionResponse<Models.Group> GetSubgroupsByFullPathAsync(string fullPath, SubgroupQuery query = null)
    {
        using (Context.BeginOperationScope())
        {
            var parentGroup = this[fullPath];
            var groups = Server.AllGroups;
            if (query != null)
            {
                if (query.SkipGroups != null && query.SkipGroups.Length > 0)
                {
                    groups = groups.Where(g => !query.SkipGroups.Contains(g.Id));
                }

                if (query.Owned is true)
                {
                    groups = groups.Where(g => g.IsUserOwner(Context.User));
                }

                if (query.MinAccessLevel != null)
                {
                    groups = groups.Where(g => g.GetEffectivePermissions().GetAccessLevel(Context.User) >= query.MinAccessLevel);
                }

                if (!string.IsNullOrEmpty(query.Search))
                    throw new NotImplementedException();
            }

            var clientGroups = groups.Select(g => g.ToClientGroup(Context.User));

            return GitLabCollectionResponse.Create(clientGroups.Where(g => g.ParentId == parentGroup.Id));
        }
    }
}
