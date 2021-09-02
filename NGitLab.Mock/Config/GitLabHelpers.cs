using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NGitLab.Models;

#nullable enable

#pragma warning disable RS0026 // Adding optional parameters to public methods
#pragma warning disable RS0037 // Activate nullable values in public API

namespace NGitLab.Mock.Config
{
    public static class GitLabHelpers
    {
        public static GitLabConfig Configure(this GitLabConfig config, Action<GitLabConfig> configure)
        {
            configure(config);
            return config;
        }

        public static GitLabUser Configure(this GitLabUser user, Action<GitLabUser> configure)
        {
            configure(user);
            return user;
        }

        public static GitLabGroup Configure(this GitLabGroup group, Action<GitLabGroup> configure)
        {
            configure(group);
            return group;
        }

        public static GitLabProject Configure(this GitLabProject project, Action<GitLabProject> configure)
        {
            configure(project);
            return project;
        }

        public static GitLabCommit Configure(this GitLabCommit commit, Action<GitLabCommit> configure)
        {
            configure(commit);
            return commit;
        }

        public static GitLabIssue Configure(this GitLabIssue issue, Action<GitLabIssue> configure)
        {
            configure(issue);
            return issue;
        }

        public static GitLabMergeRequest Configure(this GitLabMergeRequest mergeRequest, Action<GitLabMergeRequest> configure)
        {
            configure(mergeRequest);
            return mergeRequest;
        }

        /// <summary>
        /// Add a user description in config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="username">Username (required)</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabConfig WithUser(this GitLabConfig config, string username, Action<GitLabUser> configure)
        {
            return Configure(config, _ =>
            {
                var user = new GitLabUser
                {
                    Username = username ?? throw new ArgumentNullException(nameof(username)),
                };

                config.Users.Add(user);
                configure(user);
            });
        }

        /// <summary>
        /// Add a user description in config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="username">Username (required)</param>
        /// <param name="name">Name.</param>
        /// <param name="email">Email.</param>
        /// <param name="avatarUrl">Avatar URL.</param>
        /// <param name="isAdmin">Indicates if user is admin in GitLab server</param>
        /// <param name="isDefault">Define as default user for next descriptions</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabConfig WithUser(this GitLabConfig config, string username, string? name = null, string? email = null, string? avatarUrl = null, bool isAdmin = false, bool isDefault = false, Action<GitLabUser>? configure = null)
        {
            return WithUser(config, username, user =>
            {
                user.Name = name;
                user.Email = email;
                user.AvatarUrl = avatarUrl;
                user.IsAdmin = isAdmin;
                if (isDefault)
                {
                    user.AsDefaultUser();
                }

                configure?.Invoke(user);
            });
        }

        /// <summary>
        /// Define a user as default for next descriptions
        /// </summary>
        /// <param name="user">User.</param>
        public static GitLabUser AsDefaultUser(this GitLabUser user)
        {
            return Configure(user, _ =>
            {
                user.Parent.DefaultUser = user.Username;
            });
        }

        /// <summary>
        /// Define a user as default for next descriptions
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="username">Username.</param>
        public static GitLabConfig SetDefaultUser(this GitLabConfig config, string username)
        {
            return Configure(config, _ =>
            {
                config.DefaultUser = username;
            });
        }

        /// <summary>
        /// Define default branch for next descriptions
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="branchName">Branch name.</param>
        /// <returns></returns>
        public static GitLabConfig SetDefaultBranch(this GitLabConfig config, string branchName)
        {
            return Configure(config, _ =>
            {
                config.DefaultBranch = branchName;
            });
        }

        /// <summary>
        /// Define default groups and projects visibility
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="visibility">Visibility.</param>
        /// <returns></returns>
        public static GitLabConfig SetDefaultVisibility(this GitLabConfig config, VisibilityLevel visibility)
        {
            return Configure(config, _ =>
            {
                config.DefaultVisibility = visibility;
            });
        }

        /// <summary>
        /// Add an explicit group description in config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="name">Name.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabConfig WithGroup(this GitLabConfig config, string? name, Action<GitLabGroup> configure)
        {
            return Configure(config, _ =>
            {
                var group = new GitLabGroup
                {
                    Name = name ?? Guid.NewGuid().ToString("D"),
                };

                config.Groups.Add(group);
                configure(group);
            });
        }

        /// <summary>
        /// Add an explicit group description in config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="name">Name.</param>
        /// <param name="id">Explicit ID (config increment)</param>
        /// <param name="namespace">Parent namespace.</param>
        /// <param name="description">Description.</param>
        /// <param name="visibility">Visibility.</param>
        /// <param name="addDefaultUserAsMaintainer">Define default user as maintainer.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabConfig WithGroup(this GitLabConfig config, string? name = null, int id = default, string? @namespace = null, string? description = null, VisibilityLevel? visibility = null, bool addDefaultUserAsMaintainer = false, Action<GitLabGroup>? configure = null)
        {
            return WithGroup(config, name, group =>
            {
                if (id != default)
                    group.Id = id;

                group.Namespace = @namespace;
                group.Description = description;
                group.Visibility = visibility;

                if (addDefaultUserAsMaintainer)
                {
                    if (string.IsNullOrEmpty(group.Parent.DefaultUser))
                        throw new InvalidOperationException("Default user not configured");

                    WithUserPermission(group, group.Parent.DefaultUser, AccessLevel.Maintainer);
                }

                configure?.Invoke(group);
            });
        }

