using System;
using System.Collections.Generic;
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
        [Test]
        public async Task Test_calls_are_retried_when_they_fail_in_gitlab()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var requestOptions = new MockRequestOptions(1, TimeSpan.FromMilliseconds(10), isIncremental: false);
            var httpRequestor = new HttpRequestor(context.DockerContainer.GitLabUrl.ToString(), context.DockerContainer.Credentials.UserToken, MethodType.Get, requestOptions);

            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));
            Assert.That(requestOptions.ShouldRetryCalled, Is.True);
            Assert.That(requestOptions.HandledRequests.Count, Is.EqualTo(2));
        }

        [TestCase("http://feedbooks.com/type/Crime%2FMystery/books/top")]
        [TestCase("http://feedbooks.com/type/Crime%252FMystery/books/top")]
        [TestCase("https://dummy.org/api/v4/projects/42400/environments?name=env_test_name_with_url&external_url=https%3A%2F%2Fdummy2.org")]
        public void Test_UriFix(string str)
        {
            var uri = UriFix.Build(str);

            Assert.That(uri.AbsoluteUri, Is.EqualTo(str));
        }

        [Test]
        public async Task Test_the_timeout_can_be_overridden_in_the_request_options()
        {
            using var context = await GitLabTestContext.CreateAsync();

            var requestOptions = new MockRequestOptions { HttpClientTimeout = TimeSpan.FromMinutes(2) };

            var httpRequestor = new HttpRequestor(context.DockerContainer.GitLabUrl.ToString(), context.DockerContainer.Credentials.UserToken, MethodType.Get, requestOptions);
            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));

            Assert.AreEqual(TimeSpan.FromMinutes(2).TotalMilliseconds, requestOptions.HandledRequests.Single().Timeout);
        }

        private sealed class MockRequestOptions : RequestOptions
        {
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
                HandledRequests.Add(request);
                throw new GitLabException { StatusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}
