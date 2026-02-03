using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories Repository
    /// 
    /// Scoped by:
    /// Company (tenant)
    /// 
    /// IMPORTANT:
    /// This repository is the last line of defense for multi-tenant security.
    /// All operations MUST be filtered by CompanyId.
    /// </summary>
    public class InventoryCategoriesWMSAPIRepository : IInventoryCategoriesWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public InventoryCategoriesWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all inventory categories for a company.
        /// </summary>
        public async Task<List<Inventorycategories>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _db.Inventorycategories
                .AsNoTracking()
                .Where(x => x.companyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get all active inventory categories for a company.
        /// </summary>
        public async Task<List<Inventorycategories>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _db.Inventorycategories
                .AsNoTracking()
                .Where(x => x.companyId == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get inventory category by id validating ownership.
        /// </summary>
        public async Task<Inventorycategories?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            return await _db.Inventorycategories
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id && x.companyId == companyId,
                    ct);
        }

        /// <summary>
        /// Check if an inventory category name already exists for a company.
        /// </summary>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.Inventorycategories
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.Name != null &&
                    x.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Create a new inventory category.
        /// 
        /// IMPORTANT:
        /// - Entity must already contain CompanyId
        /// - Repository must NOT infer ownership
        /// </summary>
        public async Task<Inventorycategories> CreateAsync(
            Inventorycategories entity,
            CancellationToken ct)
        {
            _db.Inventorycategories.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Update an existing inventory category.
        /// 
        /// IMPORTANT:
        /// - Ownership (CompanyId) must NOT be changed here
        /// </summary>
        public async Task<Inventorycategories> UpdateAsync(
            Inventorycategories entity,
            CancellationToken ct)
        {
            _db.Inventorycategories.Update(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Activate or deactivate an inventory category.
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.Inventorycategories
                .FirstOrDefaultAsync(
                    x => x.Id == id && x.companyId == companyId,
                    ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
