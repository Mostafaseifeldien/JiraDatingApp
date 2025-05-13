using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Movva.Repositories.Extensions;

public static class IncludeExtension
{
    public static IQueryable<TEntity> Include<TEntity>(this DbSet<TEntity> dbSet,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        IQueryable<TEntity> query = null;
        foreach (var include in includes) query = dbSet.Include(include);

        return query == null ? dbSet : query;
    }
}