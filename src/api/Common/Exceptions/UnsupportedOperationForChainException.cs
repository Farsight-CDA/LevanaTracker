using LevanaTracker.API.Configuration;

namespace LevanaTracker.API.Common.Exceptions;
public class UnsupportedOperationForChainException : Exception
{
    public UnsupportedOperationForChainException(ChainIdentifier chain) 
        : base($"Operation not supported on chain with id {chain.ChainId}")
    {
    }

    public UnsupportedOperationForChainException(string chainId)
    : base($"Operation not supported on chain with id {chainId}")
    {
    }
}
