using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<DeviceEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceEntity> builder)
        {
            //devices propeerties configuration
            builder.ToTable("Devices");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(200);

            builder.Property(d => d.Brand)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(d => d.State)
                    .IsRequired();

            builder.Property(d => d.CreationTime)
                    .IsRequired();

            //idexes for brand and state
            builder.HasIndex(d => d.Brand).HasDatabaseName("ix_devices_brand");
            builder.HasIndex(d => d.State).HasDatabaseName("ix_devices_state");
        }
    }
}
