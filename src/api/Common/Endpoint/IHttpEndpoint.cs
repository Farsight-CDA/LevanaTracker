using FluentValidation;

namespace LevanaTracker.Api.Common.Endpoint;
public interface IHttpEndpoint<TContract> : IRouteOwner
{
    Task<IResult> HandleAsync(HttpContext context, TContract requestContract, IValidator<TContract>? validator, CancellationToken cancellationToken);
}
