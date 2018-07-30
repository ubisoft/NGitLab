using System;
using System.Collections.Generic;
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
            var requestOptions = new MockRetryer(1, TimeSpan.FromMilliseconds(10), isIncremental: false);
            var httpRequestor = new HttpRequestor(Initialize.GitLabHost, Initialize.GitLabToken, MethodType.Get, requestOptions);

            Assert.Throws<GitLabException>(() => httpRequestor.Execute("invalidUrl"));
            Assert.That(requestOptions.ShouldRetryCalled, Is.True);
            Assert.That(requestOptions.HandledRequests.Count, Is.EqualTo(2));
        }

        private class MockRetryer : RequestOptions
        {
            public bool ShouldRetryCalled { get; set; }

            public HashSet<WebRequest> HandledRequests { get; } = new HashSet<WebRequest>();

            public MockRetryer(int retryCount, TimeSpan retryInterval, bool isIncremental = true)
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
                throw new GitLabException() { StatusCode = HttpStatusCode.InternalServerError };
            }
        }
    }
}
