using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class WikiPage
    {
        [DataMember(Name = "content")]
        public string Content;

        [DataMember(Name = "format")]
        public string Format;

        [DataMember(Name = "slug")]
        public string Slug;

        [DataMember(Name = "title")]
        public string Title;
    }
}
