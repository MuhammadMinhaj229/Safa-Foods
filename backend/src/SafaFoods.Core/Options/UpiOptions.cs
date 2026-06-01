namespace SafaFoods.Core.Options;

public sealed class UpiOptions
{
    public const string SectionName = "Payments:Upi";

    public string MerchantUpiId { get; set; } = string.Empty;
    public string MerchantName { get; set; } = string.Empty;
    public string GooglePayQrImagePath { get; set; } = string.Empty;
}
