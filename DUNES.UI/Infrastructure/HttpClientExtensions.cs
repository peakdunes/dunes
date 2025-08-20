// DUNES.UI/Infrastructure/HttpClientExtensions.cs
using DUNES.Shared.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json; // Para JsonContent.Create
using System.Text.Json;

namespace DUNES.UI.Infrastructure
{
    /// <summary>
    /// Métodos de extensión para HttpClient con contrato ESTRICTO:
    /// el backend debe devolver SIEMPRE ApiResponse<T> (en 2xx/4xx/5xx).
    /// Si el payload no cumple el contrato (null, JSON inválido u otro esquema),
    /// se lanza InvalidOperationException con el body crudo para depuración.
    /// </summary>
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerOptions JsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        // -------------------------
        // GET (contrato estricto)
        // -------------------------
        public static async Task<ApiResponse<T>> GetApiResponseAsync<T>(
            this HttpClient client,
            string relativeUrl,
            string? bearerToken = null,
            CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(bearerToken))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            try
            {
                var api = JsonSerializer.Deserialize<ApiResponse<T>>(raw, JsonOpts);
                if (api is null)
                    throw new InvalidOperationException(
                        $"Non-conforming API payload (null). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}");

                api.StatusCode = (int)resp.StatusCode; // sincroniza con HTTP real
                return api;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"Non-conforming API payload (invalid JSON). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}", ex);
            }
        }

        // -------------------------
        // POST (contrato estricto)
        // -------------------------
        public static async Task<ApiResponse<TResponse>> PostApiResponseAsync<TRequest, TResponse>(
            this HttpClient client,
            string relativeUrl,
            TRequest body,
            string? bearerToken = null,
            CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, relativeUrl);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(bearerToken))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            req.Content = JsonContent.Create(body);

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            try
            {
                var api = JsonSerializer.Deserialize<ApiResponse<TResponse>>(raw, JsonOpts);
                if (api is null)
                    throw new InvalidOperationException(
                        $"Non-conforming API payload (null). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}");

                api.StatusCode = (int)resp.StatusCode;
                return api;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"Non-conforming API payload (invalid JSON). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}", ex);
            }
        }

        // -------------------------
        // PUT (contrato estricto)
        // -------------------------
        public static async Task<ApiResponse<TResponse>> PutApiResponseAsync<TRequest, TResponse>(
            this HttpClient client,
            string relativeUrl,
            TRequest body,
            string? bearerToken = null,
            CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Put, relativeUrl);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(bearerToken))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            req.Content = JsonContent.Create(body);

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            try
            {
                var api = JsonSerializer.Deserialize<ApiResponse<TResponse>>(raw, JsonOpts);
                if (api is null)
                    throw new InvalidOperationException(
                        $"Non-conforming API payload (null). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}");

                api.StatusCode = (int)resp.StatusCode;
                return api;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"Non-conforming API payload (invalid JSON). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}", ex);
            }
        }

        // -------------------------
        // DELETE (contrato estricto)
        // -------------------------
        public static async Task<ApiResponse<T>> DeleteApiResponseAsync<T>(
            this HttpClient client,
            string relativeUrl,
            string? bearerToken = null,
            CancellationToken ct = default)
        {
            using var req = new HttpRequestMessage(HttpMethod.Delete, relativeUrl);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(bearerToken))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            try
            {
                var api = JsonSerializer.Deserialize<ApiResponse<T>>(raw, JsonOpts);
                if (api is null)
                    throw new InvalidOperationException(
                        $"Non-conforming API payload (null). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}");

                api.StatusCode = (int)resp.StatusCode;
                return api;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"Non-conforming API payload (invalid JSON). HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {raw}", ex);
            }
        }
    }
}

