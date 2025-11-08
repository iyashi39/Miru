// Copyright iyashi39 @ https://github.com/iyashi39 
// Licensed under the GNU General Public License v3.0,
// go to https://github.com/iyashi39/Miru/blob/master/LICENSE for full license details.

using OpenQA.Selenium.Appium;

namespace Miru.Tests.UI.AppiumTests
{
    public class ClearCacheTests : AppiumUiTestBase
    {
        [Fact]
        public void CheckClearCacheDialogButtons()
        {
            var button = appSession.FindElement(MobileBy.Name("Clear Cache"));
            Assert.NotNull(button);
            button.Click();
            var closeButton = appSession.FindElement(MobileBy.Name("No"));
            var primaryButton = appSession.FindElement(MobileBy.Name("Yes"));
            Assert.NotNull(closeButton);
            Assert.NotNull(primaryButton);
            closeButton.Click();
        }
    }
}
