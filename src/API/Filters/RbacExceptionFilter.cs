using HotelManagementService.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelmanagementService.API.Filters;
public class RbacExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is RbacException ex)
        {
            context.Result = new ObjectResult(new { status = "error", message = ex.Message })
            {
                StatusCode = ex.StatusCode
            };
            context.ExceptionHandled = true;
        }
    }
}