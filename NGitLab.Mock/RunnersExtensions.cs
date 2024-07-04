using NGitLab.Models;

namespace NGitLab.Mock;

internal static class RunnersExtensions
{
    public static bool? IsActive(this RunnerRegister runner)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return runner.Active ?? !runner.Paused;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public static bool? IsActive(this RunnerUpdate runner)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return runner.Active ?? !runner.Paused;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public static bool IsActive(this Runner runner)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return runner.Active || !runner.Paused;
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
