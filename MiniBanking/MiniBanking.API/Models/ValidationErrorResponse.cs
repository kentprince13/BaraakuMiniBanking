namespace MiniBanking.API.Models;

public class ValidationErrorResponse : StandardErrorResponse
{
    public IDictionary<string, string[]> Errors { get; set; }
}