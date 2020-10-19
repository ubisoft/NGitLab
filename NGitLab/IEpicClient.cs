using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IEpicClient
    {
        /// <summary>
        /// Return all group's epics
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <param name="query">Filtering and ordering query</param>
        /// <returns></returns>
        IEnumerable<Epic> Get(int groupId, EpicQuery query);

        /// <summary>
        /// Return a group epic
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <param name="epicId">Epic ID</param>
        /// <returns></returns>
        Epic Get(int groupId, int epicId);

        /// <summary>
        /// Update an epic
        /// </summary>
        /// <param name="groupId">Group ID</param>
        /// <param name="epicEdit">New properties values</param>
        /// <returns></returns>
        Epic Edit(int groupId, EpicEdit epicEdit);
    }
}
