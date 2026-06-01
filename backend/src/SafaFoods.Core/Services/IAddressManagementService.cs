using SafaFoods.Core.Services.Models;

namespace SafaFoods.Core.Services;

public interface IAddressManagementService
{
    Task<ServiceResult<AddressCreationResult>> CreateAsync(
        AddressCreationCommand command,
        CancellationToken cancellationToken = default);

    Task<ServiceResult<AddressCreationResult>> CreateForCustomerAsync(
        CustomerAddressCreationCommand command,
        CancellationToken cancellationToken = default);
}
