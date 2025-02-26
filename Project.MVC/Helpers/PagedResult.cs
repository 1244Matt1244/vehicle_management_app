namespace Project.MVC.Helpers  
{  
    public class PagedResult<T>  
    {  
        public List<T> Items { get; set; }  
        public int TotalCount { get; set; }  
        public int PageIndex { get; set; }  // Renamed from PageNumber  
        public int PageSize { get; set; }  
    }  
}  