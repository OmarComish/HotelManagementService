using HotelManagementService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManagementService.Infrastructure.Data.Configurations;
  public class NotificationConfiguration: IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Recipient)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(n => n.Channel)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(n => n.Subject)
                .HasMaxLength(255);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasColumnType("text");

            builder.Property(n => n.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(n => n.ErrorMessage)
                .HasMaxLength(2000);

            builder.Property(n => n.CreatedAt)
                .IsRequired();

            builder.HasIndex(n => n.ReservationId);

            builder.HasIndex(n => new { n.Status, n.CreatedAt });
        }
    }