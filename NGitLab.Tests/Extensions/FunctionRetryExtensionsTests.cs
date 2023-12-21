using System;
using System.Net;
using NGitLab.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace NGitLab.Tests.Extensions;

public class FunctionRetryExtensionsTests
{
    [Test]
    public void Test_methods_retry_fail_retry_two_time()
    {
        var options = new RequestOptions(2, TimeSpan.FromMilliseconds(50));

        var mockClass = Substitute.For<ICustomTestClass>();
        mockClass.TestRetryMethod(Arg.Any<bool>()).Throws(new GitLabException { StatusCode = HttpStatusCode.InternalServerError });

        // act
        Assert.Throws<GitLabException>(() => ((Func<string>)(() => mockClass.TestRetryMethod(isFailed: true))).Retry(options.ShouldRetry, options.RetryInterval, options.RetryCount, options.IsIncremental));

        // assert
        mockClass.ReceivedWithAnyArgs(options.RetryCount + 1).TestRetryMethod(Arg.Any<bool>());
    }

    [Test]
    public void Test_methods_dont_fail_dont_retry()
    {
        var options = new RequestOptions(2, TimeSpan.FromMilliseconds(50));
        var mockClass = Substitute.For<ICustomTestClass>();
        mockClass.TestRetryMethod(Arg.Any<bool>()).Returns(string.Empty);

        // act
        ((Func<string>)(() => mockClass.TestRetryMethod(isFailed: false))).Retry(options.ShouldRetry, options.RetryInterval, options.RetryCount, options.IsIncremental);

        // assert
        mockClass.ReceivedWithAnyArgs(1).TestRetryMethod(Arg.Any<bool>());
    }
}

public interface ICustomTestClass
{
    string TestRetryMethod(bool isFailed);
}
