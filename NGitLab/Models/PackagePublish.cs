using System.IO;

namespace NGitLab.Models
{
    public class PackagePublish
    {
        public string PackageName { get; set; }

        public string PackageVersion { get; set; }

        public Stream PackageStream { get; set; }

        public string FileName { get; set; }

        public string Status { get; set; }
    }
}
