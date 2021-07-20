using System.Collections.Generic;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MilestoneClient : IMilestoneClient
    {
        private readonly API _api;
        private readonly string _milestonePath;

        public MilestoneClient(API api, int projectId)
        {
            _api = api;
            _milestonePath = $"{Project.Url}/{projectId.ToStringInvariant()}/milestones";
        }

        public IEnumerable<Milestone> All => Get(new MilestoneQuery());

        public IEnumerable<Milestone> AllInState(MilestoneState state) => Get(new MilestoneQuery { State = state });

        public IEnumerable<Milestone> Get(MilestoneQuery query)
        {
            var url = _milestonePath;

            url = Utils.AddParameter(url, "state", query.State);
            url = Utils.AddParameter(url, "search", query.Search);

            return _api.Get().GetAll<Milestone>(url);
        }

        public Milestone this[int id] => _api.Get().To<Milestone>($"{_milestonePath}/{id.ToStringInvariant()}");

        public Milestone Create(MilestoneCreate milestone) => _api
            .Post().With(milestone)
            .To<Milestone>(_milestonePath);

        public Milestone Update(int milestoneId, MilestoneUpdate milestone) => _api
            .Put().With(milestone)
            .To<Milestone>($"{_milestonePath}/{milestoneId.ToStringInvariant()}");

        public Milestone Close(int milestoneId) => _api
            .Put().With(new MilestoneUpdateState { NewState = nameof(MilestoneStateEvent.close) })
            .To<Milestone>($"{_milestonePath}/{milestoneId.ToStringInvariant()}");

        public Milestone Activate(int milestoneId) => _api
            .Put().With(new MilestoneUpdateState { NewState = nameof(MilestoneStateEvent.activate) })
            .To<Milestone>($"{_milestonePath}/{milestoneId.ToStringInvariant()}");

        public void Delete(int milestoneId) => _api
            .Delete()
            .Execute($"{_milestonePath}/{milestoneId.ToStringInvariant()}");
    }
}
