﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NGitLab.Models;

#pragma warning disable RS0026 // Adding optional parameters to public methods

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

        public static GitLabConfig WithUser(this GitLabConfig config, string username, string name = "User", string email = "user@example.com", string avatarUrl = null, bool isAdmin = false, bool asDefault = false, Action<GitLabUser> configure = null)
        {
            return WithUser(config, username, user =>
            {
                user.Name = name;
                user.Email = email;
                user.AvatarUrl = avatarUrl;
                user.IsAdmin = isAdmin;
                if (asDefault)
                {
                    user.AsDefaultUser();
                }

                configure?.Invoke(user);
            });
        }

        public static GitLabUser AsDefaultUser(this GitLabUser user)
        {
            return Configure(user, _ =>
            {
                user.Parent.DefaultUser = user.Username;
            });
        }

        public static GitLabConfig SetDefaultUser(this GitLabConfig config, string username)
        {
            return Configure(config, _ =>
            {
                config.DefaultUser = username;
            });
        }

        public static GitLabConfig WithGroup(this GitLabConfig config, string name, Action<GitLabGroup> configure)
        {
            return Configure(config, _ =>
            {
                var group = new GitLabGroup
                {
                    Name = name ?? throw new ArgumentNullException(nameof(name)),
                };

                config.Groups.Add(group);
                configure(group);
            });
        }

        public static GitLabConfig WithGroup(this GitLabConfig config, string name, int id = default, string @namespace = null, string description = null, VisibilityLevel visibility = VisibilityLevel.Internal, bool defaultAsMaintainer = false, Action<GitLabGroup> configure = null)
        {
            return WithGroup(config, name, group =>
            {
                if (id != default)
                    group.Id = id;

                group.Namespace = @namespace;
                group.Description = description;
                group.Visibility = visibility;

                if (defaultAsMaintainer)
                {
                    if (string.IsNullOrEmpty(group.Parent.DefaultUser))
                        throw new InvalidOperationException("Default user not configured");

                    WithUserPermission(group, group.Parent.DefaultUser, AccessLevel.Maintainer);
                }

                configure?.Invoke(group);
            });
        }

        public static GitLabConfig WithProject(this GitLabConfig config, string name, Action<GitLabProject> configure)
        {
            return Configure(config, _ =>
            {
                var project = new GitLabProject
                {
                    Name = name ?? throw new ArgumentNullException(nameof(name)),
                };

                config.Projects.Add(project);
                configure(project);
            });
        }

        public static GitLabConfig WithProject(this GitLabConfig config, string name, int id = default, string @namespace = "functional", string description = null, string defaultBranch = "main", VisibilityLevel visibility = VisibilityLevel.Internal, bool initialCommit = false, bool defaultAsMaintainer = false, string clonePath = null, Action<GitLabProject> configure = null)
        {
            return WithProject(config, name, project =>
            {
                if (id != default)
                    project.Id = id;

                project.Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
                project.Description = description;
                project.DefaultBranch = defaultBranch;
                project.Visibility = visibility;
                project.ClonePath = clonePath;

                if (initialCommit)
                {
                    WithCommit(project, commit =>
                    {
                        commit.Message = "Initial commit";
                        WithFile(commit, "README.md", $"# {name}{Environment.NewLine}");
                    });
                }

                if (defaultAsMaintainer)
                {
                    if (string.IsNullOrEmpty(project.Parent.DefaultUser))
                        throw new InvalidOperationException("Default user not configured");

                    WithUserPermission(project, project.Parent.DefaultUser, AccessLevel.Maintainer);
                }

                configure?.Invoke(project);
            });
        }

        public static GitLabGroup WithLabel(this GitLabGroup group, string name, string color = null, string description = null)
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

        public static GitLabProject WithLabel(this GitLabProject project, string name, string color = null, string description = null)
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

        public static GitLabProject WithCommit(this GitLabProject project, Action<GitLabCommit> configure)
        {
            return Configure(project, _ =>
            {
                var commit = new GitLabCommit
                {
                    User = project.Parent.DefaultUser ?? throw new InvalidOperationException("User is required when user is not configured"),
                };

                project.Commits.Add(commit);
                configure(commit);
            });
        }

        public static GitLabProject WithCommit(this GitLabProject project, string message, string user = null, string sourceBranch = null, string targetBranch = null, IEnumerable<string> tags = null, Action<GitLabCommit> configure = null)
        {
            return WithCommit(project, commit =>
            {
                commit.Message = message;
                commit.User = user ?? project.Parent.DefaultUser ?? throw new InvalidOperationException("User is required when user is not configured");
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

        public static GitLabProject WithMergeCommit(this GitLabProject project, string sourceBranch, string targetBranch = null, string user = null, IEnumerable<string> tags = null, Action<GitLabCommit> configure = null)
        {
            return WithCommit(project, commit =>
            {
                commit.User = user ?? project.Parent.DefaultUser;
                commit.SourceBranch = sourceBranch ?? throw new ArgumentNullException(nameof(sourceBranch));
                commit.TargetBranch = targetBranch ?? project.DefaultBranch;
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

        public static GitLabCommit WithTag(this GitLabCommit commit, string name)
        {
            return Configure(commit, _ =>
            {
                commit.Tags.Add(name ?? throw new ArgumentNullException(nameof(name)));
            });
        }

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

        public static GitLabProject WithIssue(this GitLabProject project, string title, string author, Action<GitLabIssue> configure)
        {
            return Configure(project, _ =>
            {
                var issue = new GitLabIssue
                {
                    Title = title ?? throw new ArgumentNullException(nameof(title)),
                    Author = author ?? project.Parent.DefaultUser ?? throw new InvalidOperationException("User is required when user is not configured"),
                };

                project.Issues.Add(issue);
                configure(issue);
            });
        }

        public static GitLabProject WithIssue(this GitLabProject project, string title, int id = default, string description = null, string author = null, string assignee = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, IEnumerable<string> labels = null, Action<GitLabIssue> configure = null)
        {
            return WithIssue(project, title, author, issue =>
            {
                if (id != default)
                    issue.Id = id;

                issue.Description = description;
                issue.Assignee = assignee;
                issue.CreatedAt = createdAt ?? DateTime.Now;
                issue.UpdatedAt = updatedAt ?? DateTime.Now;
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

        public static GitLabIssue WithLabel(this GitLabIssue issue, string label)
        {
            return Configure(issue, _ =>
            {
                issue.Labels.Add(label ?? throw new ArgumentNullException(nameof(label)));
            });
        }

        public static GitLabProject WithMergeRequest(this GitLabProject project, string title, string sourceBranch, string author, Action<GitLabMergeRequest> configure)
        {
            return Configure(project, _ =>
            {
                var mergeRequest = new GitLabMergeRequest
                {
                    Title = title ?? throw new ArgumentNullException(nameof(title)),
                    Author = author ?? project.Parent.DefaultUser ?? throw new InvalidOperationException("User is required when user is not configured"),
                    SourceBranch = sourceBranch ?? throw new ArgumentNullException(nameof(sourceBranch)),
                    TargetBranch = project.DefaultBranch,
                };

                project.MergeRequests.Add(mergeRequest);
                configure(mergeRequest);
            });
        }

        public static GitLabProject WithMergeRequest(this GitLabProject project, string title, string sourceBranch, int id = default, string targetBranch = null, string description = null, string author = null, string assignee = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, DateTime? mergedAt = null, IEnumerable<string> approvers = null, IEnumerable<string> labels = null, Action<GitLabMergeRequest> configure = null)
        {
            return WithMergeRequest(project, title, sourceBranch, author, mergeRequest =>
            {
                if (id != default)
                    mergeRequest.Id = id;

                mergeRequest.Description = description;
                mergeRequest.Assignee = assignee;
                mergeRequest.TargetBranch = targetBranch ?? mergeRequest.TargetBranch;
                mergeRequest.CreatedAt = createdAt ?? DateTime.Now;
                mergeRequest.UpdatedAt = updatedAt ?? DateTime.Now;
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

        public static GitLabMergeRequest WithLabel(this GitLabMergeRequest mergeRequest, string label)
        {
            return Configure(mergeRequest, _ =>
            {
                mergeRequest.Labels.Add(label ?? throw new ArgumentNullException(nameof(label)));
            });
        }

        public static GitLabMergeRequest WithApprover(this GitLabMergeRequest mergeRequest, string approver)
        {
            return Configure(mergeRequest, _ =>
            {
                mergeRequest.Approvers.Add(approver ?? throw new ArgumentNullException(nameof(approver)));
            });
        }

        public static GitLabGroup WithUserPermission(this GitLabGroup group, string user, AccessLevel level = AccessLevel.Developer)
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

        public static GitLabProject WithUserPermission(this GitLabProject project, string user, AccessLevel level = AccessLevel.Developer)
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

        public static GitLabGroup WithGroupPermission(this GitLabGroup grp, string group, AccessLevel level = AccessLevel.Developer)
        {
            return Configure(grp, _ =>
            {
                var permission = new GitLabPermission
                {
                    Group = group ?? throw new ArgumentNullException(nameof(group)),
                    Level = level,
                };

                grp.Permissions.Add(permission);
            });
        }

        public static GitLabProject WithGroupPermission(this GitLabProject project, string group, AccessLevel level = AccessLevel.Developer)
        {
            return Configure(project, _ =>
            {
                var permission = new GitLabPermission
                {
                    Group = group ?? throw new ArgumentNullException(nameof(group)),
                    Level = level,
                };

                project.Permissions.Add(permission);
            });
        }

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

        public static GitLabProject WithMilestone(this GitLabProject project, string title, int id = default, string description = null, DateTime? dueDate = null, DateTime? startDate = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, Action<GitLabMilestone> configure = null)
        {
            return WithMilestone(project, title, milestone =>
            {
                if (id != default)
                    milestone.Id = id;

                milestone.Description = description;
                milestone.DueDate = dueDate;
                milestone.StartDate = startDate;
                milestone.CreatedAt = createdAt ?? DateTime.Now;
                milestone.UpdatedAt = updatedAt ?? DateTime.Now;
                milestone.ClosedAt = closedAt;
            });
        }

        public static GitLabServer ResolveServer(this GitLabConfig config)
        {
            var server = CreateServer();
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

            foreach (var project in config.Projects)
            {
                if (string.IsNullOrEmpty(project.ClonePath))
                    continue;

                var projectNamespace = project.Namespace;
                var projectName = project.Name;
                var remoteUrl = server.AllProjects.FindProject($"{projectNamespace}/{projectName}").SshUrl;

                var folderPath = Path.GetDirectoryName(Path.GetFullPath(project.ClonePath));
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath!);

                // libgit2sharp cannot clone with an other folder name
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = $"clone \"{remoteUrl}\" \"{Path.GetFileName(project.ClonePath)}\"",
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
                    throw new GitLabException($"Cannot clone '{projectNamespace}/{projectName}' in '{project.ClonePath}': {error}");
                }
            }

            return server;
        }

        public static IGitLabClient ResolveClient(this GitLabConfig config, string username = null)
        {
            return ResolveClient(ResolveServer(config), username ?? config.DefaultUser ?? config.Users.FirstOrDefault()?.Username ?? throw new InvalidOperationException("No user configured"));
        }

        public static IGitLabClient ResolveClient(this GitLabServer server, string username = null)
        {
            return CreateClient(server, username ?? server.Users.FirstOrDefault()?.UserName ?? throw new InvalidOperationException("No user configured"));
        }

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

        private static GitLabServer CreateServer()
        {
            return new GitLabServer
            {
                DefaultBranchName = "main",
                Url = new Uri(Path.GetTempPath()),
            };
        }

        private static void CreateUser(GitLabServer server, GitLabUser user)
        {
            server.Users.Add(new User(user.Username ?? throw new ArgumentException(@"user.Username == null", nameof(user)))
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
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
                Visibility = group.Visibility,
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
            var prj = new Project(project.Name ?? throw new ArgumentException(@"project.Name == null", nameof(project)))
            {
                Id = project.Id,
                Description = project.Description,
                DefaultBranch = project.DefaultBranch,
                Visibility = project.Visibility,
            };

            var group = GetOrCreateGroup(server, project.Namespace ?? throw new ArgumentException(@"project.Namespace == null", nameof(project)));
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
        }

        private static void CreateCommit(GitLabServer server, Project prj, GitLabCommit commit)
        {
            var username = commit.User;
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
                Color = label.Color,
                Description = label.Description,
            });
        }

        private static void CreateLabel(Project project, GitLabLabel label)
        {
            project.Labels.Add(new Label
            {
                Name = label.Name ?? throw new ArgumentException(@"label.Name == null", nameof(label)),
                Color = label.Color,
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

            var issueAuthor = issue.Author;
            var issueAssignee = issue.Assignee;
            project.Issues.Add(new Issue
            {
                Iid = issue.Id,
                Title = issue.Title ?? throw new ArgumentException(@"issue.Title == null", nameof(issue)),
                Description = issue.Description,
                Labels = issue.Labels.ToArray(),
                Author = new UserRef(server.Users.First(x => string.Equals(x.UserName, issueAuthor, StringComparison.Ordinal))),
                Assignee = string.IsNullOrEmpty(issueAssignee) ? null : new UserRef(server.Users.First(x => string.Equals(x.UserName, issueAssignee, StringComparison.Ordinal))),
                Milestone = string.IsNullOrEmpty(issue.Milestone) ? null : GetOrCreateMilestone(project, issue.Milestone),
                UpdatedAt = issue.UpdatedAt,
                ClosedAt = issue.ClosedAt,
            });
        }

        private static void CreateMergeRequest(GitLabServer server, Project project, GitLabMergeRequest mergeRequest)
        {
            foreach (var label in mergeRequest.Labels)
            {
                CreateLabel(project, label);
            }

            var mergeRequestAuthor = mergeRequest.Author;
            var mergeRequestAssignee = mergeRequest.Assignee;
            var request = new MergeRequest
            {
                Iid = mergeRequest.Id,
                Title = mergeRequest.Title ?? throw new ArgumentException(@"mergeRequest.Title == null", nameof(mergeRequest)),
                Description = mergeRequest.Description,
                Author = new UserRef(server.Users.First(x => string.Equals(x.UserName, mergeRequestAuthor, StringComparison.Ordinal))),
                Assignee = string.IsNullOrEmpty(mergeRequestAssignee) ? null : new UserRef(server.Users.First(x => string.Equals(x.UserName, mergeRequestAssignee, StringComparison.Ordinal))),
                SourceBranch = mergeRequest.SourceBranch,
                TargetBranch = mergeRequest.TargetBranch,
                CreatedAt = mergeRequest.CreatedAt,
                UpdatedAt = mergeRequest.UpdatedAt,
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
                UpdatedAt = milestone.UpdatedAt,
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

        private static IGitLabClient CreateClient(GitLabServer server, string username)
        {
            return server.CreateClient(server.Users.First(x => string.Equals(x.UserName, username, StringComparison.Ordinal)));
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