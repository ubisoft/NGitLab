using System.Collections.Generic;
using NGitLab.Mock.Clients;
using NGitLab.Models;

namespace NGitLab.Mock
{
    internal sealed class MilestoneClient : ClientBase, IMilestoneClient
    {
        private readonly int _projectId;

        public MilestoneClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public Milestone this[int id] => throw new System.NotImplementedException();

        public IEnumerable<Milestone> All => throw new System.NotImplementedException();

        public Milestone Activate(int milestoneId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Milestone> AllInState(MilestoneState state)
        {
            throw new System.NotImplementedException();
        }

        public Milestone Close(int milestoneId)
        {
            throw new System.NotImplementedException();
        }

        public Milestone Create(MilestoneCreate milestone)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int milestoneId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Milestone> Get(MilestoneQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Milestone Update(int milestoneId, MilestoneUpdate milestone)
        {
            throw new System.NotImplementedException();
        }
    }
}
