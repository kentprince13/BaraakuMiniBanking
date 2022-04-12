using MiniBanking.Core.Models;

namespace MiniBanking.API.Models;

public class FundTransferRequest:BaseModel
{
    public decimal Amount { get; set; }
    public string DestinationAccount { get; set; }
    public string DestinationAccountName { get; set; }
    public string BankCode { get; set; }
    public string Narration { get; set; }
    public string PaymentReference { get; set; }
}