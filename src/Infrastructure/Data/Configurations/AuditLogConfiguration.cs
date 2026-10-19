    using HotelManagementService.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace HotelManagementService.Infrastructure.Data.Configurations;

    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(a => a.ActorName).HasMaxLength(150).IsRequired();
            builder.Property(a => a.Action).HasMaxLength(50).IsRequired();
            builder.Property(a => a.EntityType).HasMaxLength(30).IsRequired();
            builder.Property(a => a.EntityId).HasMaxLength(50);
            builder.Property(a => a.EntityLabel).HasMaxLength(200).IsRequired();
            builder.Property(a => a.IpAddress).HasMaxLength(64);
            builder.Property(a => a.UserAgent).HasMaxLength(512);
            builder.Property(a => a.PreviousValue).HasColumnType("jsonb");
            builder.Property(a => a.NewValue).HasColumnType("jsonb");

            builder.HasIndex(a => a.CreatedAt);
            builder.HasIndex(a => new { a.EntityType, a.Action });
        }
    }