using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DUNES.UI.Services.Inventory
{
    public class ASNService : IASNService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public ASNService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }
        

        public async Task<ApiResponse<ASNDto>> GetAsnInfo(string asnNumber, string token, CancellationToken ct)
        {

            bool usedExtensionMethod = true;


            if (usedExtensionMethod)
            {

                //#################################################################################
                //CON METODO DE EXTENSION valida si la respuesta esta bien formada (apiResponse)
                //#################################################################################

                //si el usuario cierra el navegador
                //CancellationToken ct = default

                //enviar esto al metodo de EXTENSION hace que que este metodo de extension valide si la 
                //respuesta esta bien formada es decir viene en forma de apiResponse si no es asi lanza una exception

                return await _httpClient.GetApiResponseAsync<ASNDto>($"/api/CommonQueryINV/asn-info/{asnNumber}",
                        bearerToken: token, ct: ct);


            }
            else
            {
                //#################################################################################
                //SIN METODO DE EXTENSION no valida si la respuesta esta bien formada (apiResponse)
                //#################################################################################
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage resp;


                resp = await _httpClient.GetAsync($"/api/CommonQueryINV/asn-info/{asnNumber}");

                //se obtiene el string de la respuesta
                var RespJsonString = await resp.Content.ReadAsStringAsync();

                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                //se deserializa la respuesta al objeto tipo ApiReponse
                ApiResponse<ASNDto>? result = JsonSerializer.Deserialize<ApiResponse<ASNDto>>(RespJsonString, opts);


                return result!;
            }
        }
    }
}
