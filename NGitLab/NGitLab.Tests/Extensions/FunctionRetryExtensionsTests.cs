using System;
using NGitLab.Extensions;
using NGitLab.Impl;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace NGitLab.Tests.Extensions
{
    public class FunctionRetryExtensionsTests
    {
        [Test]
        public void Test_methods_retry_fail_retry_two_time()
        {
            RequestOptions options = new RequestOptions(2, TimeSpan.FromMilliseconds(50));

            var mockClass = Substitute.For<ICustomTestClass>();
            mockClass.TestRetryMethod(Arg.Any<bool>()).Throws(new Exception());

            //act
            Assert.Throws<Exception>(() => ((Func<string>) (() => mockClass.TestRetryMethod(true))).Retry(options.ShouldRetry, options.RetryInterval, options.RetryCount));

            //assert
            mockClass.ReceivedWithAnyArgs(options.RetryCount + 1).TestRetryMethod(Arg.Any<bool>());
        }

        [Test]
        public void Test_methods_dont_fail_dont_retry()
        {
            RequestOptions options = new RequestOptions(2, TimeSpan.FromMilliseconds(50));
            var mockClass = Substitute.For<ICustomTestClass>();
            mockClass.TestRetryMethod(Arg.Any<bool>()).Returns(string.Empty);

            // act
            ((Func<string>)(() => mockClass.TestRetryMethod(false))).Retry(options.ShouldRetry, options.RetryInterval, options.RetryCount);

            // assert
            mockClass.ReceivedWithAnyArgs(1).TestRetryMethod(Arg.Any<bool>());
        }
    }

    public interface ICustomTestClass
    {
        string TestRetryMethod(bool isFailed);
    }
}