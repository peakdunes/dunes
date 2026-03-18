using DUNES.API.ModelsWMS.Auth;
using DUNES.API.ServicesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Provides user management operations using ASP.NET Identity.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userManager">Identity user manager.</param>
        /// <param name="roleManager">Identity role manager.</param>
        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of users.</returns>
        public async Task<ApiResponse<List<UserReadDTO>>> GetAllAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var users = await _userManager.Users
                .OrderBy(x => x.UserName)
                .ToListAsync(ct);

            var result = new List<UserReadDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserReadDTO
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    IsActive = user.IsActive,
                    MustChangePassword = user.MustChangePassword,
                    RoleName = roles.FirstOrDefault(),
                    CreatedAt = user.CreatedAt
                });
            }

            return ApiResponseFactory.Ok(result, "Users loaded successfully.");
        }

        /// <summary>
        /// Returns a user by identifier.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>User information.</returns>
        public async Task<ApiResponse<UserReadDTO>> GetByIdAsync(string id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(id))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_ID_REQUIRED",
                    message: "User id is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (user == null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new UserReadDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                IsActive = user.IsActive,
                MustChangePassword = user.MustChangePassword,
                RoleName = roles.FirstOrDefault(),
                CreatedAt = user.CreatedAt
            };

            return ApiResponseFactory.Ok(dto, "User loaded successfully.");
        }

        /// <summary>
        /// Creates a new user and assigns the selected role.
        /// </summary>
        /// <param name="dto">Create user DTO.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Created user information.</returns>
        public async Task<ApiResponse<UserReadDTO>> CreateAsync(UserCreateDTO dto, string? currentUserId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (dto == null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "REQUEST_REQUIRED",
                    message: "Request is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USERNAME_REQUIRED",
                    message: "UserName is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "PASSWORD_REQUIRED",
                    message: "Password is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "FULLNAME_REQUIRED",
                    message: "FullName is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "ROLE_REQUIRED",
                    message: "RoleId is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var existingUser = await _userManager.FindByNameAsync(dto.UserName.Trim());
            if (existingUser != null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "DUPLICATE_USERNAME",
                    message: $"There is already a user with the username '{dto.UserName}'.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role == null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "ROLE_NOT_FOUND",
                    message: "Selected role not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var user = new ApplicationUser
            {
                UserName = dto.UserName.Trim(),
                Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
                FullName = dto.FullName.Trim(),
                IsActive = dto.IsActive,
                MustChangePassword = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserId
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(" | ", createResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_CREATE_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(" | ", roleResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_ROLE_ASSIGN_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var result = new UserReadDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                IsActive = user.IsActive,
                MustChangePassword = user.MustChangePassword,
                RoleName = role.Name,
                CreatedAt = user.CreatedAt
            };

            return ApiResponseFactory.Ok(result, "User created successfully.");
        }

        /// <summary>
        /// Updates the basic information of an existing user.
        /// </summary>
        /// <param name="dto">Update user DTO.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Updated user information.</returns>
        public async Task<ApiResponse<UserReadDTO>> UpdateAsync(UserUpdateDTO dto, string? currentUserId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (dto == null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "REQUEST_REQUIRED",
                    message: "Request is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_ID_REQUIRED",
                    message: "User id is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USERNAME_REQUIRED",
                    message: "UserName is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "FULLNAME_REQUIRED",
                    message: "FullName is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "ROLE_REQUIRED",
                    message: "RoleId is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == dto.Id, ct);
            if (user == null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var duplicatedUser = await _userManager.Users
                .FirstOrDefaultAsync(x => x.NormalizedUserName == dto.UserName.Trim().ToUpper() && x.Id != dto.Id, ct);

            if (duplicatedUser != null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "DUPLICATE_USERNAME",
                    message: $"There is already a user with the username '{dto.UserName}'.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role == null)
            {
                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "ROLE_NOT_FOUND",
                    message: "Selected role not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            user.UserName = dto.UserName.Trim();
            user.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
            user.FullName = dto.FullName.Trim();
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUserId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(" | ", updateResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_UPDATE_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    var errors = string.Join(" | ", removeRolesResult.Errors.Select(x => x.Description));

                    return ApiResponseFactory.Fail<UserReadDTO>(
                        error: "USER_ROLE_REMOVE_FAILED",
                        message: errors,
                        statusCode: StatusCodes.Status400BadRequest);
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, role.Name!);
            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join(" | ", addRoleResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<UserReadDTO>(
                    error: "USER_ROLE_ASSIGN_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var result = new UserReadDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                IsActive = user.IsActive,
                MustChangePassword = user.MustChangePassword,
                RoleName = role.Name,
                CreatedAt = user.CreatedAt
            };

            return ApiResponseFactory.Ok(result, "User updated successfully.");
        }

        /// <summary>
        /// Resets the password of an existing user and forces password change on next login.
        /// </summary>
        /// <param name="dto">Reset password DTO.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDTO dto, string? currentUserId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (dto == null)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "REQUEST_REQUIRED",
                    message: "Request is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.UserId))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_ID_REQUIRED",
                    message: "UserId is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "NEW_PASSWORD_REQUIRED",
                    message: "NewPassword is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == dto.UserId, ct);
            if (user == null)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);

            if (!resetResult.Succeeded)
            {
                var errors = string.Join(" | ", resetResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<bool>(
                    error: "PASSWORD_RESET_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            user.MustChangePassword = true;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUserId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(" | ", updateResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<bool>(
                    error: "USER_UPDATE_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return ApiResponseFactory.Ok(true, "Password reset successfully.");
        }

        /// <summary>
        /// Activates or deactivates a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="isActive">New active status.</param>
        /// <param name="currentUserId">Current authenticated user id for audit fields.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(string userId, bool isActive, string? currentUserId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_ID_REQUIRED",
                    message: "User id is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, ct);
            if (user == null)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUserId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<bool>(
                    error: "USER_UPDATE_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return ApiResponseFactory.Ok(
                true,
                isActive ? "User activated successfully." : "User deactivated successfully.");
        }

        /// <summary>
        /// Changes the password of the current authenticated user.
        /// </summary>
        /// <param name="dto">Change password DTO.</param>
        /// <param name="currentUserId">Current authenticated user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> ChangePasswordAsync(ChangePasswordDTO dto, string currentUserId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (dto == null)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "REQUEST_REQUIRED",
                    message: "Request is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_ID_REQUIRED",
                    message: "Current user id is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.CurrentPassword))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "CURRENT_PASSWORD_REQUIRED",
                    message: "Current password is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "NEW_PASSWORD_REQUIRED",
                    message: "New password is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "CONFIRM_PASSWORD_REQUIRED",
                    message: "Confirm password is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            if (!string.Equals(dto.NewPassword, dto.ConfirmPassword, StringComparison.Ordinal))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "PASSWORD_CONFIRMATION_MISMATCH",
                    message: "The new password and confirmation password do not match.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == currentUserId, ct);
            if (user == null)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<bool>(
                    error: "CHANGE_PASSWORD_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            user.MustChangePassword = false;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = currentUserId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(" | ", updateResult.Errors.Select(x => x.Description));

                return ApiResponseFactory.Fail<bool>(
                    error: "USER_UPDATE_FAILED",
                    message: errors,
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return ApiResponseFactory.Ok(true, "Password changed successfully.");
        }

    }
}