using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using Vaft.Framework.Settings;

namespace Vaft.Framework.DriverFactory
{
    public static class AppiumDriverFactory
    {
        public static IWebDriver CreateAppiumDriver()
        {
            string platformName = Config.Settings.AppiumSettings.PlatformName;
            switch (platformName)
            {
                case "Android":
                    return CreateAndroidDriver();
                case "iOS":
                    throw new NotImplementedException("iOS driver is not implemented in VAFT");
                default:
                    throw new InvalidOperationException("Unexpected value platformName = " + platformName);
            }
        }

        private static IWebDriver CreateAndroidDriver()
        {
            var capabilities = new DesiredCapabilities();

            capabilities.SetCapability("platformName", Config.Settings.AppiumSettings.PlatformName);
            capabilities.SetCapability("platformVersion", Config.Settings.AppiumSettings.PlatformVersion);

            if (Config.Settings.AppiumSettings.DeviceName == null)
            {
                throw new ArgumentNullException("deviceName", "Parameter cannot be null");
            }

            capabilities.SetCapability("deviceName", Config.Settings.AppiumSettings.DeviceName);
            capabilities.SetCapability("browserName", Config.Settings.AppiumSettings.BrowserName);

            IWebDriver driver = new AppiumDriver(new Uri(Config.Settings.AppiumSettings.AppiumHubUrl), capabilities);
            return driver;
        }
    }
}
