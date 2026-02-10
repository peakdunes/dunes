using AutoMapper;
using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.ItemStatus
{
    /// <summary>
    /// Repository implementation for Item Status (WMS).
    /// Scoped per company (tenant). All methods enforce STANDARD COMPANYID.
    /// </summary>
    public class ItemStatusWMSAPIRepository : IItemStatusWMSAPIRepository
    {
        private readonly appWmsDbContext _db;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="ItemStatusWMSAPIRepository"/>.
        /// </summary>
        /// <param name="db">WMS database context.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        public ItemStatusWMSAPIRepository(appWmsDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<List<WMSItemStatusReadDTO>> GetAllAsync(int companyId, CancellationToken ct)
        {
            var entities = await _db.Itemstatus
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<WMSItemStatusReadDTO>>(entities);
        }

        /// <inheritdoc />
        public async Task<List<WMSItemStatusReadDTO>> GetActiveAsync(int companyId, CancellationToken ct)
        {
            var entities = await _db.Itemstatus
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

            return _mapper.Map<List<WMSItemStatusReadDTO>>(entities);
        }

        /// <inheritdoc />
        public async Task<WMSItemStatusReadDTO?> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            var entity = await _db.Itemstatus
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.Idcompany == companyId, ct);

            return entity is null ? null : _mapper.Map<WMSItemStatusReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct)
        {
            name = (name ?? string.Empty).Trim();

            var query = _db.Itemstatus
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Name != null && x.Name.Trim() == name);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSItemStatusReadDTO> CreateAsync(WMSItemStatusCreateDTO dto, int companyId, CancellationToken ct)
        {
            var entity = _mapper.Map<Itemstatus>(dto);

            // STANDARD COMPANYID
            entity.Idcompany = companyId;

            // Backend standard
            entity.Active = true;

            _db.Itemstatus.Add(entity);
            await _db.SaveChangesAsync(ct);

            return _mapper.Map<WMSItemStatusReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<WMSItemStatusReadDTO> UpdateAsync(WMSItemStatusUpdateDTO dto, int companyId, CancellationToken ct)
        {
            var entity = await _db.Itemstatus
                .FirstOrDefaultAsync(x => x.Id == dto.Id && x.Idcompany == companyId, ct);

            if (entity is null)
                throw new KeyNotFoundException("Item status not found.");

            _mapper.Map(dto, entity);
            await _db.SaveChangesAsync(ct);

            return _mapper.Map<WMSItemStatusReadDTO>(entity);
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            var entity = await _db.Itemstatus
                .FirstOrDefaultAsync(x => x.Id == id && x.Idcompany == companyId, ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
