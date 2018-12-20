using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class NotePosition
    {
        [DataMember(Name = "base_sha")]
        public string BaseSha;

        [DataMember(Name = "start_sha")]
        public string StartSha;

        [DataMember(Name = "head_sha")]
        public string HeadSha;

        [DataMember(Name = "old_path")]
        public string OldPath;

        [DataMember(Name = "new_path")]
        public string NewPath;

        [DataMember(Name = "position_type")]
        public string PositionType;

        [DataMember(Name = "old_line")]
        public int OldLine;

        [DataMember(Name = "new_line")]
        public int NewLine;
    }
}