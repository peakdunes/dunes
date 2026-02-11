using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.Common
{
    // Token por request (no “pegado” en DefaultRequestHeaders)
    // CancellationToken siempre viaja
    // GET / POST / PUT / DELETE listos
    // Devuelve ApiResponse<T> directamente (services quedan de 1 línea)

    public abstract class UIApiServiceBase
    {
        private readonly HttpClient _http;

        protected UIApiServiceBase(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("DUNES_API");
        }

        protected async Task<ApiResponse<T>> GetApiAsync<T>(
            string url,
            string token,
            CancellationToken ct)
        {
            try
            {
                using var req = CreateJsonRequest(HttpMethod.Get, url, body: null, token);
                using var resp = await _http.SendAsync(
                    req,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                return await resp.ReadAsApiResponseAsync<T>(ct);
            }
            catch (OperationCanceledException)
            {
                return ApiResponseFactory.Fail<T>(error: "REQUEST_CANCELED",
        message: "Request canceled by user.",
        statusCode: 499);
            }
        }

        protected async Task<ApiResponse<T>> DeleteApiAsync<T>(
            string url,
            string token,
            CancellationToken ct)
        {
            try
            {
                using var req = CreateJsonRequest(HttpMethod.Delete, url, body: null, token);
                using var resp = await _http.SendAsync(
                    req,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                return await resp.ReadAsApiResponseAsync<T>(ct);
            }
            catch (OperationCanceledException)
            {
                return ApiResponseFactory.Fail<T>(error: "REQUEST_CANCELED",
        message: "Request canceled by user.",
        statusCode: 499);
            }
        }

        protected async Task<ApiResponse<TResp>> PostApiAsync<TResp, TBody>(
            string url,
            TBody body,
            string token,
            CancellationToken ct)
        {
            try
            {
                using var req = CreateJsonRequest(HttpMethod.Post, url, body, token);
                using var resp = await _http.SendAsync(
                    req,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                return await resp.ReadAsApiResponseAsync<TResp>(ct);
            }
            catch (OperationCanceledException)
            {
                return ApiResponseFactory.Fail<TResp>(error: "REQUEST_CANCELED",
        message: "Request canceled by user.",
        statusCode: 499);
            }
        }

        protected async Task<ApiResponse<TResp>> PutApiAsync<TResp, TBody>(
            string url,
            TBody body,
            string token,
            CancellationToken ct)
        {
            try
            {
                using var req = CreateJsonRequest(HttpMethod.Put, url, body, token);
                using var resp = await _http.SendAsync(
                    req,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                return await resp.ReadAsApiResponseAsync<TResp>(ct);
            }
            catch (OperationCanceledException)
            {
                return ApiResponseFactory.Fail<TResp>(error: "REQUEST_CANCELED",
        message: "Request canceled by user.",
        statusCode: 499);
            }
        }

        protected async Task<ApiResponse<TResp>> PatchApiAsync<TResp>(
    string url,
    string token,
    CancellationToken ct)
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Patch, url);
                req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var resp = await _http.SendAsync(
                    req,
                    HttpCompletionOption.ResponseHeadersRead,
                    ct);

                return await resp.ReadAsApiResponseAsync<TResp>(ct);
            }
            catch (OperationCanceledException)
            {
                return ApiResponseFactory.Fail<TResp>(
                    error: "REQUEST_CANCELED",
                    message: "Request canceled by user.",
                    statusCode: 499);
            }
        }

        protected HttpRequestMessage CreateJsonRequest(
            HttpMethod method,
            string url,
            object? body,
            string token)
        {
            var req = new HttpRequestMessage(method, NormalizeUrl(url));

            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");
            }

            return req;
        }

        private static string NormalizeUrl(string url)
        {
            // Evita errores por "api/..." sin slash inicial
            if (string.IsNullOrWhiteSpace(url))
                return url;

            return url.StartsWith("/") ? url : "/" + url;
        }
    }
}
