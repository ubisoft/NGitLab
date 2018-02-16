using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ForkProject
    {
        /// <summary>
        /// The ID or path of the namespace that the project will be forked to
        /// </summary>
        [DataMember(Name = "namespace")]
        public string Namespace { get; set; }
    }
}
