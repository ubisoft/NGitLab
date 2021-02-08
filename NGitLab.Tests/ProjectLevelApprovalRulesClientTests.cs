using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectLevelApprovalRulesClientTests
    {
        [Test]
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
        public async Task DeleteApprovalRule()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

            string approvalRuleName = "TestApprovalRuleDelete";
            int approvalsRequired = 1;
            var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalsRequired));

            projectLevelApprovalRulesClient.DeleteProjectLevelRule(approvalRule.RuleId);

            Assert.AreEqual(0, projectLevelApprovalRulesClient.GetProjectLevelApprovalRules().Count);
        }

        [Test]
        public async Task UpdateApprovalRule()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectLevelApprovalRulesClient = context.Client.GetProjectLevelApprovalRulesClient(project.Id);

            var approvalRuleName = "TestApprovalRuleUpdate";
            var approvalRuleApprovalsRequired = 1;

            var approvalRule = projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(project, approvalRuleName, approvalRuleApprovalsRequired));

            var firstApprovalRule = projectLevelApprovalRulesClient.GetProjectLevelApprovalRules().First();

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
            Assert.AreEqual(firstApprovalRuleName, approvalRules.First().Name);
            Assert.AreEqual(firstApprovalRuleApprovalsRequired, approvalRules.First().ApprovalsRequired);
        }

        private ApprovalRuleCreate CreateTestApprovalRuleCreate(Project project, string approvalRuleName, int approvalsRequired)
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
