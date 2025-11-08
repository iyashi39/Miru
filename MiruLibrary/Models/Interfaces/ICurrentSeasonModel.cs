// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using JikanDotNet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiruLibrary.Models
{
    public interface ICurrentSeasonModel
    {
        Season SeasonData { get; }

        Task<bool> GetCurrentSeasonList(int requestRetryDelayInMs);
        List<AnimeSubEntry> GetFilteredSeasonList();
    }
}