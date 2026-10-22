// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MyInternetConnectionLibrary.Anime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MiruLibrary.Models
{
    public static class MiruAnimeModelExtensions
    {
        public static void FilterByTitle(this List<MiruAnimeModel> animeList, string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                animeList.RemoveAll(x => x.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) < 0);
            }
        }
        public static bool MatchesTitle(this MiruAnimeModel x, string title)
        {
            // indexof cuz no contains overload for case insensitive in this version of net framework
            return (x.Title?.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        // filter list of the animes depending on the broadcast type
        public static void FilterByBroadcastType(this List<MiruAnimeModel> animeList, AnimeType broadcastType)
        {
            switch (broadcastType)
            {
                case AnimeType.Both:
                    animeList.RemoveAll(x => x.Type != "TV" && x.Type != "ONA");
                    break;

                case AnimeType.TV:
                    animeList.RemoveAll(x => x.Type != "TV");
                    break;

                case AnimeType.ONA:
                    animeList.RemoveAll(x => x.Type != "ONA");
                    break;

                case AnimeType.Any:
                    break;

                case AnimeType.Movie:
                    animeList.RemoveAll(x => x.Type != "Movie");
                    break;

                case AnimeType.Special:
                    animeList.RemoveAll(x => x.Type != "Special");
                    break;

                case AnimeType.OVA:
                    animeList.RemoveAll(x => x.Type != "OVA");
                    break;
            }
        }

        public static bool MatchesBroadcastType(this MiruAnimeModel x, AnimeType broadcastType)
        {
            switch (broadcastType)
            {
                case AnimeType.Any:
                    return true;
                case AnimeType.Both:
                    return x.Type == "TV" || x.Type == "ONA";
                default:
                    return x.Type == broadcastType.ToString();
                //case AnimeType.TV:
                //    return x.Type == "TV";
                //case AnimeType.ONA:
                //    return x.Type == "ONA";
                //case AnimeType.Movie:
                //    return x.Type == "Movie";
                //case AnimeType.Special:
                //    return x.Type == "Special";
                //case AnimeType.OVA:
                //    return x.Type != "OVA";
            }
        }
        public static bool MatchesAgeRating(this MiruAnimeModel x, AgeRating ageRating)
        {
            if (ageRating == AgeRating.Any) return true;
            return x.AgeRating != "G - All Ages" && x.AgeRating != "PG - Children";
        }

        public static void FilterByAgeRating(this List<MiruAnimeModel> animeList, AgeRating ageRating)
        {
            switch (ageRating)
            {
                case AgeRating.Any:
                    break;
                case AgeRating.ExcludeAllAgesAndChildren:
                    animeList.RemoveAll(x => x.AgeRating is "G - All Ages" || x.AgeRating is "PG - Children");
                    break;
            }
        }

        public static void ConvertJstBroadcastTimeToSelectedTimeZone(this MiruAnimeModel anime, 
            TimeZoneInfo selectedTimeZone)
        {
            if (!anime.JSTBroadcastTime.HasValue)
            {
                anime.LocalBroadcastTime = null;
                return;
            }
            // covert JST to UTC
            var broadcastTimeInSelectedTimeZone = TimeZoneInfo.ConvertTimeToUtc(anime.JSTBroadcastTime.Value, 
                TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));

            // get the UTC offset for the selected time zone
            var utcOffset = selectedTimeZone.GetUtcOffset(DateTime.UtcNow);

            // return Japanese broadcast time converted to selected time zone
            anime.LocalBroadcastTime = broadcastTimeInSelectedTimeZone.Add(utcOffset);
        }

        /// <summary>
        /// Parses day and time from the broadcast string, converts it to the local time zone and saves to the LocalBroadcastTime property of the MiruAnimeModel.
        /// </summary>
        /// <param name="detailedAnimeList">List of animes with broadcast strings but without parsed day and time data.</param>
        /// <returns>List of animes with parsed data saved in LocalBroadcastTime properties.</returns>
        public static void ParseTimeFromBroadcast(this List<MiruAnimeModel> detailedAnimeList, IFileSystemService fileSystemService)
        {
            var jpCultureInfo = CultureInfo.GetCultureInfo("ja-JP");
            string[] formats = { "dd/MM/yyyy HH:mm", "d/MM/yyyy HH:mm", "dd/M/yyyy HH:mm", "d/M/yyyy HH:mm"};

            // for each anime parse time and day of the week from the broadcast string
            foreach (var anime in detailedAnimeList)
            {
                bool parsed = TryParseAndSetAirTime(anime.Broadcast, anime, formats, jpCultureInfo);

                if (!parsed)
                {
                    anime.TryParseAndSetAirTimeFromMyAnimeList();
                }

                // save JST broadcast time converted to your computer's local time to the model's property
                anime.ConvertJstBroadcastTimeToSelectedTimeZone(TimeZoneInfo.Local);
            }
        }

        public static bool TryParseAndSetAirTime(string airTimeText, MiruAnimeModel animeToUpdate, string[] formats, CultureInfo cultureInfo)
        {
            var parsed = DateTime.TryParseExact(airTimeText, formats, cultureInfo, DateTimeStyles.None, out DateTime parsedBroadcastTime);
            if (parsed)
            {
                animeToUpdate.Broadcast = airTimeText;
                animeToUpdate.JSTBroadcastTime = parsedBroadcastTime;
            }
            return parsed;
        }

        /// <summary>
        /// This method parses DateTime from MAL strings that look like this: "Saturdays at 02:25 (JST)".
        /// </summary>
        /// <param name="animeToUpdate">Anime that will be updated with parsed data.</param>
        /// <returns></returns>
        public static bool TryParseAndSetAirTimeFromMyAnimeList(this MiruAnimeModel animeToUpdate)
        {
            // split the broadcast string into words
            var broadcastWords = animeToUpdate.Broadcast?.Split(' ');

            if (broadcastWords == null || broadcastWords.Length < 3)
            {
                return false;
            }
            // parse time from the 3rd broadcast string word
            if (!DateTime.TryParse(broadcastWords[2], out DateTime airTime))
            {
                return false;
            }

            // set the first word of the broadcast string as a day of the week
            var dayOfTheWeek = broadcastWords[0];
            int day;

            // depending on the 1st word set the correct day
            switch (dayOfTheWeek)
            {
                case "Mondays":
                    day = 20;
                    break;
                case "Tuesdays":
                    day = 21;
                    break;
                case "Wednesdays":
                    day = 22;
                    break;
                case "Thursdays":
                    day = 23;
                    break;
                case "Fridays":
                    day = 24;
                    break;
                case "Saturdays":
                    day = 25;
                    break;
                case "Sundays":
                    day = 26;
                    break;
                default:
                    day = 27;
                    break;
            }
            animeToUpdate.JSTBroadcastTime = new DateTime(2020, 01, day, airTime.Hour, airTime.Minute, 0);
            return true;
        }

        public static void SetAiringAnimeModelData(this MiruAnimeModel animeModel, AnimeDto animeInfo, 
            AnimeDto animeListEntry, ICurrentUserAnimeListModel currentUserAnimeList)
        {
            animeModel.MalId = animeInfo.MalId ?? 0;
            animeModel.Broadcast = GetBroadcastString(animeInfo);
            animeModel.Title = animeInfo.Title;
            animeModel.ImageURL = GetImageUrl(animeInfo);
            animeModel.LocalImagePath = animeModel.MalId.ToString();
            animeModel.TotalEpisodes = animeListEntry.TotalEpisodes ?? animeInfo.Episodes ?? 0;
            animeModel.URL = animeListEntry.Url;
            animeModel.WatchedEpisodes = animeListEntry.WatchedEpisodes ?? 0;
            animeModel.IsOnWatchingList = true;
            animeModel.CurrentlyAiring = IsAiring(animeListEntry);
            animeModel.Type = animeInfo.Type;
            animeModel.UpdateDroppedStatus(currentUserAnimeList);
            animeModel.OpeningThemes = JsonConvert.SerializeObject(GetOpeningTheme(animeInfo));
            animeModel.EndingThemes = JsonConvert.SerializeObject(GetEndingTheme(animeInfo));
            animeModel.AgeRating = animeInfo.Rating;
        }

        public static void SetSeasonalAnimeModelData(this MiruAnimeModel animeModel, AnimeDto animeInfo, 
            AnimeDto seasonEntry, ICurrentUserAnimeListModel currentUserAnimeList)
        {
            animeModel.MalId = animeInfo.MalId ?? 0;
            animeModel.Broadcast = GetBroadcastString(animeInfo);
            animeModel.Title = animeInfo.Title;
            animeModel.ImageURL = GetImageUrl(animeInfo);
            animeModel.LocalImagePath = animeModel.MalId.ToString();
            animeModel.URL = seasonEntry.Url;
            animeModel.IsOnWatchingList = false;
            animeModel.CurrentlyAiring = true;
            animeModel.Type = animeInfo.Type;
            animeModel.UpdateDroppedStatus(currentUserAnimeList);
            animeModel.OpeningThemes = JsonConvert.SerializeObject(GetOpeningTheme(animeInfo));
            animeModel.EndingThemes = JsonConvert.SerializeObject(GetEndingTheme(animeInfo));
            animeModel.AgeRating = animeInfo.Rating;
        }

        /// <summary>
        /// Creates the broadcast string in the same format as the one returned by the old api
        /// (e.g. "Sundays at 17:00 (JST)"), because the day and the time are parsed from it.
        /// </summary>
        public static string GetBroadcastString(AnimeDto animeInfo)
        {
            var broadcast = animeInfo?.Broadcast;

            if (broadcast != null)
            {
                // the api returns the broadcast string ready to use, e.g. "Fridays at 23:00 (JST)"
                if (!string.IsNullOrWhiteSpace(broadcast.String))
                {
                    return broadcast.String;
                }

                if (!string.IsNullOrWhiteSpace(broadcast.Day) && !string.IsNullOrWhiteSpace(broadcast.Time))
                {
                    var timezone = string.IsNullOrWhiteSpace(broadcast.Timezone) ? string.Empty : $" ({broadcast.Timezone})";
                    return $"{broadcast.Day} at {broadcast.Time}{timezone}";
                }
            }

            return animeInfo?.Aired?.From?.ToString();
        }

        public static string GetImageUrl(AnimeDto animeInfo)
        {
            return animeInfo?.Images?.Jpg?.LargeImageUrl
                   ?? animeInfo?.Images?.Jpg?.ImageUrl
                   ?? animeInfo?.Images?.WebP?.LargeImageUrl;
        }

        public static bool IsAiring(AnimeDto anime)
        {
            if (anime == null) return false;

            if (anime.Airing == true) return true;
            if (anime.AiringStatus != null && anime.AiringStatus.Equals("Currently Airing", StringComparison.OrdinalIgnoreCase)) return true;

            return anime.Status != null && anime.Status.Equals("Currently Airing", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetOpeningTheme(AnimeDto animeInfo)
        {
            return animeInfo?.Theme?.Openings?.FirstOrDefault();
        }

        public static string GetEndingTheme(AnimeDto animeInfo)
        {
            return animeInfo?.Theme?.Endings?.FirstOrDefault();
        }

        public static void UpdateDroppedStatus(this MiruAnimeModel animeModel, ICurrentUserAnimeListModel currentUserAnimeListModel)
        {
            animeModel.Dropped = currentUserAnimeListModel
                ?.UserDroppedAnimeListData
                ?.Anime
                .Select(x => x.MalId)
                .Contains(animeModel.MalId) ?? false;
        }    }
}
