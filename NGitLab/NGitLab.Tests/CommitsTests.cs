using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class CommitsTests
    {
        private static bool _ciEnabled;
        private ICommitClient _commits;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _commits = Initialize.GitLabClient.GetCommits(Initialize.UnitTestProject.Id);
            EnableCiOnTestProject();
        }

        public static void EnableCiOnTestProject()
        {
            var projectId = Initialize.UnitTestProject.Id;

            if (!_ciEnabled)
            {
                var defaultRunner = RunnerTests.GetDefaultRunner();
                if (!defaultRunner.Online)
                {
                    Utils.FailInCiEnvironment($"Cannot run the test since the test runner {defaultRunner.Id} is offline.");
                }

                const string yml =
                    @"
variables:
  TEST: HELLO WORLD

build:  
  tags:
    - win10
  script: 
  - echo OK
  artifacts:
    untracked: true
    expire_in: 1h
    # add an artifact path to force uploading even if there are no binaries to upload
    paths:
      - .gitlab-ci.yml

manual:
  tags:
    - win10
  when: manual
  script: 
  - echo manual OK
";

                var repository = Initialize.GitLabClient.GetRepository(projectId);
                repository.Files.Create(new FileUpsert
                {
                    Path = ".gitlab-ci.yml",
                    Branch = "master",
                    CommitMessage = "Enable ci",
                    RawContent = yml
                });

                Initialize.GitLabClient.Runners.EnableRunner(projectId, new RunnerId(defaultRunner.Id));
                _ciEnabled = true;
            }
        }

        [Test]
        public void Test_can_get_commit()
        {
            var commit = _commits.GetCommit("master");
            Assert.IsNotNull(commit.Message);
            Assert.IsNotNull(commit.ShortId);
        }
    }
}
