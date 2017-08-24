using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IProjectClient {
        IEnumerable<Project> Accessible();
        IEnumerable<Project> Owned();
        IEnumerable<Project> Starred();
        Project Get(int id);
        Project Create(ProjectCreate project);
        bool Delete(int id);
        Project Star(int id);
    }
}