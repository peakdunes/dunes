using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Locations
{
    /// <summary>
    /// Locations Service
    /// Business logic layer scoped by Company (STANDARD COMPANYID)
    /// </summary>
    public interface ILocationsWMSAPIService
    {
        /// <summary>
        /// Get all locations for a company
        /// </summary>
        Task<ApiResponse<List<WMSLocationsReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active locations for a company
        /// </summary>
        Task<ApiResponse<List<WMSLocationsReadDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get location by id validating ownership
        /// </summary>
        Task<ApiResponse<WMSLocationsReadDTO?>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new location
        /// </summary>
        Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSLocationsUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update an existing location
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSLocationsUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a location
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Validate if a location name already exists for a company
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);
    }
}
