using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;
using Vaft.Framework.Exceptions;
using Vaft.Framework.Settings;

namespace Vaft.Framework.DriverFactory
{
    public class LocalDriverFactory
    {
        public IWebDriver Driver;

        public IWebDriver CreateWebDriver()
        {
            var browser = Config.Settings.RuntimeSettings.SeleniumBrowser;

            switch (browser)
            {
                case BrowserType.Firefox:
                    return CreateFirefoxDriver();
                case BrowserType.Chrome:
                    return CreateChromeDriver();
                case BrowserType.Ie:
                    return CreateIeDriver();
                case BrowserType.Safari:
                    return CreateSafariDriver();
                case BrowserType.Edge:
                    return CreateEdgeDriver();
                default:
                    throw new InvalidBrowserException($"Invalid browser type: '{browser}'.");
            }
        }

        private IWebDriver CreateChromeDriver()
        {
            var chromeServerConfig = Config.Settings.RuntimeSettings.ChromeServerPath;
            var chromeDriverServerPath = AppDomain.CurrentDomain.BaseDirectory + chromeServerConfig;
            var options = new ChromeOptions();
            options.AddArguments("--start-maximized");
            options.AddArguments("--lang=" + Config.Settings.RuntimeSettings.BrowserLanguage);
            options.AddArguments("--test-type");
            options.AddArguments("--disable-popup-blocking");

            var proxy = GetProxy();
            if (proxy != null)
            {
                options.Proxy = proxy;
            }

            InitProfile(options);
            Driver = new ChromeDriver(chromeDriverServerPath, options);
            SetBrowserSize(Driver);
            Driver.VaftExt().TurnOnImplicitlyWait();
            Driver.VaftExt().SetPageLoadTimeout();
            return Driver;
        }

        private IWebDriver CreateFirefoxDriver()
        {
            FirefoxOptions ffOpts = new FirefoxOptions();
            ffOpts.SetPreference("intl.accept_languages", Config.Settings.RuntimeSettings.BrowserLanguage);

            var ffp = new FirefoxProfile();
            ffp.SetPreference("intl.accept_languages", Config.Settings.RuntimeSettings.BrowserLanguage);

            var proxy = GetProxy();
            if (proxy != null)
            {
                ffp.SetProxyPreferences(proxy);
            }

            InitProfile(ffp);

            Driver = new FirefoxDriver(ffOpts);
            SetBrowserSize(Driver);
            Driver.VaftExt().TurnOnImplicitlyWait();
            Driver.VaftExt().SetPageLoadTimeout();
            return Driver;
        }

        private IWebDriver CreateIeDriver()
        {
            var options = new InternetExplorerOptions
            {
                EnableNativeEvents = false,
                RequireWindowFocus = false,
                EnsureCleanSession = true,
                EnablePersistentHover = true,
                IgnoreZoomLevel = true
            };

            var proxy = GetProxy();
            if (proxy != null)
            {
                options.Proxy = proxy;
            }

            options.AddAdditionalCapability("disable-popup-blocking", true);

            InitProfile(options);

            var ieServerConfig = Config.Settings.RuntimeSettings.IeServerPath;
            var ieDriverServerPath = AppDomain.CurrentDomain.BaseDirectory + ieServerConfig;
            Driver = new InternetExplorerDriver(ieDriverServerPath, options);

            Driver.VaftExt().TurnOnImplicitlyWait();
            Driver.VaftExt().SetPageLoadTimeout();
            Driver.Manage().Cookies.DeleteAllCookies();
            SetBrowserSize(Driver);
            return Driver;
        }

        private IWebDriver CreateSafariDriver()
        {
            var options = new SafariOptions();
            Driver = new SafariDriver(options);
            SetBrowserSize(Driver);
            Driver.VaftExt().TurnOnImplicitlyWait();
            Driver.VaftExt().SetPageLoadTimeout();
            return Driver;
        }

        private IWebDriver CreateEdgeDriver()
        {
            var edgeServerConfig = Config.Settings.RuntimeSettings.EdgeServerPath;
            var edgeDriverServerPath = AppDomain.CurrentDomain.BaseDirectory + edgeServerConfig;
            var options = new EdgeOptions();
            options.PageLoadStrategy = (PageLoadStrategy)EdgePageLoadStrategy.Eager;

            InitProfile(options);
            Driver = new EdgeDriver(edgeDriverServerPath, options);
            SetBrowserSize(Driver);
            Driver.VaftExt().TurnOnImplicitlyWait();
            Driver.VaftExt().SetPageLoadTimeout();
            return Driver;
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

        private void InitProfile(object profile)
        {
            var type = Config.Settings.RuntimeSettings.DriverInitializationType;
            if (type == null)
            {
                return;
            }

            if (!typeof(IProfileInitializer).IsAssignableFrom(type))
            {
                Trace.WriteLine("The specified profile initializer type is not of type IProfileInitializer");
                return;
            }

            var initializer = Activator.CreateInstance(type) as IProfileInitializer;
            initializer.InitializeProfile(profile);
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
