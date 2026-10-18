namespace HotelManagementService.Application.Exceptions;
public class RbacException: Exception
{
    public int StatusCode { get; }
    public RbacException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

}