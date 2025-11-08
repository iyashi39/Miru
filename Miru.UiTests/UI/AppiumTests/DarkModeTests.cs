// Copyright iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using OpenQA.Selenium.Appium;

namespace Miru.Tests.UI.AppiumTests
{
    public class DarkModeTests : AppiumUiTestBase
    {
        [Fact]
        public void TurnOffDarkMode()
        {
            var darkModeSwitch = appSession.FindElement(MobileBy.ClassName("ToggleSwitch"));
            Assert.NotNull(darkModeSwitch);
            Assert.Contains("On", darkModeSwitch.Text);
            darkModeSwitch.Click();
            Assert.Contains("Off", darkModeSwitch.Text);
        }

        [Fact]
        public void TurnOnDarkMode()
        {
            var darkModeSwitch = appSession.FindElement(MobileBy.ClassName("ToggleSwitch"));
            Assert.NotNull(darkModeSwitch);
            Assert.Contains("On", darkModeSwitch.Text);
            darkModeSwitch.Click();
            darkModeSwitch.Click();
            Assert.Contains("On", darkModeSwitch.Text);
        }
    }
}
