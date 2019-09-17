using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class GroupVariableClient : ClientBase, IGroupVariableClient
    {
        private readonly int _groupId;

        public GroupVariableClient(ClientContext context, int groupId)
            : base(context)
        {
            _groupId = groupId;
        }

        public Variable this[string key] => throw new NotImplementedException();

        public IEnumerable<Variable> All => throw new NotImplementedException();

        public Variable Create(VariableCreate model)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public Variable Update(string key, VariableUpdate model)
        {
            throw new NotImplementedException();
        }
    }
}
