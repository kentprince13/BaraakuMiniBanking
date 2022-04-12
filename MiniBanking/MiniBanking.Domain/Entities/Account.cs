namespace MiniBanking.Domain.Entities;

public class Account : EntityBase
{
    public decimal AvailableBalance { get; set; }
    public decimal LedgerBalance { get; set; }
    public User User { get; set; }
    public long UserId { get; set; }
}