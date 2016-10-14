using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Label
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "color")]
        public string Color;
    }
}
