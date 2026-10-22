// Copyright (c) 2022-2024 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

namespace MiruLibrary.Models
{
    public class UserSettings
    {
        public double AnimeImageSize { get; set; } = 90.0;
        public AnimeListType DisplayedAnimeListType { get; set; } = AnimeListType.Watching;
        public AnimeType DisplayedAnimeType { get; set; } = AnimeType.TV;
        public AgeRating AgeRating { get; set; } = AgeRating.Any;
        public bool GetDroppedAnimeData { get; set; } = true;
        public double WatchingStatusHighlightOpacity { get; set; } = 0.66;

        /// <summary>
        /// Url of the anime api (Jikan REST API v4). The Tenrai API v1 uses the same schema.
        /// </summary>
        public string AnimeApiBaseUrl { get; set; } = Constants.DefaultAnimeApiBaseUrl;
    }
}
