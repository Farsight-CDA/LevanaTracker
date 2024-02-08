namespace MinimalAPITemplate.Api.Common.Attributes.Route;

[AttributeUsage(AttributeTargets.Class)]
public class PUTAttribute : RouteAttribute
{
    public PUTAttribute(string route)
        : base(route)
    {
    }
    public override RouteHandlerBuilder Register(IEndpointRouteBuilder routes, Delegate handler) => routes.MapPut(Route, handler);
}
