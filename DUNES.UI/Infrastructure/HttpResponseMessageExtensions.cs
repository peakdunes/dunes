using System.Net.Http;
using System.Text.Json;
using DUNES.Shared.Models;

namespace DUNES.UI.Infrastructure
{
    public static class HttpResponseMessageExtensions
    {
        private static readonly JsonSerializerOptions JsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Normaliza cualquier respuesta a ApiResponse&lt;T&gt; sin lanzar excepciones:
        /// - Si viene ApiResponse&lt;T&gt; válido → lo devuelve (sin tocar Error).
        /// - Si viene T "plano" → lo envuelve en ApiResponse.Data.
        /// - Si el JSON es inválido/otro esquema → ApiResponse.Error con detalle.
        /// </summary>
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
                // seguimos al fallback
            }

            // 2) Intentar payload T "plano"
            try
            {
                var payload = JsonSerializer.Deserialize<T>(raw, JsonOpts);
                if (payload is not null)
                    return new ApiResponse<T>
                    {
                        StatusCode = (int)resp.StatusCode,
                        Data = payload
                    };
            }
            catch (JsonException jx)
            {
                return Fail<T>(resp, $"JSON inválido: {jx.Message}");
            }

            // 3) Nada calzó
            return Fail<T>(resp, "La respuesta no coincide con el contrato esperado.");
        }

        private static async Task<string> SafeReadAsync(HttpResponseMessage resp, CancellationToken ct)
        {
            try { return await resp.Content.ReadAsStringAsync(ct); }
            catch { return string.Empty; }
        }

        private static ApiResponse<T> Fail<T>(HttpResponseMessage resp, string msg) => new()
        {
            StatusCode = (int)resp.StatusCode,
            Error = $"{msg} HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}."
        };
    }
}
