using DUNES.API.Data;
using DUNES.API.Models.Configuration;
using DUNES.API.Repositories.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// user configuration service implementation
    /// </summary>
    public class UserConfigurationService : IUserConfigurationService
    {
        private readonly IUserConfigurationRepository _repo;
        private readonly IdentityDbContext _db;
        private readonly IValidator<UserConfigurationUpdateDto> _validator;

        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="db"></param>
        /// <param name="validator"></param>
        public UserConfigurationService(
            IUserConfigurationRepository repo,
            IdentityDbContext db,
            IValidator<UserConfigurationUpdateDto> validator)
        {
            _repo = repo;
            _db = db;
            _validator = validator;
        }

        /// <summary>
        /// Get active user configuration (Read DTO)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UserConfigurationReadDto?>> GetActiveAsync(string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto?>("UserId is required.");

            var active = await _repo.GetActiveReadByUserIdAsync(userId, ct);
            return ApiResponseFactory.Ok(active, "Active configuration loaded.");
        }

        /// <summary>
        /// Get configurations by user (Read DTO list)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<UserConfigurationReadDto>>> GetByUserAsync(string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<List<UserConfigurationReadDto>>("UserId is required.");

            var list = await _repo.GetReadByUserIdAsync(userId, ct);
            return ApiResponseFactory.Ok(list, "User configurations loaded.");
        }

        /// <summary>
        /// Get configuration by id (Read DTO)
        /// (Validates ownership by userId)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UserConfigurationReadDto?>> GetByIdAsync(int id, string userId, CancellationToken ct)
        {
            if (id <= 0)
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto?>("Id is required.");

            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto?>("UserId is required.");

            var entity = await _repo.GetByIdAsync(id, ct);
            if (entity == null)
                return ApiResponseFactory.NotFound<UserConfigurationReadDto?>("Configuration not found.");

            if (!string.Equals(entity.Userid, userId, StringComparison.OrdinalIgnoreCase))
                return ApiResponseFactory.Forbidden<UserConfigurationReadDto?>("You cannot access a configuration that does not belong to you.");

            // NOTE: At repository level, we currently resolve RoleName only.
            // CompanyName / CompanyClientName will be null until we join with WMS context.
            var read = await _repo.GetReadByUserIdAsync(userId, ct);
            var dto = read.FirstOrDefault(x => x.Id == id);

            // fallback (should not be null if list is correct, but keep safe)
            if (dto == null)
            {
                dto = new UserConfigurationReadDto
                {
                    Id = entity.Id,
                    Userid = entity.Userid,
                    Enviromentname = entity.Enviromentname,
                    Isactive = entity.Isactive,
                    Datecreated = entity.Datecreated,
                    Companydefault = entity.Companydefault,
                    Companyclientdefault = entity.Companyclientdefault,
                    Locationdefault = entity.Locationdefault,
                    Wmsbin = entity.Wmsbin,
                    Divisiondefault = entity.Divisiondefault,
                    Roleid = entity.Roleid,
                    Isdepot = entity.Isdepot,
                    Binesdistribution = entity.Binesdistribution,
                    Concepttransferdefault = entity.Concepttransferdefault,
                    Transactiontransferdefault = entity.Transactiontransferdefault,
                    AllowChangeSettings = entity.AllowChangeSettings,
                    Deleteonlymytran = entity.Deleteonlymytran,
                    Processonlymytran = entity.Processonlymytran
                };
            }

            return ApiResponseFactory.Ok(dto, "Configuration loaded.");
        }

        /// <summary>
        /// Create new user configuration
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId">
        /// The logged-in user id (recommended to set ownership server-side).
        /// If you want admin to create for another user, extend signature later.
        /// </param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<UserConfigurationReadDto>> CreateAsync(UserConfigurationCreateDto dto, string userId, CancellationToken ct)
        {
            if (dto == null)
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto>("Payload is required.");

            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto>("UserId is required.");

            // IMPORTANT:
            // We don't trust Userid from UI. Ownership is server-side.
            // Still, validator allows it (length) but service enforces userId.
            var upsert = ToUpsert(dto, userId);

            // Shape validation (Create ruleset)
            var vr = await _validator.ValidateAsync(upsert, o => o.IncludeRuleSets("Create"), ct);
            if (!vr.IsValid)
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto>(string.Join(" | ", vr.Errors.Select(e => e.ErrorMessage)));

            // Business validation: EnvName unique per user
            var existsEnv = await _repo.ExistsEnvNameAsync(userId, upsert.Enviromentname!, excludeId: null, ct);
            if (existsEnv)
                return ApiResponseFactory.BadRequest<UserConfigurationReadDto>("EnvironmentName already exists for this user.");

            // Transaction: if new config is active => deactivate others first
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (upsert.Isactive)
                await _repo.DeactivateAllAsync(userId, ct);

            var entity = new UserConfiguration
            {
                Userid = userId,
                Enviromentname = upsert.Enviromentname,
                Companydefault = upsert.Companydefault,
                Companyclientdefault = upsert.Companyclientdefault,
                Locationdefault = upsert.Locationdefault,
                Bindcr1default = upsert.Bindcr1default,
                Wmsbin = upsert.Wmsbin,
                Divisiondefault = upsert.Divisiondefault,
                Isactive = upsert.Isactive,
                Binesdistribution = upsert.Binesdistribution,
                Concepttransferdefault = upsert.Concepttransferdefault,
                Transactiontransferdefault = upsert.Transactiontransferdefault,
                AllowChangeSettings = upsert.AllowChangeSettings,
                Deleteonlymytran = upsert.Deleteonlymytran,
                Datecreated = DateTime.UtcNow,
                Processonlymytran = upsert.Processonlymytran,
                Roleid = upsert.Roleid,
                Isdepot = upsert.Isdepot
            };

            await _repo.CreateAsync(entity, ct);
            await _repo.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            // Return Read DTO (with names if available)
            // NOTE: RoleName will be resolved, Company names null until WMS join is implemented.
            var createdDto = (await _repo.GetReadByUserIdAsync(userId, ct)).FirstOrDefault(x => x.Id == entity.Id)
                            ?? new UserConfigurationReadDto
                            {
                                Id = entity.Id,
                                Userid = entity.Userid,
                                Enviromentname = entity.Enviromentname,
                                Isactive = entity.Isactive,
                                Datecreated = entity.Datecreated,
                                Companydefault = entity.Companydefault,
                                Companyclientdefault = entity.Companyclientdefault,
                                Locationdefault = entity.Locationdefault,
                                Wmsbin = entity.Wmsbin,
                                Divisiondefault = entity.Divisiondefault,
                                Roleid = entity.Roleid,
                                Isdepot = entity.Isdepot,
                                Binesdistribution = entity.Binesdistribution,
                                Concepttransferdefault = entity.Concepttransferdefault,
                                Transactiontransferdefault = entity.Transactiontransferdefault,
                                AllowChangeSettings = entity.AllowChangeSettings,
                                Deleteonlymytran = entity.Deleteonlymytran,
                                Processonlymytran = entity.Processonlymytran
                            };

            return ApiResponseFactory.Ok(createdDto, "Configuration created.");
        }

        /// <summary>
        /// Update user configuration
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> UpdateAsync(UserConfigurationUpdateDto dto, string userId, CancellationToken ct)
        {
            if (dto == null)
                return ApiResponseFactory.BadRequest<bool>("Payload is required.");

            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<bool>("UserId is required.");

            // Shape validation (Update ruleset)
            var vr = await _validator.ValidateAsync(dto, o => o.IncludeRuleSets("Update"), ct);
            if (!vr.IsValid)
                return ApiResponseFactory.BadRequest<bool>(string.Join(" | ", vr.Errors.Select(e => e.ErrorMessage)));

            var id = dto.Id!.Value;

            // Load current (tracked) for update
            var current = await _db.UserConfiguration.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (current == null)
                return ApiResponseFactory.NotFound<bool>("Configuration not found.");

            // Ownership validation
            if (!string.Equals(current.Userid, userId, StringComparison.OrdinalIgnoreCase))
                return ApiResponseFactory.Forbidden<bool>("You cannot modify a configuration that does not belong to you.");

            // Business validation: EnvName unique per user (exclude current id)
            var envName = dto.Enviromentname ?? string.Empty;
            if (string.IsNullOrWhiteSpace(envName))
                return ApiResponseFactory.BadRequest<bool>("Enviromentname is required.");

            var existsEnv = await _repo.ExistsEnvNameAsync(userId, envName, excludeId: id, ct);
            if (existsEnv)
                return ApiResponseFactory.BadRequest<bool>("EnvironmentName already exists for this user.");

            // Transaction: if set active => deactivate others first
            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            if (dto.Isactive)
                await _repo.DeactivateAllAsync(userId, ct);

            // Copy fields (Userid is NOT updated)
            current.Enviromentname = envName;
            current.Companydefault = dto.Companydefault;
            current.Companyclientdefault = dto.Companyclientdefault;
            current.Locationdefault = dto.Locationdefault;
            current.Bindcr1default = dto.Bindcr1default;
            current.Wmsbin = dto.Wmsbin;
            current.Divisiondefault = dto.Divisiondefault;
            current.Isactive = dto.Isactive;
            current.Binesdistribution = dto.Binesdistribution;
            current.Concepttransferdefault = dto.Concepttransferdefault;
            current.Transactiontransferdefault = dto.Transactiontransferdefault;
            current.AllowChangeSettings = dto.AllowChangeSettings;
            current.Deleteonlymytran = dto.Deleteonlymytran;
            current.Processonlymytran = dto.Processonlymytran;
            current.Roleid = dto.Roleid;
            current.Isdepot = dto.Isdepot;

            await _repo.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return ApiResponseFactory.Ok(true, "Configuration updated.");
        }

        /// <summary>
        /// Delete user configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteAsync(int id, string userId, CancellationToken ct)
        {
            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Id is required.");

            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<bool>("UserId is required.");

            var deleted = await _repo.DeleteAsync(id, userId, ct);
            if (!deleted)
                return ApiResponseFactory.NotFound<bool>("Configuration not found or does not belong to user.");

            await _repo.SaveChangesAsync(ct);

            return ApiResponseFactory.Ok(true, "Configuration deleted.");
        }

        /// <summary>
        /// Activate a user configuration (deactivates any other active config for that user)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(int id, string userId, CancellationToken ct)
        {
            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Id is required.");

            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<bool>("UserId is required.");

            var target = await _repo.GetByIdAsync(id, ct);
            if (target == null)
                return ApiResponseFactory.NotFound<bool>("Configuration not found.");

            if (!string.Equals(target.Userid, userId, StringComparison.OrdinalIgnoreCase))
                return ApiResponseFactory.Forbidden<bool>("You cannot activate a configuration that does not belong to you.");

            await using var tx = await _db.Database.BeginTransactionAsync(ct);

            await _repo.DeactivateAllAsync(userId, ct);

            var affected = await _repo.ActivateAsync(id, userId, ct);
            if (affected != 1)
            {
                await tx.RollbackAsync(ct);
                return ApiResponseFactory.BadRequest<bool>("Unable to activate configuration.");
            }

            await tx.CommitAsync(ct);
            return ApiResponseFactory.Ok(true, "Active configuration updated.");
        }

        /// <summary>
        /// Exists environment name for user (optional endpoint to support UI validations)
        /// </summary>
        /// <param name="envName"></param>
        /// <param name="excludeId"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> ExistsEnvNameAsync(string envName, int? excludeId, string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseFactory.BadRequest<bool>("UserId is required.");

            if (string.IsNullOrWhiteSpace(envName))
                return ApiResponseFactory.BadRequest<bool>("EnvName is required.");

            var exists = await _repo.ExistsEnvNameAsync(userId, envName, excludeId, ct);
            return ApiResponseFactory.Ok(exists, "Exists check completed.");
        }

        /// <summary>
        /// helper: map CreateDto to UpdateDto (for validator ruleset Create)
        /// </summary>
        private static UserConfigurationUpdateDto ToUpsert(UserConfigurationCreateDto dto, string userId)
        {
            return new UserConfigurationUpdateDto
            {
                Id = null,
                Userid = userId, // enforce ownership server-side
                Enviromentname = dto.Enviromentname,
                Companydefault = dto.Companydefault,
                Companyclientdefault = dto.Companyclientdefault,
                Locationdefault = dto.Locationdefault,
                Bindcr1default = dto.Bindcr1default,
                Wmsbin = dto.Wmsbin,
                Divisiondefault = dto.Divisiondefault,
                Isactive = dto.Isactive,
                Binesdistribution = dto.Binesdistribution,
                Concepttransferdefault = dto.Concepttransferdefault,
                Transactiontransferdefault = dto.Transactiontransferdefault,
                AllowChangeSettings = dto.AllowChangeSettings,
                Deleteonlymytran = dto.Deleteonlymytran,
                Processonlymytran = dto.Processonlymytran,
                Roleid = dto.Roleid,
                Isdepot = dto.Isdepot
            };
        }
    }
}
