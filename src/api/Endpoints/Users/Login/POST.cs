using Common.Services;
using Microsoft.AspNetCore.Mvc;
using LevanaTracker.Api.Common.Attributes.Route;
using LevanaTracker.Api.Common.Contracts;
using LevanaTracker.Api.Common.Endpoint;
using LevanaTracker.Api.Models.DTOs;
using LevanaTracker.Api.Models.Ids;
using LevanaTracker.Api.Persistence;
using LevanaTracker.Api.Persistence.Concurrency;
using LevanaTracker.Api.Services;

namespace LevanaTracker.Api.Endpoints.Users.Login;

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
