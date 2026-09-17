using HotelManagementService.Application.DTOs;
using HotelManagementService.Core.Entities;

namespace HotelManagement.Application.Interfaces;
public interface IReservationService
{
    Task<ResponseDto> CreateReservation(CreateReservationDto record, ReservationStatuses status);
    Task<List<ReservationDto>> GetAllReservations();
    Task<ReservationDto> UpdateReservationAsync(UpdateReservationDto updatereservationdto);
    Task<ResponseDto> CheckIn(CheckInDto dto);
    //Task<ReservationDto> GetCurrentReservationByRoomNumberAsync(string roomNumber);
    //Task<ReservationDto> TestInterface(string roomNumber);
}