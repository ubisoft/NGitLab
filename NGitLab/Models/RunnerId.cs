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

        [JsonPropertyName("runner_id")]
        public int Id;
    }
}
