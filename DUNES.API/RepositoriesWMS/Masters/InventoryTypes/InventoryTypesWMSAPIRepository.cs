using AutoMapper;
using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
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
    }
}
