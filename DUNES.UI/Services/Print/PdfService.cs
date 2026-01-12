using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.Utils.Reponse;
using DUNES.UI.Interfaces.Print;
using DUNES.UI.Services.Common;
using QuestPDF.Infrastructure;
using System.Net;

namespace DUNES.UI.Services.Print
{
    public class PdfService
        : UIApiServiceBase, IPdfService
    {
        private readonly IPdfDocumentService _documentService;

        public PdfService(
            IHttpClientFactory factory,
            IPdfDocumentService documentService)
            : base(factory)
        {
            _documentService = documentService;
        }

        public async Task<ApiResponse<string>> GeneratePackingList(
            string userName,
            TorderRepairTm model,
            CancellationToken ct)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            try
            {
                // 1. Obtener el parámetro general 5 (ruta base)
                //    No requiere token → se pasa vacío
                var paramResp = await GetApiAsync<MvcGeneralParametersDto>(
                    "/general-parameter-by-number/5",
                    token: string.Empty,
                    ct);

                // 2. Validación lógica
                if (!paramResp.Success || paramResp.Data is null)
                {
                    return ApiResponseFactory.Fail<string>(
                        message: string.IsNullOrWhiteSpace(paramResp.Message)
                            ? "Parameter 5 not found."
                            : paramResp.Message,
                        statusCode: paramResp.StatusCode != 0 ? paramResp.StatusCode : 404,
                        error: paramResp.Error,
                        traceId: paramResp.TraceId
                    );
                }

                var basePath = paramResp.Data.ParameterValue;

                if (string.IsNullOrWhiteSpace(basePath))
                {
                    return ApiResponseFactory.Fail<string>(
                        message: "Parameter 5 is empty or null.",
                        error: "Invalid parameter value.",
                        statusCode: 500
                    );
                }

                // 3. Armar nombre de archivo
                var fileName = $"{userName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                var fullPath = Path.Combine(basePath, fileName);

                // 4. Generar PDF físico
                await _documentService.CreatePackingListPdfAsync(
                    fullPath,
                    model,
                    ct);

                // 5. Respuesta OK
                return ApiResponseFactory.Ok(
                    data: fullPath,
                    message: "Packing list generado correctamente."
                );
            }
            catch (Exception ex)
            {
                return ApiResponseFactory.Fail<string>(
                    error: ex.GetBaseException().Message,
                    message: "Unexpected error while generating the packing list.",
                    statusCode: 500
                );
            }
        }
    }
}
