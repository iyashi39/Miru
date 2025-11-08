// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiruLibrary.Models
{
    // model for senpai JSON data serialization
    [ExcludeFromCodeCoverage]
    public class SenpaiEntryModel
    {
        public List<Item> Items { get; set; }

        public class Item
        {
            public long MalId { get; set; }
            public string Airdate { get; set; }
        }
    }
}