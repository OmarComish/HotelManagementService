using HotelManagementService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementService.Infrastructure.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskType> TaskTypes {get; set;}
    public DbSet<RestaurantTable> RestaurantTables { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<RestaurantOrder> RestaurantOrders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Amenities> Amenities { get; set; }
    public DbSet<RoomAmenities> RoomAmenities { get; set; }
    public DbSet<GuestPreferences> GuestPreferences {get; set;}
    public DbSet<Preferences> Preferences { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Employee> Employees {get; set;}
    public DbSet<Department> Departments {get; set;}
    public DbSet<InvoiceLineItem> InvoiceLineItems {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => new { e.Name, e.City, e.Country }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
        
    }
 

}