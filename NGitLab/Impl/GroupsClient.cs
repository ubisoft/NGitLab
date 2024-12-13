using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

/// <summary>
/// https://docs.gitlab.com/ce/api/groups.html
/// </summary>
public class GroupsClient : IGroupsClient
{
    private readonly API _api;

    public const string Url = Group.Url;

    public GroupsClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Group> Accessible => Get(query: null);

    public IEnumerable<Group> Get(GroupQuery query)
    {
        var url = CreateGetUrl(query);
        return _api.Get().GetAll<Group>(url);
    }

    public GitLabCollectionResponse<Group> GetAsync(GroupQuery query)
    {
        var url = CreateGetUrl(query);
        return _api.Get().GetAllAsync<Group>(url);
    }

    public Task<PagedResponse<Group>> PageAsync(PageQuery<GroupQuery> query, CancellationToken cancellationToken = default)
    {
        var url = CreateGetUrl(query?.Query, query?.Page, query?.PerPage);
        return _api.Get().PageAsync<Group>(url, cancellationToken);
    }

    private static string CreateGetUrl(GroupQuery query, int? page = null, int? perPage = null)
    {
        var url = Url;

        url = Utils.AddPageParams(url, page, perPage);

        if (query is null)
        {
            return url;
        }

        if (query.SkipGroups != null && query.SkipGroups.Length != 0)
        {
            foreach (var skipGroup in query.SkipGroups)
            {
                url = Utils.AddParameter(url, "skip_groups[]", skipGroup);
            }
        }

        if (query.AllAvailable != null)
        {
            url = Utils.AddParameter(url, "all_available", query.AllAvailable);
        }

        if (!string.IsNullOrEmpty(query.Search))
        {
            url = Utils.AddParameter(url, "search", query.Search);
        }

        url = Utils.AddOrderBy(url, query.OrderBy, supportKeysetPagination: page is null);

        if (query.Sort != null)
        {
            url = Utils.AddParameter(url, "sort", query.Sort);
        }

        if (query.Statistics != null)
        {
            url = Utils.AddParameter(url, "statistics", query.Statistics);
        }

        if (query.WithCustomAttributes != null)
        {
            url = Utils.AddParameter(url, "with_custom_attributes", query.WithCustomAttributes);
        }

        if (query.Owned != null)
        {
            url = Utils.AddParameter(url, "owned", query.Owned);
        }

        if (query.MinAccessLevel != null)
        {
            url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel);
        }

        if (query.TopLevelOnly != null)
        {
            url = Utils.AddParameter(url, "top_level_only", query.TopLevelOnly);
        }

