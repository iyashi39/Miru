// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MiruLibrary.Services;
using MyInternetConnectionLibrary.Anime;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiruLibrary.Models
{
    public class CurrentSeasonModel : ICurrentSeasonModel
    {
        public CurrentSeasonModel(IAnimeApiClient animeApiClient, ITimerService timerService)
        {
            AnimeApiClient = animeApiClient;
            TimerService = timerService;
        }

        private IAnimeApiClient AnimeApiClient { get; }
        private ITimerService TimerService { get; }

        // stores data model of the current anime season
        public AnimePageEnvelope SeasonData { get; private set; }

        // get current anime season data
        public async Task<bool> GetCurrentSeasonList(int requestRetryDelayInMs)
        {
            var seasonEntries = new List<AnimeDto>();
            var page = 1;
            PaginationDto pagination = null;

            while (true)
            {
                AnimePageEnvelope pageData = null;

                try
                {
                    pageData = await AnimeApiClient.GetCurrentSeasonAsync(page);
                }
                catch (HttpRequestException)
                {
                    return false;
                }
                catch (AnimeApiResponseFormatException)
                {
                    // the response cannot be parsed - retrying would not help
                    throw;
                }
                catch (AnimeApiException)
                {
                    // if there is no response from the api wait for a specified time and retry
                    await TimerService.DelayTask(requestRetryDelayInMs);
                    continue;
                }

                if (pageData?.Data != null)
                {
                    seasonEntries.AddRange(pageData.Data);
                }

                pagination = pageData?.Pagination;

                if (pagination?.HasNextPage == true)
                {
                    page++;
                    await TimerService.DelayTask(requestRetryDelayInMs);
                    continue;
                }

                break;
            }

            SeasonData = new AnimePageEnvelope
            {
                Data = seasonEntries,
                Pagination = pagination
            };

            return true;
        }

        public List<AnimeDto> GetFilteredSeasonList()
        {
            // list of anime entries in the current season
            var currentSeasonList = SeasonData?.Data?.ToList() ?? new List<AnimeDto>();

            // remove anime entries marked as 'for kids' from the list
            currentSeasonList.RemoveAll(x => IsForKids(x));

            return currentSeasonList;
        }

        /// <summary>
        /// The v4 api does not return the 'kids' flag anymore, so it is derived
        /// from the demographics and the genres of the anime (MAL marks them as 'Kids').
        /// </summary>
        public static bool IsForKids(AnimeDto anime)
        {
            if (anime == null) return false;

            return anime.Demographics?.Any(x => IsKidsName(x.Name)) == true
                   || anime.Genres?.Any(x => IsKidsName(x.Name)) == true;
        }

        private static bool IsKidsName(string name)
        {
            return !string.IsNullOrWhiteSpace(name)
                   && name.IndexOf("Kids", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
