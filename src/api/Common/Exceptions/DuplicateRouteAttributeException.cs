namespace MinimalAPITemplate.Api.Common.Exceptions;

public class DuplicateRouteAttributeException : Exception
{
    public DuplicateRouteAttributeException(Type endpointType)
        : base($"Found more than one route attribute (GET / POST / PUT / DELETE) on the endpoint {endpointType.Name}!")
    {
    }
}
