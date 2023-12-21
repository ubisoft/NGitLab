using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IClusterClient
{
    /// <summary>
    /// All the cluster of the project
    /// </summary>
    IEnumerable<ClusterInfo> All { get; }
}
