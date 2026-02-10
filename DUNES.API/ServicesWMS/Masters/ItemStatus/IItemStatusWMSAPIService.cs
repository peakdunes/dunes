using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.ItemStatus
{
    /// <summary>
    /// Service contract for Item Status (WMS).
    /// All operations are scoped by company (tenant) under STANDARD COMPANYID.
    /// </summary>
    public interface IItemStatusWMSAPIService
    {
        /// <summary>Returns all item statuses for tenant.</summary>
        Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetAllAsync(int companyId, CancellationToken ct);

        /// <summary>Returns active item statuses for tenant.</summary>
        Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetActiveAsync(int companyId, CancellationToken ct);

        /// <summary>Returns an item status by id for tenant.</summary>
        Task<ApiResponse<WMSItemStatusReadDTO>> GetByIdAsync(int companyId, int id, CancellationToken ct);

        /// <summary>Creates an item status for tenant.</summary>
        Task<ApiResponse<bool>> CreateAsync(int companyId, WMSItemStatusCreateDTO dto, CancellationToken ct);

        /// <summary>Updates an item status for tenant.</summary>
        Task<ApiResponse<bool>> UpdateAsync(int companyId, int id, WMSItemStatusUpdateDTO dto, CancellationToken ct);

        /// <summary>Activates/deactivates an item status for tenant.</summary>
        Task<ApiResponse<bool>> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct);
    }
}
