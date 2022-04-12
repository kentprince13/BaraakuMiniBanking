using MiniBanking.Domain.Entities;
using System.Net.Http;
using MiniBanking.Core.Models;

namespace MiniBanking.Core.Services;

public interface IGenericService
{
   Task CreateUser(User user);
   Task<User> GetUser(string email);
   Task<Transaction> GetTransaction(string reference);
   Task CreateTransaction(Transaction transaction);

   Task<Account> GetAccountById(long id);

   Task<Account> GetAccountByUserId(long userId);

   Task UpdateAccount(Account account);

   Task SaveChangesAsync();
   // Task<User> FundWallet();
   // Task<User> TransferFunds();
}