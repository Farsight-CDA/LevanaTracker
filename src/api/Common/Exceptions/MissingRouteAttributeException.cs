namespace LevanaTracker.Api.Common.Exceptions;

public class MissingRouteAttributeException : Exception
{
    public MissingRouteAttributeException(Type endpointType)
        : base($"Could not find a route attribute (GET / POST / PUT / DELETE) on the endpoint {endpointType.Name}!")
    {
    }
}
