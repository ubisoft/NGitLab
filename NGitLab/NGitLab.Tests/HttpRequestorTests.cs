using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NGitLab.Impl;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class HttpRequestorTests
    {
        [Test]
        public void Test_calls_are_retried_when_they_fail_in_gitlab()
        {
            var requestOptions = new MockRequestOptions(1, TimeSpan.FromMilliseconds(10), isIncremental: false);
            var httpRequestor = new HttpRequestor(Initialize.GitLabHost, Initialize.GitLabToken, MethodType.Get, requestOptions);

            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));
            Assert.That(requestOptions.ShouldRetryCalled, Is.True);
            Assert.That(requestOptions.HandledRequests.Count, Is.EqualTo(2));
        }

        [Test]
        public void Test_the_timeout_can_be_overridden_in_the_request_options()
        {
            var requestOptions = new MockRequestOptions { HttpClientTimeout = TimeSpan.FromMinutes(2) };

            var httpRequestor = new HttpRequestor(Initialize.GitLabHost, Initialize.GitLabToken, MethodType.Get, requestOptions);
            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));

            Assert.AreEqual(TimeSpan.FromMinutes(2).TotalMilliseconds, requestOptions.HandledRequests.Single().Timeout);
        }

        private class MockRequestOptions : RequestOptions
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
