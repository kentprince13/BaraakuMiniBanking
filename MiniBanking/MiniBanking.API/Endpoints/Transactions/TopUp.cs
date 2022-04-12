using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniBanking.API.Application.Validations;
using MiniBanking.API.Exception;
using MiniBanking.API.Filter;
using MiniBanking.API.Models;
using MiniBanking.Core.Models;
using MiniBanking.Core.Services;
using MiniBanking.Domain.Entities;
using MiniBanking.Domain.Enums;
using MiniBanking.Domain.Exception;
using Swashbuckle.AspNetCore.Annotations;

namespace MiniBanking.API.Endpoints.Transactions;

public class TopUp : EndpointBaseAsync.WithRequest<WalletTopUpRequest>.WithActionResult<TransferResponse>
{
    
    private readonly IGenericService _genericService;
    private readonly ILogger<TopUp> _logger;
    private readonly IMapper _mapper;
    private readonly ITransferService _service;

    public TopUp(IGenericService genericService,ILogger<TopUp> logger, IMapper mapper,
        ITransferService service)
    {
        _genericService = genericService;
        _logger = logger;
        _mapper = mapper;
        _service = service;
    }


    [HttpPost("/api/Transaction/TopUp")]
    [Authorize]
    [SwaggerOperation("TopUp User account",
        Tags = new []{"Transaction Management"})]
    public override async Task<ActionResult<TransferResponse>> HandleAsync(WalletTopUpRequest request, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Handling Account TopUp request: {Request}",request);
        try
        {
            var validator = new WalletTopUpRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                //todo extract userid from user signed in
                var user = (User)HttpContext.Items["User"];
                var userId = user.Id;
                var account = await _genericService.GetAccountByUserId(userId);
                if (account == null)
                {
                    throw new MiniBankingBadRequestException($"User Account does not exists");
                }

                var status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), request.Status);
                var topUpRequest = new TopUpRequest
                {
                    Amount = request.Amount,
                    Narration = request.Narration,
                    Status = status,
                    AccountId = account.Id,
                    PaymentReference = request.PaymentReference
                };
                var topUpResponse = await _service.TopUp(topUpRequest);   
                
                var transaction = new Transaction
                {
                    Amount = request.Amount,
                    Currency = "NGN",
                    Narration = request.Narration,
                    Reference = request.PaymentReference,
                    InitiatedBy = userId,
                    TransactionStatus = TransactionStatus.Success,
                    TransactionType = TransactionType.TopUp,
                    DestinationAccount = string.Empty,
                    ProviderReference = string.Empty
                };
                await _genericService.CreateTransaction(transaction);
                
                _logger.LogInformation("TopUp Transaction Completed");
                var response = _mapper.Map<TransferResponse>(topUpResponse);
                return Ok(response);
            }

            _logger.LogInformation("validation Check Failed, Errors: {Errors}",validationResult.Errors);
            throw new MiniBankingValidationException(validationResult.Errors.ToList());
        
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Error Occur while Performing TopUp Transaction --> {e.Message}");
            throw;
        }
    }
}