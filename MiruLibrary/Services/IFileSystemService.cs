// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using MyInternetConnectionLibrary;
using System.IO;
using System.IO.Abstractions;

namespace MiruLibrary
{
    public interface IFileSystemService
    {
        IDirectoryInfo ImageCacheFolder { get; }
        IFileSystem FileSystem { get; }

        void ClearImageCache();
        void DownloadFile(IWebClientWrapper client, string fileLocation, string url);
        void GetSenpaiData();
        void UpdateSenpaiData();
    }
}