        /// <summary>
        /// Add a project description in config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="name">Name.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabConfig WithProject(this GitLabConfig config, string? name, Action<GitLabProject> configure)
        {
            return Configure(config, _ =>
            {
                var project = new GitLabProject
                {
                    Name = name,
                };

                config.Projects.Add(project);
                configure(project);
            });
        }

        /// <summary>
        /// Add a project description in config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="name">Name.</param>
        /// <param name="id">Explicit ID (config increment)</param>
        /// <param name="namespace">Group fullname.</param>
        /// <param name="description">Description.</param>
        /// <param name="defaultBranch">Repository default branch.</param>
        /// <param name="visibility">Visibility.</param>
        /// <param name="initialCommit">Indicates if an initial commit is added.</param>
        /// <param name="addDefaultUserAsMaintainer">Define default user as maintainer.</param>
        /// <param name="clonePath">Path where to clone repository after server resolving</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabConfig WithProject(this GitLabConfig config, string? name = null, int id = default, string? @namespace = null, string? description = null, string? defaultBranch = null, VisibilityLevel visibility = VisibilityLevel.Internal, bool initialCommit = false, bool addDefaultUserAsMaintainer = false, string? clonePath = null, Action<GitLabProject>? configure = null)
        {
            return WithProject(config, name, project =>
            {
                if (id != default)
                    project.Id = id;

                project.Namespace = @namespace;
                project.Description = description;
                project.DefaultBranch = defaultBranch ?? config.DefaultBranch ?? "main";
                project.Visibility = visibility;
                project.ClonePath = clonePath;

                if (initialCommit)
                {
                    WithCommit(project, "Initial commit", null, commit =>
                    {
                        WithFile(commit, "README.md", $"# {name}{Environment.NewLine}");
                    });
                }

                if (addDefaultUserAsMaintainer)
                {
                    if (string.IsNullOrEmpty(project.Parent.DefaultUser))
                        throw new InvalidOperationException("Default user not configured");

                    WithUserPermission(project, project.Parent.DefaultUser, AccessLevel.Maintainer);
                }

                configure?.Invoke(project);
            });
        }

        /// <summary>
        /// Add a group description in group
        /// </summary>
        /// <param name="group">Group.</param>
        /// <param name="name">Name (required)</param>
        /// <param name="color">Color in RGB hex format (example: #A1F8C3)</param>
        /// <param name="description">Description.</param>
        public static GitLabGroup WithLabel(this GitLabGroup group, string name, string? color = null, string? description = null)
        {
            return Configure(group, _ =>
            {
                var label = new GitLabLabel
                {
                    Name = name ?? throw new ArgumentNullException(nameof(name)),
                    Color = color,
                    Description = description,
                };

                group.Labels.Add(label);
            });
        }

        /// <summary>
        /// Add a label description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="name">Name (required)</param>
        /// <param name="color">Color in RGB hex format (example: #A1F8C3)</param>
        /// <param name="description">Description.</param>
        public static GitLabProject WithLabel(this GitLabProject project, string name, string? color = null, string? description = null)
        {
            return Configure(project, _ =>
            {
                var label = new GitLabLabel
                {
                    Name = name ?? throw new ArgumentNullException(nameof(name)),
                    Color = color,
                    Description = description,
                };

                project.Labels.Add(label);
            });
        }

        /// <summary>
        /// Add a commit description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="message">Message (required)</param>
        /// <param name="user">Author username (required if default user not defined)</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithCommit(this GitLabProject project, string? message, string? user, Action<GitLabCommit> configure)
        {
            return Configure(project, _ =>
            {
                var commit = new GitLabCommit
                {
                    Message = message,
                    User = user,
                };

                project.Commits.Add(commit);
                configure(commit);
            });
        }

