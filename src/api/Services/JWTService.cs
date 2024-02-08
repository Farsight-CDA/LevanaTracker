using Common.Services;
using Microsoft.IdentityModel.Tokens;
using MinimalAPITemplate.Api.Configuration;
using MinimalAPITemplate.Api.Models.Ids;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalAPITemplate.Api.Services;

public class JWTService : Singleton
{
    [Inject]
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = null!;
    [Inject]
    private readonly SecurityOptions _securityOptions = null!;
    private byte[] _jwtKeyBytes = null!;

    protected override ValueTask InitializeAsync()
    {
        _jwtKeyBytes = Encoding.UTF8.GetBytes(_securityOptions.JWTKey);
        return base.InitializeAsync();
    }

    public (string Token, DateTimeOffset Exiration) GenerateUserLoginJWT(UserId userId)
    {
        List<Claim> claims = [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        ];

        return GenerateJwt(claims);
    }

    private (string Token, DateTimeOffset Exiration) GenerateJwt(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(_jwtKeyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);

        var expiration = DateTime.UtcNow + TimeSpan.FromSeconds(_securityOptions.JWTExpirationSeconds);
        var token = new JwtSecurityToken(_securityOptions.JWTIssuer,
          _securityOptions.JWTIssuer,
          claims,
          expires: expiration,
          signingCredentials: credentials);

        return (_jwtSecurityTokenHandler.WriteToken(token), expiration);
    }
}
