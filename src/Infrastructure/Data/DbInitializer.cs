
using HotelManagementService.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagementService.Infrastructure.Data;
public class DbInitializer
{
    public static void DbInit(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        SeedData(scope.ServiceProvider.GetService<HotelDbContext>());
    }

    private static void SeedData(HotelDbContext context)
    {
        context.Database.Migrate();
        if(context.Hotels.Any())
        {
            var hotels = new List<Hotel>
            {
                 new(){ Id = 1, Name = "Grand Hotel", Address = "123 Main Street, CBD", CreatedAt = DateTime.UtcNow}
            };
            context.AddRange(hotels);
            context.SaveChanges();
        }
        
        if(context.RoomTypes.Any())
        {
            var roomtypes = new List<RoomType>
            {
                new () { Id = 1, Type = "Standard", Price = 100.00m, Capacity = 2, CreatedBy = "admin", CreatedAt = DateTime.UtcNow },
                new () { Id = 2, Type = "Delux", Price = 150.00m, Capacity = 4, CreatedBy = "admin", CreatedAt = DateTime.UtcNow },
                new () {Id = 3, Type = "Standard", Price = 300.00m, Capacity = 6, CreatedBy = "admin", CreatedAt = DateTime.UtcNow}
            };
            context.AddRange(roomtypes);
            context.SaveChanges();
        }
        if(context.Rooms.Any())
        {
            var rooms = new List<Room>
            {
               new (){ Id = 1, HotelId = 1, RoomNumber = "101", RoomTypeId = 1, Status = RecordStatus.Available, CreatedAt = DateTime.UtcNow },
               new () { Id = 2, HotelId = 1, RoomNumber = "102", RoomTypeId = 2, Status = RecordStatus.Available,CreatedAt = DateTime.UtcNow },
               new () { Id = 3, HotelId = 1, RoomNumber = "201", RoomTypeId = 3, Status = RecordStatus.Available, CreatedAt = DateTime.UtcNow }
            };
            context.AddRange(rooms);
            context.SaveChanges();
        }
        if(context.Users.Any())
        {
            var users = new List<User>
            {
               new () { Id = 1, Name = "Admin User", Email = "admin@hotel.com", PasswordHash = "$2a$11$hash", Role = "Admin", CreatedAt = DateTime.UtcNow },
               new () { Id = 2, Name = "Staff Member", Email = "staff@hotel.com", PasswordHash = "$2a$11$hash", Role = "Staff", CreatedAt = DateTime.UtcNow }
            };
            context.AddRange(users);
            context.SaveChanges();
        }
        if(context.Amenities.Any())
        {
            var amenities = new List<Amenities>
            {
                new () { Id = 1, Name = "AC", CreatedAt = DateTime.UtcNow, },
                new () { Id = 2, Name = "Balcony", CreatedAt = DateTime.UtcNow, },
                new () { Id = 3, Name = "Coffee Maker", CreatedAt = DateTime.UtcNow, },
                new () { Id = 4, Name = "Hair Dryer", CreatedAt = DateTime.UtcNow, },
                new () { Id = 5, Name = "Jacuzzi", CreatedAt = DateTime.UtcNow, },
                new () { Id = 6, Name = "Mini Bar", CreatedAt = DateTime.UtcNow, },
                new () { Id = 7, Name = "Room Service", CreatedAt = DateTime.UtcNow, },
                new () { Id = 8, Name = "Safe", CreatedAt = DateTime.UtcNow, },
                new () { Id = 9, Name = "TV", CreatedAt = DateTime.UtcNow, },
                new () { Id = 10, Name = "WiFi", CreatedAt = DateTime.UtcNow, }
            };
            context.AddRange(amenities);
            context.SaveChanges();
        }
        if(context.RoomAmenities.Any())
        {
            var amenities = new List<RoomAmenities>
            {
            new () {Id = 1,RoomTypeId= 2, AmenitiesId =9, CreatedAt = DateTime.UtcNow, },
            new () {Id = 2,RoomTypeId= 2, AmenitiesId =1, CreatedAt = DateTime.UtcNow, },
            new () {Id = 3, RoomTypeId = 2, AmenitiesId = 10, CreatedAt = DateTime.UtcNow, },
            new () {Id = 4,RoomTypeId= 2, AmenitiesId =7, CreatedAt = DateTime.UtcNow, },
            new () {Id = 5,RoomTypeId= 2, AmenitiesId =9, CreatedAt = DateTime.UtcNow, },
            new () {Id = 6,RoomTypeId= 1, AmenitiesId =9, CreatedAt = DateTime.UtcNow, },
            new () {Id = 7,RoomTypeId= 1, AmenitiesId =10, CreatedAt = DateTime.UtcNow, },
            new () {Id = 8,RoomTypeId= 1, AmenitiesId =7, CreatedAt = DateTime.UtcNow, },
            new () {Id = 9,RoomTypeId= 1, AmenitiesId =1, CreatedAt = DateTime.UtcNow, },
            new (){Id = 10,RoomTypeId= 2, AmenitiesId =3, CreatedAt = DateTime.UtcNow, },
            new () {Id = 11,RoomTypeId= 3, AmenitiesId =1, CreatedAt = DateTime.UtcNow, },
            new () {Id = 12,RoomTypeId= 3, AmenitiesId =2, CreatedAt = DateTime.UtcNow, },
            new () {Id = 13,RoomTypeId= 3, AmenitiesId =3, CreatedAt = DateTime.UtcNow, },
            new () {Id = 14,RoomTypeId= 3, AmenitiesId =4, CreatedAt = DateTime.UtcNow, },
            new () {Id = 15,RoomTypeId= 3, AmenitiesId =5, CreatedAt = DateTime.UtcNow, },
            new (){Id = 16,RoomTypeId= 3, AmenitiesId =6, CreatedAt = DateTime.UtcNow, },
            new () {Id = 17,RoomTypeId= 3, AmenitiesId =7, CreatedAt = DateTime.UtcNow, },
            new () {Id = 18,RoomTypeId= 3, AmenitiesId =8, CreatedAt = DateTime.UtcNow, },
            new () {Id = 19,RoomTypeId= 3, AmenitiesId =9, CreatedAt = DateTime.UtcNow, },
            new () {Id = 20,RoomTypeId= 3, AmenitiesId =10, CreatedAt = DateTime.UtcNow, }  
            };

            context.AddRange(amenities);
            context.SaveChanges();
        }
        if(context.Preferences.Any())
        {
            var preferences = new List<Preferences>
            {
                new () {Id = 1, Name="Non-smoking", CreatedAt = DateTime.UtcNow},
                new () {Id = 2, Name="Early checkin", CreatedAt = DateTime.UtcNow},
                new () {Id = 3, Name="High floor", CreatedAt = DateTime.UtcNow},
                new ()  {Id = 4, Name="Newspaper", CreatedAt = DateTime.UtcNow},
                new ()  {Id = 5, Name="Late checkout", CreatedAt = DateTime.UtcNow},
                new ()  {Id = 6, Name="Quiet room", CreatedAt = DateTime.UtcNow},
                new ()  {Id = 7, Name="Room service", CreatedAt = DateTime.UtcNow},
                new ()  {Id = 8, Name="Extra towels", CreatedAt = DateTime.UtcNow}
            };
            context.AddRange(preferences);
            context.SaveChanges();
        }
        if(context.Employees.Any())
        {
            var employees = new List<Employee>
            {
                new () {Id= 1, DepartmentId = 1, FirstName ="John", LastName ="Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"},
                new () {Id= 2, DepartmentId = 2, FirstName ="Mary", LastName ="Johnson", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"},
                new () {Id= 3, DepartmentId = 3, FirstName ="Sarah", LastName ="Wilson", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"},
                new () {Id= 4, DepartmentId = 2, FirstName ="Tracy", LastName ="Morgan", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"}  
            };
            context.AddRange(employees);
            context.SaveChanges();
        }
        if(context.TaskTypes.Any())
        {
            var tasktypes = new List<TaskType>
            {
                new () {Id = 1, Name="HouseKeeping", Title = "House Keeping", CreatedAt = DateTime.UtcNow, CreatedBy ="Admin"},
                new () {Id = 2, Name="Maintenance", Title = "Maintenance", CreatedAt = DateTime.UtcNow, CreatedBy ="Admin"},
                new () {Id = 3, Name="FrontDesk", Title = "Front Desk", CreatedAt = DateTime.UtcNow, CreatedBy ="Admin"},
                new () {Id = 4, Name="Restaurant", Title = "Restaurant", CreatedAt = DateTime.UtcNow, CreatedBy ="Admin"},
                new () {Id = 5, Name="Administration", Title = "Administration", CreatedAt = DateTime.UtcNow, CreatedBy ="Admin"}
            };
            context.AddRange(tasktypes);
            context.SaveChanges();
        }
    
    }
}