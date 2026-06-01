using SafaFoods.Core.Enums;

namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminBootstrapRequest(
    string Name,
    string Email,
    string Password,
    AdminRole Role);
