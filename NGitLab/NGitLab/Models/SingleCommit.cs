using System;
using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class SingleCommit : Commit
    {
        [JsonProperty("committed_date")]
        public DateTime CommittedDate;
        [JsonProperty("authored_date")]
        public DateTime AuthoredDate;
        [JsonProperty("parent_ids")]
        public string[] Parents;
    }
}