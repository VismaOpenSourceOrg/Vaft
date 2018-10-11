using System;
using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;
using Vaft.Framework.Settings;

namespace Vaft.Framework.DriverFactory
{
    public class RemoteDriverFactory
    {
        public IWebDriver Driver;

        public IWebDriver CreateWebDriver()
        {
            var browser = Config.Settings.RuntimeSettings.SeleniumBrowser;

            switch (browser)
            {
                case BrowserType.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--lang=" + Config.Settings.RuntimeSettings.BrowserLanguage);
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArguments("--disable-popup-blocking");
                    return CreateRemoteWebDriver(chromeOptions.ToCapabilities());
                case BrowserType.Firefox:
                    var firefoxProfile = new FirefoxProfile();
                    var firefoxOptions = new FirefoxOptions();
                    firefoxProfile.SetPreference("intl.accept_languages", Config.Settings.RuntimeSettings.BrowserLanguage);
                    firefoxProfile.ToBase64String();
                    firefoxOptions.Profile = firefoxProfile;
                    return CreateRemoteWebDriver(firefoxOptions.ToCapabilities());
                case BrowserType.Ie:
                    var ieOptions = new InternetExplorerOptions
                    {
                        IgnoreZoomLevel = true,
                        EnableNativeEvents = false
                    };

                    ieOptions.AddAdditionalCapability("disable-popup-blocking", true);

                    var browserVersion = Config.Settings.RuntimeSettings.BrowserVersion;
                    if (browserVersion != null)
                    {
                        ieOptions.AddAdditionalCapability(CapabilityType.Version, int.Parse(browserVersion), true);


                        return CreateRemoteWebDriver(ieOptions.ToCapabilities());
                    }
                    return CreateRemoteWebDriver(ieOptions.ToCapabilities());
                case BrowserType.Safari:
                    throw new NotImplementedException("Remote WebDriver Safari is not implemented");
                case BrowserType.Edge:
                    throw new NotImplementedException("Remote WebDriver Edge is not implemented");
                default:
                    throw new InvalidEnumArgumentException($"Invalid BrowserType specified {browser}.");
            }
        }

        private IWebDriver CreateRemoteWebDriver(ICapabilities capabilities)
        {
            SetProxyIfNeeded(capabilities);
            var driver = new ScreenShotRemoteWebDriver(new Uri(Config.Settings.RuntimeSettings.SeleniumGridUrl), capabilities);

            SetBrowserSize(driver);
            driver.VaftExt().TurnOnImplicitlyWait();
            driver.VaftExt().SetPageLoadTimeout();

            var detector = new LocalFileDetector();
            driver.FileDetector = detector;
            Driver = driver;
            return Driver;
        }

        private void SetProxyIfNeeded(ICapabilities capabilities)
        {
            var proxy = GetProxy();
            if (proxy == null)
            {
                return;
            }

            var desiredCapabilities = capabilities as DesiredCapabilities;
            if (desiredCapabilities != null)
            {
                desiredCapabilities.SetCapability(CapabilityType.Proxy, proxy);
            }
            else
            {
                var options = capabilities as ChromeOptions;
                options.AddAdditionalCapability(CapabilityType.Proxy, proxy);
            }
        }

        private Proxy GetProxy()
        {
            var proxyParameter = Config.Settings.RuntimeSettings.Proxy;
            if (string.IsNullOrEmpty(proxyParameter))
            {
                return null;
            }

            var proxy = new Proxy
            {
                IsAutoDetect = false,
                Kind = ProxyKind.Manual,
                HttpProxy = proxyParameter,
                SslProxy = proxyParameter
            };
            return proxy;
        }

        private void SetBrowserSize(IWebDriver driver)
        {
            if (Config.Settings.RuntimeSettings.WindowMaximized)
            {
                driver.Manage().Window.Maximize();
            }
            else
            {
                driver.VaftExt().SetBrowserSize(
                    Config.Settings.RuntimeSettings.WindowWidth,
                    Config.Settings.RuntimeSettings.WindowHeight);
            }
        }
    }
}
