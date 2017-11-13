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
        public int CommitCount;

        [DataMember(Name = "storage_size")]
        public int StorageSize;

        [DataMember(Name = "repository_size")]
        public int RepositorySize;

        [DataMember(Name = "lfs_objects_size")]
        public int LfsObjectsSize;

        [DataMember(Name = "job_artifacts_size")]
        public int JobArtifactsSize;
    }
}
