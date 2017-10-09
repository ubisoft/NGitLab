using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Namespaces
    {
        public const string Url = "/namespaces";

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "full_path")]
        public string FullPath { get; set; }
        [DataMember(Name = "path")]
        public string Path { get; set; }
        [DataMember(Name = "kind")]
        public string Kind { get; set; }
      
    }
}