using MiniBanking.Domain.Enums;
using MiniBanking.Domain.Utilities;

namespace MiniBanking.Domain.Entities;

public class Transaction : EntityBase
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public User InitiatedByUser { get; set; }
    public long InitiatedBy { get; set; }
    public string Reference { get; set; }
    public string ProviderReference { get; set; }
    public string Narration { get; set; }
    public string DestinationAccount { get; set; }
    public TransactionStatus TransactionStatus
    {
        get => Status.ParseEnum<TransactionStatus>();
        set => Status = value.ToString();
    }
    private string Status { get; set; } 
    
    public TransactionType TransactionType
    {
        get => Type.ParseEnum<TransactionType>();
        set => Type = value.ToString();
    }
    private string Type { get; set; }
    
}