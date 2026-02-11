using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Repository implementation for managing inventory type mappings by client.
    /// Enforces scoping by CompanyId and CompanyClientId.
    /// </summary>
    public class CompanyClientInventoryTypeWMSAPIRepository : ICompanyClientInventoryTypeWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        public CompanyClientInventoryTypeWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<List<WMSCompanyClientInventoryTypeReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(x =>
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractNavigation.CompanyClientId == companyClientId)
                .OrderBy(x => x.InventoryTypeNavigation.Name)
                .Select(x => new WMSCompanyClientInventoryTypeReadDTO
                {
                    Id = x.Id,
                    InventoryTypeId = x.InventoryTypeId,
                    InventoryTypeName = x.InventoryTypeNavigation.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientInventoryTypeReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(x =>
                    x.Id == id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractNavigation.CompanyClientId == companyClientId)
                .Select(x => new WMSCompanyClientInventoryTypeReadDTO
                {
                    Id = x.Id,
                    InventoryTypeId = x.InventoryTypeId,
                    InventoryTypeName = x.InventoryTypeNavigation.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc/>
        public async Task<WMSCompanyClientInventoryTypeReadDTO> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var contractId = await _db.CompaniesContract
                .Where(x => x.CompanyId == companyId && x.CompanyClientId == companyClientId)
                .Select(x => x.Id)
                .FirstAsync(ct);

            var entity = new ModelsWMS.Masters.CompanyClientInventoryType
            {
                CompaniesContractId = contractId,
                InventoryTypeId = dto.InventoryTypeId,
                IsActive = dto.IsActive
            };

            _db.CompanyClientInventoryTypes.Add(entity);
            await _db.SaveChangesAsync(ct);

            var name = await _db.InventoryTypes
                .Where(x => x.Id == dto.InventoryTypeId)
                .Select(x => x.Name)
                .FirstAsync(ct);

            return new WMSCompanyClientInventoryTypeReadDTO
            {
                Id = entity.Id,
                InventoryTypeId = entity.InventoryTypeId,
                InventoryTypeName = name,
                IsActive = entity.IsActive
            };
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryTypes
                .Include(x => x.CompaniesContractNavigation)
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractNavigation.CompanyClientId == companyClientId,
                    ct);

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
            var entity = await _db.CompanyClientInventoryTypes
                .Include(x => x.CompaniesContractNavigation)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractNavigation.CompanyClientId == companyClientId,
                    ct);

            if (entity is null)
                return false;

            entity.IsActive = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryTypeId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientInventoryTypes
                .Include(x => x.CompaniesContractNavigation)
                .Where(x =>
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractNavigation.CompanyClientId == companyClientId &&
                    x.InventoryTypeId == inventoryTypeId);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }
    }
}
