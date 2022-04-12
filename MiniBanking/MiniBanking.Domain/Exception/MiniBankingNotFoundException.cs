namespace MiniBanking.Domain.Exception;

public class MiniBankingNotFoundException : MiniBankingDomainException
{
    public MiniBankingNotFoundException(string message) : base(message, "NotFound")
    {

    }
}