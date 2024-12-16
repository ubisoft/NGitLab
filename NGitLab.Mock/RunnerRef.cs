using System;
using System.Collections.Generic;

namespace NGitLab.Mock;

public sealed class RunnerRef : GitLabObject
{
    private readonly Runner _runner;

    public long Id => _runner.Id;

    public RunnerRef(Runner runner)
    {
        _runner = runner ?? throw new ArgumentNullException(nameof(runner));
    }

    public override bool Equals(object obj)
    {
        return obj is RunnerRef @ref &&
               EqualityComparer<long>.Default.Equals(Id, @ref.Id);
    }

    public override int GetHashCode()
    {
        return -1771473357 + EqualityComparer<long>.Default.GetHashCode(Id);
    }
}
