using System.Diagnostics;
using NLog;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Vaft.Framework.BrowserStack;
using Vaft.Framework.Driver;
using Vaft.Framework.Logging;
using Vaft.Framework.Settings;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Core
{
    public class TestBase : VaftDriver
    {
        protected static Process Process { get; set; }
        protected Logger Log { get; }

        protected static IVaftLogger VaftLog;
        protected VaftTestContext VaftContext;

        public TestBase()
        {
            Log = LogManager.GetLogger(GetType().FullName);
        }

        [OneTimeSetUp]
        public void SetUpWebDriver()
        {
            VaftContext = new VaftTestContext
            {
                ClassName = TestContext.CurrentContext.Test.Name
            };

            VaftLog = VaftLogInitializer.Run();
            Log.Debug("----- TEST STARTED -----");

            Config.Settings = new ConfigurationSettings();
            RunBeforeBrowserOpens();
            LaunchWebBrowser();
        }

        [OneTimeTearDown]
        public void CloseWebDriver()
        {
            CloseDriver();
        }

        [SetUp]
        public void SetTestMethodName()
        {
            VaftContext.MethodName = TestContext.CurrentContext.Test.MethodName;
        }

        [TearDown]
        public void TearDown()
        {
            if (Config.Settings.RuntimeSettings.ScreenshotOnFailure)
            {
                TakeScreenshotOnFailure();
            }

            if (!Config.Settings.RuntimeSettings.SeleniumDebugMode)
            {
                VaftCleanup();
            }
        }

        public IWebDriver GetDriver()
        {
            return Driver;
        }

        private void CloseDriver()
        {
            CloseWebBrowser();

            if (Config.Settings.RuntimeSettings.RunOnRemoteMachine == "BrowserStack" &
                Config.Settings.BrowserStackSettings.BsTunnel)
            {
                BsTunnel.StopTunnel();
            }
        }

        public void TakeScreenshotOnFailure()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                TakeScreenshot();
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
