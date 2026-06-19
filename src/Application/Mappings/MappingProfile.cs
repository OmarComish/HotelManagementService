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
            .ForMember(dest =>dest.Amenitieslist, opt=>opt.MapFrom(src=>src.RoomType.Amenities));

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
    }
}