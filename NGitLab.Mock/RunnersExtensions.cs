using NGitLab.Models;

namespace NGitLab.Mock;

internal static class RunnersExtensions
{
    public static bool? IsActive(this RunnerRegister runner)
    {
        return !runner.Paused;
    }

    public static bool? IsActive(this RunnerUpdate runner)
    {
        return !runner.Paused;
    }

    public static bool IsActive(this Runner runner)
    {
        return !runner.Paused;
    }
}
