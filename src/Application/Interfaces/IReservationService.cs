using HotelManagementService.Application.DTOs;

namespace HotelManagement.Application.Interfaces;
public interface IReservationService
{
    Task<ResponseDto> CreateReservation(CreateReservationDto record);
    Task<List<ReservationDto>> GetAllReservations();
    Task<ReservationDto> UpdateReservationAsync(UpdateReservationDto updatereservationdto);
    Task<ResponseDto> CheckIn(CheckInDto dto);
}