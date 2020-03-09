using System;
using System.Net;

namespace NGitLab.Mock.Clients
{
    internal abstract class ClientBase
    {
        protected ClientBase(ClientContext context)
        {
            Context = context;
        }

        protected GitLabServer Server => Context.Server;

        protected ClientContext Context { get; }

        protected Project GetProject(object id, ProjectPermission permissions)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            if (Context.User.State == UserState.blocked && permissions != ProjectPermission.View)
            {
                throw new GitLabException("403 Forbidden  - Your account has been blocked.")
                {
                    StatusCode = HttpStatusCode.Forbidden,
                };
            }

            var project = id is int idInt
                ? Server.AllProjects.FindById(idInt)
                : Server.AllProjects.FindProject((string)id);

            if (project == null || !project.CanUserViewProject(Context.User))
                throw new GitLabNotFoundException();

            switch (permissions)
            {
                case ProjectPermission.View:
                    if (!project.CanUserViewProject(Context.User))
                        throw new GitLabForbiddenException();
                    break;

                case ProjectPermission.Contribute:
                    if (!project.CanUserContributeToProject(Context.User))
                        throw new GitLabForbiddenException();
                    break;

                case ProjectPermission.Edit:
                    if (!project.CanUserEditProject(Context.User))
                        throw new GitLabForbiddenException();
                    break;

                case ProjectPermission.Delete:
                    if (!project.CanUserDeleteProject(Context.User))
                        throw new GitLabForbiddenException();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(permissions));
            }

            return project;
        }

        protected User GetUser(int userId)
        {
            var user = Server.Users.GetById(userId);
            if (user == null)
                throw new GitLabNotFoundException();

            return user;
        }

        protected Issue GetIssue(int projectId, int issueId)
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var issue = project.Issues.GetByIid(issueId);
            if (issue == null)
                throw new GitLabNotFoundException();

            return issue;
        }

        protected Milestone GetMilestone(int projectId, int milestoneId)
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var milestone = project.Milestones.GetByIid(milestoneId);
            if (milestone == null)
                throw new GitLabNotFoundException();

            return milestone;
        }

        protected MergeRequest GetMergeRequest(int projectId, int mergeRequestIid)
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
                throw new GitLabException { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        internal enum ProjectPermission
        {
            View,
            Contribute,
            Edit,
            Delete,
        }
    }
}
