using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IPipelinesClient {
        IEnumerable<PipelineData> All();
        Pipeline Get(int id);
        IEnumerable<Job> GetJobs(int id);
    }
}
