public class QueryParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public int Page { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public string SortBy { get; set; } = "Name";
    public string SortOrder { get; set; } = "asc";
    public string Search { get; set; }
    public int? MakeId { get; set; }
}