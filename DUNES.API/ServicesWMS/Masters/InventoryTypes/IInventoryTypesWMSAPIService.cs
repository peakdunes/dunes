using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.InventoryTypes
{
    /// <summary>
    /// Service contract for Inventory Types (WMS).
    /// All operations are scoped by company (tenant) under STANDARD COMPANYID.
    /// </summary>
    public interface IInventoryTypesWMSAPIService
    {
        /// <summary>Returns all inventory types for tenant.</summary>
        Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetAllAsync(int companyId, CancellationToken ct);

        /// <summary>Returns active inventory types for tenant.</summary>
        Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetActiveAsync(int companyId, CancellationToken ct);

        /// <summary>Returns an inventory type by id for tenant.</summary>
        Task<ApiResponse<WMSInventoryTypesReadDTO>> GetByIdAsync(int companyId, int id, CancellationToken ct);

        /// <summary>Creates an inventory type for tenant.</summary>
        Task<ApiResponse<bool>> CreateAsync(int companyId, WMSInventoryTypesCreateDTO dto, CancellationToken ct);

        /// <summary>Updates an inventory type for tenant.</summary>
        Task<ApiResponse<bool>> UpdateAsync(int companyId, int id, WMSInventoryTypesUpdateDTO dto, CancellationToken ct);

        /// <summary>Activates/deactivates an inventory type for tenant.</summary>
        Task<ApiResponse<bool>> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct);
    }
}

