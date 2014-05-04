using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Diff
    {
        [DataMember(Name = "diff")]
        public string Difference;

        [DataMember(Name = "new_path")]
        public string NewPath;

        [DataMember(Name = "old_path")]
        public string OldPath;

        [DataMember(Name = "a_mode")]
        public string AMode;

        [DataMember(Name = "b_mode")]
        public string BMode;

        [DataMember(Name = "new_file")]
        public bool IsNewFile;

        [DataMember(Name = "renamed_file")]
        public bool IsRenamedFile;

        [DataMember(Name = "deleted_file")]
        public bool IsDeletedFile;
    }
}