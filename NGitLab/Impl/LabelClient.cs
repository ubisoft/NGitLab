using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class LabelClient : ILabelClient
    {
        public const string ProjectLabelUrl = "/projects/{0}/labels";
        public const string GroupLabelUrl = "/groups/{0}/labels";

        private readonly API _api;

        public LabelClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Label> ForProject(int projectId)
        {
            return _api.Get().GetAll<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
        }

        public IEnumerable<Label> ForGroup(int groupId)
        {
            return _api.Get().GetAll<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
        }

        public Label GetProjectLabel(int projectId, string name)
        {
            return ForProject(projectId).FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Label GetLabel(int projectId, string name)
        {
            return GetProjectLabel(projectId, name);
        }

        public Label GetGroupLabel(int groupId, string name)
        {
            return ForGroup(groupId).FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
        }

        public Label CreateProjectLabel(int projectId, ProjectLabelCreate label)
        {
            return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Label Create(LabelCreate label)
        {
            return CreateProjectLabel(label.Id, new ProjectLabelCreate
            {
                Name = label.Name,
                Color = label.Color,
                Description = label.Description,
            });
        }

        public Label CreateGroupLabel(int groupId, GroupLabelCreate label)
        {
            return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Label CreateGroupLabel(LabelCreate label)
        {
            return CreateGroupLabel(label.Id, new GroupLabelCreate
            {
                Name = label.Name,
                Color = label.Color,
                Description = label.Description,
            });
        }

        public Label EditProjectLabel(int projectId, ProjectLabelEdit label)
        {
            return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Label Edit(LabelEdit label)
        {
            return EditProjectLabel(label.Id, new ProjectLabelEdit
            {
                Name = label.Name,
                NewName = label.NewName,
                Color = label.Color,
                Description = label.Description,
            });
        }

        public Label EditGroupLabel(int groupId, GroupLabelEdit label)
        {
            return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Label EditGroupLabel(LabelEdit label)
        {
            return EditGroupLabel(label.Id, new GroupLabelEdit
            {
                Name = label.Name,
                NewName = label.NewName,
                Color = label.Color,
                Description = label.Description,
            });
        }

        public Label DeleteProjectLabel(int projectId, ProjectLabelDelete label)
        {
            return _api.Delete().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Label Delete(LabelDelete label)
        {
            return DeleteProjectLabel(label.Id, new ProjectLabelDelete
            {
                Id = label.Id,
                Name = label.Name,
            });
        }
    }
}
