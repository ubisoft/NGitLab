using System.Security.Permissions;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class BuildsTests
    {
        private static bool _ciEnabled;
        private IBuildClient _builds;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _builds = Initialize.GitLabClient.GetBuilds(Initialize.UnitTestProject.Id);
            EnableCiOnTestProject();
        }

        public static void EnableCiOnTestProject()
        {
            var projectId = Initialize.UnitTestProject.Id;

            if (!_ciEnabled)
            {
                const string yml =
                    @"
build:
  script: 
  - echo OK
";

                var repository = Initialize.GitLabClient.GetRepository(projectId);
                repository.Files.Create(new FileUpsert
                {
                    Path = ".gitlab-ci.yml",
                    Branch = "master",
                    CommitMessage = "Enable ci",
                    RawContent = yml
                });

                _ciEnabled = true;
            }

            Initialize.GitLabClient.Runners.EnableRunner(projectId, new RunnerId(RunnerTests.GetDefaultRunner().Id));
        }
    }
}