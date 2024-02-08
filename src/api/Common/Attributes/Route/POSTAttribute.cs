namespace LevanaTracker.Api.Common.Attributes.Route;

[AttributeUsage(AttributeTargets.Class)]
public class POSTAttribute : RouteAttribute
{
    public POSTAttribute(string route)
        : base(route)
    {
    }

    public override RouteHandlerBuilder Register(IEndpointRouteBuilder routes, Delegate handler) => routes.MapPost(Route, handler);
}
