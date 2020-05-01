using System;
using System.Collections.Generic;

namespace NGitLab.Mock.Clients
{
    internal sealed class GroupBadgeClient : ClientBase, IGroupBadgeClient
    {
        private readonly int _groupId;

        public GroupBadgeClient(ClientContext context, int groupId)
            : base(context)
        {
            _groupId = groupId;
        }

        public Models.Badge this[int id] => throw new NotImplementedException();

        public IEnumerable<Models.Badge> All => throw new NotImplementedException();

        public Models.Badge Create(Models.BadgeCreate badge)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Models.Badge Update(int id, Models.BadgeUpdate badge)
        {
            throw new NotImplementedException();
        }
    }
}
