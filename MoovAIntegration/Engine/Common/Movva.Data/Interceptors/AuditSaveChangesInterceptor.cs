using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Movva.Data.Interfaces;

namespace Movva.Data.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries())
            if (entry.Entity is IAuditable auditable)
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.CreatedAtUtc = DateTime.UtcNow;
                        auditable.CreatedBy = Guid.Empty;
                        break;

                    case EntityState.Modified:
                        auditable.UpdatedAtUtc = DateTime.UtcNow;
                        auditable.UpdatedBy ??= Guid.Empty;
                        break;

                    case EntityState.Deleted:
                        auditable.UpdatedAtUtc = DateTime.UtcNow;
                        auditable.UpdatedBy ??= Guid.Empty;
                        if (entry.Entity is not IHardDeletable)
                        {
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["IsDeleted"] = true;
                        }

                        break;
                }
            else if (entry.State == EntityState.Added && entry.Entity is not IHardDeletable)
                entry.CurrentValues["IsDeleted"] = false;
    }
}