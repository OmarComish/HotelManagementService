using AutoMapper;
using HotelManagementService.Application.DTOs;
using HotelManagementService.Application.Interfaces;
using HotelManagementService.Core.Entities;
using HotelManagementService.Core.Interfaces;
//using Microsoft.Extensions.Logging;

namespace HotelFlowAPI.Application.Services;
public class GuestService :  IGuestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly ILogger<GuestService> _logger;

    public GuestService(IUnitOfWork unitOfWork, IMapper mapper/*,ILogger<GuestService> logger*/)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
       // _logger = logger;
    }

    public async Task<GuestDto> CreateGuestAsync(CreateGuestDto createGuestDto)
    {
        //Step 1: Map basic Guest fields
        var guest = _mapper.Map<Guest>(createGuestDto);

        //Step 2: Save to database
        var createdGuest = await _unitOfWork.Guests.AddAsync(guest);
        await _unitOfWork.SaveChangesAsync();
        

        //Step 3: Add Guest Preferences if any
         Console.WriteLine("preference IDS data");
         Console.WriteLine(createGuestDto.PreferenceIds);
        if(createGuestDto.PreferenceIds != null && createGuestDto.PreferenceIds.Any())
        {
            Console.WriteLine("Adding Preferences...");
            Console.Write(createGuestDto.PreferenceIds );
            var preferences = createGuestDto.PreferenceIds
            .Select(id =>new GuestPreferences
            {
                GuestId = createdGuest.Id,
                PreferenceId = id
            }).ToList();

            await _unitOfWork.GuestPreferences.AddRangeAsync(preferences);
            await _unitOfWork.SaveChangesAsync();
        }

        /*
        if(createGuestDto.Booking!= null) //if booking details provided, create with booking
        {
            var room_details = await _unitOfWork.Rooms.GetByIdAsync(createGuestDto.Booking.RoomId);
            var booking = new Booking
            {
                RoomId = createGuestDto.Booking.RoomId,
                GuestId = createdGuest.Id,
                CheckIn = createGuestDto.Booking.CheckIn.ToUniversalTime(),
                CheckOut = createGuestDto.Booking.CheckOut.ToUniversalTime(),
                Status = "Reserved",
                TotalAmount = 100 //room_details.Price DISABLED for brevity

            };  
           
            //await _unitOfWork.Bookings.AddAsync(booking);
            //await _unitOfWork.SaveChangesAsync();
        }
        */
        //_logger.LogInformation("Guest created successfully with ID: {GuestId}", createdGuest.Id);

        //Step 4: Eager load preferences for mapping
        
        var guestwithPrefs = await _unitOfWork.Guests.GuestWithPreferenceAsync(createdGuest.Id);

        return _mapper.Map<GuestDto>(guestwithPrefs);
    }

    public async Task<GuestDto?> GetGuestByIdAsync(int id)
    {
        var guest = await _unitOfWork.Guests.GetByIdAsync(id);
        return guest != null ? _mapper.Map<GuestDto>(guest) : null;
    }

    public async Task<IEnumerable<GuestDto>> GetAllGuestsAsync()
    {
        //var guests = await _unitOfWork.Guests.GetAllAsync();
        var guests = await _unitOfWork.Guests.AllGuestsWithPreferenceAsync();
        return _mapper.Map<IEnumerable<GuestDto>>(guests);
    }

    public async Task<GuestDto> UpdateGuestAsync(int id, UpdateGuestDto updateGuestDto)
    {
        var guest = await _unitOfWork.Guests.GetByIdAsync(id);
        if (guest == null)
            throw new ArgumentException("Guest not found");

        if (!string.IsNullOrEmpty(updateGuestDto.FirstName))
            guest.FirstName = updateGuestDto.FirstName;

        if (!string.IsNullOrEmpty(updateGuestDto.LastName))
            guest.LastName = updateGuestDto.LastName;

        if (!string.IsNullOrEmpty(updateGuestDto.PhoneNumber))
            guest.PhoneNumber = updateGuestDto.PhoneNumber;
        
        if(!string.IsNullOrEmpty(updateGuestDto.IDNumber))
        {
            guest.IDNumber = updateGuestDto.IDNumber;
        }
        if(!string.IsNullOrEmpty(updateGuestDto.Address))
        {
           guest.Address = updateGuestDto.Address;
        }
        guest.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Guests.UpdateAsync(guest);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<GuestDto>(guest);
    }

    public async Task<bool> DeleteGuestAsync(int id)
    {
        var guest = await _unitOfWork.Guests.GetByIdAsync(id);
        if (guest == null)
            return false;

        // Check if guest has active bookings
        var activeBookings = await _unitOfWork.Reservations.FindAsync(b =>
            b.GuestName == guest.FirstName && (b.Status == ReservationStatuses.Reserved 
            || b.Status == ReservationStatuses.CheckedIn));

        if (activeBookings.Any())
            throw new InvalidOperationException("Cannot delete guest with active bookings");

        await _unitOfWork.Guests.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}