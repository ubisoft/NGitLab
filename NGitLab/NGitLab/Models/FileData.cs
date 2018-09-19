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

        [DataMember(Name = "last_commit_id")]
        public string LastCommitId;

        public string DecodedContent
        {
            get
            {
                if(Encoding == "base64")
                    return Base64Decode(Content);
                return Content;
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}