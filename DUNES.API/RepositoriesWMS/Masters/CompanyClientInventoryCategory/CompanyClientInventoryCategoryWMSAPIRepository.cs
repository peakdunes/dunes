using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Repository implementation for client-level inventory category mappings.
    /// </summary>
    public class CompanyClientInventoryCategoryWMSAPIRepository : ICompanyClientInventoryCategoryWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public CompanyClientInventoryCategoryWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public async Task<List<WMSCompanyClientInventoryCategoryReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .Where(x =>
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId)
                .Select(x => new WMSCompanyClientInventoryCategoryReadDTO
                {
                    Id = x.Id,
                    InventoryCategoryId = x.InventoryCategoryId,
                    InventoryCategoryName = x.InventoryCategoryNavigation.Name,
                    IsActive = x.IsActive
                })
                .OrderBy(x => x.InventoryCategoryName)
                .ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSCompanyClientInventoryCategoryReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .Where(x =>
                    x.Id == id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId)
                .Select(x => new WMSCompanyClientInventoryCategoryReadDTO
                {
                    Id = x.Id,
                    InventoryCategoryId = x.InventoryCategoryId,
                    InventoryCategoryName = x.InventoryCategoryNavigation.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSCompanyClientInventoryCategoryReadDTO> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = new ModelsWMS.Masters.CompanyClientInventoryCategory
            {
                CompaniesContractId = companyClientId,
                InventoryCategoryId = dto.InventoryCategoryId,
                IsActive = dto.IsActive
            };

            _db.CompanyClientInventoryCategories.Add(entity);
            await _db.SaveChangesAsync(ct);

            var categoryName = await _db.Inventorycategories
                .Where(x => x.Id == dto.InventoryCategoryId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(ct) ?? string.Empty;

            return new WMSCompanyClientInventoryCategoryReadDTO
            {
                Id = entity.Id,
                InventoryCategoryId = entity.InventoryCategoryId,
                InventoryCategoryName = categoryName,
                IsActive = entity.IsActive
            };
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(
            WMSCompanyClientInventoryCategoryUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == dto.Id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId,
                    ct);

            if (entity is null)
                return false;

            entity.IsActive = dto.IsActive;

            _db.CompanyClientInventoryCategories.Update(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryCategories
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId,
                    ct);

            if (entity is null)
                return false;

            entity.IsActive = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryCategoryId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .Where(x =>
                    x.CompaniesContractNavigation.CompanyId == companyId &&
                    x.CompaniesContractId == companyClientId &&
                    x.InventoryCategoryId == inventoryCategoryId);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }
    }
}
