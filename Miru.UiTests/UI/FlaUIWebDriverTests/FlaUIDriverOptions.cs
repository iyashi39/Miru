// Copyright iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using OpenQA.Selenium.Appium;

namespace Miru.UiTests.UI.FlaUIWebDriverTests
{
    public class FlaUIDriverOptions : AppiumOptions
    {
        public static FlaUIDriverOptions ForApp(string path)
        {
            return new FlaUIDriverOptions()
            {
                PlatformName = "windows",
                AutomationName = "flaui",
                App = path
            };
        }
    }
}
