using System.Net.Http;
using System.Text.Json;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Infrastructure
{
    public static class HttpResponseMessageExtensions
    {
        private static readonly JsonSerializerOptions JsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        public static async Task<ApiResponse<T>> ReadAsApiResponseAsync<T>(
            this HttpResponseMessage resp,
            CancellationToken ct = default)
        {
            var raw = await SafeReadAsync(resp, ct);

            if (string.IsNullOrWhiteSpace(raw))
                return Fail<T>(resp, "El servicio respondió vacío.");

            // 1) Intentar contrato ApiResponse<T>
            try
            {
                var api = JsonSerializer.Deserialize<ApiResponse<T>>(raw, JsonOpts);
                if (api is not null)
                {
                    api.StatusCode = (int)resp.StatusCode;
                    return api;
                }
            }
            catch (JsonException)
            {
                // seguimos
            }

            // 1.5) Intentar ProblemDetails (muy común en 400/500)
            try
            {
                var pd = JsonSerializer.Deserialize<ProblemDetails>(raw, JsonOpts);
                if (pd?.Title != null)
                {
                    var detail = string.IsNullOrWhiteSpace(pd.Detail) ? "" : $" - {pd.Detail}";
                    return Fail<T>(resp, $"{pd.Title}{detail}");
                }
            }
            catch (JsonException)
            {
                // seguimos
            }

            // 2) Intentar payload T "plano"
            try
            {
                var payload = JsonSerializer.Deserialize<T>(raw, JsonOpts);
                if (payload is not null)
                    return new ApiResponse<T>
                    {
                        StatusCode = (int)resp.StatusCode,
                        Data = payload,
                        Message = resp.IsSuccessStatusCode ? null : "Respuesta plana del API (no ApiResponse)."
                    };
            }
            catch (JsonException jx)
            {
                return Fail<T>(resp, $"JSON inválido: {jx.Message}", raw);
            }

            // 3) Nada calzó
            return Fail<T>(resp, "La respuesta no coincide con el contrato esperado.", raw);
        }

        private static async Task<string> SafeReadAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            try { return await resp.Content.ReadAsStringAsync(ct); }
            catch { return string.Empty; }
        }

        private static ApiResponse<T> Fail<T>(HttpResponseMessage resp, string msg, string? raw = null) => new()
        {
            StatusCode = (int)resp.StatusCode,
            Success = false,
            Message = raw is null ? $"{msg} HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}."
                                : $"{msg} HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}",
            Error = msg
        };
    }
}

