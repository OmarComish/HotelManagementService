
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
        if(!context.Hotels.Any())
        {
            var hotels = new List<Hotel>
            {
                 new(){ Id = 1, Name = "Grand Hotel",City="Gaborone",Country="Botswana",
                  Address = "123 Main Street, CBD", 
                  CreatedAt = DateTime.UtcNow,
                  Description ="Five-star Presidential hotel"}
            };
            context.AddRange(hotels);
            context.SaveChanges();
        }
        if(!context.Departments.Any())
        {
            var departments = new List<Department>
            {
                new () { Id = 1, Name = "Housekeeping", CreatedAt = DateTime.UtcNow, CreatedBy="admin" },
                new () { Id = 2, Name = "Maintenance", CreatedAt = DateTime.UtcNow, CreatedBy="admin" },
                new () { Id = 3, Name = "Front Desk", CreatedAt = DateTime.UtcNow, CreatedBy="admin" },
                new () { Id = 4, Name = "Restaurant", CreatedAt = DateTime.UtcNow, CreatedBy="admin" },
                new () { Id = 5, Name = "Administration", CreatedAt = DateTime.UtcNow, CreatedBy="admin" }
            };
            context.AddRange(departments);
            context.SaveChanges();
        }
        if(!context.RoomTypes.Any())
        {
            var roomtypes = new List<RoomType>
            {
                new () { Id = 1, Type = "Standard", Price = 100.00m, Capacity = 2, CreatedBy = "admin", CreatedAt = DateTime.UtcNow },
                new () { Id = 2, Type = "Delux", Price = 150.00m, Capacity = 4, CreatedBy = "admin", CreatedAt = DateTime.UtcNow },
                new () {Id = 3, Type = "Suite", Price = 300.00m, Capacity = 6, CreatedBy = "admin", CreatedAt = DateTime.UtcNow}
            };
            context.AddRange(roomtypes);
            context.SaveChanges();
        }
        if(!context.Rooms.Any())
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
        if(!context.Users.Any())
        {
            var users = new List<User>
            {
               new () { Id = 1, Name = "Admin User", Email = "admin@hotel.com", PasswordHash = "$2a$11$hash", Role = "Admin", CreatedAt = DateTime.UtcNow },
               new () { Id = 2, Name = "Staff Member", Email = "staff@hotel.com", PasswordHash = "$2a$11$hash", Role = "Staff", CreatedAt = DateTime.UtcNow }
            };
            context.AddRange(users);
            context.SaveChanges();
        }
        if(!context.Amenities.Any())
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
        if(!context.RoomAmenities.Any())
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
        if(!context.Preferences.Any())
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
        if(!context.Employees.Any())
        {
            var employees = new List<Employee>
            {
                new () {Id= 1, DepartmentId = 1, FirstName ="John", LastName ="Doe", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"},
                new () {Id= 2, DepartmentId = 2, FirstName ="Mary", LastName ="Johnson", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"},
                new () {Id= 3, DepartmentId = 3, FirstName ="Sarah", LastName ="Wilson", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"},
                new () {Id= 4, DepartmentId = 4, FirstName ="Tracy", LastName ="Morgan", CreatedAt = DateTime.UtcNow, CreatedBy = "Admin"}  
            };
            context.AddRange(employees);
            context.SaveChanges();
        }
        if(!context.TaskTypes.Any())
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
        if(!context.OrderTypes.Any())
        {
            var ordertypes = new List<OrderType>
            {
                new () {
                    Id = 1, 
                    Name="Restaurant",
                    Abbreviation="RES",
                    Status=RecordStatus.Active,
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 2, 
                    Name="TakeAway", 
                    Abbreviation="TAW", 
                    Status=RecordStatus.Active, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 3, 
                    Name="Room-service", 
                    Abbreviation="RMS", 
                    Status=RecordStatus.Active, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                }
            };
            context.AddRange(ordertypes);
            context.SaveChanges();
        }   
        if(!context.MenuItems.Any())
        {
            var menuitems = new List<MenuItem>
            {
                new () {
                    Id = 1, 
                    HotelId = 1, 
                    Name="Fried Rice", 
                    Description="Wok-tossed rice with mixed vegetables, egg, and a hint of soy sauce", 
                    Price=10.00m, 
                    Category="Main Course", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 2, 
                    HotelId = 1, 
                    Name="Ice-cream", 
                    Description="Creamy vanilla ice-cream topped with chocolate syrup and a cherry.", 
                    Price=12.00m, 
                    Category="Dessert", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 3, 
                    HotelId = 1, 
                    Name="Grilled Salmon", 
                    Description="Fresh Atlantic salmon fillet grilled to perfection, served with lemon butter sauce and seasonal vegetables.", 
                    Price=11.00m, 
                    Category="Main Course", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                    },
                new () {
                    Id = 4, 
                    HotelId = 1, 
                    Name="Caesar Salad", 
                    Description="Crisp romaine lettuce tossed with creamy Caesar dressing, parmesan cheese, and crunchy croutons.", 
                    Price=8.00m, 
                    Category="Appetizer", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 5, 
                    HotelId = 1, 
                    Name="Red Wine", 
                    Description="A bold and smooth house red wine, perfect to accompany any meal.", 
                    Price=7.00m, 
                    Category="Beverage", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                 new () {
                    Id = 6, 
                    HotelId = 1, 
                    Name="Vanilla Cake", 
                    Description="Classic vanilla sponge cake with a smooth buttercream frosting and fresh berries.", 
                    Price=5.00m, 
                    Category="Dessert", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                  new () {
                    Id = 7, 
                    HotelId = 1, 
                    Name="Chocolate Cake", 
                    Description="Rich chocolate cake with a decadent chocolate ganache frosting and fresh strawberries.", 
                    Price=5.50m, 
                    Category="Dessert", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                  new () {
                    Id = 8, 
                    HotelId = 1, 
                    Name="Buffalo Wings", 
                    Description="Crispy chicken wings tossed in a spicy buffalo sauce, served with ranch dressing.", 
                    Price=8.00m, 
                    Category="Appetizer", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 9, 
                    HotelId = 1, 
                    Name="Grilled Baby Pork Ribs", 
                    Description="Tender pork ribs slow-cooked and grilled with a smoky barbecue glaze.", 
                    Price=8.00m, 
                    Category="Main Course", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },
                new () {
                    Id = 10, 
                    HotelId = 1, 
                    Name="Nsima with Local chicken", 
                    Description="Nsima, with Tender slow-cooked and grilled local runner chicken.", 
                    Price=8.00m, 
                    Category="Main Course", 
                    IsAvailable=true, 
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy ="Admin"
                },

            };
            context.AddRange(menuitems);
            context.SaveChanges();
        }
    }
}