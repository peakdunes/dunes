using AutoMapper;
using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.InventoryCategories;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Repository implementation for managing Inventory Categories.
    /// Scoped per company (tenant). All methods enforce multi-tenant security.
    /// </summary>
    public class InventoryCategoriesWMSAPIRepository : IInventoryCategoriesWMSAPIRepository
    {
        private readonly appWmsDbContext _db;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="db">WMS database context</param>
        /// <param name="mapper">AutoMapper instance</param>
        public InventoryCategoriesWMSAPIRepository(appWmsDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<WMSInventorycategoriesReadDTO>> GetAllAsync(int companyId, CancellationToken ct)
        {
            var entities = await _db.Inventorycategories
                .AsNoTracking()
                .Include(x => x.IdcompanyNavigation)
                .Where(x => x.companyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<WMSInventorycategoriesReadDTO>>(entities);
        }

        /// <inheritdoc />
        public async Task<List<WMSInventorycategoriesReadDTO>> GetActiveAsync(int companyId, CancellationToken ct)
        {
            var entities = await _db.Inventorycategories
                .AsNoTracking()
                .Include(x => x.IdcompanyNavigation)
                .Where(x => x.companyId == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<WMSInventorycategoriesReadDTO>>(entities);
        }

        /// <inheritdoc />
        public async Task<WMSInventorycategoriesReadDTO?> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            var entity = await _db.Inventorycategories
                .AsNoTracking()
                .Include(x => x.IdcompanyNavigation)
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId, ct);

            return entity is null ? null : _mapper.Map<WMSInventorycategoriesReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct)
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

        /// <inheritdoc />
        public async Task<WMSInventorycategoriesReadDTO> CreateAsync(WMSInventorycategoriesCreateDTO dto, int companyId, CancellationToken ct)
        {
            var entity = _mapper.Map<Inventorycategories>(dto);
            entity.companyId = companyId;
            entity.Active = true;

            _db.Inventorycategories.Add(entity);
            await _db.SaveChangesAsync(ct);

            await _db.Entry(entity).Reference(x => x.IdcompanyNavigation).LoadAsync(ct);
            return _mapper.Map<WMSInventorycategoriesReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<WMSInventorycategoriesReadDTO> UpdateAsync(WMSInventorycategoriesUpdateDTO dto, int companyId, CancellationToken ct)
        {
            var entity = await _db.Inventorycategories
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.companyId == companyId, ct);

            if (entity is null)
                throw new KeyNotFoundException("Inventory category not found.");

            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync(ct);

            await _db.Entry(entity).Reference(x => x.IdcompanyNavigation).LoadAsync(ct);
            return _mapper.Map<WMSInventorycategoriesReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            var entity = await _db.Inventorycategories
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId, ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
