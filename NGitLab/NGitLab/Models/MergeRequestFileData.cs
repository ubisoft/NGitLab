using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class MergeRequestFileData {
        [DataMember(Name = "old_path")]
        public string OldPath;

        [DataMember(Name = "new_path")]
        public string NewPath;

        [DataMember(Name = "a_mode")]
        public string AMode;

        [DataMember(Name = "b_mode")]
        public string BMode;

        [DataMember(Name = "diff")]
        public string Diff;

        [DataMember(Name = "new_file")]
        public bool IsNew;

        [DataMember(Name = "renamed_file")]
        public bool IsRenamed;

        [DataMember(Name = "deleted_file")]
        public bool IsDeleted;
    }
}
