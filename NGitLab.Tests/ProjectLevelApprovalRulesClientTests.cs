using System.Threading.Tasks;
using Meziantou.Framework.Versioning;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectLevelApprovalRulesClientTests
    {
        // Starting at version 15.2.0, project-level approval rules require a Premium subscription
        private readonly SemanticVersion MaxVersion = new(15, 1, 99);
        private GitLabTestContext context;

        [SetUp]
        public async Task SetUp()
        {
            context = await GitLabTestContext.CreateAsync();
            context.ReportTestAsInconclusiveIfVersionOutOfRange(maxVersion: MaxVersion);
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

            Assert.NotNull(approvalRule);
            Assert.AreEqual(approvalRuleName, approvalRule.Name);
            Assert.AreEqual(approvalRuleApprovalsRequired, approvalRule.ApprovalsRequired);
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

            Assert.AreEqual(0, projectLevelApprovalRulesClient.GetProjectLevelApprovalRules().Count);
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

            Assert.NotNull(approvalRule);
            Assert.AreEqual(approvalRuleName, approvalRule.Name);
            Assert.AreEqual(approvalRuleApprovalsRequired, approvalRule.ApprovalsRequired);
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

            Assert.AreEqual(1, approvalRules.Count);
            Assert.AreEqual(firstApprovalRuleName, approvalRules[0].Name);
            Assert.AreEqual(firstApprovalRuleApprovalsRequired, approvalRules[0].ApprovalsRequired);
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
}
