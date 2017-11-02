using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models {
    [DataContract]
    public class CompareInfo {
        [DataMember(Name = "commit")]
        public Commit Commit { get; set; }
        [DataMember(Name = "commits")]
        public Commit[] Commits { get; set; }
        [DataMember(Name = "diffs")]
        public Diff[] Diff { get; set; }
        [DataMember(Name = "compare_timeout")]
        public bool CompareTimeout { get; set; }
        [DataMember(Name = "compare_same_ref")]
        public bool CompareSameRefs { get; set; }
    }
}
