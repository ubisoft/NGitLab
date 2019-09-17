using System;
using System.Collections.Generic;
using NGitLab.Models;

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

        public Badge this[int id] => throw new NotImplementedException();

        public IEnumerable<Badge> All => throw new NotImplementedException();

        public Badge Create(BadgeCreate badge)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Badge Update(int id, BadgeUpdate badge)
        {
            throw new NotImplementedException();
        }
    }
}
