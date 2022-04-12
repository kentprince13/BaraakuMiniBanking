namespace MiniBanking.Domain.Exception;

public class MiniBankingDomainException:ApplicationException
{
    private string Code { get; set; }
    public MiniBankingDomainException()
    { }

    public MiniBankingDomainException(string message)
        : base(message)
    { }

    public MiniBankingDomainException(string message, string code)
        : base(message)
    {
        Code = code;
    }

    public MiniBankingDomainException(string message, System.Exception innerException)
        : base(message, innerException)
    { }
}