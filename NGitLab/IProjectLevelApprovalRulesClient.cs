using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IProjectLevelApprovalRulesClient
{
    /// <summary>
    /// Get a list of a Project level approval rules.
    /// </summary>
    /// <returns>A list of approval rules for a project.</returns>
    List<ApprovalRule> GetProjectLevelApprovalRules();

    /// <summary>
    /// Update the approval rule.
    /// </summary>
    /// <param name="approvalRuleIdToUpdate">The Id of the approval rule to update.</param>
    /// <param name="approvalRuleUpdate">New values of the approval rule.</param>
    /// <returns>The approval rule updated for a project.</returns>
    ApprovalRule UpdateProjectLevelApprovalRule(long approvalRuleIdToUpdate, ApprovalRuleUpdate approvalRuleUpdate);

    /// <summary>
    /// Create an approval rule for a project.
    /// </summary>
    /// <param name="approvalRuleCreate">The approval rule to add to a project.</param>
    /// <returns>The approval rule created for a project.</returns>
    ApprovalRule CreateProjectLevelRule(ApprovalRuleCreate approvalRuleCreate);

    /// <summary>
    /// Delete an approval rule for a project.
    /// </summary>
    /// <param name="approvalRuleIdToDelete">The Id of the approval rule to delete for a project.</param>
    void DeleteProjectLevelRule(long approvalRuleIdToDelete);
}
