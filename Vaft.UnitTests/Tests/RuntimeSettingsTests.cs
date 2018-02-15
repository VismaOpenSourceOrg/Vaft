using System;
using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.Framework.Settings;

namespace Vaft.UnitTests.Tests
{
    [TestFixture]
    public class RuntimeSettingsTests
    {
        [SetUp]
        public void Setup()
        {
            Config.Settings = new ConfigurationSettings();
        }

        [Test]
        public void Settings_ShouldGetAppBaseUrl()
        {
            var url = Config.Settings.RuntimeSettings.AppBaseUrl;
            Assert.AreEqual(url, "");
        }

        [Test]
        public void Settings_ShouldGetSeleniumBrowser()
        {
            var browser = Config.Settings.RuntimeSettings.SeleniumBrowser;
            Assert.AreEqual(BrowserType.Firefox, browser);
        }

        [Test]
        public void Settings_ShouldSetSeleniumBrowser()
        {
            Config.Settings.RuntimeSettings.SeleniumBrowser = BrowserType.Chrome;
            var browser = Config.Settings.RuntimeSettings.SeleniumBrowser;
            Assert.AreEqual(BrowserType.Chrome, browser);
        }

        [Test]
        public void Settings_ShouldGetSeleniumGridUrl()
        {
            var gridUrl = Config.Settings.RuntimeSettings.SeleniumGridUrl;
            Assert.AreEqual(gridUrl, "http://localhost:4444/wd/hub");
        }

        [Test]
        public void Settings_ShouldGetExplicitWait()
        {
            TimeSpan expectedTimeout = TimeSpan.FromSeconds(5);
            var acualTimeout = Config.Settings.RuntimeSettings.ExplicitWaitTimeout;
            Assert.AreEqual(acualTimeout, expectedTimeout);
        }

        [Test]
        public void Settings_ShouldGetImplicitWait()
        {
            TimeSpan expectedTimeout = TimeSpan.FromSeconds(5);
            var acualTimeout = Config.Settings.RuntimeSettings.ImplicitWaitTimeout;
            Assert.AreEqual(acualTimeout, expectedTimeout);
        }

        [Test]
        public void Settings_ShouldGetAjaxWaitTimeout()
        {
            TimeSpan expectedTimeout = TimeSpan.FromSeconds(5);
            var acualTimeout = Config.Settings.RuntimeSettings.AjaxWaitSeconds;
            Assert.AreEqual(acualTimeout, expectedTimeout);
        }

        [Test]
        public void Settings_ShouldGetBrowserLanguage()
        {
            var browser = Config.Settings.RuntimeSettings.BrowserLanguage;
            Assert.AreEqual(browser, "en-US");
        }
    }
}
