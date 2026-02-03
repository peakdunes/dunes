using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Items
{
    /// <summary>
    /// Items Repository
    /// Data access implementation for inventory items.
    /// Scoped strictly by Company (STANDARD COMPANYID).
    /// </summary>
    public class ItemsWMSAPIRepository : IItemsWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Constructor (Dependency Injection)
        /// </summary>
        /// <param name="context">WMS database context</param>
        public ItemsWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all items for a company (CompanyClientId = null)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of items</returns>
        public async Task<List<DUNES.API.ModelsWMS.Masters.Items>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _context.items
                .AsNoTracking()
                .Include(x => x.InventoryCategory)
                .Where(x =>
                    x.companyId == companyId &&
                    x.CompanyClientId == null)
                .OrderBy(x => x.sku)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get all active items for a company (CompanyClientId = null)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active items</returns>
        public async Task<List<DUNES.API.ModelsWMS.Masters.Items>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _context.items
                .AsNoTracking()
                .Include(x => x.InventoryCategory)
                .Where(x =>
                    x.companyId == companyId &&
                    x.CompanyClientId == null &&
                    x.active)
                .OrderBy(x => x.sku)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get item by id (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Item identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Item entity or null</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.Items?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            return await _context.items
                .Include(x => x.InventoryCategory)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.companyId == companyId &&
                    x.CompanyClientId == null,
                    ct);
        }

        /// <summary>
        /// Check if an item exists by SKU (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="sku">SKU value</param>
        /// <param name="excludeId">Optional item id to exclude</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if SKU exists</returns>
        public async Task<bool> ExistsBySkuAsync(
            int companyId,
            string sku,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _context.items
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.CompanyClientId == null &&
                    x.sku == sku);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Check if an item exists by Barcode (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="barcode">Barcode value</param>
        /// <param name="excludeId">Optional item id to exclude</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if barcode exists</returns>
        public async Task<bool> ExistsByBarcodeAsync(
            int companyId,
            string barcode,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _context.items
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.CompanyClientId == null &&
                    x.Barcode == barcode);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        /// <param name="entity">Item entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created item</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.Items> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct)
        {
            _context.items.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="entity">Item entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated item</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.Items> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct)
        {
            _context.items.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Activate or deactivate an item
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Item identifier</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if operation succeeded</returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.items
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.companyId == companyId &&
                    x.CompanyClientId == null,
                    ct);

            if (entity is null)
                return false;

            entity.active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
