using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectLevelApprovalRulesClient : ClientBase, IProjectLevelApprovalRulesClient
{
    private readonly long _projectId;

    public ProjectLevelApprovalRulesClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public List<ApprovalRule> GetProjectLevelApprovalRules()
    {
        throw new NotImplementedException();
    }

    public ApprovalRule UpdateProjectLevelApprovalRule(long approvalRuleIdToUpdate, ApprovalRuleUpdate approvalRuleUpdate)
    {
        throw new NotImplementedException();
    }

    public ApprovalRule CreateProjectLevelRule(ApprovalRuleCreate approvalRuleCreate)
    {
        throw new NotImplementedException();
    }

    public void DeleteProjectLevelRule(long approvalRuleIdToDelete)
    {
        throw new NotImplementedException();
    }
}
