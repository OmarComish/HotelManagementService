using HotelManagementService.Application.DTOs;
using HotelManagementService.Core.Entities;

namespace HotelManagementService.Application.Interfaces;
public interface IReservationService
{
    Task<ResponseDto> CreateReservation(CreateReservationDto record);
    Task<List<ReservationDto>> GetAllReservations();
    Task<ReservationDto> UpdateReservationAsync(UpdateReservationDto updatereservationdto);
    Task<ResponseDto> CheckIn(CheckInDto dto);
    Task<ResponseDto> CheckOut(int reservationId);
    Task<IEnumerable<ReservationDto>?> GetReservationOnCheckOutAsync();
}