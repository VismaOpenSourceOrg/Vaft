using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;
using Vaft.Framework.Settings;

namespace Vaft.Framework.DriverFactory
{
    public static class BrowserStackDriverFactory
    {
        public static IWebDriver CreateWebDriver()
        {
            var platform = Config.Settings.BrowserStackSettings.BsPlatform;

            switch (platform)
            {
                case "Desktop":
                    return CreateDesktopWebDriver();
                case "Android":
                    return CreateMobileWebDriver();
                case "MAC":
                    return CreateMobileWebDriver();
                default:
                    throw new ArgumentOutOfRangeException("'Platform' value: " + platform);
            }
        }

        private static IWebDriver CreateDesktopWebDriver()
        {
            DesiredCapabilities capabilities;

            var browser = Config.Settings.BrowserStackSettings.BsBrowser;

            switch (browser)
            {
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--disable-popup-blocking");
                    capabilities = chromeOptions.ToCapabilities() as DesiredCapabilities;
                    SetCapabilities(capabilities, "Chrome");
                    return CreateRemoteWebDriver(capabilities);
                case "Firefox":
                    capabilities = DesiredCapabilities.Firefox();
                    SetCapabilities(capabilities, "Firefox");
                    return CreateRemoteWebDriver(capabilities);
                case "IE":
                    capabilities = DesiredCapabilities.InternetExplorer();
                    capabilities.SetCapability("browserstack.ie.enablePopups", "true");
                    SetCapabilities(capabilities, "IE");
                    return CreateRemoteWebDriver(capabilities);
                case "Edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    capabilities = edgeOptions.ToCapabilities() as DesiredCapabilities;
                    SetCapabilities(capabilities, "Edge");
                    return CreateRemoteWebDriver(capabilities);
                case "Safari":
                    capabilities = DesiredCapabilities.Safari();
                    capabilities.SetCapability("browserstack.safari.enablePopups", "true");
                    SetCapabilities(capabilities, "Safari");
                    return CreateRemoteWebDriver(capabilities);
                case "Opera":
                    capabilities = DesiredCapabilities.Opera();
                    SetCapabilities(capabilities, "Opera");
                    return CreateRemoteWebDriver(capabilities);
                default:
                    throw new ArgumentOutOfRangeException("'Browser' value: " + browser);
            }
        }

        private static IWebDriver CreateMobileWebDriver()
        {
            DesiredCapabilities capabilities;

            var browser = Config.Settings.BrowserStackSettings.BsBrowserName;

            switch (browser)
            {
                case "android":
                    capabilities = DesiredCapabilities.Android();
                    SetCapabilities(capabilities, "android");
                    return CreateMobileRemoteWebDriver(capabilities);
                case "iPad":
                    capabilities = DesiredCapabilities.IPad();
                    SetCapabilities(capabilities, "iPad");
                    return CreateMobileRemoteWebDriver(capabilities);
                case "iPhone":
                    capabilities = DesiredCapabilities.IPhone();
                    SetCapabilities(capabilities, "iPhone");
                    return CreateMobileRemoteWebDriver(capabilities);
                default:
                    throw new ArgumentOutOfRangeException("'Browser' value: " + browser);
            }
        }

        private static void SetCapabilities(DesiredCapabilities caps, string browser)
        {
            var user = Config.Settings.BrowserStackSettings.BsUser;
            var key = Config.Settings.BrowserStackSettings.BsKey;
            var tunnel = Config.Settings.BrowserStackSettings.BsTunnel;
            var project = Config.Settings.BrowserStackSettings.BsProjectName;
            var build = Config.Settings.BrowserStackSettings.BsBuildVersion;
            var os = Config.Settings.BrowserStackSettings.BsOs;
            var osVersion = Config.Settings.BrowserStackSettings.BsOsVersion;
            var browserVersion = Config.Settings.BrowserStackSettings.BsBrowserVersion;
            var browserStackDebug = Config.Settings.BrowserStackSettings.BsDebug;
            var acceptSslCerts = Config.Settings.BrowserStackSettings.BsAcceptSslCerts;
            var resolution = Config.Settings.BrowserStackSettings.BsResolution;
            var platform = Config.Settings.BrowserStackSettings.BsPlatform;
            var browserName = Config.Settings.BrowserStackSettings.BsBrowserName;
            var device = Config.Settings.BrowserStackSettings.BsDevice;
            var autoAcceptAlerts = Config.Settings.AppiumSettings.AutoAcceptAlerts;

            if (user == null)
            {
                throw new ConfigurationErrorsException("BrowserStack_User parameter cannot be null in App.config");
            }

            if (key == null)
            {
                throw new ConfigurationErrorsException("BrowserStack_Key parameter cannot be null in App.config");
            }

            if (project == null)
            {
                throw new ConfigurationErrorsException("BrowserStack_Project parameter cannot be null in App.config");
            }

            caps.SetCapability("browserstack.user", user);
            caps.SetCapability("browserstack.key", key);
            caps.SetCapability("project", project);

            if (browser != "android" || browser != "iPad" || browser != "iPhone") //Not applicable for mobile
            {
                caps.SetCapability("browser", browser);
            }

            if (tunnel)
            {
                caps.SetCapability("browserstack.tunnel", "true");
            }

            if (build != null)
            {
                caps.SetCapability("build", build);
            }

            if (os != null)
            {
                caps.SetCapability("os", os);
            }

            if (osVersion != null)
            {
                caps.SetCapability("os_version", osVersion);
            }

            if (browserVersion != null)
            {
                caps.SetCapability("browser_version", browserVersion);
            }

            if (browserStackDebug != null)
            {
                caps.SetCapability("browserstack.debug", browserStackDebug);
            }

            if (acceptSslCerts != null)
            {
                caps.SetCapability("acceptSslCerts", acceptSslCerts);
            }

            if (resolution != null)
            {
                caps.SetCapability("resolution", resolution);
            }

            if (browser == "android" || browser == "iPad" || browser == "iPhone") //Only for mobile
            {
                caps.SetCapability("browserName", browserName);
                caps.SetCapability("platform", platform);
                caps.SetCapability("device", device);
            }

            if (autoAcceptAlerts != null)
            {
                caps.SetCapability("autoAcceptAlerts", autoAcceptAlerts);
            }
        }

        private static IWebDriver CreateRemoteWebDriver(ICapabilities capabilities)
        {
            var driver = new ScreenShotRemoteWebDriver(new Uri("http://hub.browserstack.com/wd/hub/"), capabilities);

            var resolution = Config.Settings.BrowserStackSettings.BsResolution;

            if (resolution == null)
            {
                driver.Manage().Window.Maximize();
            }

            driver.VaftExt().TurnOnImplicitlyWait();

            var detector = new LocalFileDetector();
            driver.FileDetector = detector;

            return driver;
        }

        private static IWebDriver CreateMobileRemoteWebDriver(ICapabilities capabilities)
        {
            var driver = new ScreenShotRemoteWebDriver(new Uri("http://hub.browserstack.com/wd/hub/"), capabilities);

            driver.VaftExt().TurnOnImplicitlyWait();

            var detector = new LocalFileDetector();
            driver.FileDetector = detector;

            return driver;
        }
    }
}
