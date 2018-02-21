using System;
using OpenQA.Selenium;
using Vaft.Framework.BrowserStack;
using Vaft.Framework.Driver;
using Vaft.Framework.Logging;
using Vaft.Framework.Settings;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Core
{
    public class XUnitTestBase : VaftDriver, IDisposable
    {
        protected static IVaftLogger VaftLog;
        protected VaftTestContext VaftContext;

        public XUnitTestBase()
        {
            VaftContext = new VaftTestContext
            {
                ClassName = "NoneAvailable"
            };

            VaftLog = VaftLogInitializer.Run();
            Config.Settings = new ConfigurationSettings();
            //RunBeforeBrowserOpens();
            LaunchWebBrowser();
        }

        public void Dispose()
        {
            if (!Config.Settings.RuntimeSettings.SeleniumDebugMode)
            {
                VaftCleanup();
            }

            CloseDriver();
        }

        public IWebDriver GetDriver()
        {
            return Driver;
        }

        public void CloseDriver()
        {
            CloseWebBrowser();

            if (Config.Settings.RuntimeSettings.RunOnRemoteMachine == "BrowserStack" &
                Config.Settings.BrowserStackSettings.BsTunnel)
            {
                BsTunnel.StopTunnel();
            }
        }

        public string TakeScreenshot()
        {
            return ScreenShot.SaveScreenShot(Driver, VaftContext.FullName);
        }

        /// <summary>
        /// Virtual method for performing cleanup before WebDriver closes.
        /// The method will not be executed when running tests in debug mode
        /// </summary>
        public virtual void VaftCleanup() { }

        /// <summary>Virtual method for performing setup before WebDriver opens.</summary>
        public virtual void RunBeforeBrowserOpens() { }
    }
}
