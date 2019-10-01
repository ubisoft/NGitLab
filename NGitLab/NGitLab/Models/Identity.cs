using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models
{
    [DataContract]
    public class Identity
    {
        [DataMember(Name = "provider")]
        public string Provider;

        [DataMember(Name = "extern_uid")]
        public string ExternUid;
    }
}
