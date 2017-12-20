using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Vaft.Framework.Core
{
    public class ScreenShotRemoteWebDriver : RemoteWebDriver, ITakesScreenshot
    {
        public ScreenShotRemoteWebDriver(Uri remoteAddress, ICapabilities capabilities)
            : base(remoteAddress, capabilities)
        {
        }

        public new Screenshot GetScreenshot()
        {
            Response screenshotResponse = Execute(DriverCommand.Screenshot, null);
            string base64 = screenshotResponse.Value.ToString();
            return new Screenshot(base64);
        }
    }
}
