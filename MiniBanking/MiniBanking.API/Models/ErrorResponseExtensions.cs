using MiniBanking.API.Exception;
using MiniBanking.Domain.Exception;

namespace MiniBanking.API.Models;

public static class ErrorResponseExtensions
{
    
    public static StandardErrorResponse ToErrorResponse(this MiniBankingBadRequestException e)
    {
        return new StandardErrorResponse()
        {
            Code = "BAD REQUEST",
            StatusMessage = e.Message,
            Status = false
        };
    }  
   
    public static StandardErrorResponse ToErrorResponse(this MiniBankingDomainException e)
    {
        return new StandardErrorResponse()
        {
            Code = "Unexpected Error occurred",
            StatusMessage = e.Message,
            Status = false
        };
    }
    public static StandardErrorResponse ToErrorResponse(this MiniBankingValidationException e)
    {
        var msg = string.Empty;
        foreach (var item in e.Failures)
        {
            var errors = item.Value.ToList();
            msg += errors.Aggregate("",
                (current, next) => $"{current} {next}");
        }
            
        return new ValidationErrorResponse()
        {
            StatusMessage = $"{e.Message}  {msg}",
            Status = false,
            Errors = e.Failures
        };
    }
    public static StandardErrorResponse ToErrorResponse(this System.Exception e)
    {
        return new StandardErrorResponse()
        {
            Code = "SYSTEM_ERROR",
            StatusMessage = e.Message,
            Status = false
        };
    }
}