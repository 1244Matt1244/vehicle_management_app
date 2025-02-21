namespace Project.Service.Data.Helpers
{
    public class QueryParams
    {
        public string Search { get; set; } = string.Empty;
        public int? MakeId { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
