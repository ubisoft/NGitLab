using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectStatistics
    {
        [DataMember(Name = "commit_count")]
        public long CommitCount;

        [DataMember(Name = "storage_size")]
        public long StorageSize;

        [DataMember(Name = "repository_size")]
        public long RepositorySize;

        [DataMember(Name = "lfs_objects_size")]
        public long LfsObjectsSize;

        [DataMember(Name = "job_artifacts_size")]
        public long JobArtifactsSize;
    }
}
