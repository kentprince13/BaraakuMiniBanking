using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniBanking.API.Exception;
using MiniBanking.API.Models;
using MiniBanking.Domain.Exception;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiniBanking.API.Filter;

    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
            _logger.LogInformation($"Environment: {_env.EnvironmentName}");
            HttpStatusCode code;
            StandardErrorResponse response;

            switch (context.Exception)
            {
                case MiniBankingBadRequestException e:
                    code = HttpStatusCode.BadRequest;
                    response = e.ToErrorResponse();
                    break;
                case MiniBankingValidationException e:
                    code = HttpStatusCode.BadRequest;
                    response = e.ToErrorResponse();
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    response = context.Exception.ToErrorResponse();
                    break;
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
            var result = JsonConvert.SerializeObject(response, serializerSettings);
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.HttpContext.Response.WriteAsync(result);
            context.ExceptionHandled = true;
        }
    }