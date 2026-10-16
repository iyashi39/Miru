// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;
using System.IO;
using System.IO.Abstractions;
using MiruLibrary.Models;
using MyInternetConnectionLibrary;
using Newtonsoft.Json;

namespace MiruLibrary
{
    public class FileSystemService : IFileSystemService
    {
        public FileSystemService(IFileSystem fileSystem, IWebService webService, Lazy<UserSettings> userSettings)
        {
            FileSystem = fileSystem;
            WebService = webService;
            UserSettings = userSettings;
            ImageCacheFolder = FileSystem.DirectoryInfo.FromDirectoryName(Constants.ImageCacheFolderPath);

            if (!ImageCacheFolder.Exists)
            {
                ImageCacheFolder.Create();
            }
        }
        public IFileSystem FileSystem { get; }
        public IDirectoryInfo ImageCacheFolder { get; }
        public IWebService WebService { get; }
        public Lazy<UserSettings> UserSettings { get; }

        public void ClearImageCache()
        {
            foreach (IFileInfo file in ImageCacheFolder.EnumerateFiles())
            {
                file.Delete();
            }
        }

        public void DownloadFile(IWebClientWrapper client, string fileLocation, string url)
        {
            if (!FileSystem.File.Exists(fileLocation))
            {
                client.DownloadFile(url, fileLocation);
            }
        }
    }
}