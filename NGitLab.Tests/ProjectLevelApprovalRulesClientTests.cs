using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NuGet.Versioning;
using NUnit.Framework;

namespace NGitLab.Tests;

public class ProjectLevelApprovalRulesClientTests
{
    // Starting at version 15.2.0, project-level approval rules require a Premium subscription
    private readonly VersionRange SupportedVersionRange = VersionRange.Parse("[,15.2)");
    private GitLabTestContext context;

    [SetUp]
    public async Task SetUp()
    {
        context = await GitLabTestContext.CreateAsync();
        context.ReportTestAsInconclusiveIfGitLabVersionOutOfRange(SupportedVersionRange);
    }

    [TearDown]
    public void TearDown()
    {
        context?.Dispose();
    }

    [Test]
    [NGitLabRetry]
    public void CreateApprovalRule()
    {
        var project = context.CreateProject();
        var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

        var approvalRuleName = "TestApprovalRule";
        var approvalRuleApprovalsRequired = 1;

        var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalRuleApprovalsRequired));

        Assert.That(approvalRule, Is.Not.Null);
        Assert.That(approvalRule.Name, Is.EqualTo(approvalRuleName));
        Assert.That(approvalRule.ApprovalsRequired, Is.EqualTo(approvalRuleApprovalsRequired));
    }

    [Test]
    [NGitLabRetry]
    public void DeleteApprovalRule()
    {
        var project = context.CreateProject();
        var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

        var approvalRuleName = "TestApprovalRuleDelete";
        var approvalsRequired = 1;
        var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalsRequired));

        projectLevelApprovalRulesClient.DeleteProjectLevelRule(approvalRule.RuleId);

        Assert.That(projectLevelApprovalRulesClient.GetProjectLevelApprovalRules(), Is.Empty);
    }

    [Test]
    [NGitLabRetry]
    public void UpdateApprovalRule()
    {
        var project = context.CreateProject();
        var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

        var approvalRuleName = "TestApprovalRuleUpdate";
        var approvalRuleApprovalsRequired = 1;

        var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalRuleApprovalsRequired));

        var firstApprovalRule = projectLevelApprovalRulesClient.GetProjectLevelApprovalRules()[0];

        approvalRuleApprovalsRequired = 3;
        approvalRule =
            projectLevelApprovalRulesClient.UpdateProjectLevelApprovalRule(
                firstApprovalRule.RuleId,
                new ApprovalRuleUpdate
                {
                    ApprovalsRequired = approvalRuleApprovalsRequired,
                });

        Assert.That(approvalRule, Is.Not.Null);
        Assert.That(approvalRule.Name, Is.EqualTo(approvalRuleName));
        Assert.That(approvalRule.ApprovalsRequired, Is.EqualTo(approvalRuleApprovalsRequired));
    }

    [Test]
    [NGitLabRetry]
    public void GetApprovalRules()
    {
        var project = context.CreateProject();
        var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

        var firstApprovalRuleName = "TestApprovalRule";
        var firstApprovalRuleApprovalsRequired = 1;

        projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, firstApprovalRuleName, firstApprovalRuleApprovalsRequired));

        var approvalRules = projectLevelApprovalRulesClient.GetProjectLevelApprovalRules();

        Assert.That(approvalRules, Has.Count.EqualTo(1));
        Assert.That(approvalRules[0].Name, Is.EqualTo(firstApprovalRuleName));
        Assert.That(approvalRules[0].ApprovalsRequired, Is.EqualTo(firstApprovalRuleApprovalsRequired));
    }

    private static ApprovalRuleCreate CreateTestApprovalRuleCreate(Project project, string approvalRuleName, int approvalsRequired)
    {
        return new ApprovalRuleCreate
        {
            Name = approvalRuleName,
            ApprovalsRequired = approvalsRequired,
            RuleType = "regular",
        };
    }
}
