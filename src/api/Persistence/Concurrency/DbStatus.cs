namespace MinimalAPITemplate.Api.Persistence.Concurrency;
[Flags]
public enum DbStatus : byte
{
    Success = 1,
    DuplicatePrimaryKey = 2,
    DuplicateIndex = 4,
    ConcurrencyEntryDeleted = 8,
}
