using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class DeviceDBContext : DbContext
{

    public DeviceDBContext(DbContextOptions<DeviceDBContext> options) : base(options)
    {
    }

    public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the assembly inside EntitiesConfigurations folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeviceDBContext).Assembly);
    }
}
