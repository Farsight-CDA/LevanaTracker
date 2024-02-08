using System.Net;

namespace MinimalAPITemplate.Api.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ResponseCodeAttribute : Attribute
{
    public HttpStatusCode StatusCode { get; set; }
    public Type? ResponseType { get; set; }

    public ResponseCodeAttribute(HttpStatusCode statusCode, Type? responseType = null)
    {
        StatusCode = statusCode;
        ResponseType = responseType;
    }
}
