using Common.Services;

namespace MinimalAPITemplate.Api.Common.Attributes.Route;

public abstract class RouteAttribute : IgnoreServiceLevels
{
    public string Route { get; }

    public RouteAttribute(string route)
    {
        Route = route;
    }

    public abstract RouteHandlerBuilder Register(IEndpointRouteBuilder routes, Delegate handler);
}
