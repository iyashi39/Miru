// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MiruLibrary.Models;

namespace MiruLibrary.Services
{
    public class UserDataService : IUserDataService
    {
        public UserDataService(ICurrentSeasonModel currentSeasonModel,
            ICurrentUserAnimeListModel currentUserAnimeListModel, ISyncedMyAnimeListUser syncedMyAnimeListUser)
        {
            CurrentSeasonModel = currentSeasonModel;
            CurrentUserAnimeListModel = currentUserAnimeListModel;
            SyncedMyAnimeListUser = syncedMyAnimeListUser;
        }

        public ICurrentSeasonModel CurrentSeasonModel { get; }
        public ICurrentUserAnimeListModel CurrentUserAnimeListModel { get; }
        public ISyncedMyAnimeListUser SyncedMyAnimeListUser { get; }
    }
}
