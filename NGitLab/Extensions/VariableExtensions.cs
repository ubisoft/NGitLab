using System.Text.RegularExpressions;
using NGitLab.Models;

#pragma warning disable MA0009 // Add regex evaluation timeout

namespace NGitLab.Extensions;

public static class VariableExtensions
{
    public static bool IsMatchForEnvironment(this Variable variable, string environment)
    {
        var sqlStyleLike = variable.EnvironmentScope.Replace('*', '%').Replace('?', '_');

        if (string.IsNullOrEmpty(environment))
            return false; // or throw exception if source == null
        if (string.IsNullOrEmpty(sqlStyleLike))
            return false; // or throw exception if like == null

        return Regex.IsMatch(environment, "^" + Regex.Escape(sqlStyleLike).Replace("_", ".").Replace("%", ".*") + "$");
    }
}
