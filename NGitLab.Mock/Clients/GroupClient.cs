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
                Path = group.Path,
                Description = group.Description,
                Visibility = group.Visibility,
                LfsEnabled = group.LfsEnabled,
                RequestAccessEnabled = group.RequestAccessEnabled,
                SharedRunnersLimit = TimeSpan.FromMinutes(group.SharedRunnersMinutesLimit ?? 0),

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

    public void Delete(long id)
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
    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        Delete(id);
    }

    public IEnumerable<Models.Group> Accessible => Get(query: null);

    public IEnumerable<Models.Group> Get(GroupQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var groups = Server.AllGroups.Where(group => group.CanUserViewGroup(Context.User));

            if (query is not null)
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

                if (query.TopLevelOnly is true)
                {
                    groups = groups.Where(g => g.Parent is null);
                }

                if (!string.IsNullOrEmpty(query.Search))
                    throw new NotImplementedException();
            }

            return groups.Select(g => g.ToClientGroup(Context.User)).ToArray();
        }
    }

    public GitLabCollectionResponse<Models.Group> GetAsync(GroupQuery query) => GitLabCollectionResponse.Create(Get(query));

    public async Task<PagedResponse<Models.Group>> PageAsync(PageQuery<GroupQuery> query, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        var pageNum = Math.Max(1, query?.Page ?? PageQuery.FirstPage);
        var perPage = query?.PerPage ?? PageQuery.DefaultPerPage;
        if (perPage < PageQuery.MinPerPage)
        {
            // Max isn't enforced the same way
            throw new GitLabBadRequestException($"per_page value ({perPage}) is invalid: cannot be lower than MinPerPage ({PageQuery.MinPerPage})");
        }

        var all = Get(query?.Query).ToArray();
        var page = all
            .Skip((pageNum - 1) * perPage)
            .Take(perPage)
            .ToArray();
        return new(page, all.Length > PagedResponse.MaxQueryCountLimit ? null : all.Length);
    }

    public Models.Group this[long id] => GetGroup(id);

    public Models.Group this[string fullPath] => GetGroup(fullPath);

    public Models.Group GetGroup(GroupId id)
    {
        using (Context.BeginOperationScope())
        {
            var group = Server.AllGroups.FirstOrDefault(g => id.Equals(g.PathWithNameSpace, g.Id));

            if (group == null || !group.CanUserViewGroup(Context.User))
                throw new GitLabNotFoundException();

            return group.ToClientGroup(Context.User);
        }
    }

    public Task<Models.Group> GetByFullPathAsync(string fullPath, CancellationToken cancellationToken = default) => GetGroupAsync(fullPath, cancellationToken);

    public Task<Models.Group> GetByIdAsync(long id, CancellationToken cancellationToken = default) => GetGroupAsync(id, cancellationToken);

    [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
    public async Task<Models.Group> GetGroupAsync(GroupId groupId, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return GetGroup(groupId);
    }

    public void Restore(long id)
    {
        throw new NotImplementedException();
    }

    public async Task RestoreAsync(long id, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        Restore(id);
    }

    public IEnumerable<Models.Group> Search(string search)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<Models.Group> SearchAsync(string search)
    {
        return GitLabCollectionResponse.Create(Search(search));
    }

    public IEnumerable<Models.Project> SearchProjects(long groupId, string search) =>
        SearchProjectsAsync(groupId, new GroupProjectsQuery
        {
            Search = search,
        });

    public GitLabCollectionResponse<Models.Project> GetProjectsAsync(long groupId, GroupProjectsQuery query) => SearchProjectsAsync(groupId, query);

    public GitLabCollectionResponse<Models.Project> SearchProjectsAsync(GroupId groupId, GroupProjectsQuery query)
    {
        using (Context.BeginOperationScope())
        {
            var group = Server.AllGroups.FirstOrDefault(g => groupId.Equals(g.PathWithNameSpace, g.Id));

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

    public async Task<PagedResponse<Models.Project>> PageProjectsAsync(GroupId groupId, PageQuery<GroupProjectsQuery> query, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        var pageNum = Math.Max(1, query?.Page ?? PageQuery.FirstPage);
        var perPage = query?.PerPage ?? PageQuery.DefaultPerPage;
        if (perPage < 1)
        {
            throw new GitLabBadRequestException($"per_page value ({perPage}) is invalid: cannot be lower than MinPerPage ({PageQuery.MinPerPage})");
        }

        var all = SearchProjectsAsync(groupId, query?.Query).ToArray();
        var page = all
            .Skip((pageNum - 1) * perPage)
            .Take(perPage)
            .ToArray();
        return new(page, all.Length > PagedResponse.MaxQueryCountLimit ? null : all.Length);
    }

    public Models.Group Update(long id, GroupUpdate groupUpdate)
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
    public async Task<Models.Group> UpdateAsync(long id, GroupUpdate groupUpdate, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Update(id, groupUpdate);
    }

    public GitLabCollectionResponse<Models.Group> GetSubgroupsByIdAsync(long id, SubgroupQuery query = null) => GetSubgroupsAsync(id, query);

    public GitLabCollectionResponse<Models.Group> GetSubgroupsByFullPathAsync(string fullPath, SubgroupQuery query = null) => GetSubgroupsAsync(fullPath, query);

    public GitLabCollectionResponse<Models.Group> GetSubgroupsAsync(GroupId groupId, SubgroupQuery query = null)
    {
        using (Context.BeginOperationScope())
        {
            var parentGroup = GetGroup(groupId);
            var groups = Server.AllGroups.Where(group => group.CanUserViewGroup(Context.User));

            if (query is not null)
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

            groups = groups.Where(g =>
            {
                if (g.Parent is null)
                    return false;

                // is it a child of parentGroup...
                if (g.Parent.Id == parentGroup.Id)
                    return true;

                if (query?.IncludeDescendants is true)
                {
                    // is it a descendant of parentGroup...
                    var ancestor = g.Parent.Parent;
                    while (ancestor is not null)
                    {
                        if (ancestor.Id == parentGroup.Id)
                            return true;
                        ancestor = ancestor.Parent;
                    }
                }

                return false;
            });

            return GitLabCollectionResponse.Create(groups.Select(g => g.ToClientGroup(Context.User)));
        }
    }

    public async Task<PagedResponse<Models.Group>> PageSubgroupsAsync(GroupId groupId, PageQuery<SubgroupQuery> query, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        var pageNum = Math.Max(1, query?.Page ?? PageQuery.FirstPage);
        var perPage = query?.PerPage ?? PageQuery.DefaultPerPage;
        if (perPage < 1)
        {
            throw new GitLabBadRequestException($"per_page value ({perPage}) is invalid: cannot be lower than MinPerPage ({PageQuery.MinPerPage})");
        }

        var all = GetSubgroupsAsync(groupId, query?.Query).ToArray();
        var page = all
            .Skip((pageNum - 1) * perPage)
            .Take(perPage)
            .ToArray();
        return new(page, all.Length > PagedResponse.MaxQueryCountLimit ? null : all.Length);
    }
}
