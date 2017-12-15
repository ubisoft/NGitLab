using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Session : User {
        public new const string Url = "/session";

        [DataMember(Name = "private_token")]
        public string PrivateToken { get; set; }
    }
    [DataContract]
    public class oauth
    {
        public   const string Url = "/../../oauth/token";
        [DataMember(Name = "grant_type")]
        public string GrantType { get; set; }
        [DataMember(Name = "username")]
        public string UserName { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
    [DataContract]
    public class token
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}