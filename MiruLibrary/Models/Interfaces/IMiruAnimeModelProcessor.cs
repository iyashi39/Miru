// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;
using System.Collections.Generic;

namespace MiruLibrary.Models
{
    public interface IMiruAnimeModelProcessor
    {
        IEnumerable<MiruAnimeModel> FilterAnimeModelsByAirDayOfWeekAndOrderByAirTime(IEnumerable<MiruAnimeModel> animeModels, DayOfWeek? dayOfWeek);
        IEnumerable<MiruAnimeModel> FilterAnimeModelsByAnimeListType(IEnumerable<MiruAnimeModel> animeModels, AnimeListType animeListType);
    }
}