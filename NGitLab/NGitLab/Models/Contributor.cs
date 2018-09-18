using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models
{
    [DataContract]
    public class Contributor
    {
        public const string Url = "/contributors";

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "email")]
        public string Email;

        [DataMember(Name = "commits")]
        public int Commits;

        [DataMember(Name = "additions")]
        public int Addition;

        [DataMember(Name = "deletions")]
        public int Deletions;
    }
}
