using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Group : GitLabObject
    {
        private string _path;

        public Group()
            : this(Guid.NewGuid().ToString("N"))
        {
        }

        public Group(string name)
        {
            Groups = new GroupCollection(this);
            Projects = new ProjectCollection(this);
            Permissions = new PermissionCollection(this);
            Name = name;
        }

        public Group(User user)
            : this(user.Name)
        {
            IsUserNamespace = true;
            Path = user.UserName;
            Permissions.Add(new Permission(user, AccessLevel.Owner));
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public VisibilityLevel Visibility { get; set; }
        public bool IsUserNamespace { get; }

        public new Group Parent => base.Parent as Group;

        public GroupCollection Groups { get; }
        public ProjectCollection Projects { get; }
        public PermissionCollection Permissions { get; }

        public string Path
        {
            get
            {
                if (_path == null )
                    _path = Slug.Create(Name);

                return _path;
            }

            private set
            {
                if (!IsUserNamespace)
                    return;

                _path = value;
            }
        }

        public string PathWithNameSpace => Parent == null ? Path : (Parent.PathWithNameSpace + "/" + Path);

        public string FullName => Parent == null ? Name : (Parent.FullName + "/" + Name);

        public IEnumerable<Group> DescendantGroups
        {
            get
            {
                foreach (var group in Groups)
                {
                    yield return group;
                    foreach (var subGroup in group.DescendantGroups)
                    {
                        yield return subGroup;
                    }
                }
            }
        }

        public IEnumerable<Project> AllProjects => Projects.Concat(DescendantGroups.SelectMany(group => group.Projects));

        public EffectivePermissions GetEffectivePermissions()
        {
            var result = new Dictionary<User, AccessLevel>();

            if (Parent != null)
            {
                foreach (var effectivePermission in Parent.GetEffectivePermissions().Permissions)
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

        public bool CanUserViewGroup(User user)
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

        public bool CanUserDeleteGroup(User user)
        {
            if (user == null)
                return false;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value == AccessLevel.Owner;
        }

        public bool CanUserAddGroup(User user)
        {
            if (user == null)
                return false;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value >= AccessLevel.Developer;
        }

        public bool CanUserAddProject(User user)
        {
            if (user == null)
                return false;

            var accessLevel = GetEffectivePermissions().GetAccessLevel(user);
            return accessLevel.HasValue && accessLevel.Value >= AccessLevel.Developer;
        }

        public Models.Group ToClientGroup()
        {
            return new Models.Group
            {
                Id = Id,
                Name = Name,
                Visibility = Visibility,
                ParentId = Parent?.Id,
                Projects = Projects.Select(p => p.ToClientProject()).ToArray(),
                FullName = FullName,
                FullPath = PathWithNameSpace,
                Path = Path,
                Description = Description,
            };
        }
    }
}
