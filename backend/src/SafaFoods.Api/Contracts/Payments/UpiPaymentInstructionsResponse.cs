namespace SafaFoods.Api.Contracts.Payments;

public sealed record UpiPaymentInstructionsResponse(
    string UpiId,
    string MerchantName,
    decimal Amount,
    string TransactionReference,
    string QrImageUrl);
