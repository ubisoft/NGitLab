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

        /// <summary>
        /// The path that will be assigned to the resultant project after forking
        /// </summary>
        [DataMember(Name = "path")]
        public string Path { get; set; }

        /// <summary>
        /// The name that will be assigned to the resultant project after forking
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
