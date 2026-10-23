
using HotelManagementService.Core.Constants;
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
        if(!context.Users.Any())
        {
            var users = new List<User>
            {
               new () { Id = 1,
                Username = "bob", 
                FirstName = "Bob",
                LastName="Dylan", 
                Email = "bob.dylan@hotel.com", 
                PasswordHash = "$2a$11$hash", 
                Role = "Staff", 
                CreatedAt = DateTime.UtcNow,
                EmployeeId = 1, 
                },
               new () { Id = 2,
                Username="alice", 
                FirstName = "Alice", 
                LastName="Mphempo", 
                Email = "alice.mphempo@hotel.com", 
                PasswordHash = "$2a$11$hash",
                Role = "Admin", 
                CreatedAt = DateTime.UtcNow,
                EmployeeId = 2, 
               }
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
        if(!context.Guests.Any())
        {
            var guests = new List<Guest>
            {
                new () { Id = 1, FirstName = "Walk-in", 
                LastName = "Walk-in", 
                PhoneNumber="0999123987",
                Email = "alice.smith@example.com", Notes = "Walk-in guest used as generic customer for making orders in restaurant" },
            };
            context.AddRange(guests);
            context.SaveChanges();
        }
        if(!context.RestaurantTables.Any())
        {
            var tables = new List<RestaurantTable>
            {
                new () { Id = 1, HotelId = 1, TableNumber = "T1", Capacity = 4, CreatedAt = DateTime.UtcNow, CreatedBy ="Admin" },
                new () { Id = 2, HotelId = 1, TableNumber = "T2", Capacity = 2, CreatedAt = DateTime.UtcNow, CreatedBy ="Admin" },
                new () { Id = 3, HotelId = 1, TableNumber = "T3", Capacity = 6, CreatedAt = DateTime.UtcNow, CreatedBy ="Admin" }
            };
            context.AddRange(tables);
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
                    CreatedBy ="Admin",
                    PrepTime = 25
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
                    CreatedBy ="Admin",
                    PrepTime = 10
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
                    CreatedBy ="Admin",
                    PrepTime = 30
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
                    CreatedBy ="Admin",
                    PrepTime = 15
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
                    CreatedBy ="Admin",
                    PrepTime = 5
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
                    CreatedBy ="Admin",
                    PrepTime = 25
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
                    CreatedBy ="Admin",
                    PrepTime = 25
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
                    CreatedBy ="Admin",
                    PrepTime = 10
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
                    CreatedBy ="Admin",
                    PrepTime = 30
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
                    CreatedBy ="Admin",
                    PrepTime = 15
                },

            };
            context.AddRange(menuitems);
            context.SaveChanges();
        }
       /*if(!context.Permissions.Any())
        {
            var permissions = new Dictionary<string, string>
            {
                [RbacPermissions.UsersView] = "View Users",
                [RbacPermissions.UsersCreate] = "Create Users",
                [RbacPermissions.UsersEdit] = "Edit Users",
                [RbacPermissions.UsersDelete] = "Delete Users",
                [RbacPermissions.UsersActivate] = "Activate Users",
                [RbacPermissions.UsersDeactivate] = "Deactivate Users",
                [RbacPermissions.UsersResetPassword] = "Reset User Password",
                [RbacPermissions.UsersChangeRole] = "Change User Role",

                [RbacPermissions.RolesView] = "View Roles",
                [RbacPermissions.RolesCreate] = "Create Roles",
                [RbacPermissions.RolesEdit] = "Edit Roles",
                [RbacPermissions.RolesDelete] = "Delete Roles",
                [RbacPermissions.RolesAssignPermissions] = "Assign Permissions",

                [RbacPermissions.SettingsView] = "View Settings",
                [RbacPermissions.SettingsEdit] = "Edit Settings",

                [RbacPermissions.AuditLogsView] = "View Audit Logs"
            };

            var existingKeys = context.Permissions.Select(p=>p.Key).ToList();
            var newPermissions = permissions
                .Where(kvp => !existingKeys.Contains(kvp.Key))
                .Select(kvp => new Permission
                {
                    Key = kvp.Key,  
                    Name = kvp.Value,
                    Description = kvp.Value,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            if(newPermissions.Count > 0) return;
         
            context.AddRange(newPermissions);
            context.SaveChanges();
        }
        if(!context.RolePermissions.Any())
        {
            var roles = new List<Role>
            {
                new () { Key = "Admin", 
                         Name = "Administrator", 
                         Description = "System Administrator with full access", 
                         IsActive = true, 
                         IsSystem = true, 
                         IsSuperAdmin = true, 
                         CreatedAt = DateTime.UtcNow 
                        },
                new () { Key = "Staff", 
                         Name = "Staff Member", 
                         Description = "Hotel Staff with limited access", 
                         IsActive = true, 
                         IsSystem = false, 
                         IsSuperAdmin = false, 
                         CreatedAt = DateTime.UtcNow 
                         },
                          new Role
                        {
                            Key = "front_desk",
                            Name = "Front Desk",
                            Description = "Front desk operations",
                            IsActive = true,
                            IsSystem = true,
                            IsSuperAdmin = false, 
                            CreatedAt = DateTime.UtcNow 
                        },
                        new Role
                        {
                            Key = "housekeeping",
                            Name = "Housekeeping",
                            Description = "Housekeeping operations",
                            IsActive = true,
                            IsSystem = true,
                            IsSuperAdmin = false, 
                            CreatedAt = DateTime.UtcNow 
                        },
                        new Role
                        {
                            Key = "restaurant",
                            Name = "Restaurant",
                            Description = "Restaurant operations",
                            IsActive = true,
                            IsSystem = true,
                            IsSuperAdmin = false, 
                            CreatedAt = DateTime.UtcNow 
                        }
            };
            context.AddRange(roles);
            context.SaveChanges();
        }
        if(!context.Roles.Any())
        {
           Console.WriteLine("Checking RolePermissions...");

            var roles = context.Roles
                .ToDictionary(r => r.Key, StringComparer.OrdinalIgnoreCase);

            var permissions = context.Permissions
                .ToDictionary(p => p.Key, StringComparer.OrdinalIgnoreCase);

            var assignments = new Dictionary<string, string[]>
            {
                ["Admin"] = permissions.Keys.ToArray(),

                ["Staff"] =
                [
                    RbacPermissions.SettingsView,
                    RbacPermissions.UsersView,
                    RbacPermissions.RolesView
                ]
            };

            var existingAssignments = context.RolePermissions
                .Select(rp => new
                {
                    rp.RoleId,
                    rp.PermissionId
                })
                .ToHashSet();

            var newAssignments = new List<RolePermission>();

            foreach (var assignment in assignments)
            {
                if (!roles.TryGetValue(assignment.Key, out var role))
                {
                    Console.WriteLine(
                        $"Role '{assignment.Key}' was not found.");
                    continue;
                }

                foreach (var permissionKey in assignment.Value)
                {
                    if (!permissions.TryGetValue(
                            permissionKey, out var permission))
                    {
                        Console.WriteLine(
                            $"Permission '{permissionKey}' was not found.");
                        continue;
                    }

                    var alreadyExists = existingAssignments.Any(x =>
                        x.RoleId == role.Id &&
                        x.PermissionId == permission.Id);

                    if (alreadyExists)
                        continue;

                    newAssignments.Add(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id
                    });

                    // Prevent duplicate pairs within this batch too.
                    existingAssignments.Add(new
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            if (newAssignments.Count > 0)
            {
                context.RolePermissions.AddRange(newAssignments);
                context.SaveChanges();

                Console.WriteLine(
                    $"Added {newAssignments.Count} RolePermissions.");
            }
            else
            {
                Console.WriteLine(
                    "No new RolePermissions to add.");
            }
        }
        */
        SeedRbac(context);

        context.Database.ExecuteSqlRaw(
            "SELECT setval(pg_get_serial_sequence('\"Users\"', 'Id'), COALESCE((SELECT MAX(\"Id\") FROM \"Users\"), 0) + 1, false);" +
            "SELECT setval(pg_get_serial_sequence('\"Employees\"', 'Id'), COALESCE((SELECT MAX(\"Id\") FROM \"Employees\"), 0) + 1, false);");
    }
    private static void SeedRbac(HotelDbContext context)
    {
        // 1. Permission catalog: defined in code, so always ensure every key exists.
        var permissionDefinitions = new Dictionary<string, string>
        {
            [RbacPermissions.UsersView] = "View Users",
            [RbacPermissions.UsersCreate] = "Create Users",
            [RbacPermissions.UsersEdit] = "Edit Users",
            [RbacPermissions.UsersDelete] = "Delete Users",
            [RbacPermissions.UsersActivate] = "Activate Users",
            [RbacPermissions.UsersDeactivate] = "Deactivate Users",
            [RbacPermissions.UsersResetPassword] = "Reset User Password",
            [RbacPermissions.UsersChangeRole] = "Change User Role",

            [RbacPermissions.RolesView] = "View Roles",
            [RbacPermissions.RolesCreate] = "Create Roles",
            [RbacPermissions.RolesEdit] = "Edit Roles",
            [RbacPermissions.RolesDelete] = "Delete Roles",
            [RbacPermissions.RolesAssignPermissions] = "Assign Permissions",

            [RbacPermissions.SettingsView] = "View Settings",
            [RbacPermissions.SettingsEdit] = "Edit Settings",

            [RbacPermissions.AuditLogsView] = "View Audit Logs"
        };

        var existingPermissionKeys = context.Permissions
            .Select(p => p.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missingPermissions = permissionDefinitions
            .Where(kvp => !existingPermissionKeys.Contains(kvp.Key))
            .Select(kvp => new Permission
            {
                Key = kvp.Key,
                Name = kvp.Value,
                Description = kvp.Value,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            })
            .ToList();

        if (missingPermissions.Count > 0)
        {
            context.Permissions.AddRange(missingPermissions);
            context.SaveChanges();
        }

        // 2. Roles: add any that are missing, matched by key (same as rbac.ts).
        var roleDefinitions = new[]
        {
            (Key: "admin", Name: "Administrator",
             Description: "Full system access including settings, users, roles and audit logs.", IsSuperAdmin: true),
            (Key: "manager", Name: "Manager",
             Description: "Operational oversight with read-only access to users, roles and audit logs.", IsSuperAdmin: false),
            (Key: "front_desk", Name: "Front Desk",
             Description: "Reception operations: check-in/out, reservations, guests and billing.", IsSuperAdmin: false),
            (Key: "housekeeping", Name: "Housekeeping",
             Description: "Housekeeping operations: room status and cleaning tasks.", IsSuperAdmin: false),
            (Key: "restaurant", Name: "Restaurant",
             Description: "Restaurant operations: tables, menu and orders.", IsSuperAdmin: false)
        };

        var existingRoleKeys = context.Roles
            .Select(r => r.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missingRoles = roleDefinitions
            .Where(d => !existingRoleKeys.Contains(d.Key))
            .Select(d => new Role
            {
                Key = d.Key,
                Name = d.Name,
                Description = d.Description,
                IsActive = true,
                IsSystem = true,
                IsSuperAdmin = d.IsSuperAdmin,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            })
            .ToList();

        if (missingRoles.Count > 0)
        {
            context.Roles.AddRange(missingRoles);
            context.SaveChanges();
        }

        var newRoleKeys = missingRoles
            .Select(r => r.Key)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        // 3. Role permissions.
        var roles = context.Roles.ToDictionary(r => r.Key, StringComparer.OrdinalIgnoreCase);
        var permissions = context.Permissions.ToDictionary(p => p.Key, StringComparer.OrdinalIgnoreCase);

        var assignments = new Dictionary<string, string[]>
        {
            ["admin"] = permissionDefinitions.Keys.ToArray(),
            ["manager"] = new[]
            {
                RbacPermissions.SettingsView,
                RbacPermissions.UsersView,
                RbacPermissions.RolesView,
                RbacPermissions.AuditLogsView
            }
        };

        var existingPairs = context.RolePermissions
            .Select(rp => new { rp.RoleId, rp.PermissionId })
            .AsEnumerable()
            .Select(x => (x.RoleId, x.PermissionId))
            .ToHashSet();

        var newPairs = new List<RolePermission>();

        foreach (var (roleKey, permissionKeys) in assignments)
        {
            if (!roles.TryGetValue(roleKey, out var role))
                throw new InvalidOperationException($"RBAC seed: role '{roleKey}' was not found.");

            // The super admin always holds the full catalog. Every other role gets its
            // defaults only when it is first created, so restarts never undo edits
            // made through the roles API.
            if (!role.IsSuperAdmin && !newRoleKeys.Contains(roleKey))
                continue;

            foreach (var permissionKey in permissionKeys)
            {
                if (!permissions.TryGetValue(permissionKey, out var permission))
                    throw new InvalidOperationException($"RBAC seed: permission '{permissionKey}' was not found.");

                if (existingPairs.Add((role.Id, permission.Id)))
                {
                    newPairs.Add(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "system"
                    });
                }
            }
        }

        if (newPairs.Count > 0)
        {
            context.RolePermissions.AddRange(newPairs);
            context.SaveChanges();
        }

        // 4. Backfill Users.RoleId from the legacy Role string.
        var usersWithoutRole = context.Users.Where(u => u.RoleId == null).ToList();
        if (usersWithoutRole.Count > 0)
        {
            foreach (var user in usersWithoutRole)
            {
                var key = user.Role switch
                {
                    "Admin" => "admin",
                    "Staff" => "manager",
                    _ => user.Role.ToLowerInvariant()
                };

                if (roles.TryGetValue(key, out var matchedRole))
                    user.RoleId = matchedRole.Id;
            }
            context.SaveChanges();
        }
    }
}