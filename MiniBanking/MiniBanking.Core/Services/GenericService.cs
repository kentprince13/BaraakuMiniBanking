using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniBanking.Domain.Entities;
using MiniBanking.Infrastructure.Persistence;

namespace MiniBanking.Core.Services;

public class GenericService : IGenericService
{
    private readonly MiniBankingContext _context;
    private readonly ILogger<GenericService> _logger;

    public GenericService(MiniBankingContext context,ILogger<GenericService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CreateUser(User user)
    {
        _logger.LogInformation("creating user for {email}",user.Email);
        await _context.Users.AddAsync(user);

        var account = new Account
        {
            User = user,
            AvailableBalance = decimal.Zero,
            LedgerBalance = decimal.Zero,
        };
        await CreateAccount(account);
        _logger.LogInformation("User Successfully created for {email}", user.Email);
    }

    public async Task<User> GetUser(string email)
    {
        _logger.LogInformation("Fetching user for {email}",email);
        var user = await _context.Users.Include(c => c.Account).FirstOrDefaultAsync(c => c.Email == email);
        return user;
    }
   
    public async Task CreateTransaction(Transaction transaction)
    {
        _logger.LogInformation("creating Transaction for {Reference}", transaction.Reference);
        await _context.AddAsync(transaction);
        _logger.LogInformation("Transaction Successfully created for {Reference}", transaction.Reference);
    }

    public async Task<Transaction> GetTransaction(string reference)
    {
        _logger.LogInformation("Fetching Transaction for {Reference}", reference);
        var transaction = await _context.Transactions.Include(c=>c.InitiatedByUser)
            .SingleOrDefaultAsync(c => c.Reference == reference);
        return transaction;
    }

    private async Task CreateAccount(Account account)
    {
        _logger.LogInformation("creating Transaction for {user}", account.UserId);
        await _context.Accounts.AddAsync(account);
    }
   
    public async Task UpdateAccount(Account account)
    {
        _logger.LogInformation("Updating Balance for {account}", account.Id);
        _context.Accounts.Attach(account);
        _context.Entry(account).State = EntityState.Modified;
    }

    public async Task<Account> GetAccountById(long id)
    {
        _logger.LogInformation("Fetching Account for Id {id}", id);
        var transaction = await _context.Accounts.SingleOrDefaultAsync(c => c.Id == id);
        return transaction;
    }
    public async Task<Account> GetAccountByUserId(long userId)
    {
        _logger.LogInformation("Fetching Account for userid {userId}", userId);
        var transaction = await _context.Accounts.SingleOrDefaultAsync(c => c.UserId == userId);
        return transaction;
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        _logger.LogInformation("Transaction entries completed");
    }
   
}