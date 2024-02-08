namespace LevanaTracker.Api.Common.Attributes.Route;

[AttributeUsage(AttributeTargets.Class)]
public class GETAttribute : RouteAttribute
{
    public GETAttribute(string route)
        : base(route)
    {
    }

    public override RouteHandlerBuilder Register(IEndpointRouteBuilder routes, Delegate handler) => routes.MapGet(Route, handler);
}
