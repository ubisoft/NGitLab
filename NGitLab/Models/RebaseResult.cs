using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RebaseResult
    {
        [DataMember(Name = "rebase_in_progress")]
        [JsonPropertyName("rebase_in_progress")]
        public bool RebaseInProgress;
    }
}
