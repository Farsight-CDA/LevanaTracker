using MinimalAPITemplate.Api.Common.Endpoint;
using System.Reflection;

namespace MinimalAPITemplate.Api.Common.Extensions;

public static partial class Extensions
{
    public static Type[] GetHttpEndpointTypes(this Assembly assembly)
        => assembly.GetTypes()
            .Where(x => x.GetInterface(nameof(IRouteOwner)) != null)
            .Where(x => !x.IsInterface && !x.IsAbstract && !x.IsGenericType)
            .ToArray();
}
