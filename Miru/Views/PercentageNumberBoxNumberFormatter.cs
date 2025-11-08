// Copyright (c) 2022 iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using ModernWpf.Controls;

namespace Miru.Views
{
    public class PercentageNumberBoxNumberFormatter : INumberBoxNumberFormatter
    {
        public string FormatDouble(double value)
        {
            return $"{value * 100}%";
        }

        public double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result / 100;
            }
            return null;
        }
    }
}
