using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class WikiPageCreate
    {
        [DataMember(Name = "format")]
        public string Format;

        [DataMember(Name = "content")]
        public string Content;

        [DataMember(Name = "title")]
        public string Title;
    }
}
