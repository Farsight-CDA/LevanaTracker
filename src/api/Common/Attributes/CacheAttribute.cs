namespace MinimalAPITemplate.Api.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CacheAttribute : Attribute
{
    public TimeSpan Expiration { get; }

    public CacheAttribute(int cacheSeconds)
    {
        Expiration = TimeSpan.FromSeconds(cacheSeconds);
    }
}
