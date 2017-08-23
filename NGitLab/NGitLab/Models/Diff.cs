using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Diff {
        [DataMember(Name = "diff")]
        public string Difference { get; set; }

        [DataMember(Name = "new_path")]
        public string NewPath { get; set; }

        [DataMember(Name = "old_path")]
        public string OldPath { get; set; }

        [DataMember(Name = "a_mode")]
        public string AMode { get; set; }

        [DataMember(Name = "b_mode")]
        public string BMode { get; set; }

        [DataMember(Name = "new_file")]
        public bool IsNewFile { get; set; }

        [DataMember(Name = "renamed_file")]
        public bool IsRenamedFile { get; set; }

        [DataMember(Name = "deleted_file")]
        public bool IsDeletedFile { get; set; }
    }
}