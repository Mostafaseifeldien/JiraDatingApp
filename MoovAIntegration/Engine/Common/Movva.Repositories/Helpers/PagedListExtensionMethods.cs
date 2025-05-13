using Microsoft.EntityFrameworkCore;

namespace Movva.Repositories.Helpers;

public static class PagedListExtensionMethods
{
    public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageNumber = 0, int pageSize = 0)
    {
        var totalCount = source.CountAsync().Result;
        var items = pageNumber == 0 || pageSize == 0
            ? source.ToList()
            : source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }

    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageNumber = 0, int pageSize = 0)
    {
        var totalCount = source.Count();
        var items = pageNumber == 0 || pageSize == 0
            ? source.ToList()
            : source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }
}