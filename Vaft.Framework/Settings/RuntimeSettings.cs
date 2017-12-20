using System;
using NLog;
using Vaft.Framework.Core;
using Vaft.Framework.Exceptions;

namespace Vaft.Framework.Settings
{
    public class RuntimeSettings
    {
        public string AppBaseUrl { get; set; }
        public BrowserType SeleniumBrowser { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserLanguage { get; set; }
        private string BrowserResolution { get; set; }
        public bool WindowMaximized { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public string SeleniumGridUrl { get; set; }
        public TimeSpan? PageLoadTimeout { get; set; }
        public TimeSpan ImplicitWaitTimeout { get; set; }
        public TimeSpan ExplicitWaitTimeout { get; set; }
        public TimeSpan AjaxWaitSeconds { get; set; }
        public TimeSpan AnimationWaitSeconds { get; set; }
        public TimeSpan AngularWaitSeconds { get; set; }
        public string ChromeServerPath { get; set; }
        public string IeServerPath { get; set; }
        public string EdgeServerPath { get; set; }
        public string FirefoxDriverPath { get; set; }
        public bool ScreenshotOnFailure { get; set; }
        public string Proxy { get; set; }
        public string DatabaseType { get; set; }
        public Type DriverInitializationType { get; set; }
        public string RunOnRemoteMachine { get; set; }
        public bool SeleniumDebugMode { get; set; }

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [Obsolete("TimeoutSeconds parameter is deprecated. Please use ImplicitWait and ExplicitWait instead.")]
        public TimeSpan? TimeoutSeconds;

        public RuntimeSettings()
        {
            AppBaseUrl = Config.GetSettingValue("ApplicationBaseUrl", null);
            SeleniumBrowser = getBrowserTypeFromConfig();
            BrowserVersion = Config.GetSettingValue("BrowserVersion", null);
            BrowserLanguage = Config.GetSettingValue("BrowserLanguage", "en-US");
            BrowserResolution = Config.GetSettingValue("BrowserResolution", "maximized");

            if (BrowserResolution.Contains("maximized"))
            {
                WindowMaximized = true;
            }
            else
            {
                WindowMaximized = false;
                WindowWidth = ParseWindowWidth();
                WindowHeight = ParseWindowHeight();
            }

            SeleniumGridUrl = Config.GetSettingValue("SeleniumGridUrl", "http://localhost:4444/wd/hub");
            TimeoutSeconds = GetSeleniumTimeout();
            PageLoadTimeout = GetPageLoadTimeout();
            ImplicitWaitTimeout = GetImplicitWaitTimeout();
            ExplicitWaitTimeout = GetExplicitWaitTimeout();
            AjaxWaitSeconds = TimeSpan.FromSeconds(double.Parse(Config.GetSettingValue("AjaxWaitTimeout", "5")));
            AnimationWaitSeconds = TimeSpan.FromSeconds(double.Parse(Config.GetSettingValue("AnimationWaitTimeout", "5")));
            AngularWaitSeconds = TimeSpan.FromSeconds(double.Parse(Config.GetSettingValue("AngularWaitTimeout", "5")));
            ChromeServerPath = Config.GetSettingValue("ChromeServerPath", null);
            IeServerPath = Config.GetSettingValue("IeServerPath", null);
            EdgeServerPath = Config.GetSettingValue("EdgeServerPath", null);
            FirefoxDriverPath = Config.GetSettingValue("FirefoxDriverPath", null);
            ScreenshotOnFailure = Config.GetBooleanSettingValue("ScreenshotOnFailure", false);
            Proxy = Config.GetSettingValue("Proxy", "");
            DatabaseType = Config.GetSettingValue("DatabaseType", "mssql");
            DriverInitializationType = GetDriverInitializationType("DriverInitializationType");
            RunOnRemoteMachine = Config.GetSettingValue("RunOnRemoteMachine", "false");
            SeleniumDebugMode = Config.GetBooleanSettingValue("SeleniumDebugMode", false);

            if (SeleniumDebugMode & RunOnRemoteMachine != "false")
            {
                throw new DebugModeException(
                    "It is not allowed to run Selenium test in debug mode remotely, as it leaves browser window opened.");
            }

            Log.Debug(
                $@"Initializing Runtime settings:
                        AppBaseUrl: {AppBaseUrl}                    
                        SeleniumBrowser: {SeleniumBrowser}
                        ScreenshotOnFailure: {ScreenshotOnFailure}
                        SeleniumDebugMode: {SeleniumDebugMode}");
        }

        /// <summary>Loads the driver initializer type from App.config</summary>
        private Type GetDriverInitializationType(string key)
        {
            var typeName = Config.GetSettingValue(key);

            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            return Type.GetType(typeName);
        }

        private TimeSpan? GetSeleniumTimeout()
        {
            var seleniumTimeout = Config.GetSettingValue("SeleniumTimeout", null);

            if (seleniumTimeout != null)
            {
                return TimeSpan.FromSeconds(double.Parse(seleniumTimeout));
            }

            return null;
        }

        private TimeSpan GetImplicitWaitTimeout()
        {
            if (TimeoutSeconds != null)
            {
                return (TimeSpan)TimeoutSeconds;
            }

            return TimeSpan.FromSeconds(double.Parse(Config.GetSettingValue("ImplicitWaitTimeout", "5")));
        }

        private TimeSpan GetExplicitWaitTimeout()
        {
            if (TimeoutSeconds != null)
            {
                return (TimeSpan)TimeoutSeconds;
            }

            return TimeSpan.FromSeconds(double.Parse(Config.GetSettingValue("ExplicitWaitTimeout", "5")));
        }

        private BrowserType getBrowserTypeFromConfig()
        {
            var browser = Config.GetSettingValue("SeleniumBrowser", "firefox");

            switch (browser)
            {
                case "firefox":
                case "Firefox":
                case "FIREFOX":
                    return BrowserType.Firefox;
                case "chrome":
                case "Chrome":
                case "CHROME":
                    return BrowserType.Chrome;
                case "ie":
                case "Ie":
                case "IE":
                    return BrowserType.Ie;
                case "safari":
                case "Safari":
                case "SAFARI":
                    return BrowserType.Safari;
                case "edge":
                case "Edge":
                case "EDGE":
                    return BrowserType.Edge;
                default:
                    throw new InvalidBrowserException($"Browser '{browser}' is not supported.");
            }
        }

        private int ParseWindowWidth()
        {
            int width;

            string[] dimensions = BrowserResolution.Split('x');

            try
            {
                width = int.Parse(dimensions[0]);
            }
            catch (FormatException e)
            {
                throw new InvalidWindowSizeException($"Invalid window width specified of resolution {BrowserResolution}", e);
            }

            return width;
        }

        private int ParseWindowHeight()
        {
            int height;

            string[] dimensions = BrowserResolution.Split('x');

            try
            {
                height = int.Parse(dimensions[1]);
            }
            catch (FormatException e)
            {
                throw new InvalidWindowSizeException($"Invalid window height specified of resolution {BrowserResolution}", e);
            }

            return height;
        }

        private TimeSpan? GetPageLoadTimeout()
        {
            var pageLoadTimeout = Config.GetSettingValue("PageLoadTimeout", null);

            if (pageLoadTimeout != null)
            {
                return TimeSpan.FromSeconds(double.Parse(pageLoadTimeout));
            }

            return null;
        }
    }
}
