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
using MiniBanking.Domain.Exception;
using Swashbuckle.AspNetCore.Annotations;

namespace MiniBanking.API.Endpoints.Transactions;

public class Transfer : EndpointBaseAsync.WithRequest<FundTransferRequest>.WithActionResult<TransferResponse>
{
    
    private readonly IGenericService _genericService;
    private readonly ILogger<Transfer> _logger;
    private readonly IMapper _mapper;
    private readonly ITransferService _service;
    private EndpointBaseAsync.WithRequest<FundTransferRequest>.WithActionResult<TransferResponse>
        _withRequestImplementation;

    public Transfer(IGenericService genericService,ILogger<Transfer> logger, IMapper mapper,
        ITransferService service)
    {
        _genericService = genericService;
        _logger = logger;
        _mapper = mapper;
        _service = service;
    }
    
    [HttpPost("/api/Transaction/Transfer")]
    [Authorize]
    [SwaggerOperation("Transfer Funds To Bank Account",
        Tags = new []{"Transaction Management"})]
    public override async Task<ActionResult<TransferResponse>> HandleAsync(FundTransferRequest request, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Handling Bank Transfer Request: {Request}",request);
        try
        {
            var validator = new FundTransferRequestValidator();
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

                if (account.AvailableBalance < request.Amount)
                {
                    throw new MiniBankingBadRequestException("You do not have Sufficient Balance");
                }
                var transferRequest = _mapper.Map<PayStackRequestModel>(request);
                transferRequest.UserId = userId;
                var transferResponse = await _service.Transfer(transferRequest);   
                
                _logger.LogInformation("Transfer Completed");
                var response = _mapper.Map<TransferResponse>(transferResponse);
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