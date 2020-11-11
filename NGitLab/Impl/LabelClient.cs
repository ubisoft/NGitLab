using System.Collections.Generic;
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

        public Label GetLabel(int projectId, string name)
        {
            return ForProject(projectId).FirstOrDefault((x) => string.Equals(x.Name, name, System.StringComparison.Ordinal));
        }

        public Label GetGroupLabel(int groupId, string name)
        {
            return ForGroup(groupId).FirstOrDefault((x) => string.Equals(x.Name, name, System.StringComparison.Ordinal));
        }

        public Label Create(LabelCreate label)
        {
            return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, label.Id));
        }

        public Label CreateGroupLabel(LabelCreate label)
        {
            return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, label.Id));
        }

        public Label Edit(LabelEdit label)
        {
            return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, label.Id));
        }

        public Label EditGroupLabel(LabelEdit label)
        {
            return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, label.Id));
        }

        public Label Delete(LabelDelete label)
        {
            return _api.Delete().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, label.Id));
        }
    }
}
