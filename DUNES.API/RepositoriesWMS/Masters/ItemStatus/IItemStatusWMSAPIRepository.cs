using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.ItemStatus
{
    /// <summary>
    /// Repository contract for Item Status (WMS).
    /// All methods must enforce tenant scoping via companyId (STANDARD COMPANYID).
    /// </summary>
    public interface IItemStatusWMSAPIRepository
    {
        /// <summary>Returns all item statuses for the given tenant.</summary>
        Task<List<WMSItemStatusReadDTO>> GetAllAsync(int companyId, CancellationToken ct);

        /// <summary>Returns only active item statuses for the given tenant.</summary>
        Task<List<WMSItemStatusReadDTO>> GetActiveAsync(int companyId, CancellationToken ct);

        /// <summary>Returns an item status by id for the given tenant.</summary>
        Task<WMSItemStatusReadDTO?> GetByIdAsync(int companyId, int id, CancellationToken ct);

        /// <summary>Checks if a name exists within the tenant (optionally excluding a record id).</summary>
        Task<bool> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct);

        /// <summary>Creates a new item status for the given tenant.</summary>
        Task<WMSItemStatusReadDTO> CreateAsync(WMSItemStatusCreateDTO dto, int companyId, CancellationToken ct);

        /// <summary>Updates an item status for the given tenant.</summary>
        Task<WMSItemStatusReadDTO> UpdateAsync(WMSItemStatusUpdateDTO dto, int companyId, CancellationToken ct);

        /// <summary>Activates/deactivates an item status for the given tenant.</summary>
        Task<bool> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct);
    }
}
