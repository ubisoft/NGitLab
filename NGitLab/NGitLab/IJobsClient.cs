using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models {
    public interface IJobsClient {
        IEnumerable<Job> All();

        void DownloadArtifact(Job job, Action<Stream> parser);
        void DownloadTrace(Job job, Action<Stream> parser);
    }
}
