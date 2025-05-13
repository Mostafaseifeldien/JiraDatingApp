using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Movva.Data.Interfaces;

namespace Movva.Data.Interceptors;

public class AuditInterceptor
{
    public void OnBeforeSaving(ChangeTracker changeTracker)
    {
        foreach (var entry in changeTracker.Entries())
            switch (entry.State)
            {
                case EntityState.Added:
                    SetCreatedProperties(entry);
                    if (entry.Entity is not IHardDeletable)
                        entry.CurrentValues["IsDeleted"] = false;
                    break;

                case EntityState.Modified:
                    SetUpdatedProperties(entry);
                    break;

                case EntityState.Deleted:
                    SetUpdatedProperties(entry);
                    if (entry.Entity is not IHardDeletable)
                    {
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                    }

                    break;
            }
    }

    private void SetCreatedProperties(EntityEntry entry)
    {
        if (entry.Entity is IAuditable auditableEntity)
        {
            auditableEntity.CreatedAtUtc = DateTime.UtcNow;
            if (auditableEntity.CreatedBy == Guid.Empty)
                auditableEntity.CreatedBy = Guid.Empty;
        }
    }

    private void SetUpdatedProperties(EntityEntry entry)
    {
        if (entry.Entity is IAuditable auditableEntity)
        {
            auditableEntity.UpdatedAtUtc = DateTime.UtcNow;
            auditableEntity.UpdatedBy ??= null;
        }
    }
}