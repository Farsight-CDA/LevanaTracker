namespace MinimalAPITemplate.Api.Common.Exceptions;

public class DuplicateAttributeException : Exception
{
    public DuplicateAttributeException(Type endpointType, Type attributeType)
    : base($"Only one attribute of type {attributeType.Name} allowed on the endpoint {endpointType.Name}!")
    {
    }
}
