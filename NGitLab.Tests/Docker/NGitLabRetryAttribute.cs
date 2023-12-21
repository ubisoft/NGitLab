using System;
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
                    context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentRepeatCount++; // increment Retry count for next iteration. will only happen if we are guaranteed another iteration
                }
            }

            return context.CurrentResult;
        }
    }
}
