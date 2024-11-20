namespace NGitLab.Models;

public class TagQuery
{
    public string OrderBy { get; set; }

    public string Sort { get; set; }

    public int? PerPage { get; set; }

    public string Search { get; set; }
}
