using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
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
    [Ignore("Tests are hanging in TeamCity")]
    public class WebDriverFactoryTests
    {
        [Test]
        public void CreateWebDriver_ShouldInitializeProfile_WhenFirefox()
        {
            AssertInitialization(BrowserType.Firefox, "firefox");
        }

        [Test]
        public void CreateWebDriver_ShouldInitializeProfile_WhenIE()
        {
            AssertInitialization(BrowserType.Ie, "internet explorer");
        }

        [Test]
        public void CreateWebDriver_ShouldInitializeProfile_WhenChrome()
        {
            AssertInitialization(BrowserType.Chrome, "chrome");
        }

        private void AssertInitialization(BrowserType browserSpec, string browserName)
        {
            Config.Settings = new ConfigurationSettings
            {
                RuntimeSettings =
                {
                    DriverInitializationType = Type.GetType("Vaft.UnitTests.Tests.TestProfileInitializer, Vaft.UnitTests"),
                    SeleniumBrowser = browserSpec,
                    IeServerPath = "/somefolder",
                    ChromeServerPath = "/somefolder",
                    FirefoxDriverPath = "/somefolder"
                }
            };
            
            string profile = null;
            JObject driverProfile = null;

            Assert.Throws<ApplicationException>(() =>
            {
                try
                {
                    new LocalDriverFactory().CreateWebDriver();
                }
                catch (ApplicationException ae)
                {
                    string prof = ae.Data["profile"] as string;
                    driverProfile = JObject.Parse(prof);

                    profile = ae.Data["profile"] as string;
                    throw;
                }
            });

            Assert.IsNotNull(profile);
            Assert.AreEqual(browserName, driverProfile.First.First.ToString());

        }
    }
}
