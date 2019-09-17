using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class IssueClient : ClientBase, IIssueClient
    {
        public IssueClient(ClientContext context)
            : base(context)
        {
        }

        public IEnumerable<Issue> Owned => throw new NotImplementedException();

        public Issue Create(IssueCreate issueCreate)
        {
            throw new NotImplementedException();
        }

        public Issue Edit(IssueEdit issueEdit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Issue> ForProject(int projectId)
        {
            throw new NotImplementedException();
        }

        public Issue Get(int projectId, int issueId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Issue> Get(IssueQuery query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Issue> Get(int projectId, IssueQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
