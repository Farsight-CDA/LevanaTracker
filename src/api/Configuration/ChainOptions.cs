using Common.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LevanaTracker.API.Configuration;

public class ChainOptions : Option
{
    public List<ChainIdentifier> Chains { get; set; } = null!;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Chains is null || Chains.Count == 0)
        {
            yield return new ValidationResult("No Chains configured");
        }
    }
}
