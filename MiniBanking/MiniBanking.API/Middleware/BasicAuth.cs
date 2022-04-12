using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using MiniBanking.Core.Configuration;
using MiniBanking.Core.Services;
using MiniBanking.Domain.Entities;
using MiniBanking.Domain.Utilities;

namespace MiniBanking.API.Middleware;

public class BasicAuth
{
    private readonly RequestDelegate _next;

    public BasicAuth(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IGenericService service,IOptions<PayStackSettings> options)
    {
        try
        {
            var auth = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
            var credentialByte = Convert.FromBase64String(auth.Parameter);
            var credential = Encoding.UTF8.GetString(credentialByte).Split(':',2);
            var email = credential[0];
            var password = credential[1];
            var user = await service.GetUser(email);
            User tempUser = null;
            
           
            if (user != null && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
            {
                var decryptedPassword = CommonHelper.Decrypt(user.Password, options.Value.Secret);
                if (!string.Equals(password, decryptedPassword, StringComparison.InvariantCultureIgnoreCase))
                    user = null;
            }
            else
            {
                user = null;
            }
            
            
            context.Items["User"] = user;
            //context.User.Identity = new UserIdentity(user)
        }
        
        catch 
        {
        }

        await _next(context);
    }
}