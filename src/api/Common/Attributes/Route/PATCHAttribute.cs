namespace MinimalAPITemplate.Api.Common.Attributes.Route;

[AttributeUsage(AttributeTargets.Class)]
public class PATCHAttribute : RouteAttribute
{
    public PATCHAttribute(string route)
    : base(route)
    {
    }

    public override RouteHandlerBuilder Register(IEndpointRouteBuilder routes, Delegate handler) => routes.MapPatch(Route, handler);
}
