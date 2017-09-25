using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NGitLab.Models {
    [DataContract]
    public class ArtifactsFile {
        [DataMember(Name = "filename")]
        public string FileName { get; set; }
        [DataMember(Name = "size")]
        public string Size { get; set; }
    }
}
