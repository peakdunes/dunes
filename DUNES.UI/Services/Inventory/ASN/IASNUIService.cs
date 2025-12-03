using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Services.Inventory.ASN
{
    public interface IASNUIService
    {
        /// <summary>
        /// Get all information (Header and Items Detail for an ASN)
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<ASNWm>> GetAsnInfo(string asnNumber, string token,CancellationToken ct);


        Task<ApiResponse<ASNResponseDto>> ProcessASNTransaction(string asnNumber, ProcessAsnRequestTm objInvData, string trackingNumber, string token, CancellationToken ct);

       
    }
}
