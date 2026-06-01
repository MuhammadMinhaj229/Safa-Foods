namespace SafaFoods.Core.Entities;

public sealed class WalletTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; } // Positive for credit, negative for debit
    public required string TransactionType { get; set; } // "referral_bonus", "order_payment", "refund", "admin_credit"
    public string? ReferenceId { get; set; } // e.g., OrderId or RefundId
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public CustomerWallet Wallet { get; set; } = null!;
}