        /// <summary>
        /// Add a commit description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="message">Message (required)</param>
        /// <param name="user">Author username (required if default user not defined)</param>
        /// <param name="sourceBranch">Source branch (required if checkout or merge)</param>
        /// <param name="targetBranch">Target branch (required if merge)</param>
        /// <param name="tags">Tags.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithCommit(this GitLabProject project, string? message = null, string? user = null, string? sourceBranch = null, string? targetBranch = null, IEnumerable<string>? tags = null, Action<GitLabCommit>? configure = null)
        {
            return WithCommit(project, message, user, commit =>
            {
                commit.SourceBranch = sourceBranch;
                commit.TargetBranch = targetBranch;
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        commit.Tags.Add(tag);
                    }
                }

                configure?.Invoke(commit);
            });
        }

        /// <summary>
        /// Add a merge commit in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="sourceBranch">Source branch (required)</param>
        /// <param name="targetBranch">Target branch</param>
        /// <param name="user">Author username (required if default user not defined)</param>
        /// <param name="deleteSourceBranch">Indicates if source branch must be deleted after merge.</param>
        /// <param name="tags">Tags.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithMergeCommit(this GitLabProject project, string sourceBranch, string? targetBranch = null, string? user = null, bool deleteSourceBranch = false, IEnumerable<string>? tags = null, Action<GitLabCommit>? configure = null)
        {
            return WithCommit(project, $"Merge '{sourceBranch}' into '{targetBranch ?? project.DefaultBranch}'", user, commit =>
            {
                commit.SourceBranch = sourceBranch ?? throw new ArgumentNullException(nameof(sourceBranch));
                commit.TargetBranch = targetBranch ?? project.DefaultBranch;
                commit.DeleteSourceBranch = deleteSourceBranch;
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        commit.Tags.Add(tag);
                    }
                }

                configure?.Invoke(commit);
            });
        }

        /// <summary>
        /// Add a tag description in commit
        /// </summary>
        /// <param name="commit">Commit.</param>
        /// <param name="name">Name (required)</param>
        public static GitLabCommit WithTag(this GitLabCommit commit, string name)
        {
            return Configure(commit, _ =>
            {
                commit.Tags.Add(name ?? throw new ArgumentNullException(nameof(name)));
            });
        }

        /// <summary>
        /// Add a file description in commit
        /// </summary>
        /// <param name="commit">Commit.</param>
        /// <param name="relativePath">Relative path (required)</param>
        /// <param name="content">File Content</param>
        public static GitLabCommit WithFile(this GitLabCommit commit, string relativePath, string content = "")
        {
            return Configure(commit, _ =>
            {
                commit.Files.Add(new GitLabFileDescriptor
                {
                    Path = relativePath ?? throw new ArgumentNullException(nameof(relativePath)),
                    Content = content,
                });
            });
        }

        /// <summary>
        /// Add an issue description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="title">Title.</param>
        /// <param name="author">Author username (required if default user not defined)</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithIssue(this GitLabProject project, string? title, string? author, Action<GitLabIssue> configure)
        {
            return Configure(project, _ =>
            {
                var issue = new GitLabIssue
                {
                    Title = title,
                    Author = author,
                };

                project.Issues.Add(issue);
                configure(issue);
            });
        }

        /// <summary>
        /// Add an issue description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="title">Title.</param>
        /// <param name="id">Explicit ID (project increment)</param>
        /// <param name="description">Description.</param>
        /// <param name="author">Author username (required if default user nor defined)</param>
        /// <param name="assignee">Assignee username (allow multiple separated by ',')</param>
        /// <param name="milestone">Milestone title (create it if not exists)</param>
        /// <param name="createdAt">Creation date time.</param>
        /// <param name="updatedAt">Update date time.</param>
        /// <param name="closedAt">Close date time.</param>
        /// <param name="labels">Labels names.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithIssue(this GitLabProject project, string? title = null, int id = default, string? description = null, string? author = null, string? assignee = null, string? milestone = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, IEnumerable<string>? labels = null, Action<GitLabIssue>? configure = null)
        {
            return WithIssue(project, title, author, issue =>
            {
                if (id != default)
                    issue.Id = id;

                issue.Description = description;
                issue.Assignee = assignee;
                issue.Milestone = milestone;
                issue.CreatedAt = createdAt;
                issue.UpdatedAt = updatedAt;
                issue.ClosedAt = closedAt;
                if (labels != null)
                {
                    foreach (var label in labels)
                    {
                        WithLabel(issue, label);
                    }
                }

                configure?.Invoke(issue);
            });
        }

        /// <summary>
        /// Add label in issue (create it if not exists)
        /// </summary>
        /// <param name="issue">Issue.</param>
        /// <param name="label">Label name (required)</param>
        public static GitLabIssue WithLabel(this GitLabIssue issue, string label)
        {
            return Configure(issue, _ =>
            {
                issue.Labels.Add(label ?? throw new ArgumentNullException(nameof(label)));
            });
        }

        /// <summary>
        /// Add merge request description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="sourceBranch">Source branch (required)</param>
        /// <param name="title">Title.</param>
        /// <param name="author">Author username (required if default user not defined)</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithMergeRequest(this GitLabProject project, string sourceBranch, string? title, string? author, Action<GitLabMergeRequest> configure)
        {
            return Configure(project, _ =>
            {
                var mergeRequest = new GitLabMergeRequest
                {
                    Title = title,
                    Author = author,
                    SourceBranch = sourceBranch ?? throw new ArgumentNullException(nameof(sourceBranch)),
                    TargetBranch = project.DefaultBranch,
                };

                project.MergeRequests.Add(mergeRequest);
                configure(mergeRequest);
            });
        }

        /// <summary>
        /// Add merge request description in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="sourceBranch">Source branch (required)</param>
        /// <param name="title">Title.</param>
        /// <param name="id">Explicit ID (project increment)</param>
        /// <param name="targetBranch">Target branch (use default branch if null)</param>
        /// <param name="description">Description.</param>
        /// <param name="author">Author username (required if default user not defined)</param>
        /// <param name="assignee">Assignee username.</param>
        /// <param name="createdAt">Creation date time.</param>
        /// <param name="updatedAt">Update date time.</param>
        /// <param name="closedAt">Close date time.</param>
        /// <param name="mergedAt">Merge date time.</param>
        /// <param name="approvers">Approvers usernames.</param>
        /// <param name="labels">Labels names.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithMergeRequest(this GitLabProject project, string sourceBranch, string? title = null, int id = default, string? targetBranch = null, string? description = null, string? author = null, string? assignee = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, DateTime? mergedAt = null, IEnumerable<string>? approvers = null, IEnumerable<string>? labels = null, Action<GitLabMergeRequest>? configure = null)
        {
            return WithMergeRequest(project, sourceBranch, title, author, mergeRequest =>
            {
                if (id != default)
                    mergeRequest.Id = id;

                mergeRequest.Description = description;
                mergeRequest.Assignee = assignee;
                mergeRequest.TargetBranch = targetBranch;
                mergeRequest.CreatedAt = createdAt;
                mergeRequest.UpdatedAt = updatedAt;
                mergeRequest.ClosedAt = closedAt;
                mergeRequest.MergedAt = mergedAt;
                if (labels != null)
                {
                    foreach (var label in labels)
                    {
                        WithLabel(mergeRequest, label);
                    }
                }

                if (approvers != null)
                {
                    foreach (var approver in approvers)
                    {
                        WithApprover(mergeRequest, approver);
                    }
                }

                configure?.Invoke(mergeRequest);
            });
        }

        /// <summary>
        /// Add label in merge request (create it if not exists)
        /// </summary>
        /// <param name="mergeRequest">Merge request.</param>
        /// <param name="label">Label name (required)</param>
        public static GitLabMergeRequest WithLabel(this GitLabMergeRequest mergeRequest, string label)
        {
            return Configure(mergeRequest, _ =>
            {
                mergeRequest.Labels.Add(label ?? throw new ArgumentNullException(nameof(label)));
            });
        }

        /// <summary>
        /// Add approver in merge request
        /// </summary>
        /// <param name="mergeRequest">Merge request.</param>
        /// <param name="approver">Approver username (required)</param>
        public static GitLabMergeRequest WithApprover(this GitLabMergeRequest mergeRequest, string approver)
        {
            return Configure(mergeRequest, _ =>
            {
                mergeRequest.Approvers.Add(approver ?? throw new ArgumentNullException(nameof(approver)));
            });
        }

        /// <summary>
        /// Add user permission in group
        /// </summary>
        /// <param name="group">Group.</param>
        /// <param name="user">Username (required)</param>
        /// <param name="level">Access level (required)</param>
        public static GitLabGroup WithUserPermission(this GitLabGroup group, string user, AccessLevel level)
        {
            return Configure(group, _ =>
            {
                var permission = new GitLabPermission
                {
                    User = user ?? throw new ArgumentNullException(nameof(user)),
                    Level = level,
                };

                group.Permissions.Add(permission);
            });
        }

        /// <summary>
        /// Add user permission in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="user">Username (required)</param>
        /// <param name="level">Access level (required)</param>
        public static GitLabProject WithUserPermission(this GitLabProject project, string user, AccessLevel level)
        {
            return Configure(project, _ =>
            {
                var permission = new GitLabPermission
                {
                    User = user ?? throw new ArgumentNullException(nameof(user)),
                    Level = level,
                };

                project.Permissions.Add(permission);
            });
        }

        /// <summary>
        /// Add group permission in group
        /// </summary>
        /// <param name="grp">Group.</param>
        /// <param name="groupName">Group fullname (required)</param>
        /// <param name="level">Access level (required)</param>
        public static GitLabGroup WithGroupPermission(this GitLabGroup grp, string groupName, AccessLevel level)
        {
            return Configure(grp, _ =>
            {
                var permission = new GitLabPermission
                {
                    Group = groupName ?? throw new ArgumentNullException(nameof(groupName)),
                    Level = level,
                };

                grp.Permissions.Add(permission);
            });
        }

        /// <summary>
        /// Add group permission in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="groupName">Group fullname (required)</param>
        /// <param name="level">Access level (required)</param>
        public static GitLabProject WithGroupPermission(this GitLabProject project, string groupName, AccessLevel level)
        {
            return Configure(project, _ =>
            {
                var permission = new GitLabPermission
                {
                    Group = groupName ?? throw new ArgumentNullException(nameof(groupName)),
                    Level = level,
                };

                project.Permissions.Add(permission);
            });
        }

        /// <summary>
        /// Add milestone in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="title">Title (required)</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithMilestone(this GitLabProject project, string title, Action<GitLabMilestone> configure)
        {
            return Configure(project, _ =>
            {
                var milestone = new GitLabMilestone
                {
                    Title = title ?? throw new ArgumentNullException(nameof(title)),
                };

                project.Milestones.Add(milestone);
                configure(milestone);
            });
        }

        /// <summary>
        /// Add milestone in project
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="title">Title (required)</param>
        /// <param name="id">Explicit ID (config increment)</param>
        /// <param name="description">Description.</param>
        /// <param name="dueDate">Due date time.</param>
        /// <param name="startDate">Start date time.</param>
        /// <param name="createdAt">Creation date time.</param>
        /// <param name="updatedAt">Update date time.</param>
        /// <param name="closedAt">Close date time.</param>
        /// <param name="configure">Configuration method</param>
        public static GitLabProject WithMilestone(this GitLabProject project, string title, int id = default, string? description = null, DateTime? dueDate = null, DateTime? startDate = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, Action<GitLabMilestone>? configure = null)
        {
            return WithMilestone(project, title, milestone =>
            {
                if (id != default)
                    milestone.Id = id;

                milestone.Description = description;
                milestone.DueDate = dueDate;
                milestone.StartDate = startDate;
                milestone.CreatedAt = createdAt;
                milestone.UpdatedAt = updatedAt;
                milestone.ClosedAt = closedAt;
            });
        }

        /// <summary>
        /// Create and fill server from config
        /// </summary>
        /// <param name="config">Config.</param>
        public static GitLabServer BuildServer(this GitLabConfig config)
        {
            var server = CreateServer(config);
            foreach (var user in config.Users)
            {
                CreateUser(server, user);
            }

            foreach (var group in config.Groups.OrderBy(x =>
                string.IsNullOrEmpty(x.Namespace) ? x.Name : $"{x.Namespace}/{x.Name}"))
            {
                CreateGroup(server, @group);
            }

            foreach (var project in config.Projects)
            {
                CreateProject(server, project);
            }

            return server;
        }

        /// <summary>
        /// Create client from config
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="username">Username that use client (if not defined, use default user or the first in the list)</param>
        public static IGitLabClient BuildClient(this GitLabConfig config, string? username = null)
        {
            return CreateClient(BuildServer(config), username ?? config.DefaultUser ?? config.Users.FirstOrDefault()?.Username ?? throw new InvalidOperationException("No user configured"));
        }

        /// <summary>
        /// Create client from server
        /// </summary>
        /// <param name="server">Server.</param>
        /// <param name="username">Username that use client (if not defined, use the first in the list)</param>
        public static IGitLabClient CreateClient(this GitLabServer server, string? username = null)
        {
            username ??= server.Users.FirstOrDefault()?.UserName ?? throw new InvalidOperationException("No user configured");
            return server.CreateClient(server.Users.First(x => string.Equals(x.UserName, username, StringComparison.Ordinal)));
        }

        /// <summary>
        /// Create config from server
        /// </summary>
        /// <param name="server">Server.</param>
        public static GitLabConfig ToConfig(this GitLabServer server)
        {
            var config = new GitLabConfig();
            foreach (var user in server.Users)
            {
                config.Users.Add(ToConfig(user));
            }

            foreach (var group in server.AllGroups.Where(x => !x.IsUserNamespace))
            {
                config.Groups.Add(ToConfig(group));
            }

            foreach (var project in server.AllProjects)
            {
                config.Projects.Add(ToConfig(project));
            }

            return config;
        }

        private static GitLabServer CreateServer(GitLabConfig config)
        {
            return new GitLabServer
            {
                DefaultBranchName = config.DefaultBranch ?? "main",
                DefaultForkVisibilityLevel = config.DefaultVisibility,
                Url = new Uri(Path.GetTempPath()),
            };
        }

        private static void CreateUser(GitLabServer server, GitLabUser user)
        {
            server.Users.Add(new User(user.Username ?? throw new ArgumentException(@"user.Username == null", nameof(user)))
            {
                Id = user.Id,
                Name = user.Name ?? user.Username,
                Email = user.Email ?? $"{user.Username}@example.com",
                AvatarUrl = user.AvatarUrl,
                IsAdmin = user.IsAdmin,
            });
        }

        private static void CreateGroup(GitLabServer server, GitLabGroup group)
        {
            var grp = new Group(group.Name ?? throw new ArgumentException(@"group.Name == null", nameof(group)))
            {
                Id = group.Id,
                Description = group.Description,
                Visibility = group.Visibility ?? group.Parent.DefaultVisibility,
            };

            if (string.IsNullOrEmpty(group.Namespace))
            {
                server.Groups.Add(grp);
            }
            else
            {
                var parent = GetOrCreateGroup(server, group.Namespace);
                parent.Groups.Add(grp);
            }

            foreach (var label in group.Labels)
            {
                CreateLabel(grp, label);
            }

            foreach (var permission in group.Permissions)
            {
                CreatePermission(server, grp, permission);
            }
        }

        private static void CreateProject(GitLabServer server, GitLabProject project)
        {
            var prj = new Project(project.Name ?? Guid.NewGuid().ToString("D"))
            {
                Id = project.Id,
                Description = project.Description,
                DefaultBranch = project.DefaultBranch ?? server.DefaultBranchName,
                Visibility = project.Visibility ?? project.Parent.DefaultVisibility,
            };

            var group = GetOrCreateGroup(server, project.Namespace ?? Guid.NewGuid().ToString("D"));
            group.Projects.Add(prj);

            foreach (var commit in project.Commits)
            {
                CreateCommit(server, prj, commit);
            }

            foreach (var label in project.Labels)
            {
                CreateLabel(prj, label);
            }

            foreach (var milestone in project.Milestones)
            {
                CreateMilestone(prj, milestone);
            }

            foreach (var issue in project.Issues)
            {
                CreateIssue(server, prj, issue);
            }

            foreach (var mergeRequest in project.MergeRequests)
            {
                CreateMergeRequest(server, prj, mergeRequest);
            }

            foreach (var permission in project.Permissions)
            {
                CreatePermission(server, prj, permission);
            }

            if (!string.IsNullOrEmpty(project.ClonePath))
            {
                var folderPath = Path.GetDirectoryName(Path.GetFullPath(project.ClonePath));
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath!);

                // libgit2sharp cannot clone with an other folder name
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = $"clone \"{prj.SshUrl}\" \"{Path.GetFileName(project.ClonePath)}\"",
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        WorkingDirectory = folderPath,
                    },
                };

                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    var error = process.StandardError.ReadToEnd();
                    throw new GitLabException($"Cannot clone '{prj.PathWithNamespace}' in '{project.ClonePath}': {error}");
                }
            }
        }

        private static void CreateCommit(GitLabServer server, Project prj, GitLabCommit commit)
        {
            var username = commit.User ?? commit.Parent.Parent.DefaultUser ?? throw new InvalidOperationException("Default user is required when author not set");
            var user = server.Users.First(x => string.Equals(x.UserName, username, StringComparison.Ordinal));
            var targetBranch = commit.TargetBranch;
            if (string.IsNullOrEmpty(targetBranch))
            {
                var branchExists = string.IsNullOrEmpty(commit.SourceBranch) || prj.Repository.GetAllBranches().Any(x => string.Equals(x.FriendlyName, commit.SourceBranch, StringComparison.Ordinal));
                if (!branchExists)
                    prj.Repository.CreateBranch(commit.SourceBranch);

                var files = commit.Files.Count == 0
                    ? new[] { File.CreateFromText("test.txt", Guid.NewGuid().ToString()) }
                    : commit.Files.Select(x => File.CreateFromText(x.Path, x.Content ?? string.Empty));
                prj.Repository.Commit(user, commit.Message ?? Guid.NewGuid().ToString("D"), commit.SourceBranch, files);
            }
            else
            {
                prj.Repository.Merge(user, commit.SourceBranch, targetBranch);
                if (commit.DeleteSourceBranch)
                    prj.Repository.RemoveBranch(targetBranch);
            }

            foreach (var tag in commit.Tags)
            {
                prj.Repository.CreateTag(tag);
            }
        }

        private static void CreateLabel(Group group, GitLabLabel label)
        {
            group.Labels.Add(new Label
            {
                Name = label.Name ?? throw new ArgumentException(@"label.Name == null", nameof(label)),
                Color = label.Color ?? "#d9534f",
                Description = label.Description,
            });
        }

        private static void CreateLabel(Project project, GitLabLabel label)
        {
            project.Labels.Add(new Label
            {
                Name = label.Name ?? throw new ArgumentException(@"label.Name == null", nameof(label)),
                Color = label.Color ?? "#d9534f",
                Description = label.Description,
            });
        }

        private static void CreateLabel(Project project, string label)
        {
            var model = project.Labels.Concat(GetLabels(project.Group)).FirstOrDefault(x => string.Equals(x.Name, label, StringComparison.Ordinal));
            if (model == null)
                project.Labels.Add(label);

            static IEnumerable<Label> GetLabels(Group group)
            {
                return group.Parent != null ? group.Labels.Concat(GetLabels(group.Parent)) : group.Labels;
            }
        }

        private static void CreateIssue(GitLabServer server, Project project, GitLabIssue issue)
        {
            foreach (var label in issue.Labels)
            {
                CreateLabel(project, label);
            }

            var issueAuthor = issue.Author ?? issue.Parent.Parent.DefaultUser ?? throw new InvalidOperationException("Default user is required when author not set");
            var issueAssignee = issue.Assignee;
            project.Issues.Add(new Issue
            {
                Iid = issue.Id,
                Title = issue.Title ?? Guid.NewGuid().ToString("D"),
                Description = issue.Description,
                Labels = issue.Labels.ToArray(),
                Author = new UserRef(server.Users.First(x => string.Equals(x.UserName, issueAuthor, StringComparison.Ordinal))),
                Assignees = string.IsNullOrEmpty(issueAssignee) ? Array.Empty<UserRef>() : issueAssignee.Split(',').Select(a => new UserRef(server.Users.First(x => string.Equals(x.UserName, a, StringComparison.Ordinal)))).ToArray(),
                Milestone = string.IsNullOrEmpty(issue.Milestone) ? null : GetOrCreateMilestone(project, issue.Milestone),
                UpdatedAt = issue.UpdatedAt ?? DateTimeOffset.UtcNow,
                ClosedAt = issue.ClosedAt,
            });
        }

        private static void CreateMergeRequest(GitLabServer server, Project project, GitLabMergeRequest mergeRequest)
        {
            foreach (var label in mergeRequest.Labels)
            {
                CreateLabel(project, label);
            }

            var mergeRequestAuthor = mergeRequest.Author ?? mergeRequest.Parent.Parent.DefaultUser ?? throw new InvalidOperationException("Default user is required when author not set");
            var mergeRequestAssignee = mergeRequest.Assignee;
            var request = new MergeRequest
            {
                Iid = mergeRequest.Id,
                Title = mergeRequest.Title ?? Guid.NewGuid().ToString("D"),
                Description = mergeRequest.Description,
                Author = new UserRef(server.Users.First(x => string.Equals(x.UserName, mergeRequestAuthor, StringComparison.Ordinal))),
                Assignee = string.IsNullOrEmpty(mergeRequestAssignee) ? null : new UserRef(server.Users.First(x => string.Equals(x.UserName, mergeRequestAssignee, StringComparison.Ordinal))),
                SourceBranch = mergeRequest.SourceBranch,
                TargetBranch = mergeRequest.TargetBranch ?? server.DefaultBranchName,
                CreatedAt = mergeRequest.CreatedAt ?? DateTimeOffset.UtcNow,
                UpdatedAt = mergeRequest.UpdatedAt ?? DateTimeOffset.UtcNow,
                ClosedAt = mergeRequest.ClosedAt,
                MergedAt = mergeRequest.MergedAt,
                SourceProject = project,
            };

            foreach (var label in mergeRequest.Labels)
            {
                request.Labels.Add(label);
            }

            foreach (var approver in mergeRequest.Approvers)
            {
                request.Approvers.Add(new UserRef(server.Users.First(x => string.Equals(x.UserName, approver, StringComparison.Ordinal))));
            }

            project.MergeRequests.Add(request);
        }

        private static void CreatePermission(GitLabServer server, Project project, GitLabPermission permission)
        {
            project.Permissions.Add(CreatePermission(server, permission));
        }

        private static void CreatePermission(GitLabServer server, Group group, GitLabPermission permission)
        {
            group.Permissions.Add(CreatePermission(server, permission));
        }

        private static Permission CreatePermission(GitLabServer server, GitLabPermission permission)
        {
            return string.IsNullOrEmpty(permission.User)
                ? new Permission(GetOrCreateGroup(server, permission.Group), permission.Level)
                : new Permission(server.Users.First(x => string.Equals(x.UserName, permission.User, StringComparison.Ordinal)), permission.Level);
        }

        private static void CreateMilestone(Project project, GitLabMilestone milestone)
        {
            project.Milestones.Add(new Milestone
            {
                Id = milestone.Id,
                Title = milestone.Title,
                Description = milestone.Description,
                DueDate = milestone.DueDate ?? DateTimeOffset.Now,
                StartDate = milestone.StartDate ?? DateTimeOffset.Now,
                UpdatedAt = milestone.UpdatedAt ?? DateTimeOffset.UtcNow,
                ClosedAt = milestone.ClosedAt,
            });
        }

        private static Milestone GetOrCreateMilestone(Project project, string title)
        {
            var milestone = project.Milestones.FirstOrDefault(x => string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase));
            if (milestone == null)
            {
                milestone = new Milestone
                {
                    Title = title,
                };

                project.Milestones.Add(milestone);
            }

            return milestone;
        }

        private static Group GetOrCreateGroup(GitLabServer server, string @namespace)
        {
            var names = @namespace.Split('/');
            var group = server.Users.FirstOrDefault(x => string.Equals(x.UserName, names[0], StringComparison.Ordinal))?.Namespace ??
                        server.Groups.FirstOrDefault(x => string.Equals(x.Name, names[0], StringComparison.Ordinal));

            if (group == null)
                return GetOrCreateGroup(server.Groups, names.ToArray());

            return names.Length > 1 ? GetOrCreateGroup(group.Groups, names.Skip(1).ToArray()) : group;
        }

        private static Group GetOrCreateGroup(GroupCollection parent, IReadOnlyList<string> names)
        {
            var group = parent.FirstOrDefault(x => string.Equals(x.Name, names[0], StringComparison.Ordinal));
            if (group == null)
            {
                group = new Group(names[0])
                {
                    Visibility = VisibilityLevel.Internal,
                };

                parent.Add(group);
            }

            return names.Count > 1 ? GetOrCreateGroup(group.Groups, names.Skip(1).ToArray()) : group;
        }

        private static GitLabUser ToConfig(User user)
        {
            return new GitLabUser
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                IsAdmin = user.IsAdmin,
            };
        }

        private static GitLabGroup ToConfig(Group group)
        {
            var grp = new GitLabGroup
            {
                Id = group.Id,
                Name = group.Name,
                Namespace = group.Parent == null ? null : GetNamespace(group.Parent),
                Description = group.Description,
                Visibility = group.Visibility,
            };

            foreach (var label in group.Labels)
            {
                grp.Labels.Add(ToConfig(label));
            }

            foreach (var permission in group.Permissions)
            {
                grp.Permissions.Add(ToConfig(permission));
            }

            return grp;
        }

        private static GitLabProject ToConfig(Project project)
        {
            var prj = new GitLabProject
            {
                Id = project.Id,
                Name = project.Name,
                Namespace = GetNamespace(project.Group),
                Description = project.Description,
                DefaultBranch = project.DefaultBranch,
                Visibility = project.Visibility,
            };

            foreach (var label in project.Labels)
            {
                prj.Labels.Add(ToConfig(label));
            }

            foreach (var milestone in project.Milestones)
            {
                prj.Milestones.Add(ToConfig(milestone));
            }

            foreach (var issue in project.Issues)
            {
                prj.Issues.Add(ToConfig(issue));
            }

            foreach (var mergeRequest in project.MergeRequests)
            {
                prj.MergeRequests.Add(ToConfig(mergeRequest));
            }

            foreach (var permission in project.Permissions)
            {
                prj.Permissions.Add(ToConfig(permission));
            }

            return prj;
        }

        private static GitLabLabel ToConfig(Label label)
        {
            return new GitLabLabel
            {
                Name = label.Name,
                Color = label.Color,
                Description = label.Description,
            };
        }

        private static GitLabPermission ToConfig(Permission permission)
        {
            return new GitLabPermission
            {
                User = permission.User?.UserName,
                Group = permission.Group == null ? null : GetNamespace(permission.Group),
                Level = permission.AccessLevel,
            };
        }

        private static GitLabMilestone ToConfig(Milestone milestone)
        {
            return new GitLabMilestone
            {
                Id = milestone.Id,
                Title = milestone.Title,
                Description = milestone.Description,
                DueDate = milestone.DueDate.Date,
                StartDate = milestone.StartDate.Date,
                CreatedAt = milestone.CreatedAt.Date,
                UpdatedAt = milestone.UpdatedAt.Date,
                ClosedAt = milestone.ClosedAt?.Date,
            };
        }

        private static GitLabIssue ToConfig(Issue issue)
        {
            var iss = new GitLabIssue
            {
                Id = issue.Iid,
                Title = issue.Title,
                Description = issue.Description,
                Author = issue.Author?.UserName ?? throw new InvalidOperationException($"Author required in issue '{issue.Title}'"),
                Assignee = issue.Assignee?.UserName,
                CreatedAt = issue.CreatedAt.DateTime,
                UpdatedAt = issue.UpdatedAt.DateTime,
                ClosedAt = issue.ClosedAt?.DateTime,
            };

            foreach (var label in issue.Labels)
            {
                iss.Labels.Add(label);
            }

            return iss;
        }

        private static GitLabMergeRequest ToConfig(MergeRequest mergeRequest)
        {
            var mrg = new GitLabMergeRequest
            {
                Id = mergeRequest.Iid,
                Title = mergeRequest.Title,
                Description = mergeRequest.Description,
                Author = mergeRequest.Author?.UserName ?? throw new InvalidOperationException($"Author required in merge request '{mergeRequest.Title}'"),
                Assignee = mergeRequest.Assignee?.UserName,
                SourceBranch = mergeRequest.SourceBranch,
                TargetBranch = mergeRequest.TargetBranch,
                CreatedAt = mergeRequest.CreatedAt.DateTime,
                UpdatedAt = mergeRequest.UpdatedAt.DateTime,
                ClosedAt = mergeRequest.ClosedAt?.DateTime,
                MergedAt = mergeRequest.MergedAt?.DateTime,
            };

            foreach (var label in mergeRequest.Labels)
            {
                mrg.Labels.Add(label);
            }

            foreach (var approver in mergeRequest.Approvers)
            {
                mrg.Approvers.Add(approver.UserName);
            }

            return mrg;
        }

        private static string GetNamespace(Group group)
        {
            return group.Parent == null ? group.Name : $"{GetNamespace(group.Parent)}/{group.Name}";
        }
    }
}
