using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Impl;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class HttpRequestorTests
    {
        [OneTimeSetUp]
        public async Task SetUpOnceAsync()
        {
            // Make sure at least 1 project exists
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            Assert.NotNull(project);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_calls_are_retried_when_they_fail_in_gitlab()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var requestOptions = new MockRequestOptions(1, TimeSpan.FromMilliseconds(10), isIncremental: false);
            var httpRequestor = new HttpRequestor(context.DockerContainer.GitLabUrl.ToString(), context.DockerContainer.Credentials.UserToken, MethodType.Get, requestOptions);

            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));
            Assert.That(requestOptions.ShouldRetryCalled, Is.True);
            Assert.That(requestOptions.HandledRequests.Count, Is.EqualTo(2));
            Assert.IsNull(requestOptions.HttpRequestSudoHeader);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_the_timeout_can_be_overridden_in_the_request_options()
        {
            using var context = await GitLabTestContext.CreateAsync();

            var requestOptions = new MockRequestOptions { HttpClientTimeout = TimeSpan.FromMinutes(2) };

            var httpRequestor = new HttpRequestor(context.DockerContainer.GitLabUrl.ToString(), context.DockerContainer.Credentials.UserToken, MethodType.Get, requestOptions);
            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));

            Assert.AreEqual(TimeSpan.FromMinutes(2).TotalMilliseconds, requestOptions.HandledRequests.Single().Timeout);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_request_options_sudo_transferred_to_request_header()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var requestOptions = new MockRequestOptions(1, TimeSpan.FromMilliseconds(10), isIncremental: false) { Sudo = "UserToImpersonate" };
            var httpRequestor = new HttpRequestor(context.DockerContainer.GitLabUrl.ToString(), context.DockerContainer.Credentials.UserToken, MethodType.Get, requestOptions);

            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));
            Assert.AreEqual("UserToImpersonate", requestOptions.HttpRequestSudoHeader);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_impersonation_via_sudo_and_username()
        {
            // Arrange
            using var context = await GitLabTestContext.CreateAsync();

            var commonUserClient = context.Client;
            var commonUserSession = commonUserClient.Users.Current;

            var requestOptions = new RequestOptions(1, TimeSpan.FromMilliseconds(10), isIncremental: false)
            {
                Sudo = commonUserSession.Username,
            };

            var adminClient = context.AdminClient as GitLabClient;
            adminClient.Options = requestOptions;

            var project = commonUserClient.Projects.Accessible.First();

            // Act
            var issue = adminClient.Issues.Create(new Models.IssueCreate
            {
                Id = project.Id,
                Title = $"An issue created on behalf of user '{commonUserSession.Username}'",
            });

            // Assert
            Assert.AreEqual(commonUserSession.Username, issue.Author.Username);
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_impersonation_via_sudo_and_user_id()
        {
            // Arrange
            using var context = await GitLabTestContext.CreateAsync();

            var commonUserClient = context.Client;
            var commonUserSession = commonUserClient.Users.Current;
            var commonUserId = commonUserSession.Id.ToString(CultureInfo.InvariantCulture);

            var requestOptions = new RequestOptions(1, TimeSpan.FromMilliseconds(10), isIncremental: false)
            {
                Sudo = commonUserId,
            };

            var adminClient = context.AdminClient as GitLabClient;
            adminClient.Options = requestOptions;

            var project = commonUserClient.Projects.Accessible.First();

            // Act
            var issue = adminClient.Issues.Create(new Models.IssueCreate
            {
                Id = project.Id,
                Title = $"An issue created on behalf of user '{commonUserId}'",
            });

            // Assert
            Assert.AreEqual(commonUserSession.Id, issue.Author.Id);
        }

        private sealed class MockRequestOptions : RequestOptions
        {
            public string HttpRequestSudoHeader { get; set; }

            public bool ShouldRetryCalled { get; set; }

            public HashSet<WebRequest> HandledRequests { get; } = new HashSet<WebRequest>();

            public MockRequestOptions()
                : base(retryCount: 0, retryInterval: TimeSpan.Zero)
            {
            }

            public MockRequestOptions(int retryCount, TimeSpan retryInterval, bool isIncremental = true)
                : base(retryCount, retryInterval, isIncremental)
            {
            }

            public override bool ShouldRetry(Exception ex, int retryNumber)
            {
                ShouldRetryCalled = true;

                return base.ShouldRetry(ex, retryNumber);
            }

            public override WebResponse GetResponse(HttpWebRequest request)
            {
                HttpRequestSudoHeader = request.Headers["Sudo"];
                HandledRequests.Add(request);
                throw new GitLabException { StatusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}
