namespace NGitLab.Models;

public sealed class EnvironmentQuery
{
    /// <summary>
    /// Return the environment with this name. Mutually exclusive with search
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Return list of environments matching the search criteria. Mutually exclusive with name
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// List all environments that match a specific state. Accepted values: available, stopping or stopped
    /// </summary>
    public string State { get; set; }
}
