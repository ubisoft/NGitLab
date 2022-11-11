namespace NGitLab.Models
{
    public class ReleaseQuery
    {
        public string OrderBy { get; set; }

        public string Sort { get; set; }

        public bool? IncludeHtmlDescription { get; set; }

        public int? PerPage { get; set; }
    }
}
