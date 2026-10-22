// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace MiruLibrary.Models
{
    // data model for the anime entries used by this app
    public class MiruAnimeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MalId { get; set; }
        public string Broadcast { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string ImageURL { get; set; }

        private string _localImagePath;
        public string LocalImagePath 
        { 
            get => _localImagePath;
            set
            {
                if (value.Contains(@"\"))
                {
                    _localImagePath = value;
                }
                else
                {
                    _localImagePath = Path.Combine(Constants.ImageCacheFolderPath, $"{ value }.jpg");
                }
            }
        }

        // returns cached image to the view
        public BitmapImage LocalImageSource
        {
            get
            {
                if (!File.Exists(LocalImagePath))
                {
                    return null;
                }
                var source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(LocalImagePath, UriKind.RelativeOrAbsolute);
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
                return source;
            }
        }
        public DateTime? LocalBroadcastTime { get; set; }
        public DateTime? JSTBroadcastTime { get; set; }
        public bool IsOnWatchingList { get; set; }

        // is the anime currently airing status flag
        public bool CurrentlyAiring { get; set; }
        public int? WatchedEpisodes { get; set; }
        public string AgeRating { get; set; } // "G", "PG", "R" etc.

        // "Movie", "TV" etc.
        public string Type { get; set; }

        private int? _totalEpisodes;

        public int? TotalEpisodes
        {
            get
            {
                if (_totalEpisodes == 0)
                {
                    return null;
                }
                return _totalEpisodes;
            }
            set { _totalEpisodes = value; }
        }

        public bool Dropped { get; set; }

        private string _openingThemes;
        //     Anime's opening themes numerically indexed with array values.
        public string OpeningThemes 
        {
            get => FormatThemesOutput("OP", _openingThemes);
            set => _openingThemes = value;
        }
        private string _endingThemes;

        //     Anime's ending themes numerically indexed with array values.
        public string EndingThemes
        {
            get => FormatThemesOutput("ED", _endingThemes);
            set => _endingThemes = value;
        }

        public string FormatThemesOutput(string themeType, string themesJsonContent)
        {
            if (string.IsNullOrWhiteSpace(themesJsonContent))
            {
                return $"{themeType}\n";
            }

            StringBuilder uiOutput = new StringBuilder($"{themeType}\n");

            try
            {
                var token = JToken.Parse(themesJsonContent);

                if (token.Type == JTokenType.Array)
                {
                    foreach (var theme in token)
                    {
                        uiOutput.Append($"{theme}\n");
                    }
                }
                else if (token.Type != JTokenType.Null)
                {
                    // a single theme saved as a json string
                    uiOutput.Append($"{token}\n");
                }
            }
            catch (JsonException)
            {
                // the content is not a json (e.g. it was saved by an older version of the app)
                uiOutput = new StringBuilder(themesJsonContent);
            }

            return uiOutput.ToString();
        }

        // Older app versions saved only the first theme as a plain json string.
        // Themes stored this way (or not as a json array at all) should be refreshed from the api.
        public bool OpeningThemesStoredWithoutList => IsThemeStoredWithoutList(_openingThemes);
        public bool EndingThemesStoredWithoutList => IsThemeStoredWithoutList(_endingThemes);

        private static bool IsThemeStoredWithoutList(string themesJsonContent)
        {
            if (string.IsNullOrWhiteSpace(themesJsonContent))
            {
                return false;
            }

            try
            {
                return JToken.Parse(themesJsonContent).Type != JTokenType.Array;
            }
            catch (JsonException)
            {
                // plain text saved by an older version of the app
                return true;
            }
        }
    }
}