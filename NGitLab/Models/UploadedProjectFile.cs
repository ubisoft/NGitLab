using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public sealed class UploadedProjectFile
    {
        [DataMember(Name = "alt")]
        public string Alt { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "full_path")]
        public string FullPath { get; set; }

        [DataMember(Name = "markdown")]
        public string Markdown { get; set; }
    }
}
