using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Movva.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyGlobalQueryFilters(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity))))
        {
            var parameter = Expression.Parameter(entityType.ClrType);
            var propertyMethodInfo = typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(bool));
            var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
            var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
            var lambda = Expression.Lambda(compareExpression, parameter);

            builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}