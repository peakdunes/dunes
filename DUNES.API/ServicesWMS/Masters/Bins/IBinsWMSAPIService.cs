using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Bins
{
    /// <summary>
    /// Bins Service
    /// 
    /// Scoped by:
    /// Company (tenant) + Location + Rack
    /// 
    /// IMPORTANT:
    /// - CompanyId is always obtained from the token (passed by controller)
    /// - LocationId and RackId are mandatory to ensure correct hierarchy
    /// - DTOs must NOT be trusted for ownership
    /// </summary>
    public interface IBinsWMSAPIService
    {
        /// <summary>
        /// Get all bins for a specific company, location and rack.
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of bins</returns>
        Task<ApiResponse<List<WMSBinsCreateDto>>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get all active bins for a specific company, location and rack.
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active bins</returns>
        Task<ApiResponse<List<WMSBinsCreateDto>>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get a bin by id validating ownership.
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Bin DTO if found, otherwise NotFound response</returns>
        Task<ApiResponse<WMSBinsCreateDto>> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new bin.
        /// 
        /// IMPORTANT:
        /// - Ownership (CompanyId, LocationId, RackId) is enforced here
        /// - DTO must not contain or override ownership fields
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="dto">Bin DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            int locationId,
            int rackId,
            WMSBinsCreateDto dto,
            CancellationToken ct);

        /// <summary>
        /// Update an existing bin.
        /// 
        /// IMPORTANT:
        /// - Ownership cannot be changed
        /// - Bin must belong to the given company, location and rack
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="dto">Bin DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            WMSBinsCreateDto dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a bin.
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="isActive">Active flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Validate if a bin name already exists within the same company, location and rack.
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="name">Bin name</param>
        /// <param name="excludeId">Optional bin id to exclude (used on update)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            int locationId,
            int rackId,
            string name,
            int? excludeId,
            CancellationToken ct);
    }
}
