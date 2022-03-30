using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestCommentEdit
    {
        [JsonPropertyName("body")]
        public string Body;
    }
}
