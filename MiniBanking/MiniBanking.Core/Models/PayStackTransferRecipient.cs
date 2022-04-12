using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MiniBanking.Core.Models;

public class PayStackTransferRecipient:BaseModel
{
    public PayStackTransferRecipient()
    {
        Type = "nuban";
        Currency = "NGN";
    }

    private string Type { get; set; }
    public string Name { get; set; }
    [JsonProperty("account_number")]
    public string AccountNumber { get; set; }
    [JsonProperty("bank_code")]
    public string BankCode { get; set; }
    private string Currency { get; set; }
}