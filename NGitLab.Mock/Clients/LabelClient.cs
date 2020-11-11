using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class LabelClient : ClientBase, ILabelClient
    {
        public LabelClient(ClientContext context)
            : base(context)
        {
        }

        public Label Create(LabelCreate label)
        {
            throw new NotImplementedException();
        }

        public Label CreateGroupLabel(LabelCreate label)
        {
            throw new NotImplementedException();
        }

        public Label Delete(LabelDelete label)
        {
            throw new NotImplementedException();
        }

        public Label Edit(LabelEdit label)
        {
            throw new NotImplementedException();
        }

        public Label EditGroupLabel(LabelEdit label)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Label> ForGroup(int groupId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Label> ForProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public Label GetGroupLabel(int groupId, string name)
        {
            throw new NotImplementedException();
        }

        public Label GetLabel(int projectId, string name)
        {
            throw new NotImplementedException();
        }
    }
}
