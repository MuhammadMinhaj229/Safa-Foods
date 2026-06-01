namespace SafaFoods.Core.Entities;

public sealed class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? ShopifyCustomerId { get; set; }
    public required string FullName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public string? ReferralCode { get; set; } // The code this customer can share
    public Guid? ReferredByCustomerId { get; set; } // Who referred this customer

    public bool MarketingOptIn { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public CustomerWallet? Wallet { get; set; }

    public ICollection<Address> Addresses { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Subscription> Subscriptions { get; set; } = [];
    public ICollection<NotificationLog> Notifications { get; set; } = [];
    public ICollection<CustomerSession> Sessions { get; set; } = [];
    public ICollection<Cart> Carts { get; set; } = [];
    public ICollection<ProductReview> Reviews { get; set; } = [];
}
