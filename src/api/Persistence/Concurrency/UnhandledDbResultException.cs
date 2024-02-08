namespace LevanaTracker.Api.Persistence.Concurrency;

public class UnhandledDbResultException : Exception
{
    public UnhandledDbResultException(DbSaveResult result)
        : base($"Unhandled DbStatus: ${result.Status}")
    {
    }
}
