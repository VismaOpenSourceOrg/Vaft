using System;
using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.Framework.DriverFactory;
using Vaft.Framework.Settings;

namespace Vaft.UnitTests.Tests
{
    /// <summary>
    /// The <see cref="WebDriverFactoryTests"/> class implements unit tests for the LocalDriverFactory class
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class WebDriverFactoryTests
    {
        [Test]
        public void CreateWebDriver_ShouldInitializeProfile_WhenFirefox()
        {
            AssertInitialization(BrowserType.Firefox, "OpenQA.Selenium.Firefox.FirefoxProfile");
        }

        [Test]
        public void CreateWebDriver_ShouldInitializeProfile_WhenIE()
        {
            AssertInitialization(BrowserType.Ie, "OpenQA.Selenium.IE.InternetExplorerOptions");
        }

        [Test]
        public void CreateWebDriver_ShouldInitializeProfile_WhenChrome()
        {
            AssertInitialization(BrowserType.Chrome, "OpenQA.Selenium.Chrome.ChromeOptions");
        }

        private void AssertInitialization(BrowserType browserSpec, string profileTypeToString)
        {
            Config.Settings = new ConfigurationSettings
            {
                RuntimeSettings =
                {
                    DriverInitializationType = Type.GetType("Vaft.UnitTests.Tests.TestProfileInitializer, Vaft.UnitTests"),
                    SeleniumBrowser = browserSpec,
                    IeServerPath = "somefolder",
                    ChromeServerPath = "somefolder"
                }
            };

            string profile = null;

            Assert.Throws<ApplicationException>(() =>
            {
                try
                {
                    new LocalDriverFactory().CreateWebDriver();
                }
                catch (ApplicationException ae)
                {
                    profile = ae.Data["profile"] as string;
                    throw;
                }
            });

            Assert.IsNotNull(profile);
            Assert.AreEqual(profileTypeToString, profile);
        }
    }
}
