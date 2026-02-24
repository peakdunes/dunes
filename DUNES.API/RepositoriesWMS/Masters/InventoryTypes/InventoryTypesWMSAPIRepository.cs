using AutoMapper;
using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryTypes
{
    /// <summary>
    /// Repository implementation for Inventory Types (WMS).
    /// Scoped per company (tenant). All methods enforce STANDARD COMPANYID.
    /// </summary>
    public class InventoryTypesWMSAPIRepository : IInventoryTypesWMSAPIRepository
    {
        private readonly appWmsDbContext _db;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryTypesWMSAPIRepository"/>.
        /// </summary>
        /// <param name="db">WMS database context.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        public InventoryTypesWMSAPIRepository(appWmsDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<WMSInventoryTypesReadDTO>> GetAllAsync(int companyId, CancellationToken ct)
        {
            var entities = await _db.InventoryTypes
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<WMSInventoryTypesReadDTO>>(entities);
        }

        /// <inheritdoc />
        public async Task<List<WMSInventoryTypesReadDTO>> GetActiveAsync(int companyId, CancellationToken ct)
        {
            var entities = await _db.InventoryTypes
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<WMSInventoryTypesReadDTO>>(entities);
        }

        /// <inheritdoc />
        public async Task<WMSInventoryTypesReadDTO?> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            var entity = await _db.InventoryTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Idcompany == companyId, ct);

            return entity is null ? null : _mapper.Map<WMSInventoryTypesReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct)
        {
            name = (name ?? string.Empty).Trim();

            var query = _db.InventoryTypes
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Name != null && x.Name.Trim() == name);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSInventoryTypesReadDTO> CreateAsync(WMSInventoryTypesCreateDTO dto, int companyId, CancellationToken ct)
        {
            var entity = _mapper.Map<DUNES.API.ModelsWMS.Masters.InventoryTypes>(dto);

            // STANDARD COMPANYID
            entity.Idcompany = companyId;

            // Backend standard
            entity.Active = true;

            _db.InventoryTypes.Add(entity);
            await _db.SaveChangesAsync(ct);

            return _mapper.Map<WMSInventoryTypesReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<WMSInventoryTypesReadDTO> UpdateAsync(WMSInventoryTypesUpdateDTO dto, int companyId, CancellationToken ct)
        {
            var entity = await _db.InventoryTypes
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Idcompany == companyId, ct);

            if (entity is null)
                throw new KeyNotFoundException("Inventory type not found.");

            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync(ct);

            return _mapper.Map<WMSInventoryTypesReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            var entity = await _db.InventoryTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.Idcompany == companyId, ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }
        /// <summary>
        /// Deletes an inventory type master record physically.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="id">Inventory type id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> when the record is deleted successfully.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the inventory type does not exist for the company.</exception>
        public async Task<bool> DeleteAsync(int companyId, int id, CancellationToken ct)
        {
            var entity = await _db.InventoryTypes
          .FirstOrDefaultAsync(x => x.Id == id && x.Idcompany == companyId, ct);

            if (entity is null)
                throw new KeyNotFoundException("Inventory type not found.");

            _db.InventoryTypes.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;

        }
        /// <summary>
        /// Checks whether the inventory type has related records that prevent physical deletion.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> HasDependenciesAsync(int companyId, int id, CancellationToken ct)
        {
            var hasClientMappings = await _db.CompanyClientInventoryTypes
        .AsNoTracking()
        .AnyAsync(x => x.CompanyId == companyId && x.InventoryTypeId == id, ct);

            if (hasClientMappings)
                return true;

            var hasInventoryDetailUsage = await _db.Inventorydetail
                .AsNoTracking()
                .AnyAsync(x => x.Idcompany == companyId && x.Idtype == id, ct);

            if (hasInventoryDetailUsage)
                return true;

            var hasInventoryMovementUsage = await _db.Inventorymovement
                .AsNoTracking()
                .AnyAsync(x => x.Idcompany == companyId && x.Idtype == id, ct);

            if (hasInventoryMovementUsage)
                return true;

            var hasInventoryTransactionDetailUsage = await _db.InventorytransactionDetail
                .AsNoTracking()
                .AnyAsync(x => x.Idcompany == companyId && x.Idtype == id, ct);

            return hasInventoryTransactionDetailUsage;
        }
    }
}
