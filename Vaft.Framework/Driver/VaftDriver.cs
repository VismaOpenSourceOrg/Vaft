using System;
using NLog;
using OpenQA.Selenium;
using Vaft.Framework.BrowserStack;
using Vaft.Framework.DriverFactory;
using Vaft.Framework.Settings;

namespace Vaft.Framework.Driver
{
    public abstract class VaftDriver
    {
        [ThreadStatic] public static IWebDriver Driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void LaunchWebBrowser()
        {
            var runOnRemote = Config.Settings.RuntimeSettings.RunOnRemoteMachine;

            if (runOnRemote == "SeleniumGrid")
            {
                Driver = new RemoteDriverFactory().CreateWebDriver();
            }

            else if (runOnRemote == "BrowserStack")
            {
                if (Config.Settings.BrowserStackSettings.BsTunnel)
                {
                    BsTunnel.LaunchTunnel();
                    Driver = BrowserStackDriverFactory.CreateWebDriver();
                }
                else
                {
                    Driver = BrowserStackDriverFactory.CreateWebDriver();
                }
            }

            else if (runOnRemote == "Appium")
            {
                Driver = AppiumDriverFactory.CreateAppiumDriver();
            }

            else
            {
                Logger.Debug("Creating Web Driver...");
                Driver = new LocalDriverFactory().CreateWebDriver();
                //try
                //{
                //    Driver = new LocalDriverFactory().CreateWebDriver();
                //}
                //catch (WebDriverException)
                //{
                //    CloseWebBrowser();
                //}

            }
        }

        public void CloseWebBrowser()
        {
            if (Driver != null & !Config.Settings.RuntimeSettings.SeleniumDebugMode)
            {
                Logger.Debug("Closing Web Driver");
                Driver.Quit(); // close entire webDriver session
            }

            Driver = null;
        }
    }
}
