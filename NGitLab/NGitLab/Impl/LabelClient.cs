using NGitLab.Models;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Impl
{
    public class LabelClient : ILabelClient
    {
        public const string ProjectLabelUrl = "/projects/{0}/labels";

        private readonly API _api;

        public LabelClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Label> ForProject(int projectId)
        {
            return _api.Get().GetAll<Label>(string.Format(ProjectLabelUrl, projectId));
        }

        public Label GetLabel(int projectId, string name)
        {
            return ForProject(projectId).FirstOrDefault((x) => string.Equals(x.Name, name, System.StringComparison.Ordinal));
        }

        public Label Create(LabelCreate label)
        {
            return _api.Post().With(label).To<Label>(string.Format(ProjectLabelUrl, label.Id));
        }

        public Label Edit(LabelEdit label)
        {
            return _api.Put().With(label).To<Label>(string.Format(ProjectLabelUrl, label.Id));
        }

        public Label Delete(LabelDelete label)
        {
            return _api.Delete().With(label).To<Label>(string.Format(ProjectLabelUrl, label.Id));
        }
    }
}
