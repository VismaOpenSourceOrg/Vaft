using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Vaft.Framework.BrowserStack;
using Vaft.Framework.Driver;
using Vaft.Framework.Logging;
using Vaft.Framework.Settings;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Core
{
    public abstract class SpecFlowTestBase : VaftDriver
    {
        protected static Process Process { get; set; }
        public TestContext TestContext { get; set; }
        protected static IVaftLogger VaftLog;

        [BeforeScenario("UI", Order = 0)]
        public static void SetUpWebDriver()
        {
            VaftLog = VaftLogInitializer.Run();
        }

        [BeforeScenario("UI", Order = 0)]
        public void InitPageObjects()
        {
            Config.Settings = new ConfigurationSettings();

            if (!ScenarioContext.Current.ContainsKey("Driver"))
            {
                LaunchWebBrowser();
                ScenarioContext.Current["Driver"] = Driver;
            }
            else
            {
                Driver = (IWebDriver)ScenarioContext.Current["Driver"];
            }

            InitPages();
        }

        [AfterScenario("UI")]
        public void TestTearDown()
        {
            if (ScenarioContext.Current.ContainsKey("Driver"))
            {
                ScenarioContext.Current.Remove("Driver");

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
        }

        public void TakeScreenshotOnFailure()
        {
            if (ScenarioContext.Current.TestError != null)
            {
                TakeScreenshot();
            }
        }

        public string TakeScreenshot()
        {
            return ScreenShot.SaveScreenShot(Driver, GetSfTestName());
        }

        /// <summary>
        /// Virtual method for performing cleanup before WebDriver closes.
        /// The method will not be executed when running tests in debug mode
        /// </summary>
        public virtual void VaftCleanup()
        {
        }

        private string GetSfTestName()
        {
            return ScenarioContext.Current.ScenarioInfo.Title;
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

        public abstract void InitPages();
    }
}
