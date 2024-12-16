using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IGroupsClient
{
    /// <summary>
    /// Gets a list of projects accessible to the authenticated user.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-groups.
    /// </summary>
    IEnumerable<Group> Accessible { get; }

    /// <inheritdoc cref="SearchAsync"/>
    IEnumerable<Group> Search(string search);

    /// <inheritdoc cref="GetAsync"/>
    /// <param name="search">The search criteria.</param>
    GitLabCollectionResponse<Group> SearchAsync(string search);

    /// <inheritdoc cref="GetAsync"/>
    IEnumerable<Group> Get(GroupQuery query);

    /// <summary>
    /// Gets a list of visible groups for the authenticated user matching the query.
    /// When accessed without authentication, only public groups are returned.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-groups.
    /// </summary>
    /// <param name="query">The query parameters.</param>
    /// <returns>All matching groups.</returns>
    GitLabCollectionResponse<Group> GetAsync(GroupQuery query);

    /// <summary>
    /// Gets a page of visible groups for the authenticated user matching the query.
    /// When accessed without authentication, only public groups are returned.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-groups.
    /// </summary>
    /// <param name="query">The query parameters, including the page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A page of groups and the total number of matching groups (used to compute the last page).</returns>
    /// <remarks>
    /// The page will be empty after the last page is read.<para/>
    /// The total maybe null if there are more than 10,000 items. See https://docs.gitlab.com/ee/user/gitlab_com/index.html#pagination-response-headers.
    /// </remarks>
    Task<PagedResponse<Group>> PageAsync(PageQuery<GroupQuery> query, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="GetByIdAsync"/>
    Group this[long id] { get; }

    /// <inheritdoc cref="GetByFullPathAsync"/>
    Group this[string fullPath] { get; }

    /// <inheritdoc cref="GetGroupAsync"/>
    /// <param name="id">The group's id</param>
    Task<Group> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="GetGroupAsync"/>
    /// <param name="fullPath">The group's path</param>
    Task<Group> GetByFullPathAsync(string fullPath, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="GetGroupAsync"/>
    Group GetGroup(GroupId id);

    /// <summary>
    /// Gets the specified group.
    /// See https://docs.gitlab.com/ee/api/groups.html#details-of-a-group.
    /// </summary>
    /// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/groups.md#details-of-a-group</remarks>
    /// <param name="groupId">The group's id or path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<Group> GetGroupAsync(GroupId groupId, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="GetSubgroupsAsync"/>
    /// <param name="id">The group's id</param>
    GitLabCollectionResponse<Group> GetSubgroupsByIdAsync(long id, SubgroupQuery query = null);

    /// <inheritdoc cref="GetSubgroupsAsync"/>
    /// <param name="fullPath">The group's path</param>
    GitLabCollectionResponse<Group> GetSubgroupsByFullPathAsync(string fullPath, SubgroupQuery query = null);

    /// <summary>
    /// Gets a list of visible subgroups.
    /// Use <see cref="SubgroupQuery.IncludeDescendants"/> to control the scope of the results.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-a-groups-subgroups.
    /// </summary>
    /// <param name="groupId">The group's id or path.</param>
    /// <param name="query">The query parameters.</param>
    /// <returns>All Groups matching the query parameters.</returns>
    GitLabCollectionResponse<Group> GetSubgroupsAsync(GroupId groupId, SubgroupQuery query = null);

    /// <summary>
    /// Gets a page of visible subgroups.
    /// Use <see cref="SubgroupQuery.IncludeDescendants"/> to control the scope of the results.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-a-groups-subgroups.
    /// </summary>
    /// <param name="groupId">The group's id or path.</param>
    /// <param name="query">The query parameters, including the page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A page of groups matching the query parameters and the total number of matching groups (used to compute the last page).</returns>
    /// <remarks>
    /// The page will be empty after the last page is read.<para/>
    /// The total maybe null if there are more than 10,000 items. See https://docs.gitlab.com/ee/user/gitlab_com/index.html#pagination-response-headers.
    /// </remarks>
    Task<PagedResponse<Group>> PageSubgroupsAsync(GroupId groupId, PageQuery<SubgroupQuery> query, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="CreateAsync"/>
    Group Create(GroupCreate group);

    /// <summary>
    /// Creates a new project group. Available only for users who can create groups.
    /// See https://docs.gitlab.com/ee/api/groups.html#new-group and https://docs.gitlab.com/ee/api/groups.html#new-subgroup.
    /// </summary>
    /// <remarks>
    /// On GitLab SaaS, you must use the GitLab UI to create groups without a parent group. You cannot use the API to do this.
    /// </remarks>
    /// <param name="group">The group configuration.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The new group.</returns>
    Task<Group> CreateAsync(GroupCreate group, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="SearchProjectsAsync"/>
    /// <param name="id">The group's id.</param>
    /// <param name="search">The search criteria.</param>
    [Obsolete("Use SearchProjectsAsync instead")]
    IEnumerable<Project> SearchProjects(long groupId, string search);

    /// <inheritdoc cref="SearchProjectsAsync"/>
    /// <param name="id">The group's id.</param>
    GitLabCollectionResponse<Project> GetProjectsAsync(long groupId, GroupProjectsQuery query);

    /// <summary>
    /// Gets a list of projects in this group. When accessed without authentication, only public projects are returned.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-a-groups-projects
    /// </summary>
    /// <param name="groupId">The group's id or path.</param>
    /// <param name="query">The query parameters, including the page.</param>
    /// <returns>All projects.</returns>
    GitLabCollectionResponse<Project> SearchProjectsAsync(GroupId groupId, GroupProjectsQuery query);

    /// <summary>
    /// Gets a page of projects in this group. When accessed without authentication, only public projects are returned.
    /// See https://docs.gitlab.com/ee/api/groups.html#list-a-groups-projects
    /// </summary>
    /// <param name="groupId">The group's id or path.</param>
    /// <param name="query">The query parameters, including the page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A page of projects and the total number of matching projects (used to compute the last page).</returns>
    /// <remarks>
    /// The page will be empty after the last page is read.<para/>
    /// The total maybe null if there are more than 10,000 items. See https://docs.gitlab.com/ee/user/gitlab_com/index.html#pagination-response-headers.
    /// </remarks>
    Task<PagedResponse<Project>> PageProjectsAsync(GroupId groupId, PageQuery<GroupProjectsQuery> query, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="DeleteAsync"/>
    void Delete(long id);

    /// <summary>
    /// Removes a group.
    /// See https://docs.gitlab.com/ee/api/groups.html#remove-group
    /// </summary>
    /// <param name="id">The group's id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="UpdateAsync"/>
    Group Update(long id, GroupUpdate groupUpdate);

    /// <summary>
    /// Updates an existing group.
    /// See https://docs.gitlab.com/ee/api/groups.html#update-group
    /// </summary>
    /// <param name="id">The group's id.</param>
    /// <param name="groupUpdate">The properties to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<Group> UpdateAsync(long id, GroupUpdate groupUpdate, CancellationToken cancellationToken = default);

    /// <inheritdoc cref="RestoreAsync"/>
    void Restore(long id);

    /// <summary>
    /// Restores a group marked for deletion.
    /// See https://docs.gitlab.com/ee/api/groups.html#restore-group-marked-for-deletion
    /// </summary>
    /// <param name="id">The group's id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task RestoreAsync(long id, CancellationToken cancellationToken = default);
}
