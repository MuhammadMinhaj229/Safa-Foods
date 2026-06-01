using Microsoft.EntityFrameworkCore;

using SafaFoods.Core.Entities;
using SafaFoods.Core.Services;
using SafaFoods.Core.Services.Models;
using SafaFoods.Infrastructure.Persistence;

namespace SafaFoods.Infrastructure.Services;

public sealed class WalletService(SafaFoodsDbContext dbContext) : IWalletService
{
    public async Task<decimal> GetBalanceAsync(Guid customerId, CancellationToken ct = default)
    {
        var wallet = await GetOrCreateWalletAsync(customerId, ct);
        return wallet.Balance;
    }

    public async Task<WalletDetails> GetWalletDetailsAsync(Guid customerId, CancellationToken ct = default)
    {
        var wallet = await GetOrCreateWalletAsync(customerId, ct);
        
        var transactions = await dbContext.WalletTransactions
            .Where(t => t.WalletId == wallet.Id)
            .OrderByDescending(t => t.CreatedAt)
            .Take(20)
            .Select(t => new WalletTransactionRecord(t.Id, t.Amount, t.TransactionType, t.Description, t.CreatedAt))
            .ToListAsync(ct);

        return new WalletDetails(wallet.Balance, transactions);
    }

    public async Task<ServiceResult<decimal>> AddFundsAsync(Guid customerId, decimal amount, string transactionType, string? description = null, string? referenceId = null, CancellationToken ct = default)
    {
        if (amount <= 0)
            return ServiceResult<decimal>.Fail("invalid_amount", "Amount to add must be positive.");

        var wallet = await GetOrCreateWalletAsync(customerId, ct);

        wallet.Balance += amount;
        wallet.UpdatedAt = DateTimeOffset.UtcNow;

        dbContext.WalletTransactions.Add(new WalletTransaction
        {
            WalletId = wallet.Id,
            Amount = amount,
            TransactionType = transactionType,
            Description = description,
            ReferenceId = referenceId
        });

        await dbContext.SaveChangesAsync(ct);
        return ServiceResult<decimal>.Ok(wallet.Balance);
    }

    public async Task<ServiceResult<decimal>> DebitAsync(Guid customerId, decimal amount, string? description = null, CancellationToken ct = default)
    {
        if (amount <= 0)
            return ServiceResult<decimal>.Fail("invalid_amount", "Amount to debit must be positive.");

        var wallet = await GetOrCreateWalletAsync(customerId, ct);

        if (wallet.Balance < amount)
            return ServiceResult<decimal>.Fail("insufficient_funds", "Insufficient wallet balance.");

        wallet.Balance -= amount;
        wallet.UpdatedAt = DateTimeOffset.UtcNow;

        dbContext.WalletTransactions.Add(new WalletTransaction
        {
            WalletId = wallet.Id,
            Amount = -amount,
            TransactionType = "Debit",
            Description = description
        });

        await dbContext.SaveChangesAsync(ct);
        return ServiceResult<decimal>.Ok(wallet.Balance);
    }

    private async Task<CustomerWallet> GetOrCreateWalletAsync(Guid customerId, CancellationToken ct)
    {
        var wallet = await dbContext.CustomerWallets.FirstOrDefaultAsync(w => w.CustomerId == customerId, ct);
        if (wallet is null)
        {
            wallet = new CustomerWallet { CustomerId = customerId };
            dbContext.CustomerWallets.Add(wallet);
            await dbContext.SaveChangesAsync(ct);
        }
        return wallet;
    }
}
