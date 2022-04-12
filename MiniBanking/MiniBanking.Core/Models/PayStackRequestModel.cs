namespace MiniBanking.Core.Models;

public class PayStackRequestModel:BaseModel
{
    public decimal Amount { get; set; }
    public string DestinationAccount { get; set; }
    public string DestinationAccountName { get; set; }
    public string BankCode { get; set; }
    public string Narration { get; set; }
    public string PaymentReference { get; set; }
    public long UserId { get; set; }
}

public class PayStackResponseModel:BaseModel
{
    public string Message { get; set; }
    public string status { get; set; }
}




