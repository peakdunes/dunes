using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Items
{
    /// <summary>
    /// Repository implementation for managing Items within WMS.
    /// Supports retrieval of company/master items, client-owned items, or both,
    /// depending on the ownership mode resolved by the service layer.
    /// </summary>
    public class ItemsWMSAPIRepository : IItemsWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public ItemsWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all items available for the given company and client context,
        /// according to the ownership filters provided.
        /// </summary>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of items available under the resolved ownership scope.</returns>
        public async Task<List<WMSItemsReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            CancellationToken ct)
        {
            var query = _context.items
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId);

            query = ApplyOwnershipFilter(query, companyClientId, includeMasterItems, includeClientItems);

            return await query
                .Select(x => new WMSItemsReadDTO
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    CompanyClientId = x.CompanyClientId,
                    InventoryCategoryId = x.InventoryCategoryId,
                    InventoryCategoryName = x.InventoryCategory.Name,
                    PartNumber = x.PartNumber,
                    Sku = x.Sku,
                    ItemDescription = x.ItemDescription,
                    Barcode = x.Barcode,
                    IsRepairable = x.IsRepairable,
                    IsSerialized = x.IsSerialized,
                    Active = x.Active
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Returns a single item by Id within the given company and client context,
        /// according to the ownership filters provided.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested item if found within the allowed scope; otherwise null.</returns>
        public async Task<WMSItemsReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            CancellationToken ct)
        {
            var query = _context.items
                .AsNoTracking()
                .Where(x => x.Id == id && x.CompanyId == companyId);

            query = ApplyOwnershipFilter(query, companyClientId, includeMasterItems, includeClientItems);

            return await query
                .Select(x => new WMSItemsReadDTO
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    CompanyClientId = x.CompanyClientId,
                    InventoryCategoryId = x.InventoryCategoryId,
                    InventoryCategoryName = x.InventoryCategory.Name,
                    PartNumber = x.PartNumber,
                    Sku = x.Sku,
                    ItemDescription = x.ItemDescription,
                    Barcode = x.Barcode,
                    IsRepairable = x.IsRepairable,
                    IsSerialized = x.IsSerialized,
                    Active = x.Active
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Returns the entity instance for a specific item by Id within the given company and client context,
        /// according to the ownership filters provided.
        /// This method is intended for update/delete operations.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The entity if found within the allowed scope; otherwise null.</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.Items?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            CancellationToken ct)
        {
            var query = _context.items
                .Where(x => x.Id == id && x.CompanyId == companyId);

            query = ApplyOwnershipFilter(query, companyClientId, includeMasterItems, includeClientItems);

            return await query.FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Checks whether a Part Number already exists in the Items catalog.
        /// Business rule: Part Number must be unique globally.
        /// </summary>
        /// <param name="partNumber">Part Number to validate.</param>
        /// <param name="excludeId">
        /// Optional item Id to exclude from the validation.
        /// Used during update scenarios.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the Part Number already exists; otherwise false.</returns>
        public async Task<bool> ExistsPartNumberAsync(
            string partNumber,
            int? excludeId,
            CancellationToken ct)
        {
            return await _context.items
                .AnyAsync(x =>
                    x.PartNumber == partNumber &&
                    (!excludeId.HasValue || x.Id != excludeId.Value),
                    ct);
        }

        /// <summary>
        /// Creates a new item record.
        /// </summary>
        /// <param name="entity">Entity to persist.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created entity including generated Id.</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.Items> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct)
        {
            _context.items.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing item record.
        /// </summary>
        /// <param name="entity">Entity with modified values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one database row was affected; otherwise false.</returns>
        public async Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct)
        {
            _context.items.Update(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Deletes an existing item record.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one database row was affected; otherwise false.</returns>
        public async Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct)
        {
            _context.items.Remove(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Updates the active status of an item within the allowed ownership scope.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="isActive">New active status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one database row was affected; otherwise false.</returns>
        public async Task<bool> SetActiveAsync(
            int id,
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            bool isActive,
            CancellationToken ct)
        {
            var query = _context.items
                .Where(x => x.Id == id && x.CompanyId == companyId);

            query = ApplyOwnershipFilter(query, companyClientId, includeMasterItems, includeClientItems);

            var entity = await query.FirstOrDefaultAsync(ct);

            if (entity is null)
                return false;

            entity.Active = isActive;

            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Applies ownership filtering to the provided query according to the current client scope
        /// and ownership mode requested by the service layer.
        /// </summary>
        /// <param name="query">Base query.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <returns>Filtered query according to the ownership mode.</returns>
        private static IQueryable<DUNES.API.ModelsWMS.Masters.Items> ApplyOwnershipFilter(
            IQueryable<DUNES.API.ModelsWMS.Masters.Items> query,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems)
        {
            if (includeMasterItems && includeClientItems)
            {
                return query.Where(x =>
                    x.CompanyClientId == null ||
                    x.CompanyClientId == companyClientId);
            }

            if (includeMasterItems)
            {
                return query.Where(x => x.CompanyClientId == null);
            }

            if (includeClientItems)
            {
                return query.Where(x => x.CompanyClientId == companyClientId);
            }

            return query.Where(x => false);
        }
    }
}