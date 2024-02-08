using Common.Services;
using Microsoft.AspNetCore.Mvc;
using MinimalAPITemplate.Api.Common.Attributes.Route;
using MinimalAPITemplate.Api.Common.Contracts;
using MinimalAPITemplate.Api.Common.Endpoint;
using MinimalAPITemplate.Api.Models.DTOs;
using MinimalAPITemplate.Api.Models.Ids;
using MinimalAPITemplate.Api.Persistence;
using MinimalAPITemplate.Api.Persistence.Concurrency;
using MinimalAPITemplate.Api.Services;

namespace MinimalAPITemplate.Api.Endpoints.Users.Login;

file record Request([FromBody] RequestBody Body) : IRequestContract
{
}
file record RequestBody(UserId UserId);
file record Response(UserDTO User, string Jwt, DateTimeOffset JwtExpiration) : IResponseContract;

[POST("Api/Users/Login")]
file class POST : HttpEndpoint<POST, Request, Response>
{
    [Inject]
    private readonly AppDbContext _dbContext = null!;
    [Inject]
    private readonly JWTService _jwtService = null!;

    public override async ValueTask<IResult> HandleRequestAsync(HttpContext context, Request request, CancellationToken cancellationToken)
    {
        var (jwt, expiration) = _jwtService.GenerateUserLoginJWT(request.Body.UserId);

        var dbResult = await _dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        return dbResult.Status switch
        {
            DbStatus.Success => Results.Ok(new Response(
                new UserDTO(request.Body.UserId), jwt, expiration
            )),
            _ => throw new UnhandledDbResultException(dbResult),
        };
    }
}
