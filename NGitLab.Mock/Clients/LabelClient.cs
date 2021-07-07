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
            var project = FindProject(label.Id) ?? throw new InvalidOperationException($"Cannot find project with ID {label.Id}");
            return project.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
        }

        public Models.Label CreateGroupLabel(LabelCreate label)
        {
            var group = FindGroup(label.Id) ?? throw new InvalidOperationException($"Cannot find group with ID {label.Id}");
            return group.Labels.Add(label.Name, label.Color, label.Description).ToClientLabel();
        }

        public Models.Label Delete(LabelDelete label)
        {
            var project = FindProject(label.Id) ?? throw new InvalidOperationException($"Cannot find project with ID {label.Id}");
            var l = FindLabel(project.Labels, label.Name) ?? throw new InvalidOperationException($"Cannot find label '{label.Name}'");
            project.Labels.Remove(l);
            return l.ToClientLabel();
        }

        public Models.Label Edit(LabelEdit label)
        {
            var project = FindProject(label.Id) ?? throw new InvalidOperationException($"Cannot find project with ID {label.Id}");
            var l = FindLabel(project.Labels, label.Name) ?? throw new InvalidOperationException($"Cannot find label '{label.Name}'");

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

        public Models.Label EditGroupLabel(LabelEdit label)
        {
            var group = FindGroup(label.Id) ?? throw new InvalidOperationException($"Cannot find group with ID {label.Id}");
            var l = FindLabel(group.Labels, label.Name) ?? throw new InvalidOperationException($"Cannot find label '{label.Name}'");

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

        public IEnumerable<Models.Label> ForGroup(int groupId)
        {
            var group = FindGroup(groupId) ?? throw new InvalidOperationException($"Cannot find group with ID {groupId}");
            return group.Labels.Select(x => x.ToClientLabel());
        }

        public IEnumerable<Models.Label> ForProject(int projectId)
        {
            var project = FindProject(projectId) ?? throw new InvalidOperationException($"Cannot find project with ID {projectId}");
            return project.Labels.Select(x => x.ToClientLabel());
        }

        public Models.Label GetGroupLabel(int groupId, string name)
        {
            var group = FindGroup(groupId) ?? throw new InvalidOperationException($"Cannot find group with ID {groupId}");
            return FindLabel(group.Labels, name)?.ToClientLabel();
        }

        public Models.Label GetLabel(int projectId, string name)
        {
            var project = FindProject(projectId) ?? throw new InvalidOperationException($"Cannot find project with ID {projectId}");
            return FindLabel(project.Labels, name)?.ToClientLabel();
        }

        private Project FindProject(int id)
        {
            return Server.AllProjects.FirstOrDefault(x => x.Id == id);
        }

        private Group FindGroup(int id)
        {
            return Server.AllGroups.FirstOrDefault(x => x.Id == id);
        }

        private Label FindLabel(LabelsCollection collection, string name)
        {
            return collection.FirstOrDefault(x => x.Name.Equals(name, StringComparison.Ordinal));
        }
    }
}
