using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ProjectClient : ClientBase, IProjectClient
    {
        public ProjectClient(ClientContext context)
            : base(context)
        {
        }

        public Models.Project this[int id]
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(id, ProjectPermission.View);
                    if (project == null || !project.CanUserViewProject(Context.User))
                        throw new GitLabNotFoundException();

                    return project.ToClientProject();
                }
            }
        }

        public Models.Project this[string fullName]
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(fullName, ProjectPermission.View);
                    return project.ToClientProject();
                }
            }
        }

        public IEnumerable<Models.Project> Accessible
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return Server.AllProjects.Where(project => project.IsUserMember(Context.User)).Select(project => project.ToClientProject()).ToList();
                }
            }
        }

        public IEnumerable<Models.Project> Owned
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return Server.AllProjects.Where(project => project.IsUserOwner(Context.User)).Select(project => project.ToClientProject()).ToList();
                }
            }
        }

        public IEnumerable<Models.Project> Visible
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    return Server.AllProjects.Where(project => project.CanUserViewProject(Context.User)).Select(project => project.ToClientProject()).ToList();
                }
            }
        }

        public Models.Project Create(ProjectCreate project)
        {
            using (Context.BeginOperationScope())
            {
                var parentGroup = GetParentGroup(project.NamespaceId);

                var newProject = new Project(project.Name)
                {
                    Description = project.Description,
                    Visibility = project.VisibilityLevel,
                    Permissions =
                    {
                        new Permission(Context.User, AccessLevel.Owner),
                    },
                };

                parentGroup.Projects.Add(newProject);
                return newProject.ToClientProject();
            }
        }

        private Group GetParentGroup(string namespaceId)
        {
            Group parentGroup;
            if (namespaceId != null)
            {
                parentGroup = Server.AllGroups.FindGroup(namespaceId);
                if (parentGroup == null || !parentGroup.CanUserViewGroup(Context.User))
                    throw new GitLabNotFoundException();

                if (!parentGroup.CanUserAddProject(Context.User))
                    throw new GitLabForbiddenException();
            }
            else
            {
                parentGroup = Context.User.Namespace;
            }

            return parentGroup;
        }

        public void Delete(int id)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(id, ProjectPermission.Delete);
                project.Remove();
            }
        }

        public void Archive(int id)
        {
            var project = GetProject(id, ProjectPermission.Edit);
            project.Archived = true;
        }

        public void Unarchive(int id)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(id, ProjectPermission.Edit);
                project.Archived = false;
            }
        }

        public Models.Project Fork(string id, ForkProject forkProject)
        {
            EnsureUserIsAuthenticated();

            using (Context.BeginOperationScope())
            {
                var project = GetProject(id, ProjectPermission.View);
                var group = forkProject.Namespace != null ? GetParentGroup(forkProject.Namespace) : Context.User.Namespace;
                var newProject = project.Fork(group, Context.User, forkProject.Name);
                return newProject.ToClientProject();
            }
        }

        public IEnumerable<Models.Project> Get(ProjectQuery query)
        {
            using (Context.BeginOperationScope())
            {
                if (query.MinAccessLevel != null
                 || query.LastActivityAfter != null
                 || query.OrderBy != null
                 || query.PerPage != null
                 || query.Search != null
                 || query.Statistics != null
                 || query.Ascending != null
                 || query.Simple != null
                 || query.UserId != null)
                {
                    throw new NotImplementedException();
                }

                var projects = Server.AllProjects;

                if (query.Archived != null)
                {
                    projects = projects.Where(p => query.Archived == p.Archived);
                }

                if (query.Visibility != null)
                {
                    projects = projects.Where(p => query.Visibility == p.Visibility);
                }

                switch (query.Scope)
                {
                    case ProjectQueryScope.Accessible:
                        projects = projects.Where(p => p.IsUserMember(Context.User));
                        break;
                    case ProjectQueryScope.Owned:
                        projects = projects.Where(p => p.IsUserOwner(Context.User));
                        break;
                    case ProjectQueryScope.Visible:
                        projects = projects.Where(p => p.CanUserViewProject(Context.User));
                        break;
                    case ProjectQueryScope.All:
                        break;
                }

                return projects.Select(project => project.ToClientProject()).ToList();
            }
        }

        public Models.Project GetById(int id, SingleProjectQuery query)
        {
            using (Context.BeginOperationScope())
            {
                return GetProject(id, ProjectPermission.View).ToClientProject();
            }
        }

        [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
        public async Task<Models.Project> GetByIdAsync(int id, SingleProjectQuery query, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return GetById(id, query);
        }

        public async Task<Models.Project> GetByNamespacedPathAsync(string path, SingleProjectQuery query = null, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return this[path];
        }

        public IEnumerable<Models.Project> GetForks(string id, ForkedProjectQuery query)
        {
            using (Context.BeginOperationScope())
            {
                if (query.Archived != null
                    || query.Membership != null
                    || query.MinAccessLevel != null
                    || query.OrderBy != null
                    || query.PerPage != null
                    || query.Search != null
                    || query.Statistics != null
                    || query.Visibility != null)
                {
                    throw new NotImplementedException();
                }

                var upstream = GetProject(id, ProjectPermission.View);
                var matches = Server.AllProjects.Where(project => project.ForkedFrom?.Id == upstream.Id);
                matches = matches.Where(project => query.Owned == null || query.Owned == project.IsUserOwner(Context.User));

                return matches.Select(project => project.ToClientProject()).ToList();
            }
        }

        public Dictionary<string, double> GetLanguages(string id)
        {
            throw new NotImplementedException();
        }

        public Models.Project Update(string id, ProjectUpdate projectUpdate)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(id, ProjectPermission.Edit);

                if (projectUpdate.Name != null)
                {
                    project.Name = projectUpdate.Name;
                }

                if (projectUpdate.DefaultBranch != null)
                {
                    project.DefaultBranch = projectUpdate.DefaultBranch;
                }

                if (projectUpdate.Description != null)
                {
                    project.Description = projectUpdate.Description;
                }

                if (projectUpdate.Visibility.HasValue)
                {
                    project.Visibility = projectUpdate.Visibility.Value;
                }

                if (projectUpdate.BuildTimeout.HasValue)
                {
                    project.BuildTimeout = TimeSpan.FromMinutes(projectUpdate.BuildTimeout.Value);
                }

                if (projectUpdate.LfsEnabled.HasValue)
                {
                    project.LfsEnabled = projectUpdate.LfsEnabled.Value;
                }

                if (projectUpdate.TagList != null)
                {
                    project.Tags = projectUpdate.TagList.Where(t => !string.IsNullOrEmpty(t)).Distinct(StringComparer.Ordinal).ToArray();
                }

                return project.ToClientProject();
            }
        }

        public UploadedProjectFile UploadFile(string id, FormDataContent data)
        {
            throw new NotImplementedException();
        }

        public GitLabCollectionResponse<Models.Project> GetAsync(ProjectQuery query)
        {
            return GitLabCollectionResponse.Create(Get(query));
        }

        [SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "Would be an infinite recursion")]
        public async Task<Models.Project> ForkAsync(string id, ForkProject forkProject, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            return Fork(id, forkProject);
        }

        public GitLabCollectionResponse<Models.Project> GetForksAsync(string id, ForkedProjectQuery query)
        {
            return GitLabCollectionResponse.Create(GetForks(id, query));
        }
    }
}
