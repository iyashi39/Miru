// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyInternetConnectionLibrary.Anime
{
    /// <summary>
    /// Client of the Jikan REST API v4 (compatible with the Tenrai API v1 schema).
    /// </summary>
    public interface IAnimeApiClient
    {
        /// <summary>Base url of the api, e.g. http://localhost:9000/v4/</summary>
        string BaseUrl { get; }

        /// <summary>Gets basic information about the anime with the given MyAnimeList id.</summary>
        Task<AnimeDto> GetAnimeAsync(long malId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets full information about the anime with the given MyAnimeList id.
        /// Only this endpoint contains the opening/ending theme texts.
        /// </summary>
        Task<AnimeDto> GetAnimeFullAsync(long malId, CancellationToken cancellationToken = default);

        /// <summary>Gets a single page of the anime that are airing in the current season.</summary>
        Task<AnimePageEnvelope> GetCurrentSeasonAsync(int page, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single page of the anime list of the given user.
        /// The api returns up to 300 entries per page and does not return the pagination data.
        /// The status can be one of: watching, completed, onhold, dropped, plantowatch.
        /// </summary>
        Task<UserAnimeListPageEnvelope> GetUserAnimeListAsync(string username, int page, string status = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>Thrown when the api returns an error response.</summary>
    public class AnimeApiException : Exception
    {
        public AnimeApiException(string message, System.Net.HttpStatusCode? statusCode = null) : base(message)
        {
            StatusCode = statusCode;
        }

        public System.Net.HttpStatusCode? StatusCode { get; }
    }

    /// <summary>
    /// Thrown when the response of the api cannot be parsed (e.g. the api changed its schema).
    /// Such an error should not be retried.
    /// </summary>
    public class AnimeApiResponseFormatException : AnimeApiException
    {
        public AnimeApiResponseFormatException(string message) : base(message)
        {
        }
    }

    /// <summary>Thrown when the requested resource (e.g. a user) does not exist.</summary>
    public class AnimeApiNotFoundException : AnimeApiException
    {
        public AnimeApiNotFoundException(string message) : base(message, System.Net.HttpStatusCode.NotFound)
        {
        }
    }
}
