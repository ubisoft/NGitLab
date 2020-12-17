using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ProjectLevelApprovalRulesClient : ClientBase, IProjectLevelApprovalRulesClient
    {
        private readonly int _projectId;

        public ProjectLevelApprovalRulesClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public List<ApprovalRule> GetProjectLevelApprovalRules()
        {
            throw new NotImplementedException();
        }

        public ApprovalRule UpdateProjectLevelApprovalRule(int approvalRuleIdToUpdate, ApprovalRuleUpdate approvalRuleUpdate)
        {
            throw new NotImplementedException();
        }

        public ApprovalRule CreateProjectLevelRule(ApprovalRuleCreate approvalRuleCreate)
        {
            throw new NotImplementedException();
        }

        public void DeleteProjectLevelRule(int approvalRuleIdToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
