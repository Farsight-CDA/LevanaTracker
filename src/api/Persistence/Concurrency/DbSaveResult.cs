namespace LevanaTracker.Api.Persistence.Concurrency;

public class DbSaveResult
{
    private const string _indexNamePrefix = "IX_";
    private const string _indexNamePropertySeparator = "_";

    public DbStatus Status { get; }
    public int AffectedRows { get; }

    public string? ConstraintName { get; }

    internal static DbSaveResult FromSuccess(int affectedRows)
        => new DbSaveResult(DbStatus.Success, affectedRows, null);

    internal static DbSaveResult FromError(DbStatus status, string? constraintName)
        => new DbSaveResult(status, 0, constraintName);

    private DbSaveResult(DbStatus status, int affectedRows, string? constraintName)
    {
        Status = status;
        AffectedRows = affectedRows;
        ConstraintName = constraintName;
    }

    public bool IsConflictingIndex(Type entitiyType, params string[] propertyNames)
    {
        if(String.IsNullOrWhiteSpace(ConstraintName))
        {
            return false;
        }

        var constraint = ConstraintName.AsSpan();
        if(!constraint.StartsWith(_indexNamePrefix))
        {
            return false;
        }

        constraint = constraint[_indexNamePrefix.Length..];

        if(!constraint.StartsWith(entitiyType.Name, StringComparison.Ordinal))
        {
            return false;
        }

        constraint = constraint[entitiyType.Name.Length..];

        if(!constraint.StartsWith("s"))
        {
            return false;
        }

        constraint = constraint[1..];

        foreach(string propertyName in propertyNames)
        {
            if(!constraint.StartsWith(_indexNamePropertySeparator))
            {
                return false;
            }

            constraint = constraint[_indexNamePropertySeparator.Length..];

            if(!constraint.StartsWith(propertyName, StringComparison.Ordinal))
            {
                return false;
            }

            constraint = constraint[propertyName.Length..];
        }

        return constraint.Length == 0;
    }
}
