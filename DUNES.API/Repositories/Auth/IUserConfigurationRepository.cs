using DUNES.API.Models.Configuration;
using DUNES.Shared.DTOs.Auth;

namespace DUNES.API.Repositories.Auth
{
    /// <summary>
    /// User Configuration Repository
    /// </summary>
    public interface IUserConfigurationRepository
    {
        /// <summary>
        /// get userconfiguration by table id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserConfiguration?> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// get user configuration by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<UserConfiguration>> GetByUserIdAsync(string userId, CancellationToken ct);

        /// <summary>
        /// get active user configuration by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserConfiguration?> GetActiveByUserIdAsync(string userId, CancellationToken ct);

        /// <summary>
        /// get active user configuration by user (READ DTO with names)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserConfigurationReadDto?> GetActiveReadByUserIdAsync(string userId, CancellationToken ct);

        /// <summary>
        /// get user configuration by user id (READ DTO list with names)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<UserConfigurationReadDto>> GetReadByUserIdAsync(string userId, CancellationToken ct);

        /// <summary>
        /// exist a user configuration ?
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="envName"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsEnvNameAsync(string userId, string envName, int? excludeId, CancellationToken ct);

        /// <summary>
        /// deactive user configuration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<int> DeactivateAllAsync(string userId, CancellationToken ct);

        /// <summary>
        /// active user configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<int> ActivateAsync(int id, string userId, CancellationToken ct);

        /// <summary>
        /// add new user configuration
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<UserConfiguration> CreateAsync(UserConfiguration entity, CancellationToken ct);

        /// <summary>
        /// update user configuration
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task UpdateAsync(UserConfiguration entity, CancellationToken ct);

        /// <summary>
        /// delete user configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id, string userId, CancellationToken ct);

        /// <summary>
        /// save changes
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
