using System.Collections.Generic;
using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class EpicClient : IEpicClient
    {
        private const string GroupEpicsUrl = "/groups/{0}/epics";
        private const string SingleEpiceUrl = "/groups/{0}/epics/{1}";
        private const string GroupEpicIssuesUrl = "/groups/{0}/epics/{1}/issues";

        private readonly API _api;

        public EpicClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Epic> Get(int groupId, EpicQuery query)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(string.Format(CultureInfo.InvariantCulture, GroupEpicsUrl, groupId), query);
            return _api.Get().GetAll<Epic>(url);
        }

        public Epic Get(int groupId, int epicId)
        {
            return _api.Get().To<Epic>(string.Format(CultureInfo.InvariantCulture, SingleEpiceUrl, groupId, epicId));
        }

        public GitLabCollectionResponse<Issue> GetIssuesAsync(int groupId, int epicId)
        {
            return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, GroupEpicIssuesUrl, groupId, epicId));
        }

        public Epic Create(int groupId, EpicCreate epic)
        {
            return _api.Post().With(epic).To<Epic>(string.Format(CultureInfo.InvariantCulture, GroupEpicsUrl, groupId));
        }

        public Epic Edit(int groupId, EpicEdit epicEdit)
        {
            return _api.Put().With(epicEdit).To<Epic>(string.Format(CultureInfo.InvariantCulture, SingleEpiceUrl, groupId, epicEdit.EpicId));
        }
    }
}
