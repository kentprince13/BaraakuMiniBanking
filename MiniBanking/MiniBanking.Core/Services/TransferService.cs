using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MiniBanking.Core.Configuration;
using MiniBanking.Core.Models;
using MiniBanking.Domain.Entities;
using MiniBanking.Domain.Enums;
using MiniBanking.Domain.Exception;
using Newtonsoft.Json;

namespace MiniBanking.Core.Services;

public class TransferService : ITransferService
{
    private readonly ILogger<TransferService> _logger;
    private readonly IHttpClientFactory _httpClient;
    private readonly IGenericService _genericService;
    private readonly PayStackSettings _options;

    public TransferService(ILogger<TransferService> logger, IHttpClientFactory httpClient,IGenericService genericService,
        IOptions<PayStackSettings> options)
    {
        _logger = logger;
        _httpClient = httpClient;
        _genericService = genericService;
        _options = options.Value;
    }
    
    public async Task<PayStackResponseModel> Transfer(PayStackRequestModel request)
    {
        try
        {
            _logger.LogInformation("Transfer Request {request}",request);

            _logger.LogInformation("Checking if transfer already exist");
            var transaction = await _genericService.GetTransaction(request.PaymentReference);
            if (transaction != null)
            {
                throw new MiniBankingBadRequestException("Transaction already exist");
            }
            transaction = new Transaction
            {
                Amount = request.Amount,
                Currency = "NGN",
                Narration = request.Narration,
                Reference = request.PaymentReference,
                InitiatedBy = request.UserId,
                DestinationAccount = request.DestinationAccount,
                TransactionType = TransactionType.Transfer
            };
            PayStackResponseModel payStackResponseModel;
            var recipient = await PayStackRecipient(new PayStackTransferRecipient
            {
                Name = request.DestinationAccountName,
                BankCode = request.BankCode,
                AccountNumber = request.DestinationAccount,
            });

            var transfer = await PayStackBankTransfer(new PayStackTransfer
            {
                Amount = (int)(request.Amount * 100), // convert amount to Kobo
                Reason = request.Narration,
                Recipient = recipient.Data.RecipientCode
                
            });
            if (transfer?.Data?.Status?.ToLower() == "success")
            {
                transaction.TransactionStatus = TransactionStatus.Success;
                transaction.ProviderReference = transfer.Data.Reference;
                await _genericService.CreateTransaction(transaction);
                
                var account = await _genericService.GetAccountByUserId(request.UserId);
                account.LedgerBalance -= request.Amount;
                account.AvailableBalance -= request.Amount;

                await _genericService.UpdateAccount(account);
                await _genericService.SaveChangesAsync();
                
                payStackResponseModel = new PayStackResponseModel
                {
                    status = transfer.Data.Status,
                    Message = transfer.Message
                };
                return payStackResponseModel;
            }
            
            // create failed transaction
            transaction.TransactionStatus = TransactionStatus.Failed;
            transaction.ProviderReference = transfer.Message;
            await _genericService.CreateTransaction(transaction);
            await _genericService.SaveChangesAsync();
            payStackResponseModel = new PayStackResponseModel
            {
                status = transfer.Data.Status,
                Message = transfer.Message
            };
            return payStackResponseModel;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }

    public async Task<PayStackResponseModel> TopUp(TopUpRequest request)
    {
        try
        {
            _logger.LogInformation("TopUpRequest Request {request}", request);
            if (request.Status == TransactionStatus.Success)
            {
                var transaction = await _genericService.GetTransaction(request.PaymentReference);
                if (transaction != null)
                {
                    throw new MiniBankingBadRequestException("Transaction already exist");
                }

                var account = await _genericService.GetAccountById(request.AccountId);
                if (account == null)
                {
                    throw new MiniBankingBadRequestException("Invalid Account ");
                }

                account.AvailableBalance += request.Amount;
                account.LedgerBalance += request.Amount;

                _genericService.UpdateAccount(account);
                _genericService.SaveChangesAsync();

                return new PayStackResponseModel
                {
                    status = "Success",
                    Message = "Top up Completed"
                };
            }

            throw new MiniBankingDomainException("TopUp Failed");
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Top up request failed, Message {Message}");
            throw;
        }

    }

    private async Task<PayStackTransferResponse> PayStackBankTransfer(PayStackTransfer payStackTransfer)
    {
        try
        {
            _logger.LogInformation("Initiating Transfer On paystack");
            var json = JsonConvert.SerializeObject(payStackTransfer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{_options.BaseUrl}/{_options.TransferUrl}";
                
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer",_options.Secret);
            requestMessage.Content = content;
            var client =  _httpClient.CreateClient();
            var request = await client.SendAsync(requestMessage);
            var response = await request.Content.ReadAsStringAsync();
            
            var transferResponse = JsonConvert.DeserializeObject<PayStackTransferResponse>(response);
       
            _logger.LogInformation($"Transfer request Completed --> response {transferResponse}");
            return transferResponse;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Transfer request failed, Message {Message}", ex.Message);
            throw;
        }
    }

    private async Task<PayStackTransferRecipientResponse> PayStackRecipient(PayStackTransferRecipient recipientRequest)
    {
        try
        {
            _logger.LogInformation("Creating Transfer Recipient On paystack");
            var json = JsonConvert.SerializeObject(recipientRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"{_options.BaseUrl}/{_options.RecipientUrl}";
                
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer",_options.Secret);
            requestMessage.Content = content;
            var client =  _httpClient.CreateClient();
            var request = await client.SendAsync(requestMessage);
            var response = await request.Content.ReadAsStringAsync();
            var recipientResponse = JsonConvert.DeserializeObject<PayStackTransferRecipientResponse>(response);

            if (request.IsSuccessStatusCode)
            {
                if (recipientResponse.Status && !string.IsNullOrWhiteSpace(recipientResponse.Data.RecipientCode) )
                {
                    _logger.LogInformation($"Transfer recipient Created Successfully --> response {recipientResponse}");
                    return recipientResponse;
                }
            }
            
            throw new MiniBankingBadRequestException($"failed to create transfer recipient--> message {recipientResponse.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Transfer request failed, Message {Message}", ex.Message);
            throw;
        }
    }
}