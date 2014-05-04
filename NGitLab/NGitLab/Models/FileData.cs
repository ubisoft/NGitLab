using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class FileData
    {
        [DataMember(Name = "file_name")]
        public string Name;
        [DataMember(Name = "file_path")]
        public string Path;
        [DataMember(Name = "size")]
        public int Size;
        [DataMember(Name = "encoding")]
        public string Encoding;
        [DataMember(Name = "content")]
        public string Content;
        [DataMember(Name = "ref")]
        public string Ref;
        [DataMember(Name = "blob_id")]
        public string BlobId;
        [DataMember(Name = "commit_id")]
        public string CommitId;
    }
}