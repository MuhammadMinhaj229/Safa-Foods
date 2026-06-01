using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public sealed record WalletDetails(decimal Balance, IReadOnlyList<WalletTransactionRecord> RecentTransactions);
public sealed record WalletTransactionRecord(Guid Id, decimal Amount, string TransactionType, string? Description, DateTimeOffset CreatedAt);

public interface IWalletService
{
    Task<decimal> GetBalanceAsync(Guid customerId, CancellationToken ct = default);
    Task<WalletDetails> GetWalletDetailsAsync(Guid customerId, CancellationToken ct = default);
    Task<ServiceResult<decimal>> AddFundsAsync(Guid customerId, decimal amount, string transactionType, string? description = null, string? referenceId = null, CancellationToken ct = default);
    Task<ServiceResult<decimal>> DebitAsync(Guid customerId, decimal amount, string? description = null, CancellationToken ct = default);
}
