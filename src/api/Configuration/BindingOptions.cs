using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPITemplate.Api.Configuration;

public class BindingOptions : Option
{
    [Required]
    public Uri BindAddress { get; set; } = null!;
    [Range(1000, UInt16.MaxValue)]
    public ushort ApplicationPort { get; set; }
}
