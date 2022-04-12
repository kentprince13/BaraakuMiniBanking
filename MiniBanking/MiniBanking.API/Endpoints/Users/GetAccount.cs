using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniBanking.API.Models;
using MiniBanking.Core.Configuration;
using MiniBanking.Core.Services;
using MiniBanking.Domain.Exception;
using Swashbuckle.AspNetCore.Annotations;

namespace MiniBanking.API.Endpoints.Users;

public class GetAccountRequest
{
    [FromRoute(Name = "email")] public string Email { get; set; }
}

public class GetAccount : EndpointBaseAsync.WithRequest<GetAccountRequest>.WithActionResult<UserResponseModel>
{
    private readonly IGenericService _genericService;
    private readonly ILogger<CreateAccount> _logger;
    private readonly IMapper _mapper;
    private readonly PayStackSettings _options;

    public GetAccount(IGenericService genericService,ILogger<CreateAccount> logger, IMapper mapper,IOptions<PayStackSettings> options)
    {
        _genericService = genericService;
        _logger = logger;
        _mapper = mapper;
        _options = options.Value;
    }
    
    [HttpGet("/api/account/{email}")]
    [SwaggerOperation("Get User account",
        Tags = new []{"user Management"})]
    public override async Task<ActionResult<UserResponseModel>> HandleAsync([FromRoute]GetAccountRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling Create Account request: {Request}",request);
        try
        {
            var user = await _genericService.GetUser(request.Email);
            if (user == null)
            {
                throw new MiniBankingBadRequestException($"User with email {request.Email} Not Found");
            }
            _logger.LogInformation("User retrieved Successfully");
            var response = _mapper.Map<UserResponseModel>(user);
            return Ok(response);
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Error Occur while Creating Account --> {e.Message}");
            throw;
        }

    }
}