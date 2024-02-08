using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPITemplate.Api.Configuration;

public class SecurityOptions : Option
{
    [Required]
    [MinLength(20)]
    public string JWTKey { get; set; } = null!;

    [Required]
    public string JWTIssuer { get; set; } = "ChainMesh";

    [Required]
    [Range(1, Int32.MaxValue)]
    public int JWTExpirationSeconds { get; set; } = 1 * 60 * 60;
}