using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;
using System.Xml.Linq;

namespace DUNES.UI.Services.WMS.Masters.InventoryCategories
{
    /// <summary>
    /// UI Service for Inventory Categories (WMS / Masters).
    ///
    /// This service calls the WMS Inventory Categories API endpoints.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER sent from UI.
    /// - The API obtains CompanyId from the authenticated token.
    /// </summary>
    public class InventoryCategoriesWMSUIService : UIApiServiceBase, IInventoryCategoriesWMSUIService
    {
        /// <summary>
        /// Base API route for Inventory Categories controller.
        /// </summary>
        private const string BasePath = "/api/wms/masters/inventory-categories";

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryCategoriesWMSUIService"/>.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public InventoryCategoriesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Retrieves all inventory categories for the current tenant.
        /// </summary>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse with a list of <see cref="WMSInventorycategoriesReadDTO"/> (can be empty).
        /// </returns>
        public Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetAllAsync(string token, CancellationToken ct)
            => GetApiAsync<List<WMSInventorycategoriesReadDTO>>(
                $"{BasePath}/GetAll",
                token,
                ct);

        /// <summary>
        /// Retrieves a single inventory category by identifier for the current tenant.
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse with <see cref="WMSInventorycategoriesReadDTO"/> if found; otherwise NotFound.
        /// </returns>
        public Task<ApiResponse<WMSInventorycategoriesReadDTO>> GetByIdAsync(int id, string token, CancellationToken ct)
            => GetApiAsync<WMSInventorycategoriesReadDTO>(
                $"{BasePath}/GetById/{id}",
                token,
                ct);

        /// <summary>
        /// Creates a new inventory category for the current tenant.
        /// </summary>
        /// <param name="entity">Create DTO with required category information.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating success, validation errors, or duplicate name errors (409).
        /// </returns>
        public Task<ApiResponse<bool>> CreateAsync(WMSInventorycategoriesCreateDTO entity, string token, CancellationToken ct)
            => PostApiAsync<bool, WMSInventorycategoriesCreateDTO>(
                $"{BasePath}/Create",
                entity,
                token,
                ct);

        /// <summary>
        /// Updates an existing inventory category for the current tenant.
        /// Route ID is authoritative (prevents tampering).
        /// </summary>
        /// <param name="id">Inventory category identifier (route).</param>
        /// <param name="entity">Update DTO containing the updated values.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating success, validation errors, not found (404), or duplicate name (409).
        /// </returns>
        public Task<ApiResponse<bool>> UpdateAsync(int id, WMSInventorycategoriesUpdateDTO entity, string token, CancellationToken ct)
            => PutApiAsync<bool, WMSInventorycategoriesUpdateDTO>(
                $"{BasePath}/Update/{id}",
                entity,
                token,
                ct);

        /// <summary>
        /// Activates or deactivates an inventory category for the current tenant.
        /// </summary>
        /// <param name="id">Inventory category identifier.</param>
        /// <param name="isActive">True to activate; false to deactivate.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating success or NotFound (404) when the category does not exist.
        /// </returns>
        public Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct)
            => PatchApiAsync<bool>(
                $"{BasePath}/SetActive/{id}?isActive={isActive.ToString().ToLowerInvariant()}",
                token: token,
                ct: ct);
   
        /// <summary>
        /// Deletes an inventory category for the current tenant (hard delete).
        /// The API enforces business rules:
        /// - It will block deletion (409) if the category is assigned to any client mappings.
        /// </summary>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="id">Inventory category identifier (route).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse indicating success, NotFound (404), or Conflict (409) if the category is in use.
        /// </returns>
        public Task<ApiResponse<bool>> DeleteByIdAsync(string token, int id, CancellationToken ct)
            => DeleteApiAsync<bool>(
                $"{BasePath}/Delete/{id}",
                token,
                ct);

        /// <summary>
        /// Checks if an inventory category name already exists for the current tenant.
        /// </summary>
        /// <param name="name">Category name to check.</param>
        /// <param name="excludeId">Optional category id to exclude (for update scenarios).</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// ApiResponse with true if a duplicate exists; otherwise false.
        /// </returns>
        /// <remarks>
        /// This method REQUIRES a matching API endpoint.
        ///
        /// Your current controller has a "GetByName" endpoint that is incorrectly defined
        /// (route parameters do not match method parameters, and it accepts companyId).
        ///
        /// Recommended API fix:
        /// [HttpGet("ExistsByName")]
        /// public Task ExistsByName([FromQuery] string name, [FromQuery] int? excludeId, ...)
        ///
        /// Once fixed, this UI method will work as expected.
        /// </remarks>
        public Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct)
            => GetApiAsync<bool>(
                $"{BasePath}/ExistsByName?name={Uri.EscapeDataString(name)}&excludeId={excludeId}",
                token,
                ct);
    }
}
