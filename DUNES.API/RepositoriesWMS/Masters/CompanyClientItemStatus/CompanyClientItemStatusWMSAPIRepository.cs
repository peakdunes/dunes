using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository implementation for item status mappings per client.
    /// All access is scoped by CompanyId and CompanyClientId.
    /// </summary>
    public class CompanyClientItemStatusWMSAPIRepository : ICompanyClientItemStatusWMSAPIRepository
    {
        private readonly appWmsDbContext _db;


        /// <summary>
        /// constructor(DI)
        /// </summary>
        /// <param name="db"></param>
        public CompanyClientItemStatusWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<List<WMSCompanyClientItemStatusReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(x =>
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId)
                .Select(x => new WMSCompanyClientItemStatusReadDTO
                {
                    Id = x.Id,
                    CompaniesContractId = x.CompaniesContractId,
                    ItemStatusId = x.ItemStatusId,
                    ItemStatusName = x.ItemStatusNavigation.Name,
                    IsActive = x.IsActive
                })
                .OrderBy(x => x.ItemStatusName)
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(x =>
                    x.Id == id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId)
                .Select(x => new WMSCompanyClientItemStatusReadDTO
                {
                    Id = x.Id,
                    CompaniesContractId = x.CompaniesContractId,
                    ItemStatusId = x.ItemStatusId,
                    ItemStatusName = x.ItemStatusNavigation.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(x =>
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId &&
                    x.ItemStatusId == itemStatusId);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientItemStatusReadDTO> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = new ModelsWMS.Masters.CompanyClientItemStatus
            {
                CompaniesContractId = companyClientId,
                ItemStatusId = dto.ItemStatusId,
                IsActive = true
            };

            _db.CompanyClientItemStatuses.Add(entity);
            await _db.SaveChangesAsync(ct);

            var itemName = await _db.Itemstatus
                .Where(x => x.Id == dto.ItemStatusId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(ct) ?? string.Empty;

            return new WMSCompanyClientItemStatusReadDTO
            {
                Id = entity.Id,
                CompaniesContractId = entity.CompaniesContractId,
                ItemStatusId = entity.ItemStatusId,
                ItemStatusName = itemName,
                IsActive = entity.IsActive
            };
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(
            WMSCompanyClientItemStatusUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientItemStatuses
                .Where(x =>
                    x.Id == dto.Id &&
                    x.CompaniesContractId == companyClientId &&
                    x.CompaniesContractNavigation.CompanyId == companyId)
                .FirstOrDefaultAsync(ct);

            if (entity is null)
                return false;

            entity.IsActive = dto.IsActive;
            await _db.SaveChangesAsync(ct);

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientItemStatuses
                .FirstOrDefaultAsync(
                    x => x.Id == id &&
                         x.CompaniesContractId == companyClientId &&
                         x.CompaniesContractNavigation.CompanyId == companyId,
                    ct);

            if (entity is null)
                return false;

            entity.IsActive = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
