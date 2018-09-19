using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models
{
    [DataContract]
    public class UserTokenCreate
    {
        [DataMember(Name = "user_id")]
        public int UserId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "expires_at")]
        private string ExpiresAtStr { get; set; }

        public DateTime? ExpiresAt
        {
            get
            {
                DateTime expiresAt;
                if (!string.IsNullOrEmpty(ExpiresAtStr) && DateTime.TryParseExact(ExpiresAtStr, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out expiresAt))
                {
                    return expiresAt;
                }

                return null;
            }
            set => ExpiresAtStr = value.HasValue ? value.Value.ToString("yyyy-MM-dd") : null;
        }

        [DataMember(Name = "scopes")]
        public string[] Scopes { get; set; }
    }
}
