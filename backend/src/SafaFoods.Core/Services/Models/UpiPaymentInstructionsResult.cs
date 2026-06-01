namespace SafaFoods.Core.Services.Models;

public sealed record UpiPaymentInstructionsResult(
    string UpiId,
    string MerchantName,
    decimal Amount,
    string TransactionReference,
    string QrImageUrl);
