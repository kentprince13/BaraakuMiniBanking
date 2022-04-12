using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniBanking.API.Application.Validations;
using MiniBanking.API.Exception;
using MiniBanking.API.Models;
using MiniBanking.Core.Configuration;
using MiniBanking.Core.Services;
using MiniBanking.Domain.Entities;
using MiniBanking.Domain.Exception;
using MiniBanking.Domain.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace MiniBanking.API.Endpoints.Users;

public class CreateAccount : EndpointBaseAsync.WithRequest<UserRequestModel>.WithActionResult<UserResponseModel>
{
    private readonly IGenericService _genericService;
    private readonly ILogger<CreateAccount> _logger;
    private readonly IMapper _mapper;
    private readonly PayStackSettings _options;

    public CreateAccount(IGenericService genericService,ILogger<CreateAccount> logger, IMapper mapper,IOptions<PayStackSettings> options)
    {
        _genericService = genericService;
        _logger = logger;
        _mapper = mapper;
        _options = options.Value;
    }
    private EndpointBaseAsync.WithRequest<UserRequestModel>.WithActionResult<UserResponseModel> _withRequestImplementation;
    [HttpPost("/api/account")]
    [SwaggerOperation("Create User account",Tags = new []{"user Management"})]
    public override async Task<ActionResult<UserResponseModel>> HandleAsync(UserRequestModel request, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Handling Create Account request: {Request}",request);
        try
        {
            var validator = new UserRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                var user = await _genericService.GetUser(request.Email);
                if (user != null)
                {
                    throw new MiniBankingBadRequestException($"User with email {request.Email} already exists");
                }

                user = _mapper.Map<User>(request);
                user.IsActive = true;
                user.Password = CommonHelper.Encrypt(request.Password,_options.Secret);
                await _genericService.CreateUser(user);
                await _genericService.SaveChangesAsync();
                _logger.LogInformation("User creation Completed");
                var response = _mapper.Map<UserResponseModel>(user);
                return Created("", response);
            }

            _logger.LogInformation("validation Check Failed, Errors: {Errors}",validationResult.Errors);
            throw new MiniBankingValidationException(validationResult.Errors.ToList());
        
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Error Occur while Creating Account --> {e.Message}");
            throw;
        }

    }
}