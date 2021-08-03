using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Fluent
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
                    Username = username,
                };

                config.Users.Add(user);
                configure(user);
            });
        }

        public static GitLabConfig WithUser(this GitLabConfig config, string username, string name = "User", string email = "user@example.com", string avatarUrl = null, bool isAdmin = false, bool isCurrent = false, Action<GitLabUser> configure = null)
        {
            return WithUser(config, username, user =>
            {
                user.Name = name;
                user.Email = email;
                user.AvatarUrl = avatarUrl;
                user.IsAdmin = isAdmin;
                if (isCurrent)
                {
                    user.AsCurrentUser();
                }

                configure?.Invoke(user);
            });
        }

        public static GitLabUser AsCurrentUser(this GitLabUser user)
        {
            return Configure(user, _ =>
            {
                user.Parent.CurrentUser = user.Username;
            });
        }

        public static GitLabConfig SetCurrentUser(this GitLabConfig config, string username)
        {
            return Configure(config, _ =>
            {
                config.CurrentUser = username;
            });
        }

        public static GitLabConfig WithProject(this GitLabConfig config, string name, Action<GitLabProject> configure)
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

        public static GitLabConfig WithProject(this GitLabConfig config, string name, string @namespace = "functional", string description = null, string defaultBranch = "main", VisibilityLevel visibility = VisibilityLevel.Internal, bool initialCommit = false, bool currentAsMaintainer = false, string clonePath = null, Action<GitLabProject> configure = null)
        {
            return WithProject(config, name, project =>
            {
                project.Namespace = @namespace;
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

                if (currentAsMaintainer)
                {
                    WithUserPermission(project, project.Parent.CurrentUser, AccessLevel.Maintainer);
                }

                configure?.Invoke(project);
            });
        }

        public static GitLabProject WithCommit(this GitLabProject project, Action<GitLabCommit> configure)
        {
            return Configure(project, _ =>
            {
                var commit = new GitLabCommit
                {
                    User = project.Parent.CurrentUser,
                };

                project.Commits.Add(commit);
                configure(commit);
            });
        }

        public static GitLabProject WithCommit(this GitLabProject project, string message, string user = null, string sourceBranch = null, string targetBranch = null, Action<GitLabCommit> configure = null)
        {
            return WithCommit(project, commit =>
            {
                commit.Message = message;
                commit.User = user ?? project.Parent.CurrentUser;
                commit.SourceBranch = sourceBranch;
                commit.TargetBranch = targetBranch;
                configure?.Invoke(commit);
            });
        }

        public static GitLabProject WithMergeCommit(this GitLabProject project, string sourceBranch, string targetBranch = null, string user = null, Action<GitLabCommit> configure = null)
        {
            return WithCommit(project, commit =>
            {
                commit.User = user ?? project.Parent.CurrentUser;
                commit.SourceBranch = sourceBranch;
                commit.TargetBranch = targetBranch ?? project.DefaultBranch;
                configure?.Invoke(commit);
            });
        }

        public static GitLabCommit WithTag(this GitLabCommit commit, string name)
        {
            return Configure(commit, _ =>
            {
                commit.Tags.Add(name);
            });
        }

        public static GitLabCommit WithFile(this GitLabCommit commit, string relativePath, string content = "")
        {
            return Configure(commit, _ =>
            {
                commit.Files.Add(new GitLabFileDescriptor
                {
                    Path = relativePath,
                    Content = content,
                });
            });
        }

        public static GitLabProject WithIssue(this GitLabProject project, string title, Action<GitLabIssue> configure)
        {
            return Configure(project, _ =>
            {
                var issue = new GitLabIssue
                {
                    Title = title,
                    Author = project.Parent.CurrentUser,
                };

                project.Issues.Add(issue);
                configure(issue);
            });
        }

        public static GitLabProject WithIssue(this GitLabProject project, string title, string description = null, string author = null, string assignee = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, IEnumerable<string> labels = null, Action<GitLabIssue> configure = null)
        {
            return WithIssue(project, title, issue =>
            {
                issue.Description = description;
                issue.Author = author ?? project.Parent.CurrentUser;
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
                issue.Labels.Add(label);
            });
        }

        public static GitLabProject WithMergeRequest(this GitLabProject project, string title, string sourceBranch, Action<GitLabMergeRequest> configure)
        {
            return Configure(project, _ =>
            {
                var mergeRequest = new GitLabMergeRequest
                {
                    Title = title,
                    Author = project.Parent.CurrentUser,
                    SourceBranch = sourceBranch,
                    TargetBranch = project.DefaultBranch,
                };

                project.MergeRequests.Add(mergeRequest);
                configure(mergeRequest);
            });
        }

        public static GitLabProject WithMergeRequest(this GitLabProject project, string title, string sourceBranch, string targetBranch = null, string description = null, string author = null, string assignee = null, DateTime? createdAt = null, DateTime? updatedAt = null, DateTime? closedAt = null, DateTime? mergedAt = null, IEnumerable<string> approvers = null, IEnumerable<string> labels = null, Action<GitLabMergeRequest> configure = null)
        {
            return WithMergeRequest(project, title, sourceBranch, mergeRequest =>
            {
                mergeRequest.Description = description;
                mergeRequest.Author = author ?? project.Parent.CurrentUser;
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
                mergeRequest.Labels.Add(label);
            });
        }

        public static GitLabMergeRequest WithApprover(this GitLabMergeRequest mergeRequest, string approver)
        {
            return Configure(mergeRequest, _ =>
            {
                mergeRequest.Approvers.Add(approver);
            });
        }

        public static GitLabProject WithUserPermission(this GitLabProject project, string user, AccessLevel level = AccessLevel.Developer)
        {
            return Configure(project, _ =>
            {
                var permission = new GitLabPermission
                {
                    User = user,
                    Level = level,
                };

                project.Permissions.Add(permission);
            });
        }

        public static GitLabProject WithGroupPermission(this GitLabProject project, string group, AccessLevel level = AccessLevel.Developer)
        {
            return Configure(project, _ =>
            {
                var permission = new GitLabPermission
                {
                    Group = group,
                    Level = level,
                };

                project.Permissions.Add(permission);
            });
        }

        public static IGitLabClient ResolveClient(this GitLabConfig config)
        {
            var server = CreateServer();
            foreach (var user in config.Users)
            {
                CreateUser(server, user);
            }

            foreach (var project in config.Projects)
            {
                CreateProject(server, project);
            }

            var client = CreateClient(server, config.CurrentUser ?? config.Users[0].Username);

            foreach (var project in config.Projects)
            {
                if (string.IsNullOrEmpty(project.ClonePath))
                    continue;

                var projectNamespace = project.Namespace;
                var projectName = project.Name;
                var remoteUrl = client.Projects[$"{projectNamespace}/{projectName}"].SshUrl;

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

            return client;
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
                Name = user.Name,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                IsAdmin = user.IsAdmin,
            });
        }

        private static void CreateProject(this GitLabServer server, GitLabProject project)
        {
            var group = GetOrCreateGroup(server, project.Namespace ?? throw new ArgumentException(@"project.Namespace == null", nameof(project)));
            var prj = new Project(project.Name ?? throw new ArgumentException(@"project.Name == null", nameof(project)))
            {
                Description = project.Description,
                DefaultBranch = project.DefaultBranch,
                Visibility = project.Visibility,
            };

            group.Projects.Add(prj);

            foreach (var commit in project.Commits)
            {
                CreateCommit(server, prj, commit);
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

        private static void CreateIssue(GitLabServer server, Project project, GitLabIssue issue)
        {
            var issueAuthor = issue.Author;
            var issueAssignee = issue.Assignee;
            project.Issues.Add(new Issue
            {
                Title = issue.Title ?? throw new ArgumentException(@"issue.Title == null", nameof(issue)),
                Description = issue.Description,
                Labels = issue.Labels.ToArray(),
                Author = new UserRef(server.Users.First(x => string.Equals(x.UserName, issueAuthor, StringComparison.Ordinal))),
                Assignee = string.IsNullOrEmpty(issueAssignee) ? null : new UserRef(server.Users.First(x => string.Equals(x.UserName, issueAssignee, StringComparison.Ordinal))),
                UpdatedAt = issue.UpdatedAt,
                ClosedAt = issue.ClosedAt,
            });
        }

        private static void CreateMergeRequest(GitLabServer server, Project project, GitLabMergeRequest mergeRequest)
        {
            var mergeRequestAuthor = mergeRequest.Author;
            var mergeRequestAssignee = mergeRequest.Assignee;
            var request = new MergeRequest
            {
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
            project.Permissions.Add(string.IsNullOrEmpty(permission.User)
                ? new Permission(GetOrCreateGroup(server, permission.Group), permission.Level)
                : new Permission(server.Users.First(x => string.Equals(x.UserName, permission.User, StringComparison.Ordinal)), permission.Level));
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
            {
                group = new Group(names[0]);
                server.Groups.Add(group);
            }

            return GetOrCreateGroup(group, names.Skip(1).ToArray());
        }

        private static Group GetOrCreateGroup(Group parent, IReadOnlyList<string> names)
        {
            var name = names.FirstOrDefault();
            if (name == null)
                return parent;

            var group = parent.Groups.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
            if (group == null)
            {
                group = new Group(name);
                parent.Groups.Add(group);
            }

            return GetOrCreateGroup(group, names.Skip(1).ToArray());
        }
    }
}
