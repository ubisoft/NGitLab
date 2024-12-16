namespace NGitLab.Models;

public class PageQuery
{
    public const int FirstPage = 1;
    public const int DefaultPerPage = 20;
    public const int MinPerPage = 1;
    public const int MaxPerPage = 100;

    public int Page { get; set; }

    public int PerPage { get; set; }

    public PageQuery(int page = FirstPage, int perPage = DefaultPerPage)
    {
        Page = page;
        PerPage = perPage;
    }
}

public class PageQuery<TQueryType> : PageQuery
{
    public TQueryType Query { get; set; }

    public PageQuery(int page = FirstPage, int perPage = DefaultPerPage, TQueryType query = default)
        : base(page, perPage)
    {
        Query = query;
    }
}
