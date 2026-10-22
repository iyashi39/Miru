// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System;
using System.IO;

namespace MiruLibrary
{
    public static class Constants
    {
        private static readonly string AppDataRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Miru");

        public static string ImageCacheFolderPath { get; } = Path.Combine(AppDataRoot, "MiruCache");
        public static string SettingsPath { get; } = Path.Combine(AppDataRoot, "MiruCache", "config.json");

        /// <summary>
        /// Default url of the Jikan REST API v4 (the Tenrai API v1 uses the same schema).
        /// It can be changed in the user settings (config.json).
        /// </summary>
        public const string DefaultAnimeApiBaseUrl = "http://localhost:8080/v4/";
    }
}