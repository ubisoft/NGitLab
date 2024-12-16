using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface ITriggerClient
{
    /// <summary>
    /// Returns the detail of a single trigger.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Trigger this[long id] { get; }

    /// <summary>
    /// Get a list of all triggers for project.
    /// </summary>
    IEnumerable<Trigger> All { get; }

    /// <summary>
    /// Create a new trigger.
    /// </summary>
    /// <param name="description">Description of the trigger</param>
    Trigger Create(string description);
}
