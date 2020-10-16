using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IEpicClient
    {
        /// <summary>
        /// Return all group's epics
        /// </summary>
        /// <param name="groupId">Project ID</param>
        /// <param name="query">Filtering and ordering query</param>
        /// <returns></returns>
        IEnumerable<Epic> Get(int groupId, EpicQuery query);

        /// <summary>
        /// Return a group epic
        /// </summary>
        /// <param name="groupId">Project ID</param>
        /// <param name="epicId">Deployment ID</param>
        /// <returns></returns>
        Epic Get(int groupId, int epicId);
    }
}
