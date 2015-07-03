using NGitLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Label GetLabel(int projectId, string Name)
        {
            return ForProject(projectId).FirstOrDefault<Label>((x) => x.Name.Equals(Name));
        }

        public Label NewLabel(LabelCreate label)
        {
            return _api.Post().With(label).To<Label>(string.Format(ProjectLabelUrl, label.Id));
        }

        public Label EditLabel(LabelEdit label)
        {
            return _api.Put().With(label).To<Label>(string.Format(ProjectLabelUrl, label.Id));
        }

        public bool DeleteLabel(LabelDelete label, out string result)
        {
            result = _api.Delete().With(label).To<string>(string.Format(ProjectLabelUrl, label.Id));
            return API.OK.Equals(result.Trim());
        }
    }
}
