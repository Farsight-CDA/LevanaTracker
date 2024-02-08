using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LevanaTracker.Api.Services.Configuration;

public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly TokenValidationParameters _tokenValidationParameters;

    public JwtBearerOptionsConfiguration(TokenValidationParameters tokenValidationParameters)
    {
        _tokenValidationParameters = tokenValidationParameters;
    }

    public void Configure(string? name, JwtBearerOptions options)
        => options.TokenValidationParameters = _tokenValidationParameters;

    public void Configure(JwtBearerOptions options)
        => options.TokenValidationParameters = _tokenValidationParameters;
}
