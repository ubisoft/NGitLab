using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectLevelApprovalRulesClientTests
    {
        private IProjectLevelApprovalRulesClient _projectLevelApprovalRulesClient;

        [SetUp]
        public void Setup()
        {
            _projectLevelApprovalRulesClient = Initialize.GitLabClient.GetProjectLevelApprovalRulesClient(Initialize.UnitTestProject.Id);
        }

        [TearDown]
        public void Cleanup()
        {
            foreach (var approvalRule in _projectLevelApprovalRulesClient.GetProjectLevelApprovalRules())
            {
                _projectLevelApprovalRulesClient.DeleteProjectLevelRule(approvalRule.RuleId);
            }
        }

        [Test]
        public void CreateApprovalRule()
        {
            var approvalRuleName = "TestApprovalRule";
            var approvalRuleApprovalsRequired = 1;

            var approvalRule =
                _projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(approvalRuleName, approvalRuleApprovalsRequired));

            Assert.NotNull(approvalRule);
            Assert.AreEqual(approvalRuleName, approvalRule.Name);
            Assert.AreEqual(approvalRuleApprovalsRequired, approvalRule.ApprovalsRequired);
        }

        [Test]
        public void DeleteApprovalRule()
        {
            string approvalRuleName = "TestApprovalRuleDelete";
            int approvalsRequired = 1;
            var approvalRule =
                _projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(approvalRuleName, approvalsRequired));

            _projectLevelApprovalRulesClient.DeleteProjectLevelRule(approvalRule.RuleId);

            Assert.AreEqual(0, _projectLevelApprovalRulesClient.GetProjectLevelApprovalRules().Count);
        }

        [Test]
        public void UpdateApprovalRule()
        {
            var approvalRuleName = "TestApprovalRuleUpdate";
            var approvalRuleApprovalsRequired = 1;

            var approvalRule =
                _projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(approvalRuleName, approvalRuleApprovalsRequired));

            var firstApprovalRule = _projectLevelApprovalRulesClient.GetProjectLevelApprovalRules().First();

            approvalRuleApprovalsRequired = 3;
            approvalRule =
                _projectLevelApprovalRulesClient.UpdateProjectLevelApprovalRule(
                    firstApprovalRule.RuleId,
                    new ApprovalRuleUpdate
                    {
                        ApprovalsRequired = approvalRuleApprovalsRequired
                    });

            Assert.NotNull(approvalRule);
            Assert.AreEqual(approvalRuleName, approvalRule.Name);
            Assert.AreEqual(approvalRuleApprovalsRequired, approvalRule.ApprovalsRequired);
        }

        [Test]
        public void GetApprovalRules()
        {
            var firstApprovalRuleName = "TestApprovalRule";
            var firstApprovalRuleApprovalsRequired = 1;

            string secondApprovalRuleName = "TestApprovalRule2";
            int secondApprovalRuleApprovalsRequired = 2;

            _projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(firstApprovalRuleName, firstApprovalRuleApprovalsRequired));
            _projectLevelApprovalRulesClient.CreateProjectLevelRule(CreateTestApprovalRuleCreate(secondApprovalRuleName, secondApprovalRuleApprovalsRequired));

            var approvalRules = _projectLevelApprovalRulesClient.GetProjectLevelApprovalRules();

            Assert.AreEqual(2, approvalRules.Count);
            Assert.AreEqual(firstApprovalRuleName, approvalRules.First().Name);
            Assert.AreEqual(firstApprovalRuleApprovalsRequired, approvalRules.First().ApprovalsRequired);
            Assert.AreEqual(secondApprovalRuleName, approvalRules.Last().Name);
            Assert.AreEqual(secondApprovalRuleApprovalsRequired, approvalRules.Last().ApprovalsRequired);
        }

        private ApprovalRuleCreate CreateTestApprovalRuleCreate(string approvalRuleName, int approvalsRequired)
        {
            return new ApprovalRuleCreate
            {
                Id = Initialize.UnitTestProject.Id,
                Name = approvalRuleName,
                ApprovalsRequired = approvalsRequired,
                RuleType = "regular"
            };
        }
    }
}
