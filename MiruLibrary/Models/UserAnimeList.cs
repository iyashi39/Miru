// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MyInternetConnectionLibrary.Anime;
using System.Collections.Generic;

namespace MiruLibrary.Models
{
    /// <summary>
    /// Anime list of a single MyAnimeList user (e.g. the watching or the dropped list).
    /// </summary>
    public class UserAnimeList
    {
        public List<AnimeDto> Anime { get; set; } = new List<AnimeDto>();

        public int Total { get; set; }
    }
}
