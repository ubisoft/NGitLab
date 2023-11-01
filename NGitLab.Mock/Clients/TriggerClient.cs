using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class TriggerClient : ClientBase, ITriggerClient
    {
        private readonly int _projectId;

        public TriggerClient(ClientContext context, ProjectId projectId)
            : base(context)
        {
            _projectId = Server.AllProjects.FindProject(projectId.ValueAsUriParameter()).Id;
        }

        public Trigger this[int id] => throw new NotImplementedException();

        public IEnumerable<Trigger> All => throw new NotImplementedException();

        public Trigger Create(string description)
        {
            throw new NotImplementedException();
        }
    }
}
