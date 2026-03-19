using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Defines user management operations for DUNES.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of users.</returns>
        Task<ApiResponse<List<UserReadDTO>>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Returns a user by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>User information.</returns>
        Task<ApiResponse<UserReadDTO>> GetByIdAsync(string id, CancellationToken ct);

        /// <summary>
        /// Creates a new user and assigns the selected role.
        /// </summary>
        /// <param name="dto">Create user DTO.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Created user information.</returns>
        Task<ApiResponse<UserReadDTO>> CreateAsync(UserCreateDTO dto, string? currentUserId, CancellationToken ct);

        /// <summary>
        /// Updates the basic information of an existing user.
        /// </summary>
        /// <param name="dto">Update user DTO.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Updated user information.</returns>
        Task<ApiResponse<UserReadDTO>> UpdateAsync(UserUpdateDTO dto, string? currentUserId, CancellationToken ct);

        /// <summary>
        /// Resets the password of an existing user and forces password change on next login.
        /// </summary>
        /// <param name="dto">Reset password DTO.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDTO dto, string? currentUserId, CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="isActive">New active status.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> SetActiveAsync(string userId, bool isActive, string? currentUserId, CancellationToken ct);

        /// <summary>
        /// Changes the password of the current authenticated user.
        /// </summary>
        /// <param name="dto">Change password DTO.</param>
        /// <param name="currentUserId">Current authenticated user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO dto, string currentUserId, CancellationToken ct);

        /// <summary>
        /// get roles
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<RoleOptionDTO>>> GetRolesAsync(CancellationToken ct);
    }
}