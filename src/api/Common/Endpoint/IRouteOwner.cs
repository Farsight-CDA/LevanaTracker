namespace LevanaTracker.Api.Common.Endpoint;

public interface IRouteOwner
{
    public IEndpointConventionBuilder RegisterRoute(IEndpointRouteBuilder routes);
}
