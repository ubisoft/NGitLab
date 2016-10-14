using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Membership
    {
        /// <summary>
        /// Example: 3
        /// </summary>
        [DataMember(Name = "id")]
        public int Id;

        /// <summary>
        /// Example john_doe
        /// </summary>
        [DataMember(Name = "username")]
        public string UserName;

        /// <summary>
        /// Example John Doe
        /// </summary>
        [DataMember(Name = "name")]
        public string Name;

        /// <summary>
        /// Example active
        /// </summary>
        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "access_level")]
        public AccessLevel AccessLevel;
    }
}