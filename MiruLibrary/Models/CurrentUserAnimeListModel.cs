// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MyInternetConnectionLibrary.Anime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiruLibrary.Models
{
    public class CurrentUserAnimeListModel : ICurrentUserAnimeListModel
    {
        public CurrentUserAnimeListModel(IAnimeApiClient animeApiClient)
        {
            AnimeApiClient = animeApiClient;
        }

        private IAnimeApiClient AnimeApiClient { get; }

        /// <summary>Number of entries returned by the api in a single page of the user's anime list.</summary>
        private const int MaxEntriesPerPage = 300;

        // stores anime list of the currently synced user
        public UserAnimeList UserAnimeListData { get; private set; }
        public UserAnimeList UserDroppedAnimeListData { get; private set; }

        // get user's watching status anime list
        public async Task<(bool Success, string ErrorMessage)> GetCurrentUserAnimeList(string malUsername)
        {
            try
            {
                // get user's watching status anime list
                UserAnimeListData = await GetUserAnimeList(malUsername, UserAnimeListStatus.Watching, "watching");
            }
            catch (HttpRequestException)
            {
                return (false, "Problems with internet connection!");
            }
            catch (AnimeApiNotFoundException)
            {
                return (false, $"Could not find the user \"{ malUsername }\". Please make sure you typed in the name correctly.");
            }
            catch (AnimeApiException e)
            {
                // the api can be misconfigured (wrong url or an api that does not support the v4 endpoints)
                return (false, $"The anime api returned an error ({e.StatusCode}). " +
                               $"Check the AnimeApiBaseUrl setting ({AnimeApiClient.BaseUrl}).");
            }

            return (true, string.Empty);
        }

        public async Task<(bool Success, string ErrorMessage)> GetCurrentUserDroppedAnimeList(string malUsername)
        {
            try
            {
                // get user's dropped status anime list
                UserDroppedAnimeListData = await GetUserAnimeList(malUsername, UserAnimeListStatus.Dropped, "dropped");
            }
            catch (Exception)
            {
                return (false, "Problem with getting user's dropped anime list!");
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Gets all the pages of the user's anime list and returns only the entries
        /// with the given list status.
        /// </summary>
        private async Task<UserAnimeList> GetUserAnimeList(string malUsername, int status, string statusName)
        {
            try
            {
                // the api can filter the list by the status itself
                return await GetUserAnimeListPages(malUsername, status, statusName);
            }
            catch (AnimeApiException)
            {
                // the api does not support the status filter (jikan-rest passes an enum
                // where the parser expects an id) - download the whole list and filter it here
                return await GetUserAnimeListPages(malUsername, status, null);
            }
        }

        private async Task<UserAnimeList> GetUserAnimeListPages(string malUsername, int status, string statusName)
        {
            var animeList = new List<AnimeDto>();
            var page = 1;

            while (true)
            {
                var pageData = await AnimeApiClient.GetUserAnimeListAsync(malUsername, page, statusName);
                var entries = pageData?.Data ?? new List<UserAnimeListEntryDto>();

                animeList.AddRange(entries
                    .Where(x => x.WatchingStatus == status)
                    .Select(ToAnime)
                    .Where(x => x != null));

                // the api does not return the pagination data, so the next page is requested
                // only when the current one was filled up completely
                if (entries.Count < MaxEntriesPerPage)
                {
                    break;
                }

                page++;
            }

            return new UserAnimeList
            {
                Anime = animeList,
                Total = animeList.Count
            };
        }

        /// <summary>
        /// Flattens an entry of the user's list - the api returns the progress of the user
        /// next to the anime itself.
        /// </summary>
        private static AnimeDto ToAnime(UserAnimeListEntryDto entry)
        {
            if (entry?.Anime == null)
            {
                return null;
            }

            var anime = entry.Anime;
            anime.WatchedEpisodes = entry.EpisodesWatched ?? 0;
            anime.TotalEpisodes = anime.Episodes ?? 0;

            return anime;
        }
    }
}
