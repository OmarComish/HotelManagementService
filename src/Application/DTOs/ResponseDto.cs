namespace HotelManagementService.Application.DTOs;
public class ResponseDto
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public object? Payload { get; set; }
}