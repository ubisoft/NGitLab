using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectLevelApprovalRulesClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task CreateApprovalRule()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

            var approvalRuleName = "TestApprovalRule";
            var approvalRuleApprovalsRequired = 1;

            var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalRuleApprovalsRequired));

            Assert.NotNull(approvalRule);
            Assert.AreEqual(approvalRuleName, approvalRule.Name);
            Assert.AreEqual(approvalRuleApprovalsRequired, approvalRule.ApprovalsRequired);
        }

        [Test]
        [NGitLabRetry]
        public async Task DeleteApprovalRule()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

            var approvalRuleName = "TestApprovalRuleDelete";
            var approvalsRequired = 1;
            var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalsRequired));

            projectLevelApprovalRulesClient.DeleteProjectLevelRule(approvalRule.RuleId);

            Assert.AreEqual(0, projectLevelApprovalRulesClient.GetProjectLevelApprovalRules().Count);
        }

        [Test]
        [NGitLabRetry]
        public async Task UpdateApprovalRule()
        {
            using var context = await GitLabTestContext.CreateAsync();
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

            Assert.NotNull(approvalRule);
            Assert.AreEqual(approvalRuleName, approvalRule.Name);
            Assert.AreEqual(approvalRuleApprovalsRequired, approvalRule.ApprovalsRequired);
        }

        [Test]
        [NGitLabRetry]
        public async Task GetApprovalRules()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

            var firstApprovalRuleName = "TestApprovalRule";
            var firstApprovalRuleApprovalsRequired = 1;

            projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, firstApprovalRuleName, firstApprovalRuleApprovalsRequired));

            var approvalRules = projectLevelApprovalRulesClient.GetProjectLevelApprovalRules();

            Assert.AreEqual(1, approvalRules.Count);
            Assert.AreEqual(firstApprovalRuleName, approvalRules[0].Name);
            Assert.AreEqual(firstApprovalRuleApprovalsRequired, approvalRules[0].ApprovalsRequired);
        }

        private static ApprovalRuleCreate CreateTestApprovalRuleCreate(Project project, string approvalRuleName, int approvalsRequired)
        {
            return new ApprovalRuleCreate
            {
                Id = project.Id,
                Name = approvalRuleName,
                ApprovalsRequired = approvalsRequired,
                RuleType = "regular",
            };
        }
    }
}
