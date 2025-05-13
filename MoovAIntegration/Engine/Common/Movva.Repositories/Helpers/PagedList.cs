namespace Movva.Repositories.Helpers;

public class PagedList<T>(IEnumerable<T> items, int count, int pageNumber, int pageSize)
{
    public int CurrentPage { get; set; } = pageNumber;
    public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = count;
    public List<T> Data { get; set; } = items.ToList();
}