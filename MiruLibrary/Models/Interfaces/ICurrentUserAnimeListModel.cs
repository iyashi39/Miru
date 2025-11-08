// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using JikanDotNet;
using System.Threading.Tasks;

namespace MiruLibrary.Models
{
    public interface ICurrentUserAnimeListModel
    {
        UserAnimeList UserAnimeListData { get; }
        UserAnimeList UserDroppedAnimeListData { get; }

        Task<(bool Success, string ErrorMessage)> GetCurrentUserAnimeList(string malUsername);
        Task<(bool Success, string ErrorMessage)> GetCurrentUserDroppedAnimeList(string malUsername);
    }
}