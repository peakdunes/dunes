using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.Utils.Reponse;
using DUNES.UI.Infrastructure;
using DUNES.UI.Interfaces.Print;
using Newtonsoft.Json.Linq;
using QuestPDF.Infrastructure;
using System.Net;
using System.Net.Http.Headers;


namespace DUNES.UI.Services.Print
{
    public class PdfService : IPdfService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;
        private readonly IPdfDocumentService _documentService;

        public PdfService(IConfiguration config, IPdfDocumentService documentService)
        {
            _config = config;
            _documentService = documentService;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }


        //public async string GeneratePackingList(string userName, TorderRepairTm model, CancellationToken ct)
        //{
        //    QuestPDF.Settings.License = LicenseType.Community;

        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    HttpResponseMessage resp;

        //    resp = await _httpClient.GetAsync($"/general-parameter-by-number/5");


        //    if (resp.IsSuccessStatusCode)
        //    {


        //    }

        //    var safeUser = string.IsNullOrWhiteSpace(userName)
        //     ? "Unknown"
        //     : userName
        //         .Replace("@", "_")
        //         .Replace(".", "_")
        //         .Replace("\\", "_")
        //         .Replace("/", "_")
        //         .Replace(" ", "_");


        //    var fileName = $"{safeUser}_{DateTime.Now:yyyyMMdd_HHmmss}_PackingList_{model.repairHdr.RefNo}.pdf";
        //    var fullPath = Path.Combine($"C:\\basura\\", fileName);

        //}
        //public string GeneratePackingList(string userName, TorderRepairTm model)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ApiResponse<string>> GeneratePackingList(string userName, TorderRepairTm model, CancellationToken ct)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            try
            {
                // 1. Obtener el parámetro general 5 (ruta base de los PDFs)
                var resp = await _httpClient.GetAsync("/general-parameter-by-number/5", ct);

                // Nivel HTTP
                if (!resp.IsSuccessStatusCode)
                {
                    var raw = await resp.Content.ReadAsStringAsync(ct);

                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return ApiResponseFactory.Fail<string>(
                   error: raw,
                   message: "Parameter 5 not found.",
                   statusCode: 404
               );
                    }



                    return ApiResponseFactory.Fail<string>(
                        message: "Can't obtain parameter 5.",
                        statusCode: (int)resp.StatusCode,
                        error: raw
                    );
                }

                // 2. Leer el ApiResponse<GeneralParameterDto>
                var apiResponse = await resp.Content.ReadFromJsonAsync<ApiResponse<MvcGeneralParametersDto>>(
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    },
                    ct
                );

                if (apiResponse is null)
                {
                    return ApiResponseFactory.Fail<string>(
                            error: "Null or invalid response from the API when reading parameter 5.",
                            message: "The API response for parameter 5 could not be deserialized.",
                            statusCode: 500
            );
                }

                // 3. Validar éxito lógico y datos
                if (!apiResponse.Success || apiResponse.Data is null)
                {
                    return ApiResponseFactory.Fail<string>(
                        message: string.IsNullOrWhiteSpace(apiResponse.Message)
                            ? "No se encontró valor para el parámetro general 5."
                            : apiResponse.Message,
                        statusCode: apiResponse.StatusCode != 0 ? apiResponse.StatusCode : 404,
                        error: apiResponse.Error,
                        traceId: apiResponse.TraceId
                    );
                }


                var basePath = apiResponse.Data.ParameterValue;

                if (string.IsNullOrWhiteSpace(basePath))
                {
                    return ApiResponseFactory.Fail<string>(
                       error: "Parameter 5 is empty or null",
                       message: "Parameter 5 is empty or null.",
                       statusCode: 500
           );
                }

                // 4. Armar nombre de archivo: usuario + timestamp
                var fileName = $"{userName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                var fullPath = Path.Combine(basePath, fileName);

                // 5. Generar el PDF físicamente en esa ruta
                // Aquí llamas a tu servicio que usa QuestPDF / lo que tengas
                // Ejemplo (ajusta al nombre real de tu servicio/método):
                await _documentService.CreatePackingListPdfAsync(fullPath, model, ct);

                // 6. Devolver éxito con la ruta completa
                return ApiResponseFactory.Ok(
                    data: fullPath,
                    message: "Packing list generado correctamente."
                );
            }
            catch (Exception ex)
            {
                // Error técnico: red, JSON, IO, etc.
                return ApiResponseFactory.Fail<string>(
                    error: ex.GetBaseException().Message,                  // detalle técnico para logs
                    message: "Error inesperado al generar el packing list.", // mensaje para el usuario/UI
                    statusCode: 500                                         // código interno/HTTP
                );
            }

            //var fileName = $"{safeUser}_{DateTime.Now:yyyyMMdd_HHmmss}_PackingList_{model.repairHdr.RefNo}.pdf";
            //var fullPath = Path.Combine($"C:\\basura\\", fileName);

        }

      
    }
}
