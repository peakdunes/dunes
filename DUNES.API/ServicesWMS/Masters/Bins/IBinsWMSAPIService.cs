using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Bins
{
    /// <summary>
    /// Bins service
    /// Scoped by Company + Location + Rack
    /// </summary>
    public interface IBinsWMSAPIService
    {
        /// <summary>
        /// Get all bins by company, location and rack
        /// </summary>
        Task<ApiResponse<List<WMSBinsDto>>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get all active bins by company, location and rack
        /// </summary>
        Task<ApiResponse<List<WMSBinsDto>>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get bin by id
        /// </summary>
        Task<ApiResponse<WMSBinsDto>> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Add new bin
        /// </summary>
        Task<ApiResponse<bool>> CreateAsync(
            WMSBinsDto entity,
            CancellationToken ct);

        /// <summary>
        /// Update bin
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            WMSBinsDto entity,
            CancellationToken ct);

        /// <summary>
        /// Activate / Deactivate bin
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Validate if a bin exists with the same name (scoped)
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            int locationId,
            int rackId,
            string name,
            int? excludeId,
            CancellationToken ct);
    }
}
