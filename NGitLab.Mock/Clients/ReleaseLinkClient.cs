using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ReleaseLinkClient : ClientBase, IReleaseLinkClient
    {
        private readonly int _projectId;
        private readonly string _tagName;

        public ReleaseLinkClient(ClientContext context, int projectId, string tagName)
            : base(context)
        {
            _projectId = projectId;
            _tagName = tagName;
        }

        public ReleaseLink this[int id] => throw new NotImplementedException();

        public IEnumerable<ReleaseLink> All => throw new NotImplementedException();

        public ReleaseLink Create(ReleaseLinkCreate data)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ReleaseLink Update(int id, ReleaseLinkUpdate data)
        {
            throw new NotImplementedException();
        }
    }
}
