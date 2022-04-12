using MiniBanking.Domain.Entities;
using MiniBanking.Domain.Enums;
using Newtonsoft.Json;

namespace MiniBanking.Core.Models;

public class PayStackTransfer:BaseModel
{
    public PayStackTransfer()
    {
        Source = "balance";
    }
    
    [JsonProperty("source")]
    public string Source { get; set; }
    [JsonProperty("amount")]
    public int Amount { get; set; }
    [JsonProperty("recipient")]
    public string Recipient { get; set; }
    [JsonProperty("reason")]
    public string Reason { get; set; }
}

public class TopUpRequest:BaseModel
{
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; }
    public string Narration { get; set; }
    public string PaymentReference { get; set; }
    public long AccountId { get; set; }
}