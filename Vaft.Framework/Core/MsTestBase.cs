using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Vaft.Framework.BrowserStack;
using Vaft.Framework.Driver;
using Vaft.Framework.Logging;
using Vaft.Framework.Settings;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Core
{
    [TestClass]
    public abstract class MsTestBase : VaftDriver
    {
        protected static Process Process { get; set; }
        public TestContext TestContext { get; set; }
        protected static IVaftLogger VaftLog;
        protected VaftTestContext VaftContext;

        [TestInitialize]
        public void SetUpWebDriver()
        {
            VaftContext = new VaftTestContext
            {
                ClassName = TestContext.FullyQualifiedTestClassName,
                MethodName = TestContext.TestName

            };

            VaftLog = VaftLogInitializer.Run();
            Config.Settings = new ConfigurationSettings();
            RunBeforeBrowserOpens();
            LaunchWebBrowser();
        }

        [TestCleanup]
        public void TestTearDown()
        {
            if (Config.Settings.RuntimeSettings.ScreenshotOnFailure)
            {
                TakeScreenshotOnFailure();
            }

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

        public void TakeScreenshotOnFailure()
        {
            if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
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
