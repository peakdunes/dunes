using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Services.Inventory.ASN
{
    public interface IASNService
    {
        /// <summary>
        /// Get all information (Header and Items Detail for an ASN)
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<ASNWm>> GetAsnInfo(string asnNumber, string token,CancellationToken ct);

       

       // Task<ApiResponse<int>> ProcessASNTransaction(string asnNumber, string token, CancellationToken ct);
    }
}
