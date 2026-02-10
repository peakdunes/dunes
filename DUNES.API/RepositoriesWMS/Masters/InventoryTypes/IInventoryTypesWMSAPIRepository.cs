using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryTypes
{
    /// <summary>
    /// Repository contract for Inventory Types (WMS).
    /// All methods must enforce tenant scoping via companyId (STANDARD COMPANYID).
    /// </summary>
    public interface IInventoryTypesWMSAPIRepository
    {
        /// <summary>Returns all inventory types for the given tenant.</summary>
        Task<List<WMSInventoryTypesReadDTO>> GetAllAsync(int companyId, CancellationToken ct);

        /// <summary>Returns only active inventory types for the given tenant.</summary>
        Task<List<WMSInventoryTypesReadDTO>> GetActiveAsync(int companyId, CancellationToken ct);

        /// <summary>Returns an inventory type by id for the given tenant.</summary>
        Task<WMSInventoryTypesReadDTO?> GetByIdAsync(int companyId, int id, CancellationToken ct);

        /// <summary>Checks if a name exists within the tenant (optionally excluding a record id).</summary>
        Task<bool> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct);

        /// <summary>Creates a new inventory type for the given tenant.</summary>
        Task<WMSInventoryTypesReadDTO> CreateAsync(WMSInventoryTypesCreateDTO dto, int companyId, CancellationToken ct);

        /// <summary>Updates an inventory type for the given tenant.</summary>
        Task<WMSInventoryTypesReadDTO> UpdateAsync(WMSInventoryTypesUpdateDTO dto, int companyId, CancellationToken ct);

        /// <summary>Activates/deactivates an inventory type for the given tenant.</summary>
        Task<bool> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct);
    }
}
