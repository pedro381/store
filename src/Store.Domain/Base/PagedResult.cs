namespace Store.Domain.Base
{
    public class PagedResult<T>(IEnumerable<T> data, int totalItems, int currentPage, int pageSize)
    {
        public IEnumerable<T> Data { get; set; } = data;
        public int TotalItems { get; set; } = totalItems;
        public int CurrentPage { get; set; } = currentPage;
        public int TotalPages { get; set; } = (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
