using HotelManagementService.Application.Interfaces;
using HotelManagementService.Application.Services;
using HotelManagementService.Application.Mapping;
using HotelManagementService.Core.Interfaces;
using HotelManagementService.Infrastructure.Data;
using HotelManagementService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using HotelFlowAPI.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IParametersService, ParameterService>();
builder.Services.AddScoped<ITaskService, TaskService>();



// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Ensure database is created and migrations applied
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    dbContext.Database.Migrate();
}

try
{
   DbInitializer.DbInit(app);
}
catch( Exception e)
{
   Console.WriteLine(e);
}

app.Run();