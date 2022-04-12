using MiniBanking.Core.Models;

namespace MiniBanking.API.Models;

public class WalletTopUpRequest:BaseModel
{
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public string Narration { get; set; }
    public string PaymentReference { get; set; }
}