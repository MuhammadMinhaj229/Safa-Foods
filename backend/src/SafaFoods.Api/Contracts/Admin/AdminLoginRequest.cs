namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminLoginRequest(
    string Email,
    string Password);
