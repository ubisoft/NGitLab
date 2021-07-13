using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class LabelClient : ClientBase, ILabelClient
    {
        public LabelClient(ClientContext context)
            : base(context)
        {
        }

        public Models.Label Create(LabelCreate label)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(label.Id, ProjectPermission.Edit);
                return project.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
            }
        }

        public Models.Label CreateGroupLabel(LabelCreate label)
        {
            using (Context.BeginOperationScope())
            {
                var group = GetGroup(label.Id, GroupPermission.Edit);
                return group.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
            }
        }

        public Models.Label Delete(LabelDelete label)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(label.Id, ProjectPermission.Edit);
                var l = FindLabel(project.Labels, label.Name) ?? throw new GitLabNotFoundException($"Cannot find label '{label.Name}'");
                project.Labels.Remove(l);
                return l.ToClientLabel();
            }
        }

        public Models.Label Edit(LabelEdit label)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(label.Id, ProjectPermission.Edit);
                var l = FindLabel(project.Labels, label.Name) ?? throw new GitLabNotFoundException($"Cannot find label '{label.Name}'");

                if (!string.IsNullOrEmpty(label.NewName))
                {
                    l.Name = label.NewName;
                }

                if (!string.IsNullOrEmpty(label.Color))
                {
                    l.Color = label.Color;
                }

                if (label.Description != null)
                {
                    l.Description = label.Description;
                }

                return l.ToClientLabel();
            }
        }

        public Models.Label EditGroupLabel(LabelEdit label)
        {
            using (Context.BeginOperationScope())
            {
                var group = GetGroup(label.Id, GroupPermission.Edit);
                var l = FindLabel(group.Labels, label.Name) ?? throw new GitLabNotFoundException($"Cannot find label '{label.Name}'");

                if (!string.IsNullOrEmpty(label.NewName))
                {
                    l.Name = label.NewName;
                }

                if (!string.IsNullOrEmpty(label.Color))
                {
                    l.Color = label.Color;
                }

                if (label.Description != null)
                {
                    l.Description = label.Description;
                }

                return l.ToClientLabel();
            }
        }

        public IEnumerable<Models.Label> ForGroup(int groupId)
        {
            using (Context.BeginOperationScope())
            {
                var group = GetGroup(groupId, GroupPermission.View);
                return group.Labels.Select(x => x.ToClientLabel());
            }
        }

        public IEnumerable<Models.Label> ForProject(int projectId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(projectId, ProjectPermission.View);
                return project.Labels.Select(x => x.ToClientLabel());
            }
        }

        public Models.Label GetGroupLabel(int groupId, string name)
        {
            using (Context.BeginOperationScope())
            {
                var group = GetGroup(groupId, GroupPermission.View);
                return FindLabel(group.Labels, name)?.ToClientLabel();
            }
        }

        public Models.Label GetLabel(int projectId, string name)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(projectId, ProjectPermission.View);
                return FindLabel(project.Labels, name)?.ToClientLabel();
            }
        }

        private static Label FindLabel(LabelsCollection collection, string name)
        {
            return collection.FirstOrDefault(x => x.Name.Equals(name, StringComparison.Ordinal));
        }
    }
}
