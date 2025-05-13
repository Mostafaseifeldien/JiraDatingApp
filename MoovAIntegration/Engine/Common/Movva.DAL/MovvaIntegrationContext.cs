using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Movva.DAL.Entities.Shared;
using Movva.Data.Extensions;

namespace Movva.DAL;

public class MovvaIntegrationContext(DbContextOptions<MovvaIntegrationContext> options) : DbContext(options)
{
    public DbSet<Configuration> Configuration { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyGlobalQueryFilters();
        base.OnModelCreating(builder);
    }
}