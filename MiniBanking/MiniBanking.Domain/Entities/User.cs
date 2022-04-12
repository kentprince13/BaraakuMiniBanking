namespace MiniBanking.Domain.Entities;

public class User : EntityBase
{
    private ICollection<Transaction> _transactions;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Image { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public Account Account { get; set; }

    public ICollection<Transaction> Transactions
    {
        get { return _transactions ??= new List<Transaction>(); }
        protected set => _transactions = value;
    }
}