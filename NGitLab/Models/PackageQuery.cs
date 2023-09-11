namespace NGitLab.Models
{
    public class PackageQuery
    {
        public int PackageId { get; set; }

        public PackageOrderBy OrderBy { get; set; }

        public PackageSort Sort { get; set; }

        public PackageType PackageType { get; set; }

        public string PackageName { get; set; }

        public bool IncludeVersionless { get; set; }

        public PackageStatus Status { get; set; }

        public int? PerPage { get; set; }

        public int? Page { get; set; }
    }
}
