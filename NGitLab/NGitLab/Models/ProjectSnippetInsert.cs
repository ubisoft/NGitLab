using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class ProjectSnippetInsert
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
 
        [DataMember(Name   = "title")]
        public string Title { get; set; }


        [DataMember(Name = "file_name")]
        public string FileName { get; set; }


        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "visibility")]
        public string Visibility { get; set; }

    }
}