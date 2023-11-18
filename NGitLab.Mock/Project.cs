using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Project : GitLabObject
    {
        public Project()
            : this(Guid.NewGuid().ToString("N"))
        {
        }

        public Project(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Permissions = new PermissionCollection(this);
            Hooks = new ProjectHookCollection(this);
            Repository = new Repository(this);
            RegisteredRunners = new RunnerCollection(this);
            EnabledRunners = new RunnerRefCollection(this);
            MergeRequests = new MergeRequestCollection(this);
            Issues = new IssueCollection(this);
            Milestones = new MilestoneCollection(this);
            Pipelines = new PipelineCollection(this);
            Jobs = new JobCollection(this);
            Badges = new BadgeCollection(this);
            Labels = new LabelsCollection(this);
            CommitInfos = new CommitInfoCollection(this);
            CommitStatuses = new CommitStatusCollection(this);
            Releases = new ReleaseCollection(this);
            ProtectedBranches = new ProtectedBranchCollection(this);
            ApprovalsBeforeMerge = 0;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        private string _defaultBranch;

        public string DefaultBranch
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultBranch))
                {
                    _defaultBranch = Parent?.Server?.DefaultBranchName ?? throw new InvalidOperationException("Project is not added to a Server");
                }

                return _defaultBranch;
            }

            set => _defaultBranch = value;
        }

        public string WebUrl => Server.MakeUrl(PathWithNamespace);

        public string SshUrl => $"file://{Repository.FullPath}";

        public string HttpUrl => $"file://{Repository.FullPath}";

        public VisibilityLevel Visibility { get; set; }

        public Project ForkedFrom { get; internal set; }

        public RepositoryAccessLevel ForkingAccessLevel { get; set; } = RepositoryAccessLevel.Enabled;

        public string ImportStatus { get; set; }

        public TimeSpan BuildTimeout { get; set; } = TimeSpan.FromHours(1);

        public bool LfsEnabled { get; set; }

        public bool Archived { get; set; }

        [Obsolete("Deprecated by GitLab. Use Topics instead")]
        public string[] Tags
        {
            get => Topics;
            set => Topics = value;
        }

        public string[] Topics { get; set; } = Array.Empty<string>();

        public bool Mirror { get; set; }

        public int MirrorUserId { get; set; }

        public bool MirrorTriggerBuilds { get; set; }

        public bool OnlyMirrorProtectedBranch { get; set; }

        public bool AccessibleMergeRequests { get; set; } = true;

        public bool MirrorOverwritesDivergedBranches { get; set; }

        public RepositoryAccessLevel RepositoryAccessLevel { get; set; } = RepositoryAccessLevel.Enabled;

        public PermissionCollection Permissions { get; }

        public Repository Repository { get; }

        public MergeRequestCollection MergeRequests { get; }

        public PipelineCollection Pipelines { get; }

        public JobCollection Jobs { get; }

        public LabelsCollection Labels { get; }

        public CommitInfoCollection CommitInfos { get; }

        public CommitStatusCollection CommitStatuses { get; }

        public ReleaseCollection Releases { get; }

        public Group Group => (Group)Parent;

        public string Path => Slug.Create(Name);

        public string PathWithNamespace => Group == null ? Path : (Group.PathWithNameSpace + "/" + Path);

        public string FullName => Group == null ? Name : (Group.FullName + "/" + Name);

        public ProjectHookCollection Hooks { get; }

        public RunnerCollection RegisteredRunners { get; }

        public RunnerRefCollection EnabledRunners { get; }

        public IssueCollection Issues { get; }

        public MilestoneCollection Milestones { get; }

        public BadgeCollection Badges { get; }

        public int ApprovalsBeforeMerge { get; set; }

        public string MergeMethod { get; set; }

        public bool AllThreadsMustBeResolvedToMerge { get; set; }

        public ProjectStatistics Statistics { get; set; }

        public ProtectedBranchCollection ProtectedBranches { get; }

        public string RunnersToken { get; internal set; }

        public void Remove()
        {
            Group.Projects.Remove(this);
        }

        public EffectivePermissions GetEffectivePermissions() => GetEffectivePermissions(includeInheritedPermissions: true);

        public EffectivePermissions GetEffectivePermissions(bool includeInheritedPermissions)
        {
            var result = new Dictionary<User, AccessLevel>();

            if (includeInheritedPermissions)
            {
                foreach (var effectivePermission in Group.GetEffectivePermissions(includeInheritedPermissions).Permissions)
                {
                    Add(effectivePermission.User, effectivePermission.AccessLevel);
                }
            }

            foreach (var permission in Permissions)
            {
                if (permission.User != null)
                {
                    Add(permission.User, permission.AccessLevel);
                }
                else
                {
                    foreach (var effectivePermission in permission.Group.GetEffectivePermissions(includeInheritedPermissions).Permissions)
                    {
                        Add(effectivePermission.User, effectivePermission.AccessLevel);
                    }
                }
            }

            return new EffectivePermissions(result.Select(kvp => new EffectiveUserPermission(kvp.Key, kvp.Value)).ToList());

            void Add(User user, AccessLevel accessLevel)
            {
                if (result.TryGetValue(user, out var existingAccessLevel))
                {
                    if (accessLevel > existingAccessLevel)
                    {
                        result[user] = accessLevel;
                    }
                }
                else
                {
                    result[user] = accessLevel;
                }
            }
        }

        public bool CanUserViewProject(User user)
        {
            if (Visibility == VisibilityLevel.Public)
                return true;

            if (Visibility == VisibilityLevel.Internal && user != null)
                return true;

            if (user == null)
                return false;

            if (user.IsAdmin)
                return true;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            if (accessLevel.HasValue)
                return true;

            return false;
        }

        public bool CanUserEditProject(User user)
        {
            if (user == null)
                return false;

            if (user.IsAdmin)
                return true;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value >= AccessLevel.Maintainer;
        }

        public bool CanUserViewConfidentialIssues(User user)
        {
            if (user == null)
                return false;

            if (user.IsAdmin)
                return true;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value >= AccessLevel.Reporter;
        }

        public bool CanUserDeleteProject(User user)
        {
            if (user.IsAdmin)
                return true;

            return IsUserOwner(user);
        }

        public bool CanUserContributeToProject(User user)
        {
            if (user == null)
                return false;

            if (user.IsAdmin)
                return true;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value >= AccessLevel.Developer;
        }

        public bool IsUserOwner(User user)
        {
            if (user == null)
                return false;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value == AccessLevel.Owner;
        }

        public bool IsUserMember(User user)
        {
            if (user == null)
                return false;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue;
        }

        public MergeRequest CreateNewMergeRequest(User user, string branchName, string targetBranch, string title, string description)
        {
            if (Repository.GetBranchTipCommit(branchName) == null)
            {
                Repository.Commit(user, "initial commit");
            }

            Repository.CreateAndCheckoutBranch(branchName);
            Repository.Commit(user, "edit");

            var mergeRequest = MergeRequests.Add(
                sourceBranch: branchName,
                targetBranch: targetBranch,
                title: title,
                user);
            mergeRequest.Description = description;

            return mergeRequest;
        }

        public MergeRequest CreateMergeRequest(User user, string title, string description, string targetBranchName, string sourceBranchName, Project sourceProject = null)
        {
            var targetProject = this;
            if (sourceProject is not null && sourceProject != targetProject && sourceProject.ForkedFrom != targetProject)
                throw new InvalidOperationException("Cannot create a merge request from a source project different from the target project if the former is not a fork of the latter");

            sourceProject ??= this;

            var sourceBranchCommit = sourceProject.Repository.GetBranchTipCommit(sourceBranchName);
            if (sourceBranchCommit is null)
                throw new InvalidOperationException($"Branch '{sourceBranchName}' not found in source project");

            var targetBranchCommit = targetProject.Repository.GetBranchTipCommit(targetBranchName);
            if (targetBranchCommit is null)
                throw new InvalidOperationException($"Branch '{targetBranchName}' not found in target project");

            // If source project is not target project, fetch the former's source branch into the latter
            var consolidatedSourceBranchName = sourceBranchName;
            if (sourceProject != targetProject)
            {
                consolidatedSourceBranchName = targetProject.Repository.FetchBranchFromFork(sourceProject, sourceBranchName);
            }

            var commonCommit = targetProject.Repository.FindMergeBase(targetBranchCommit, sourceBranchCommit);
            if (commonCommit is null)
                throw new InvalidOperationException($"Branch '{consolidatedSourceBranchName}' does not seem to stem from branch '{targetBranchName}'");

            var mr = new MergeRequest
            {
                SourceProject = sourceProject,
                SourceBranch = sourceBranchName,
                TargetBranch = targetBranchName,
                Title = title,
                Description = description,
                Author = user,
            };
            MergeRequests.Add(mr);

            return mr;
        }

        public bool RemoveRunner(int runnerId)
        {
            return RegisteredRunners.Remove(runnerId);
        }

        public Runner AddRunner(string name, string description, bool active, bool locked, bool isShared, bool runUntagged, int id)
        {
            var runner = new Runner
            {
                Name = name,
                Description = description,
                Active = active,
                Locked = locked,
                IsShared = isShared,
                IpAddress = "0.0.0.0",
                Online = true,
                RunUntagged = runUntagged,
                Id = id,
            };

            RegisteredRunners.Add(runner);
            if (active)
            {
                EnabledRunners.Add(new RunnerRef(runner));
            }

            return runner;
        }

        public Runner AddRunner(string name, string description, bool active, bool locked, bool isShared)
        {
            return AddRunner(name, description, active, locked, isShared, runUntagged: false, default);
        }

        public Runner AddRunner(string name, string description, bool active, bool locked, bool isShared, bool runUntagged)
        {
            return AddRunner(name, description, active, locked, isShared, runUntagged, default);
        }

        public Project Fork(User user)
        {
            return Fork(user.Namespace, user, Name);
        }

        public Project Fork(Group group, User user, string projectName)
        {
            projectName ??= Name;

            var existingProject = group.Projects.FirstOrDefault(p => string.Equals(p.Name, projectName, StringComparison.Ordinal));
            if (existingProject != null)
            {
                return existingProject == this ?
                    throw new InvalidOperationException($"Cannot fork '{PathWithNamespace}' onto itself") :
                    existingProject;
            }

            var newProject = new Project(projectName)
            {
                Description = Description,
                ForkedFrom = this,
                ImportStatus = "finished",
                Visibility = Server.DefaultForkVisibilityLevel,
            };

            newProject.Permissions.Add(new Permission(user, AccessLevel.Maintainer));
            group.Projects.Add(newProject);
            return newProject;
        }

        /// <summary>
        /// https://docs.gitlab.com/ee/user/project/settings/project_access_tokens.html#bot-users-for-projects
        /// </summary>
        /// <param name="tokenName">Name of the token</param>
        /// <param name="accessLevel">AccessLevel to give to the bot user</param>
        /// <returns>Bot user that have been added to the project</returns>
        public User CreateBotUser(string tokenName, AccessLevel accessLevel)
        {
            var botUsername = $"project_{Id}_bot_{Guid.NewGuid():D}";
            var bot = new User(botUsername)
            {
                Name = tokenName,
                Email = $"{botUsername}@noreply.example.com",
            };
            Permissions.Add(new Permission(bot, accessLevel));
            Server.Users.Add(bot);
            return bot;
        }

        public Models.Project ToClientProject(User currentUser)
        {
            var kind = Group.IsUserNamespace ? "user" : "group";

#pragma warning disable CS0618 // Type or member is obsolete
            return new Models.Project
            {
                Id = Id,
                Name = Name,
                Description = Description,
                EmptyRepo = Repository.IsEmpty,
                Path = Path,
                PathWithNamespace = PathWithNamespace,
                ForkedFromProject = ForkedFrom?.ToClientProject(currentUser),
                ForkingAccessLevel = ForkingAccessLevel,
                ImportStatus = ImportStatus,
                HttpUrl = HttpUrl,
                SshUrl = SshUrl,
                DefaultBranch = DefaultBranch,
                VisibilityLevel = Visibility,
                Namespace = new Namespace { FullPath = Group.PathWithNameSpace, Id = Group.Id, Kind = kind, Name = Group.Name, Path = Group.Path },
                WebUrl = WebUrl,
                BuildTimeout = (int)BuildTimeout.TotalMinutes,
                RepositoryAccessLevel = RepositoryAccessLevel,
                RunnersToken = RunnersToken,
                LfsEnabled = LfsEnabled,
                Archived = Archived,
                ApprovalsBeforeMerge = ApprovalsBeforeMerge,
                MergeMethod = MergeMethod,
                Statistics = Statistics,
                TagList = Tags,
                Topics = Topics,
                Mirror = Mirror,
                MirrorUserId = MirrorUserId,
                MirrorTriggerBuilds = MirrorTriggerBuilds,
                OnlyMirrorProtectedBranch = OnlyMirrorProtectedBranch,
                MirrorOverwritesDivergedBranches = MirrorOverwritesDivergedBranches,
                Permissions = GetProjectPermissions(currentUser),
            };
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private ProjectPermissions GetProjectPermissions(User user)
        {
            var projectPermissions = new ProjectPermissions();
            if (Permissions.FirstOrDefault(x => x.User == user)?.AccessLevel is { } accessLevel)
            {
                projectPermissions.ProjectAccess = new ProjectPermission { AccessLevel = accessLevel };
            }

            if (Group.GetEffectivePermissions().GetAccessLevel(user) is { } groupAccessLevel)
            {
                projectPermissions.GroupAccess = new ProjectPermission { AccessLevel = groupAccessLevel };
            }

            return projectPermissions;
        }
    }
}
