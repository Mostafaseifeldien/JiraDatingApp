using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movva.DAL;
using Movva.Data;
using Movva.Repositories.Helpers;
using Movva.Repositories.Interfaces;

namespace Movva.Repositories.Implementation;

public class Repository<T>(MovvaIntegrationContext dbContext) : IAsyncRepository<T>
    where T : BaseEntity
{
    protected readonly MovvaIntegrationContext DbContext = dbContext;

    public async Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
        if (saveChanges)
            await SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<T> addRangeAsync = entities as T[] ?? entities.ToArray();
        await DbContext.Set<T>().AddRangeAsync(addRangeAsync, cancellationToken);

        if (saveChanges)
            await SaveChangesAsync(cancellationToken);

        return addRangeAsync;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().AnyAsync(expression, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().CountAsync(expression, cancellationToken);
    }

    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().CountAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbContext.Entry(entity).State = EntityState.Deleted;
        if (saveChanges)
            await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().RemoveRange(entities);

        if (saveChanges)
            await SaveChangesAsync(cancellationToken);
    }

    public async Task<T> FirstAsync(Expression<Func<T, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();
        return await query.FirstAsync(expression, cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> LastOrDefaultAsync(bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();
        return await query.OrderBy(x => x.Id).LastOrDefaultAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T?> query = enableEfChangeTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();
        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<T?> LastOrDefaultAsync(Expression<Func<T, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();
        return await query.OrderBy(x => x.Id).LastOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().FindAsync([id], cancellationToken);
    }

    public IQueryable<T> GetQuery()
    {
        return DbContext.Set<T>().AsQueryable();
    }

    public DbSet<T> GetDbSet()
    {
        return DbContext.Set<T>();
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking ? DbContext.Set<T>() : DbContext.Set<T>().AsNoTracking();
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> expression,
        bool enableEfChangeTracking = true, CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking
            ? DbContext.Set<T>().Where(expression)
            : DbContext.Set<T>().AsNoTracking().Where(expression);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<PagedList<T>> ListPagedAsync(Expression<Func<T, bool>> expression, int pageNo, int pageSize,
        bool enableEfChangeTracking = true)
    {
        var query = enableEfChangeTracking
            ? DbContext.Set<T>().Where(expression)
            : DbContext.Set<T>().AsNoTracking().Where(expression);
        return query.ToPagedList(pageNo, pageSize);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T> UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        if (saveChanges)
            await SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default)
    {
        var updateRangeAsync = entities as T[] ?? entities.ToArray();
        DbContext.Set<T>().UpdateRange(updateRangeAsync);

        if (saveChanges)
            await SaveChangesAsync(cancellationToken);

        return updateRangeAsync;
    }

    public async Task<List<T>> ListWithExpressionAsync(Expression<Func<T, bool>> expression,
        bool enableEfChangeTracking = true, Expression<Func<T, object>>? sortExpression = null,
        string sortType = "DESC", CancellationToken cancellationToken = default)
    {
        var query = enableEfChangeTracking
            ? DbContext.Set<T>().Where(expression)
            : DbContext.Set<T>().AsNoTracking().Where(expression);

        if (sortExpression != null)
            query = sortType == "DESC" ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

        return await query.ToListAsync(cancellationToken);
    }
}