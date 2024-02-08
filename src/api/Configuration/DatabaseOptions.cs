using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LevanaTracker.Api.Configuration;

public class DatabaseOptions : Option
{
    [Required]
    public required string ConnectionString { get; set; }
}
