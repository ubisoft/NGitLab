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
                    _defaultBranch = Parent.Server?.DefaultBranchName ?? throw new InvalidOperationException("Project is not added to a Server");
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

        public RepositoryAccessLevel ForkingAccessLevel { get; set; }

        public string ImportStatus { get; set; }

        public TimeSpan BuildTimeout { get; set; } = TimeSpan.FromHours(1);

        public bool LfsEnabled { get; set; }

        public bool Archived { get; set; }

        public string[] Tags { get; set; } = Array.Empty<string>();

        public bool Mirror { get; set; }

        public int MirrorUserId { get; set; }

        public bool MirrorTriggerBuilds { get; set; }

        public bool OnlyMirrorProtectedBranch { get; set; }

        public bool MirrorOverwritesDivergedBranches { get; set; }

        public RepositoryAccessLevel RepositoryAccessLevel { get; set; } = RepositoryAccessLevel.Enabled;

        public bool IsEmpty { get; set; }

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

        public ProjectStatistics Statistics { get; set; }

        public void Remove()
        {
            Group.Projects.Remove(this);
        }

        public EffectivePermissions GetEffectivePermissions()
        {
            var result = new Dictionary<User, AccessLevel>();

            foreach (var effectivePermission in Group.GetEffectivePermissions().Permissions)
            {
                Add(effectivePermission.User, effectivePermission.AccessLevel);
            }

            foreach (var permission in Permissions)
            {
                if (permission.User != null)
                {
                    Add(permission.User, permission.AccessLevel);
                }
                else
                {
                    foreach (var effectivePermission in permission.Group.GetEffectivePermissions().Permissions)
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

        public Project Fork(User user)
        {
            return Fork(user.Namespace, user, Name);
        }

        public Project Fork(Group group, User user, string projectName)
        {
            projectName ??= Name;

            var existingProject = group.Projects.FirstOrDefault(p => string.Equals(p.Name, projectName, StringComparison.Ordinal));
            if (existingProject != null)
                return existingProject;

            var newProject = new Project(projectName)
            {
                Description = Description,
                ForkedFrom = this,
                ImportStatus = "finished",
            };

            newProject.Visibility = Server.DefaultForkVisibilityLevel;
            newProject.Permissions.Add(new Permission(user, AccessLevel.Maintainer));
            group.Projects.Add(newProject);
            return newProject;
        }

        public Models.Project ToClientProject()
        {
            var kind = Group.IsUserNamespace ? "user" : "group";

            return new Models.Project
            {
                Id = Id,
                Name = Name,
                Description = Description,
                EmptyRepo = IsEmpty,
                Path = Path,
                PathWithNamespace = PathWithNamespace,
                ForkedFromProject = ForkedFrom?.ToClientProject(),
                ForkingAccessLevel = ForkingAccessLevel,
                ImportStatus = ImportStatus,
                HttpUrl = HttpUrl,
                SshUrl = SshUrl,
                DefaultBranch = DefaultBranch,
                VisibilityLevel = Visibility,
                Namespace = new Namespace() { FullPath = Group.PathWithNameSpace, Id = Group.Id, Kind = kind, Name = Group.Name, Path = Group.Path },
                WebUrl = WebUrl,
                BuildTimeout = (int)BuildTimeout.TotalMinutes,
                RepositoryAccessLevel = RepositoryAccessLevel,
                LfsEnabled = LfsEnabled,
                Archived = Archived,
                ApprovalsBeforeMerge = ApprovalsBeforeMerge,
                MergeMethod = MergeMethod,
                Statistics = Statistics,
                TagList = Tags,
                Mirror = Mirror,
                MirrorUserId = MirrorUserId,
                MirrorTriggerBuilds = MirrorTriggerBuilds,
                OnlyMirrorProtectedBranch = OnlyMirrorProtectedBranch,
                MirrorOverwritesDivergedBranches = MirrorOverwritesDivergedBranches,
            };
        }
    }
}
