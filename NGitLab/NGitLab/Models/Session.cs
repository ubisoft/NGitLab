using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Session : User
    {
        public new const string Url = "/session";

        [DataMember(Name="private_token")]
        public string PrivateToken { get; set; }
    }
}