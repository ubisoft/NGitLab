using System;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal abstract class ClientBase
{
    protected ClientBase(ClientContext context)
    {
        Context = context;
    }

    protected GitLabServer Server => Context.Server;

    protected ClientContext Context { get; }

    protected Group GetGroup(object id, GroupPermission permissions)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        if (Context.User.State == UserState.blocked && permissions != GroupPermission.View)
        {
            throw new GitLabException("403 Forbidden - Your account has been blocked.")
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        var group = id switch
        {
            long idLong => Server.AllGroups.FindById(idLong),
            string idStr => Server.AllGroups.FindGroup(idStr),
            IIdOrPathAddressable gid when gid.Path != null => Server.AllGroups.FindByNamespacedPath(gid.Path),
            IIdOrPathAddressable gid => Server.AllGroups.FindById(gid.Id),
            _ => throw new ArgumentException($"Id of type '{id.GetType()}' is not supported"),
        };

        if (group == null || !group.CanUserViewGroup(Context.User))
            throw new GitLabNotFoundException("Group does not exist or user doesn't have permission to view it");

        switch (permissions)
        {
            case GroupPermission.View:
                // Already checked before
                break;

            case GroupPermission.Edit:
                if (!group.CanUserEditGroup(Context.User))
                    throw new GitLabForbiddenException($"User '{Context.User.Name}' does not have the permission to edit the group '{group.Name}'");
                break;

            case GroupPermission.Delete:
                if (!group.CanUserDeleteGroup(Context.User))
                    throw new GitLabForbiddenException($"User '{Context.User.Name}' does not have the permission to delete the group '{group.Name}'");
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(permissions));
        }

        return group;
    }

    protected Project GetProject(object id, ProjectPermission permissions)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        if (Context.User.State == UserState.blocked && permissions != ProjectPermission.View)
        {
            throw new GitLabException("403 Forbidden - Your account has been blocked.")
            {
                StatusCode = HttpStatusCode.Forbidden,
            };
        }

        var project = id switch
        {
            long idLong => Server.AllProjects.FindById(idLong),
            string idStr => Server.AllProjects.FindProject(idStr),
            IIdOrPathAddressable pid when pid.Path != null => Server.AllProjects.FindByNamespacedPath(pid.Path),
            IIdOrPathAddressable pid => Server.AllProjects.FindById(pid.Id),
            _ => throw new ArgumentException($"Id of type '{id.GetType()}' is not supported"),
        };

        if (project == null || !project.CanUserViewProject(Context.User))
            throw new GitLabNotFoundException("Project does not exist or User doesn't have permission to view it");

        switch (permissions)
        {
            case ProjectPermission.View:
                // Already checked before
                break;

            case ProjectPermission.Contribute:
                if (!project.CanUserContributeToProject(Context.User))
                    throw new GitLabForbiddenException($"User '{Context.User.Name}' does not have the permission to contribute to the project '{project.Name}'");
                break;

            case ProjectPermission.Edit:
                if (!project.CanUserEditProject(Context.User))
                    throw new GitLabForbiddenException($"User '{Context.User.Name}' does not have the permission to edit the project '{project.Name}'");
                break;

            case ProjectPermission.Delete:
                if (!project.CanUserDeleteProject(Context.User))
                    throw new GitLabForbiddenException($"User '{Context.User.Name}' does not have the permission to delete the project '{project.Name}'");
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(permissions));
        }

        return project;
    }

    protected User GetUser(long userId)
    {
        var user = Server.Users.GetById(userId);
        if (user == null)
            throw new GitLabNotFoundException();

        return user;
    }

    protected Issue GetIssue(long projectId, long issueId)
    {
        var project = GetProject(projectId, ProjectPermission.View);
        var issue = project.Issues.GetByIid(issueId);
        if (issue == null)
            throw new GitLabNotFoundException();

        return issue;
    }

    protected Milestone GetMilestone(long projectId, long milestoneId)
    {
        var project = GetProject(projectId, ProjectPermission.View);
        var milestone = project.Milestones.GetByIid(milestoneId);
        if (milestone == null)
            throw new GitLabNotFoundException();

        return milestone;
    }

    protected MergeRequest GetMergeRequest(long projectId, long mergeRequestIid)
    {
        var project = GetProject(projectId, ProjectPermission.View);
        var mergeRequest = project.MergeRequests.GetByIid(mergeRequestIid);
        if (mergeRequest == null)
            throw new GitLabNotFoundException();

        return mergeRequest;
    }

    protected void EnsureUserIsAuthenticated()
    {
        if (Context.User == null)
            throw new GitLabException { StatusCode = HttpStatusCode.Unauthorized };
    }

    internal enum ProjectPermission
    {
        View,
        Contribute,
        Edit,
        Delete,
    }

    internal enum GroupPermission
    {
        View,
        Edit,
        Delete,
    }
}
