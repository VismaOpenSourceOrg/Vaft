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
            string chromeServerConfig = Config.Settings.RuntimeSettings.ChromeServerPath;
            string chromeDriverServerPath = AppDomain.CurrentDomain.BaseDirectory + chromeServerConfig;

            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--start-maximized");
            chromeOptions.AddArguments("--lang=" + Config.Settings.RuntimeSettings.BrowserLanguage);
            chromeOptions.AddArguments("--test-type");
            chromeOptions.AddArguments("--disable-popup-blocking");
            chromeOptions.AddArgument("--ignore-certificate-errors");

            Proxy proxy = GetProxy();

            if (proxy != null)
            {
                chromeOptions.Proxy = proxy;
            }

            InitProfile(chromeOptions);
            Driver = new ChromeDriver(chromeDriverServerPath, chromeOptions);
            SetBrowserSize(Driver);
            Driver.VaftExt().TurnOnImplicitlyWait();
            Driver.VaftExt().SetPageLoadTimeout();
            return Driver;
        }

        private IWebDriver CreateFirefoxDriver()
        {
            string firefoxDriverConfig = Config.Settings.RuntimeSettings.FirefoxDriverPath;
            string firefoxDriverPath = AppDomain.CurrentDomain.BaseDirectory + firefoxDriverConfig;

            FirefoxOptions firefoxOptions = new FirefoxOptions();
            firefoxOptions.SetPreference("intl.accept_languages", Config.Settings.RuntimeSettings.BrowserLanguage);

            Proxy proxy = GetProxy();
            if (proxy != null)
            {
                firefoxOptions.Proxy = proxy;
            }

            //var ffp = new FirefoxProfile();
            //InitProfile(ffp);
            InitProfile(firefoxOptions);
            Driver = new FirefoxDriver(firefoxDriverPath, firefoxOptions);
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

            Proxy proxy = GetProxy();

            if (proxy != null)
            {
                options.Proxy = proxy;
            }

            options.AddAdditionalOption("disable-popup-blocking", true);

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
            var options = new EdgeOptions {PageLoadStrategy = PageLoadStrategy.Eager};

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

            Proxy proxy = new Proxy
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
            initializer?.InitializeProfile(profile);
        }

        private static void SetBrowserSize(IWebDriver driver)
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
