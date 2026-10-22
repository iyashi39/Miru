// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyInternetConnectionLibrary.Anime
{
    /// <summary>
    /// Thin http client of the Jikan REST API v4.
    /// The same schema is used by the Tenrai API v1, so both can be used by changing the base url.
    /// </summary>
    public class AnimeApiClient : IAnimeApiClient
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private readonly HttpClient _httpClient;

        public AnimeApiClient(HttpClient httpClient, string baseUrl)
        {
            _httpClient = httpClient;
            BaseUrl = baseUrl?.TrimEnd('/') + "/";
        }

        public string BaseUrl { get; }

        public async Task<AnimeDto> GetAnimeAsync(long malId, CancellationToken cancellationToken = default)
        {
            var envelope = await GetAsync<AnimeDataEnvelope>($"anime/{malId}", cancellationToken);
            return envelope?.Data;
        }

        public async Task<AnimeDto> GetAnimeFullAsync(long malId, CancellationToken cancellationToken = default)
        {
            var envelope = await GetAsync<AnimeDataEnvelope>($"anime/{malId}/full", cancellationToken);
            return envelope?.Data;
        }

        public Task<AnimePageEnvelope> GetCurrentSeasonAsync(int page, CancellationToken cancellationToken = default)
        {
            // continuing=true makes the api return also the anime that continue from the previous season
            // (without it the list is missing them, the same way the old api returned the whole seasonal page)
            return GetAsync<AnimePageEnvelope>($"seasons/now?page={page}&limit=25&continuing=true", cancellationToken);
        }

        public Task<UserAnimeListPageEnvelope> GetUserAnimeListAsync(string username, int page, string status = null,
            CancellationToken cancellationToken = default)
        {
            var encodedUser = Uri.EscapeDataString(username ?? string.Empty);
            var url = $"users/{encodedUser}/animelist?page={page}";

            if (!string.IsNullOrWhiteSpace(status))
            {
                url += $"&status={Uri.EscapeDataString(status)}";
            }

            return GetAsync<UserAnimeListPageEnvelope>(url, cancellationToken);
        }

        private async Task<T> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken) where T : class
        {
            var url = BaseUrl + relativeUrl;

            using var response = await _httpClient.GetAsync(url, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new AnimeApiNotFoundException($"The api returned 404 (not found) for '{url}'.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new AnimeApiException(
                    $"The api returned {(int)response.StatusCode} ({response.ReasonPhrase}) for '{url}': {body}",
                    response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(content, SerializerOptions);
            }
            catch (JsonException ex)
            {
                throw new AnimeApiResponseFormatException($"Could not parse the api response for '{url}': {ex.Message}");
            }
        }
    }
}
