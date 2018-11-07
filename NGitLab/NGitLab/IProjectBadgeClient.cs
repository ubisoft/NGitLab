using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IProjectBadgeClient
    {
        IEnumerable<Badge> All { get; }

        /// <summary>
        /// Gets the project's badges only (not group badges)
        /// </summary>
        /// <remarks>Project Badge API returns both Group and Project badges</remarks>
        IEnumerable<Badge> ProjectsOnly { get; }
        Badge this[int id] { get; }
        Badge Create(BadgeCreate badge);
        Badge Update(int id, BadgeUpdate badge);
        void Delete(int id);
    }
}
