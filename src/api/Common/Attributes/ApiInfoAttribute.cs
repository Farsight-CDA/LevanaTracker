namespace LevanaTracker.Api.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApiInfoAttribute : Attribute
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public ApiInfoAttribute(string name)
        : this(name, null)
    {
    }

    public ApiInfoAttribute(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
