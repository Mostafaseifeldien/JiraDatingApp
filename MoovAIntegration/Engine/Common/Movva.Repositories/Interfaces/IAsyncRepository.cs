using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movva.Data;
using Movva.Repositories.Helpers;

namespace Movva.Repositories.Interfaces;

public interface IAsyncRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
    Task<int> CountAllAsync(CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task<T> FirstAsync(Expression<Func<T, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(bool enableEfChangeTracking = true, CancellationToken cancellationToken = default);
    Task<T?> LastOrDefaultAsync(bool enableEfChangeTracking = true, CancellationToken cancellationToken = default);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default);

    Task<T?> LastOrDefaultAsync(Expression<Func<T, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    IQueryable<T> GetQuery();
    DbSet<T> GetDbSet();

    Task<IReadOnlyList<T>> ListAllAsync(bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> expression, bool enableEfChangeTracking = true,
        CancellationToken cancellationToken = default);

    Task<PagedList<T>> ListPagedAsync(Expression<Func<T, bool>> expression, int pageNo, int pageSize,
        bool enableEfChangeTracking = true);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, bool saveChanges = true,
        CancellationToken cancellationToken = default);

    Task<List<T>> ListWithExpressionAsync(
        Expression<Func<T, bool>> expression,
        bool enableEfChangeTracking = true,
        Expression<Func<T, object>>? sortExpression = null,
        string sortType = "DESC",
        CancellationToken cancellationToken = default);
}