        return url;
    }

    private static string CreateSubgroupGetUrl(GroupId id, SubgroupQuery query, int? page = null, int? perPage = null)
    {
        var url = $"{Url}/{id.ValueAsUriParameter()}/{(query?.IncludeDescendants == true ? "descendant_groups" : "subgroups")}";

        url = Utils.AddPageParams(url, page, perPage);

        if (query is null)
        {
            return url;
        }

        if (query.SkipGroups != null && query.SkipGroups.Length != 0)
        {
            foreach (var skipGroup in query.SkipGroups)
            {
                url = Utils.AddParameter(url, "skip_groups[]", skipGroup);
            }
        }

        if (query.AllAvailable != null)
        {
            url = Utils.AddParameter(url, "all_available", query.AllAvailable);
        }

        if (!string.IsNullOrEmpty(query.Search))
        {
            url = Utils.AddParameter(url, "search", query.Search);
        }

        url = Utils.AddOrderBy(url, query.OrderBy, supportKeysetPagination: page is null);

        if (query.Sort != null)
        {
            url = Utils.AddParameter(url, "sort", query.Sort);
        }

        if (query.Statistics != null)
        {
            url = Utils.AddParameter(url, "statistics", query.Statistics);
        }

        if (query.WithCustomAttributes != null)
        {
            url = Utils.AddParameter(url, "with_custom_attributes", query.WithCustomAttributes);
        }

        if (query.Owned != null)
        {
            url = Utils.AddParameter(url, "owned", query.Owned);
        }

        if (query.MinAccessLevel != null)
        {
            url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel);
        }

        return url;
    }

    public IEnumerable<Group> Search(string search) =>
        _api.Get().GetAll<Group>(Utils.AddOrderBy($"{Url}?search={Uri.EscapeDataString(search)}"));

    public GitLabCollectionResponse<Group> SearchAsync(string search) =>
        _api.Get().GetAllAsync<Group>(Utils.AddOrderBy($"{Url}?search={Uri.EscapeDataString(search)}"));

    public Group this[long id] => GetGroup(id);

    public Group this[string fullPath] => GetGroup(fullPath);

    public Task<Group> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        GetGroupAsync(id, cancellationToken);

    public Task<Group> GetByFullPathAsync(string fullPath, CancellationToken cancellationToken = default) =>
        GetGroupAsync(fullPath, cancellationToken);

    public Group GetGroup(GroupId id) =>
        _api.Get().To<Group>($"{Url}/{id.ValueAsUriParameter()}");

    public Task<Group> GetGroupAsync(GroupId id, CancellationToken cancellationToken = default) =>
        _api.Get().ToAsync<Group>($"{Url}/{id.ValueAsUriParameter()}", cancellationToken);

    public GitLabCollectionResponse<Group> GetSubgroupsByIdAsync(long id, SubgroupQuery query = null) =>
        GetSubgroupsAsync(id, query);

    public GitLabCollectionResponse<Group> GetSubgroupsByFullPathAsync(string fullPath, SubgroupQuery query = null) =>
        GetSubgroupsAsync(fullPath, query);

    public GitLabCollectionResponse<Group> GetSubgroupsAsync(GroupId groupId, SubgroupQuery query = null)
    {
        var url = CreateSubgroupGetUrl(groupId, query);
        return _api.Get().GetAllAsync<Group>(url);
    }

    public Task<PagedResponse<Group>> PageSubgroupsAsync(GroupId groupId, PageQuery<SubgroupQuery> query, CancellationToken cancellationToken = default)
    {
        var url = CreateSubgroupGetUrl(groupId, query?.Query, query?.Page, query?.PerPage);
        return _api.Get().PageAsync<Group>(url, cancellationToken);
    }

    public IEnumerable<Project> SearchProjects(long groupId, string search) =>
        SearchProjectsAsync(groupId, new GroupProjectsQuery
        {
            Search = search,
        });

    public GitLabCollectionResponse<Project> GetProjectsAsync(long groupId, GroupProjectsQuery query) =>
        SearchProjectsAsync(groupId, query);

    public GitLabCollectionResponse<Project> SearchProjectsAsync(GroupId groupId, GroupProjectsQuery query)
    {
        var url = CreateGetProjectsUrl(groupId, query);
        return _api.Get().GetAllAsync<Project>(url);
    }

    public Task<PagedResponse<Project>> PageProjectsAsync(GroupId groupId, PageQuery<GroupProjectsQuery> query, CancellationToken cancellationToken = default)
    {
        var url = CreateGetProjectsUrl(groupId, query?.Query, query?.Page, query?.PerPage);
        return _api.Get().PageAsync<Project>(url, cancellationToken);
    }

    private static string CreateGetProjectsUrl(GroupId groupId, GroupProjectsQuery query, int? page = null, int? perPage = null)
    {
        var url = $"{Url}/{groupId.ValueAsUriParameter()}/projects";

        url = Utils.AddPageParams(url, page, perPage);

        if (query is null)
        {
            return url;
        }

        if (query.Visibility.HasValue)
        {
            url = Utils.AddParameter(url, "visibility", query.Visibility.ToString().ToLowerInvariant());
        }

        if (query.MinAccessLevel is not null)
        {
            url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel);
        }

        url = Utils.AddParameter(url, "archived", value: query.Archived);
        url = Utils.AddParameter(url, "sort", query.Sort);
        url = Utils.AddParameter(url, "search", query.Search);
        url = Utils.AddParameter(url, "simple", query.Simple);
        url = Utils.AddParameter(url, "owned", query.Owned);
        url = Utils.AddParameter(url, "starred", query.Starred);
        url = Utils.AddParameter(url, "with_issues_enabled", query.WithIssuesEnabled);
        url = Utils.AddParameter(url, "with_merge_requests_enabled", query.WithMergeRequestsEnabled);
        url = Utils.AddParameter(url, "with_shared", query.WithShared);
        url = Utils.AddParameter(url, "include_subgroups", query.IncludeSubGroups);
        url = Utils.AddParameter(url, "with_custom_attributes", query.WithCustomAttributes);
        url = Utils.AddParameter(url, "with_security_reports ", query.WithSecurityReports);
        url = Utils.AddOrderBy(url, query.OrderBy, supportKeysetPagination: page is null);

        return url;
    }

    public Group Create(GroupCreate group) =>
        _api.Post().With(group).To<Group>(Url);

    public Task<Group> CreateAsync(GroupCreate group, CancellationToken cancellationToken = default) =>
        _api.Post().With(group).ToAsync<Group>(Url, cancellationToken);

    public void Delete(long id) =>
        _api.Delete().Execute($"{Url}/{new GroupId(id).ValueAsUriParameter()}");

    public Task DeleteAsync(long id, CancellationToken cancellationToken = default) =>
        _api.Delete().ExecuteAsync($"{Url}/{new GroupId(id).ValueAsUriParameter()}", cancellationToken);

    public Group Update(long id, GroupUpdate groupUpdate) =>
        _api.Put().With(groupUpdate).To<Group>($"{Url}/{new GroupId(id).ValueAsUriParameter()}");

    public Task<Group> UpdateAsync(long id, GroupUpdate groupUpdate, CancellationToken cancellationToken = default) =>
        _api.Put().With(groupUpdate).ToAsync<Group>($"{Url}/{new GroupId(id).ValueAsUriParameter()}", cancellationToken);

    public void Restore(long id) =>
        _api.Post().Execute($"{Url}/{new GroupId(id).ValueAsUriParameter()}/restore");

    public Task RestoreAsync(long id, CancellationToken cancellationToken = default) =>
        _api.Post().ExecuteAsync($"{Url}/{new GroupId(id).ValueAsUriParameter()}/restore", cancellationToken);
}
