using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl;

public class MembersClient : IMembersClient
{
    private readonly API _api;

    public MembersClient(API api)
    {
        _api = api;
    }

    private IEnumerable<Membership> GetAll(string url, bool includeInheritedMembers, MemberQuery query)
    {
        url += "/members";
        if (includeInheritedMembers)
        {
            url += "/all";

        }

        if (query != null)
        {
            url = Utils.AddParameter(url, "user_ids", query.UserIds);
            url = Utils.AddParameter(url, "state", query.State);
            url = Utils.AddParameter(url, "per_page ", query.PerPage);
            url = Utils.AddParameter(url, "query", query.Query);
            url = Utils.AddParameter(url, "show_seat_info", query.ShowSeatInfo);
        }

        return _api.Get().GetAll<Membership>(url);
    }

    private GitLabCollectionResponse<Membership> GetAllAsync(string url, bool includeInheritedMembers, MemberQuery query)
    {
        url += "/members";
        if (includeInheritedMembers)
        {
            url += "/all";
        }

        if (query != null)
        {
            url = Utils.AddParameter(url, "user_ids", query.UserIds);
            url = Utils.AddParameter(url, "state", query.State);
            url = Utils.AddParameter(url, "per_page ", query.PerPage);
            url = Utils.AddParameter(url, "query", query.Query);
            url = Utils.AddParameter(url, "show_seat_info", query.ShowSeatInfo);
        }

        return _api.Get().GetAllAsync<Membership>(url);
    }

    public IEnumerable<Membership> OfProject(string projectId)
    {
        return OfProject(projectId, includeInheritedMembers: false, null);
    }

    public IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers, MemberQuery query = null)
    {
        return GetAll($"{Project.Url}/{WebUtility.UrlEncode(projectId)}", includeInheritedMembers, query);
    }

    public GitLabCollectionResponse<Membership> OfProjectAsync(ProjectId projectId, bool includeInheritedMembers = false, MemberQuery query = null)
    {
        return GetAllAsync($"{Project.Url}/{projectId.ValueAsUriParameter()}", includeInheritedMembers, query);
    }

    public Membership GetMemberOfProject(string projectId, string userId)
    {
        return GetMemberOfProject(projectId, userId, includeInheritedMembers: false);
    }

    public Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers)
    {
        var url = $"{Project.Url}/{WebUtility.UrlEncode(projectId)}/members/{(includeInheritedMembers ? "all/" : string.Empty)}{WebUtility.UrlEncode(userId)}";
        return _api.Get().To<Membership>(url);
    }

    public Task<Membership> GetMemberOfProjectAsync(ProjectId projectId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default)
    {
        var url = $"{Project.Url}/{projectId.ValueAsUriParameter()}/members/{(includeInheritedMembers ? "all/" : string.Empty)}{userId.ToStringInvariant()}";
        return _api.Get().ToAsync<Membership>(url, cancellationToken);
    }

    public Membership AddMemberToProject(string projectId, ProjectMemberCreate user)
    {
        return _api.Post().With(user).To<Membership>($"{Project.Url}/{WebUtility.UrlEncode(projectId)}/members");
    }

    public Task<Membership> AddMemberToProjectAsync(ProjectId projectId, ProjectMemberCreate user, CancellationToken cancellationToken = default)
    {
        return _api.Post().With(user).ToAsync<Membership>($"{Project.Url}/{projectId.ValueAsUriParameter()}/members", cancellationToken);
    }

    public Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate user)
    {
        return _api.Put().With(user).To<Membership>($"{Project.Url}/{WebUtility.UrlEncode(projectId)}/members/{WebUtility.UrlEncode(user.UserId)}");
    }

    public Task<Membership> UpdateMemberOfProjectAsync(ProjectId projectId, ProjectMemberUpdate user, CancellationToken cancellationToken = default)
    {
        return _api.Put().With(user).ToAsync<Membership>($"{Project.Url}/{projectId.ValueAsUriParameter()}/members/{WebUtility.UrlEncode(user.UserId)}", cancellationToken);
    }

    public Task RemoveMemberFromProjectAsync(ProjectId projectId, long userId, CancellationToken cancellationToken = default)
    {
        return _api.Delete().ExecuteAsync($"{Project.Url}/{projectId.ValueAsUriParameter()}/members/{userId.ToStringInvariant()}", cancellationToken);
    }

    [Obsolete("Use OfGroup")]
    public IEnumerable<Membership> OfNamespace(string groupId)
    {
        return OfGroup(groupId);
    }

    public IEnumerable<Membership> OfGroup(string groupId)
    {
        return OfGroup(groupId, includeInheritedMembers: false, null);
    }

    public IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers, MemberQuery query = null)
    {
        return GetAll($"{Group.Url}/{WebUtility.UrlEncode(groupId)}", includeInheritedMembers, query);
    }

    public GitLabCollectionResponse<Membership> OfGroupAsync(GroupId groupId, bool includeInheritedMembers = false, MemberQuery query = null)
    {
        return GetAllAsync($"{Group.Url}/{groupId.ValueAsUriParameter()}", includeInheritedMembers, query);
    }

    public Membership GetMemberOfGroup(string groupId, string userId)
    {
        return GetMemberOfGroup(groupId, userId, includeInheritedMembers: false);
    }

    public Membership GetMemberOfGroup(string groupId, string userId, bool includeInheritedMembers)
    {
        var tailAPIUrl = $"{Group.Url}/{WebUtility.UrlEncode(groupId)}/members/{(includeInheritedMembers ? "all/" : string.Empty)}{WebUtility.UrlEncode(userId)}";
        return _api.Get().To<Membership>(tailAPIUrl);
    }

    public Task<Membership> GetMemberOfGroupAsync(GroupId groupId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default)
    {
        var tailAPIUrl = $"{Group.Url}/{groupId.ValueAsUriParameter()}/members/{(includeInheritedMembers ? "all/" : string.Empty)}{userId.ToStringInvariant()}";
        return _api.Get().ToAsync<Membership>(tailAPIUrl, cancellationToken);
    }

    public Membership AddMemberToGroup(string groupId, GroupMemberCreate user)
    {
        return _api.Post().With(user).To<Membership>($"{Group.Url}/{WebUtility.UrlEncode(groupId)}/members");
    }

    public Task<Membership> AddMemberToGroupAsync(GroupId groupId, GroupMemberCreate user, CancellationToken cancellationToken = default)
    {
        return _api.Post().With(user).ToAsync<Membership>($"{Group.Url}/{groupId.ValueAsUriParameter()}/members", cancellationToken);
    }

    public Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate user)
    {
        return _api.Put().With(user).To<Membership>($"{Group.Url}/{WebUtility.UrlEncode(groupId)}/members/{WebUtility.UrlEncode(user.UserId)}");
    }

    public Task<Membership> UpdateMemberOfGroupAsync(GroupId groupId, GroupMemberUpdate user, CancellationToken cancellationToken = default)
    {
        return _api.Put().With(user).ToAsync<Membership>($"{Group.Url}/{groupId.ValueAsUriParameter()}/members/{WebUtility.UrlEncode(user.UserId)}", cancellationToken);
    }

    public Task RemoveMemberFromGroupAsync(GroupId groupId, long userId, CancellationToken cancellationToken = default)
    {
        return _api.Delete().ExecuteAsync($"{Group.Url}/{groupId.ValueAsUriParameter()}/members/{userId.ToStringInvariant()}", cancellationToken);
    }
}
