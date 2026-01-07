using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace DUNES.UI.Services.Common
{
   // Token por request(no “pegado” en DefaultRequestHeaders)
   // CancellationToken siempre viaja
   // GET/POST/PUT/DELETE listos
   // Devuelve ApiResponse<T> directamente (para que tus services queden de 1 línea)

    public abstract class UIApiServiceBase
    {
        private readonly HttpClient _http;

        protected UIApiServiceBase(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("DUNES_API");
        }

        protected async Task<ApiResponse<T>> GetApiAsync<T>(string url, string token, CancellationToken ct)
        {
            using var req = CreateJsonRequest(HttpMethod.Get, url, body: null, token);
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            return await resp.ReadAsApiResponseAsync<T>(ct);
        }

        protected async Task<ApiResponse<T>> DeleteApiAsync<T>(string url, string token, CancellationToken ct)
        {
            using var req = CreateJsonRequest(HttpMethod.Delete, url, body: null, token);
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            return await resp.ReadAsApiResponseAsync<T>(ct);
        }

        protected async Task<ApiResponse<TResp>> PostApiAsync<TResp, TBody>(string url, TBody body, string token, CancellationToken ct)
        {
            using var req = CreateJsonRequest(HttpMethod.Post, url, body, token);
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            return await resp.ReadAsApiResponseAsync<TResp>(ct);
        }

        protected async Task<ApiResponse<TResp>> PutApiAsync<TResp, TBody>(string url, TBody body, string token, CancellationToken ct)
        {
            using var req = CreateJsonRequest(HttpMethod.Put, url, body, token);
            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            return await resp.ReadAsApiResponseAsync<TResp>(ct);
        }

        protected HttpRequestMessage CreateJsonRequest(HttpMethod method, string url, object? body, string token)
        {
            var req = new HttpRequestMessage(method, NormalizeUrl(url));

            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return req;
        }

        private static string NormalizeUrl(string url)
        {
            // Evita errores por "api/..." sin slash inicial.
            if (string.IsNullOrWhiteSpace(url)) return url;
            return url.StartsWith("/") ? url : "/" + url;
        }
    }
}
