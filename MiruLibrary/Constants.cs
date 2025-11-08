// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;
using System.IO;

namespace MiruLibrary
{
    public static class Constants
    {
        public static string SenpaiFilePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"MiruCache\senpai-data.json");

        public static string ImageCacheFolderPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "MiruCache");
        public static string SettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"MiruCache\config.json");
    }
}