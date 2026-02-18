using DUNES.API.Data;
using DUNES.API.Models.Configuration;
using DUNES.Shared.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// User configuration repository implementation
    /// </summary>
    public class UserConfigurationRepository : IUserConfigurationRepository
    {
        private readonly IdentityDbContext _ctx;

        /// <summary>
        /// contructor (DI)
        /// </summary>
        /// <param name="ctx"></param>
        public UserConfigurationRepository(IdentityDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// get userconfiguration by table id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UserConfiguration?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _ctx.UserConfiguration
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        /// <summary>
        /// get user configuration by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<UserConfiguration>> GetByUserIdAsync(string userId, CancellationToken ct)
        {
            return await _ctx.UserConfiguration
                .AsNoTracking()
                .Where(x => x.Userid == userId)
                .OrderByDescending(x => x.Isactive)
                .ThenBy(x => x.Enviromentname)
                .ToListAsync(ct);
        }

        /// <summary>
        /// get active user configuration by user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UserConfiguration?> GetActiveByUserIdAsync(string userId, CancellationToken ct)
        {
            return await _ctx.UserConfiguration
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Userid == userId && x.Isactive, ct);
        }

        /// <summary>
        /// get active user configuration by user (READ DTO with names)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UserConfigurationReadDto?> GetActiveReadByUserIdAsync(string userId, CancellationToken ct)
        {
            // NOTE:
            // This DbContext is IdentityDbContext. It can resolve RoleName (AspNetRoles),
            // but CompanyName / CompanyClientName live in the WMS DBContext (not available here).
            // For now, CompanyName / CompanyClientName will be null.


            
            return await (
                from uc in _ctx.UserConfiguration.AsNoTracking()
                join r in _ctx.Roles.AsNoTracking() on uc.Roleid equals r.Id into roles
                from role in roles.DefaultIfEmpty()
                where uc.Userid == userId && uc.Isactive
                select new UserConfigurationReadDto
                {
                    Id = uc.Id,
                    Userid = uc.Userid,
                    Enviromentname = uc.Enviromentname,
                    Isactive = uc.Isactive,
                    Datecreated = uc.Datecreated,

                    Companydefault = uc.Companydefault,
                    CompanyName =  null,

                    Companyclientdefault = uc.Companyclientdefault,
                    CompanyClientName = null,

                    Locationdefault = uc.Locationdefault,
                    LocationName = null,

                    Wmsbin = uc.Wmsbin,
                    WmsBinName = null,

                    Divisiondefault = uc.Divisiondefault,
                    DivisionName = null,

                    Roleid = uc.Roleid,
                    RoleName = role != null ? role.Name : null,

                    Isdepot = uc.Isdepot,
                    Binesdistribution = uc.Binesdistribution,

                    Concepttransferdefault = uc.Concepttransferdefault,
                    Transactiontransferdefault = uc.Transactiontransferdefault,

                    AllowChangeSettings = uc.AllowChangeSettings,
                    Deleteonlymytran = uc.Deleteonlymytran,
                    Processonlymytran = uc.Processonlymytran,
                    companiesContractId = uc.CompaniesContractId
                }
            ).FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// get user configuration by user id (READ DTO list with names)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<UserConfigurationReadDto>> GetReadByUserIdAsync(string userId, CancellationToken ct)
        {
            // NOTE:
            // This DbContext is IdentityDbContext. It can resolve RoleName (AspNetRoles),
            // but CompanyName / CompanyClientName live in the WMS DBContext (not available here).
            // For now, CompanyName / CompanyClientName will be null.
            return await (
                from uc in _ctx.UserConfiguration.AsNoTracking()
                join r in _ctx.Roles.AsNoTracking() on uc.Roleid equals r.Id into roles
                from role in roles.DefaultIfEmpty()
                where uc.Userid == userId
                orderby uc.Isactive descending, uc.Enviromentname
                select new UserConfigurationReadDto
                {
                    Id = uc.Id,
                    Userid = uc.Userid,
                    Enviromentname = uc.Enviromentname,
                    Isactive = uc.Isactive,
                    Datecreated = uc.Datecreated,

                    Companydefault = uc.Companydefault,
                    CompanyName = null,

                    Companyclientdefault = uc.Companyclientdefault,
                    CompanyClientName = null,

                    Locationdefault = uc.Locationdefault,
                    LocationName = null,

                    Wmsbin = uc.Wmsbin,
                    WmsBinName = null,

                    Divisiondefault = uc.Divisiondefault,
                    DivisionName = null,

                    Roleid = uc.Roleid,
                    RoleName = role != null ? role.Name : null,

                    Isdepot = uc.Isdepot,
                    Binesdistribution = uc.Binesdistribution,

                    Concepttransferdefault = uc.Concepttransferdefault,
                    Transactiontransferdefault = uc.Transactiontransferdefault,

                    AllowChangeSettings = uc.AllowChangeSettings,
                    Deleteonlymytran = uc.Deleteonlymytran,
                    Processonlymytran = uc.Processonlymytran
                }
            ).ToListAsync(ct);
        }

        /// <summary>
        /// exist a user configuration ?
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="envName"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> ExistsEnvNameAsync(string userId, string envName, int? excludeId, CancellationToken ct)
        {
            var q = _ctx.UserConfiguration.AsNoTracking()
                .Where(x => x.Userid == userId && x.Enviromentname == envName);

            if (excludeId.HasValue)
                q = q.Where(x => x.Id != excludeId.Value);

            return await q.AnyAsync(ct);
        }

        /// <summary>
        /// deactive user configuration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<int> DeactivateAllAsync(string userId, CancellationToken ct)
        {
            // OJO: Esto solo hace update; la transacción la maneja el Service.
            return await _ctx.UserConfiguration
                .Where(x => x.Userid == userId && x.Isactive)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Isactive, false), ct);
        }

        /// <summary>
        /// active user configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<int> ActivateAsync(int id, string userId, CancellationToken ct)
        {
            // Activa SOLO si pertenece al userId
            return await _ctx.UserConfiguration
                .Where(x => x.Id == id && x.Userid == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Isactive, true), ct);
        }

        /// <summary>
        /// add new user configuration
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<UserConfiguration> CreateAsync(UserConfiguration entity, CancellationToken ct)
        {
            await _ctx.UserConfiguration.AddAsync(entity, ct);
            return entity;
        }

        /// <summary>
        /// update user configuration
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task UpdateAsync(UserConfiguration entity, CancellationToken ct)
        {
            // ct no aplica directamente en Update, pero mantenemos firma estándar.
            _ctx.UserConfiguration.Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// delete user configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id, string userId, CancellationToken ct)
        {
            var entity = await _ctx.UserConfiguration
                .FirstOrDefaultAsync(x => x.Id == id && x.Userid == userId, ct);

            if (entity == null) return false;

            _ctx.UserConfiguration.Remove(entity);
            return true;
        }

        /// <summary>
        /// save changes
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return _ctx.SaveChangesAsync(ct);
        }
    }
}
