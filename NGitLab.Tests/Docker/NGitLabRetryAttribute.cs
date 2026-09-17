using System;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace NGitLab.Tests.Docker;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class NGitLabRetryAttribute : NUnitAttribute, IRepeatTest
{
    private readonly int _tryCount = 10;

    public TestCommand Wrap(TestCommand command)
    {
        return new RetryCommand(command, _tryCount);
    }

    public class RetryCommand : DelegatingTestCommand
    {
        // Some failures are caused by GitLab-side operations still settling asynchronously
        // (e.g. a Sidekiq deletion job). Retrying instantly gives the server no time to catch up,
        // so back off between attempts instead of hammering the same not-yet-resolved state.
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(3);

        private readonly int _tryCount;

        public RetryCommand(TestCommand innerCommand, int tryCount)
            : base(innerCommand)
        {
            _tryCount = tryCount;
        }

        public override TestResult Execute(TestExecutionContext context)
        {
            var count = _tryCount;
            while (count-- > 0)
            {
                try
                {
                    context.CurrentResult = innerCommand.Execute(context);
                }
                catch (Exception ex)
                {
                    context.CurrentResult ??= context.CurrentTest.MakeTestResult();
                    context.CurrentResult.RecordException(ex);
                }

                if (context.CurrentResult.ResultState != ResultState.Failure && context.CurrentResult.ResultState != ResultState.Error)
                    break;

                if (count > 0)
                {
                    Thread.Sleep(RetryDelay);
                    context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentRepeatCount++; // increment Retry count for next iteration. will only happen if we are guaranteed another iteration
                }
            }

            return context.CurrentResult;
        }
    }
}
