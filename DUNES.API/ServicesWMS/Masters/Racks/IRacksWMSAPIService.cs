using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Racks
{
    /// <summary>
    /// Racks Services
    /// Standard COMPANYID compliant
    /// </summary>
    public interface IRacksWMSAPIService
    {
        /// <summary>
        /// Get all racks by company and location
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of racks</returns>
        Task<ApiResponse<List<WMSRacksQueryDTO>>> GetAllAsync(
            int companyId,
            int locationId,
            CancellationToken ct);

        /// <summary>
        /// Get all active racks by company and location
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active racks</returns>
        Task<ApiResponse<List<WMSRacksQueryDTO>>> GetActiveAsync(
            int companyId,
            int locationId,
            CancellationToken ct);

        /// <summary>
        /// Get rack by id, validating company and location ownership
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="id">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Rack information</returns>
        Task<ApiResponse<WMSRacksQueryDTO>> GetByIdAsync(
            int companyId,
            int locationId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new rack for a specific company and location
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="dto">Rack data (without CompanyId / LocationId)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created rack information</returns>
        Task<ApiResponse<WMSRacksCreateDTO>> CreateAsync(
            int companyId,
            int locationId,
            WMSRacksCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update an existing rack, validating company and location ownership
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="id">Rack identifier</param>
        /// <param name="dto">Rack data to update</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated rack information</returns>
        Task<ApiResponse<WMSRacksCreateDTO>> UpdateAsync(
            int companyId,
            int locationId,
            int id,
            WMSRacksCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a rack
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="id">Rack identifier</param>
        /// <param name="isActive">Active flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int locationId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Validate if a rack name already exists for the same company and location
        /// </summary>
        /// <param name="companyId">Company identifier (comes from token)</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="name">Rack name</param>
        /// <param name="excludeId">Optional rack id to exclude from validation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            int locationId,
            string name,
            int? excludeId,
            CancellationToken ct);
    }
}
