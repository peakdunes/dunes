using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service contract for managing permission catalog operations.
    /// </summary>
    public interface IAuthPermissionService
    {
        /// <summary>
        /// Retrieves all permissions from the catalog.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission records.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Retrieves a permission by its identifier.
        /// </summary>
        /// <param name="id">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Permission record if found.</returns>
        Task<ApiResponse<AuthPermissionReadDTO>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="dto">Permission creation data.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission record.</returns>
        Task<ApiResponse<AuthPermissionReadDTO>> CreateAsync(AuthPermissionCreateDTO dto, CancellationToken ct);
    }
}