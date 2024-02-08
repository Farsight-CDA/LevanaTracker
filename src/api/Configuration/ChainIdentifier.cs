namespace LevanaTracker.API.Configuration;

public class ChainIdentifier
{
    public required string ChainId { get; init; }
    public required string Bech32Prefix { get; init; }

    public required Uri GrpcUrl { get; init; }
    public required string LevanaFactoryAddress { get; init; }
}
