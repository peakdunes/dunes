using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Inventory.ASN
{
    public class ASNUIService
        : UIApiServiceBase, IASNUIService
    {
        public ASNUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<ASNWm>> GetAsnInfo(
            string asnNumber,
            string token,
            CancellationToken ct)
        {
            return GetApiAsync<ASNWm>(
                $"/api/CommonQueryASNINV/asn-info/{asnNumber}",
                token,
                ct);
        }

        public Task<ApiResponse<ASNResponseDto>> ProcessASNTransaction(
            string asnNumber,
            ProcessAsnRequestTm objInvData,
            string trackingNumber,
            string token,
            CancellationToken ct)
        {
            return PostApiAsync<ASNResponseDto, ProcessAsnRequestTm>(
                $"/api/CommonQueryASNINV/asn-process/{asnNumber}/{trackingNumber}",
                objInvData,
                token,
                ct);
        }
    }
}
