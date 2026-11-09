using HotelManagementService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManagementService.Infrastructure.Data.Configurations;

public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.Property(s => s.Section).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Value).HasColumnType("jsonb").IsRequired();
        builder.HasIndex(s => s.Section).IsUnique();
    }
}