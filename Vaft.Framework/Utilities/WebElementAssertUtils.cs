using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Vaft.Framework.Driver;
using Vaft.Framework.Settings;

namespace Vaft.Framework.Utilities
{
    public class WebElementAssertUtils
    {
        private readonly IWebDriver _driver;
        private readonly bool _isTimeToWaitDefined;
        private readonly TimeSpan _timeToWait;

        public WebElementAssertUtils(IWebDriver driver)
        {
            _driver = driver;
            _isTimeToWaitDefined = false;
        }

        public WebElementAssertUtils(IWebDriver driver, TimeSpan timeToWait) : this(driver)
        {
            _driver = driver;
            _isTimeToWaitDefined = true;
            _timeToWait = timeToWait;
        }

        /// <summary>Assert that element is displayed in Web page. If element is not displayed the method throws an exception.</summary>
        public void IsDisplayed(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} was not displayed within {GetExplicitWait()} seconds."
            }.Until(
                d => _driver.FindElement(by).Displayed);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        private TimeSpan GetExplicitWait()
        {
            if (_isTimeToWaitDefined)
                return _timeToWait;

            return Config.Settings.RuntimeSettings.ExplicitWaitTimeout;
        }
    }
}
