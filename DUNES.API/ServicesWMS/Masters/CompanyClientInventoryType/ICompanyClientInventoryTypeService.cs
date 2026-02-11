using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// Service interface for managing inventory types per client (contract).
    /// Scoped by CompanyId and CompanyClientId.
    /// </summary>
    public interface ICompanyClientInventoryTypeService
    {
        /// <summary>
        /// Retrieve all inventory types assigned to this client.
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Retrieve a specific inventory type mapping by Id.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new mapping between the client and an inventory type.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryTypeCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update the active flag for a specific mapping.
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a client-inventory type mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}
