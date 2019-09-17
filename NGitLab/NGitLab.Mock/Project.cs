using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Mock.Clients;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Project : GitLabObject
    {
        public Project(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Permissions = new PermissionCollection(this);
            Hooks = new ProjectHookCollection(this);
            Repository = new Repository(this);
            RegisteredRunners = new RunnerCollection(this);
            EnabledRunners = new RunnerRefCollection(this);
            MergeRequests = new MergeRequestCollection(this);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultBranch { get; set; } = "master";
        public VisibilityLevel Visibility { get; set; }
        public Project ForkedFrom { get; internal set; }
        public string ImportStatus { get; set; }
        public PermissionCollection Permissions { get; }
        public Repository Repository { get; }
        public MergeRequestCollection MergeRequests { get; }

        public Group Group => (Group)Parent;

        public string Path => Slug.Create(Name);
        public string PathWithNamespace => Group == null ? Path : (Group.PathWithNameSpace + "/" + Path);
        public string FullName => Group == null ? Name : (Group.FullName + "/" + Name);

        public ProjectHookCollection Hooks { get; }
        public RunnerCollection RegisteredRunners { get; }
        public RunnerRefCollection EnabledRunners { get; }

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

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            if (accessLevel.HasValue)
                return true;

            return false;
        }

        public bool CanUserEditProject(User user)
        {
            if (user == null)
                return false;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value >= AccessLevel.Master;
        }

        public bool CanUserDeleteProject(User user)
        {
            return IsUserOwner(user);
        }

        public bool CanUserContributeToProject(User user)
        {
            if (user == null)
                return false;

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

        public Runner AddRunner(string name, string description, bool active, bool locked, bool isShared)
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
            };

            RegisteredRunners.Add(runner);
            if (active)
            {
                EnabledRunners.Add(new RunnerRef(runner));
            }

            return runner;
        }

        public Models.Project ToClientProject()
        {
            return new Models.Project
            {
                Id = Id,
                Name = Name,
                Path = Path,
                PathWithNamespace = PathWithNamespace,
                ForkedFromProject = ForkedFrom?.ToClientProject(),
                ImportStatus = ImportStatus,
                SshUrl = Repository.FullPath,
                HttpUrl = Repository.FullPath,
                DefaultBranch = DefaultBranch,
                VisibilityLevel = Visibility,
            };
        }
    }
}
