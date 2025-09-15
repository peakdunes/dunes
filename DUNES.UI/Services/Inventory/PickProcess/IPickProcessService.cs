using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using Newtonsoft.Json.Linq;

namespace DUNES.UI.Services.Inventory.PickProcess
{
    public interface IPickProcessService
    {
      
        Task<ApiResponse<PickProcessRequestDto>>  GetPickProcessAllInfo(string DeliveryId, string token, CancellationToken ct);
    }


}
