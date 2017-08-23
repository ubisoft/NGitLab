using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class FileData {
        [DataMember(Name = "file_name")]
        public string Name { get; set; }

        [DataMember(Name = "file_path")]
        public string Path { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "encoding")]
        public string Encoding { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "blob_id")]
        public string BlobId { get; set; }

        [DataMember(Name = "commit_id")]
        public string CommitId { get; set; }
    }
}