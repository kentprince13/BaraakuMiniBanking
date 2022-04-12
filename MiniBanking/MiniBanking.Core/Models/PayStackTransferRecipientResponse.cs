using Newtonsoft.Json;

namespace MiniBanking.Core.Models;

public class PayStackTransferRecipientResponse :BaseModel
{
    public bool Status { get; set; }
    public string Message { get; set; }
    public PayStackTransferRecipientResponseData Data { get; set; }
}

public class PayStackTransferRecipientResponseData :BaseModel
{
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Currency { get; set; }
    public string Domain { get; set; }
    public int Id { get; set; }
    public int Integration { get; set; }
    public string Name { get; set; }
    [JsonProperty("recipient_code")]
    public string RecipientCode { get; set; }
    public string Type { get; set; }
    public DateTime UpdatedAt { get; set; }
    [JsonProperty("is_deleted")]
    public bool IsDeleted { get; set; }
    public Details Details { get; set; }
}

public class Details :BaseModel
{
    public object AuthorizationCode { get; set; }
    public string AccountNumber { get; set; }
    public string AccountName { get; set; }
    public string BankCode { get; set; }
    public string BankName { get; set; }
}