namespace LevanaTracker.Api.Common.Attributes.Route;

[AttributeUsage(AttributeTargets.Class)]
public class DELETEAttribute : RouteAttribute
{
    public DELETEAttribute(string route)
        : base(route)
    {
    }

    public override RouteHandlerBuilder Register(IEndpointRouteBuilder routes, Delegate handler) => routes.MapDelete(Route, handler);
}
