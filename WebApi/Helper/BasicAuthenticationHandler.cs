using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace CryptoRateService.WebApi.Helper
{
    public class BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                      ILoggerFactory logger,
                                      UrlEncoder encoder,
                                      IConfiguration configuration) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (string.IsNullOrEmpty(Request.Headers.Authorization.ToString()))
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing"));

            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                var encodedCredentials = authorizationHeader.Substring("Basic ".Length).Trim();
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var credentials = decodedCredentials.Split(':');

                if (credentials.Length == 2)
                {
                    var username = credentials[0];
                    var password = credentials[1];

                    var validUsername = _configuration["Authentication:Basic:Username"];
                    var validPassword = _configuration["Authentication:Basic:Password"];

                    if (username == validUsername && password == validPassword)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, username) };
                        var identity = new ClaimsIdentity(claims, "Basic");
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                    else
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
                    }
                }
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid authorization header"));
        }
    }
}
