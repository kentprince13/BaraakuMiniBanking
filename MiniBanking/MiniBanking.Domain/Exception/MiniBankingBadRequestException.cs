namespace MiniBanking.Domain.Exception;

public class MiniBankingBadRequestException : MiniBankingDomainException
{
    public MiniBankingBadRequestException(string message)
        : base(message, "Invalid Request")
    {
    }

    public MiniBankingBadRequestException(string message, string code)
        : base(message, code)
    {

    }
}