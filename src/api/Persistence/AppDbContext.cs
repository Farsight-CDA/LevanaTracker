using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using LevanaTracker.Api.Models;
using LevanaTracker.Api.Models.Ids;
using LevanaTracker.Api.Persistence.Concurrency;
using System.Reflection;
using System.Transactions;

namespace LevanaTracker.Api.Persistence;

public class AppDbContext : MergingDbContext
{
    public DbSet<User> Users { get; private set; }

    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        _ = configurationBuilder.Properties<UserId>().HaveConversion<UserId.EfCoreValueConverter>();
        base.ConfigureConventions(configurationBuilder);
    }

    public async Task<DbSaveResult> SaveChangesAsync(DbStatus allowedStatuses = DbStatus.Success, bool discardConcurrentDeletedEntries = false,
        IDbContextTransaction? transaction = null, TransactionScope? transactionScope = null, CancellationToken cancellationToken = default)
    {
        EnsureTransactionIsUsed(transaction);
        EnsureNoNestedTransactions(transaction, transactionScope);

        try
        {
            var result = await base.SaveChangesAsync(allowedStatuses, discardConcurrentDeletedEntries, cancellationToken: cancellationToken);

            if(result.Status == DbStatus.Success)
            {
                if(transaction is not null)
                {
                    await transaction.CommitAsync(cancellationToken);
                    transaction = null;
                }

                transactionScope?.Complete();
                transactionScope = null!;
            }

            return result;
        }
        finally
        {
            if(transaction is not null)
            {
                await transaction.DisposeAsync();
            }

            transactionScope?.Dispose();
        }
    }

    private void EnsureTransactionIsUsed(IDbContextTransaction? transaction)
    {
        if(transaction is null)
        {
            return;
        }
        if(transaction != Database.CurrentTransaction)
        {
            throw new InvalidOperationException("This context does not use the given transaction!");
        }
    }
    private static void EnsureNoNestedTransactions(IDbContextTransaction? transaction, TransactionScope? transactionScope)
    {
        if(transaction is null || transactionScope is null)
        {
            return;
        }

        throw new InvalidOperationException("You cannot use both DbContextTransactions and TransactionScopes");
    }
}
