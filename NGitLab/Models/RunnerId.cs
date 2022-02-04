using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RunnerId
    {
        public RunnerId()
        {
        }

        public RunnerId(int id)
        {
            Id = id;
        }

        [DataMember(Name = "runner_id")]
        [JsonPropertyName("runner_id")]
        public int Id;
    }
}
