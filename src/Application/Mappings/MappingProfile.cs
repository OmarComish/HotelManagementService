using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        //Hotel
        CreateMap<Hotel, HotelDto>().ReverseMap();
        CreateMap<CreateHotelDto, Hotel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        //Reservations mapping
        CreateMap<CreateReservationDto, Reservation>();
        CreateMap<Reservation, ReservationDto>()
           .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room));

        //Reservation
        CreateMap<CreateRoomDto, Room>();
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.RoomType != null? src.RoomType.Type: string.Empty))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.RoomType.Price))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : null))
            .ForMember(dest =>dest.Amenitieslist, 
             opt=>opt.MapFrom(src=>src.RoomType!= null? 
             src.RoomType.Amenities.Select(ra => ra.Amenities):Enumerable.Empty<Amenities>()));

        //Invoice
        CreateMap<Invoice, CreateInvoiceDto>().ReverseMap();
        CreateMap<LineItemsDto, InvoiceLineItem>();
        
        //Guest
        CreateMap<CreateGuestDto, Guest>()
          .ForMember(dest => dest.GuestPreferences, opt => opt.Ignore()); //handled manually

        CreateMap<Guest, GuestDto>()
           .ForMember(dest => dest.Preferences, opt => opt.MapFrom(src => src.GuestPreferences
           .Where(gp => gp.Preferences != null)
           .Select(gp => gp.Preferences.Name)))
           .ForMember(dest => dest.Bookings, opt => opt.MapFrom(src => src.Bookings));

        //Task
        CreateMap<CreateTaskDto, TaskItem>();
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.FirstName + " " + src.AssignedTo.LastName : null))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : null))
            .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room != null? src.Room.RoomNumber: null))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src =>src.TaskType != null? src.TaskType.Name: null));


        //TaskType
        CreateMap<TaskType, TaskTypeDto>();
        CreateMap<CreateTaskTypeDto, TaskType>();

         //RoomType mappings
        CreateMap<CreateRoomTypeDto, RoomType>();
        CreateMap<RoomType, RoomTypeDto>();

        //MenuItem mappings
        CreateMap<CreateMenuItemDto, MenuItem>();
        CreateMap<MenuItem, MenuItemDto>()
          .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel != null ? src.Hotel.Name : null));
    
       //Guest preferences
        CreateMap<CreateGuestPreferenceDto, GuestPreferences>();
        CreateMap<GuestPreferences, GuestPreferenceDto>()
            .ForMember(dest => dest.PreferenceName, opt => opt.MapFrom(src => src.Preferences != null ? src.Preferences.Name : null));
    
       //Preference mappings
        CreateMap<CreatePreferenceDto, Preferences>();
        CreateMap<Preferences, PreferenceDto>();

        //Amenities mappings
        CreateMap<CreateAmenitiesDto, Amenities>();
        CreateMap<Amenities, AmenitiesDto>();

        //Order mappings
        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.MenuItem, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore()); // Price should be resolved from MenuItem

        
         CreateMap<UpdateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.MenuItem, opt => opt.Ignore())
            .ForMember(dest => dest.MenuItemId, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.MenuItem, opt => opt.MapFrom(src => src.MenuItem));


        //RestaurantOrder mappings
         CreateMap<CreateRestaurantOrderDto, RestaurantOrder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "New"))
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) // Calculate in service
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ReservationId, opt => opt.Ignore())
            .ForMember(dest => dest.Table, opt => opt.Ignore())
            .ForMember(dest => dest.Guest, opt => opt.Ignore())
            .ForMember(dest => dest.Reservation, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<UpdateRestaurantOrderDto, RestaurantOrder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TableId, opt => opt.Ignore())
            .ForMember(dest => dest.GuestId, opt => opt.Ignore())
            .ForMember(dest => dest.ReservationId, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Table, opt => opt.Ignore())
            .ForMember(dest => dest.Guest, opt => opt.Ignore())
            .ForMember(dest => dest.Reservation, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => 
                srcMember != null && 
                (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));
        
         CreateMap<RestaurantOrder, RestaurantOrderDto>()
            .ForMember(dest => dest.Table, opt => opt.MapFrom(src => src.Table))
            .ForMember(dest => dest.Guest, opt => opt.MapFrom(src => src.Guest))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    
    }
}