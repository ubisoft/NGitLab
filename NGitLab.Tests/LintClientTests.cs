using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class LintClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task LintValidCIYaml()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var lintClient = context.Client.Lint;

            var yaml = @"
variables:
  CI_DEBUG_TRACE: ""true""
build:
  script:
    - echo test
";

            var result = await context.Client.Lint.LintGitLabCIYamlAsync(project.Id.ToString(), yaml, new(), CancellationToken.None);

            Assert.True(result.Valid);
            Assert.False(result.Errors.Any());
            Assert.False(result.Warnings.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task LintInvalidCIYaml()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var lintClient = context.Client.Lint;

            var yaml = @"
variables:
  CI_DEBUG_TRACE: ""true""
build:
  script:
    - echo test
wrong_key
";

            var result = await context.Client.Lint.LintGitLabCIYamlAsync(project.Id.ToString(), yaml, new(), CancellationToken.None);

            Assert.False(result.Valid);
            Assert.True(result.Errors.Any());
            Assert.False(result.Warnings.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task LintValidCIProjectYaml()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var lintClient = context.Client.Lint;

            var yaml = @"
variables:
  CI_DEBUG_TRACE: ""true""
build:
  script:
    - echo test
";
            context.Client.GetRepository(project.Id).Files.Create(new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "test",
                Path = ".gitlab-ci.yml",
                Content = yaml,
            });

            var result = await context.Client.Lint.LintProjectGitLabCIYamlAsync(project.Id.ToString(), new(), CancellationToken.None);

            Assert.True(result.Valid);
            Assert.False(result.Errors.Any());
            Assert.False(result.Warnings.Any());
        }

        [Test]
        [NGitLabRetry]
        public async Task LintInvalidProjectCIYaml()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var lintClient = context.Client.Lint;

            var yaml = @"
variables:
  CI_DEBUG_TRACE: ""true""
build:
  script:
    - echo test
wrong_key
";
            context.Client.GetRepository(project.Id).Files.Create(new FileUpsert
            {
                Branch = project.DefaultBranch,
                CommitMessage = "test",
                Path = ".gitlab-ci.yml",
                Content = yaml,
            });

            var result = await context.Client.Lint.LintProjectGitLabCIYamlAsync(project.Id.ToString(), new(), CancellationToken.None);

            Assert.False(result.Valid);
            Assert.True(result.Errors.Any());
            Assert.False(result.Warnings.Any());
        }
    }
}
