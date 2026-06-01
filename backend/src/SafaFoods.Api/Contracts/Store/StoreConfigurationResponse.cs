namespace SafaFoods.Api.Contracts.Store;

public sealed record StoreConfigurationResponse(
    string Name,
    string City,
    string ServiceArea,
    string AddressHint,
    decimal Latitude,
    decimal Longitude,
    string? FssaiLicense,
    string? GstNumber,
    string? SupportEmail,
    string? ReturnPolicyUrl,
    string? TermsOfServiceUrl);
