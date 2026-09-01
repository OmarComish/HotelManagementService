using HotelManagementService.Application.Interfaces;
using HotelManagementService.Application.Services;
using HotelManagementService.Application.Mapping;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using HotelManagementService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using HotelFlowAPI.Application.Services;
using HotelmanagementService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Management API",
        Version = "v1",
        Description = "A comprehensive hotel management system API",
        Contact = new OpenApiContact
        {
            Name = "Hotel Management Team",
            Email = "support@hotelmanagement.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});



// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Database
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
     b => b.MigrationsAssembly("HotelManagementService.Infrastructure")));

// Unit of Work & Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
/*builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IRestaurantRepository,RestaurantRepository>(); 
builder.Services.AddScoped<IInvoiceLineItemRepository, InvoiceLineItemRepository>();*/ 

// Application services
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IRoomService,RoomService>();
builder.Services.AddScoped<IGuestPreferenceService, GuestPreferenceService>();
builder.Services.AddScoped<IParametersService, ParameterService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IGuestService, GuestService>();  
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();
builder.Services.AddScoped<IAmenitiesService, AmenitiesService>();
// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
   DbInitializer.DbInit(app);
}
catch( Exception e)
{
   Console.WriteLine(e.ToString());
   throw;
}

app.Run();