using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class Branch
    {
        public const string Url = "/repository/branches/";

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("protected")]
        public bool Protected;
    }
}