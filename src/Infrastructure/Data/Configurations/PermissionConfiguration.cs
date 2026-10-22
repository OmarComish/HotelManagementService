using HotelManagementService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManagementService.Infrastructure.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(p => p.Key).HasMaxLength(100);
        builder.Property(p => p.Name).HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(250);
        builder.HasIndex(p => p.Key).IsUnique();
    }
}