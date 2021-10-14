using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
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

            ValidateBrowserStackSettings();

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
            var browser = Config.Settings.BrowserStackSettings.BsBrowser;

            switch (browser)
            {
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("--disable-popup-blocking");
                    var chromeDriverOptions = SetDriverOptions(chromeOptions, "Chrome");
                    return CreateRemoteWebDriver(chromeDriverOptions.ToCapabilities());
                case "Firefox":
                    var firefoxOptions = new FirefoxOptions();
                    var firefoxDriverOptions = SetDriverOptions(firefoxOptions, "Firefox");
                    return CreateRemoteWebDriver(firefoxDriverOptions.ToCapabilities());
                case "IE":
                    InternetExplorerOptions ieOptions = new InternetExplorerOptions();
                    ieOptions.AddAdditionalOption("browserstack.ie.enablePopups", "true");
                    var internetExplorerDriverOptions = SetDriverOptions(ieOptions, "IE");
                    return CreateRemoteWebDriver(internetExplorerDriverOptions.ToCapabilities());
                case "Edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    var edgeDriverOptions = SetDriverOptions(edgeOptions, "Edge");
                    return CreateRemoteWebDriver(edgeDriverOptions.ToCapabilities());
                case "Safari":
                    SafariOptions safariOptions = new SafariOptions();
                    safariOptions.AddAdditionalOption("browserstack.safari.enablePopups", "true");
                    var safariDriverOptions = SetDriverOptions(safariOptions, "Safari");
                    return CreateRemoteWebDriver(safariDriverOptions.ToCapabilities());
                case "Opera":
                    OperaOptions operaOptions = new OperaOptions();
                    var operaDriverOptions = SetDriverOptions(operaOptions, "Opera");
                    return CreateRemoteWebDriver(operaDriverOptions.ToCapabilities());
                default:
                    throw new ArgumentOutOfRangeException("'Browser' value: " + browser);
            }
        }

        private static IWebDriver CreateMobileWebDriver()
        {
            var browser = Config.Settings.BrowserStackSettings.BsBrowserName;

            switch (browser)
            {
                case "android":
                    var androidDriverOptions = SetDriverOptions(new ChromeOptions(), "android");
                    return CreateMobileRemoteWebDriver(androidDriverOptions.ToCapabilities());
                case "iPad":
                    var iPadDriverOptions = SetDriverOptions(new SafariOptions(), "iPad");
                    return CreateMobileRemoteWebDriver(iPadDriverOptions.ToCapabilities());
                case "iPhone":
                    var iPhoneDriverOptions = SetDriverOptions(new SafariOptions(), "iPhone");
                    return CreateMobileRemoteWebDriver(iPhoneDriverOptions.ToCapabilities());
                default:
                    throw new ArgumentOutOfRangeException("'Browser' value: " + browser);
            }
        }

        private static DriverOptions SetDriverOptions(DriverOptions options, string browser)
        {
            var tunnel = Config.Settings.BrowserStackSettings.BsTunnel;
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


            options.AddAdditionalOption("browserstack.user", Config.Settings.BrowserStackSettings.BsUser);
            options.AddAdditionalOption("browserstack.key", Config.Settings.BrowserStackSettings.BsKey);
            options.AddAdditionalOption("project", Config.Settings.BrowserStackSettings.BsProjectName);

            options.AddAdditionalOption("browser", "Chrome");


            if (browser != "android" || browser != "iPad" || browser != "iPhone") //Not applicable for mobile
            {
                options.AddAdditionalOption("browser", browser);
            }

            if (tunnel)
            {
                options.AddAdditionalOption("browserstack.tunnel", "true");
            }

            if (build != null)
            {
                options.AddAdditionalOption("build", build);
            }

            if (os != null)
            {
                options.AddAdditionalOption("os", os);
            }

            if (osVersion != null)
            {
                options.AddAdditionalOption("os_version", osVersion);
            }

            if (browserVersion != null)
            {
                options.AddAdditionalOption("browser_version", browserVersion);
            }

            if (browserStackDebug != null)
            {
                options.AddAdditionalOption("browserstack.debug", browserStackDebug);
            }

            if (acceptSslCerts != null)
            {
                options.AddAdditionalOption("acceptSslCerts", acceptSslCerts);
            }

            if (resolution != null)
            {
                options.AddAdditionalOption("resolution", resolution);
            }

            if (browser == "android" || browser == "iPad" || browser == "iPhone") //Only for mobile
            {
                options.AddAdditionalOption("browser", browserName);
                options.AddAdditionalOption("device", device);
                options.AddAdditionalOption("realMobile", "true");
            }

            if (autoAcceptAlerts != null)
            {
                options.AddAdditionalOption("autoAcceptAlerts", autoAcceptAlerts);
            }

            return options;
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

        private static void ValidateBrowserStackSettings()
        {
            if (Config.Settings.BrowserStackSettings.BsUser == null)
            {
                throw new ConfigurationErrorsException("BrowserStack_User parameter cannot be null in App.config");
            }

            if (Config.Settings.BrowserStackSettings.BsKey == null)
            {
                throw new ConfigurationErrorsException("BrowserStack_Key parameter cannot be null in App.config");
            }

            if (Config.Settings.BrowserStackSettings.BsProjectName == null)
            {
                throw new ConfigurationErrorsException("BrowserStack_Project parameter cannot be null in App.config");
            }
        }
    }
}
