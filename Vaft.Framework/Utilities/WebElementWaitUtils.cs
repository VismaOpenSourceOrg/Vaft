using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Vaft.Framework.Driver;
using Vaft.Framework.Settings;

namespace Vaft.Framework.Utilities
{
    public class WebElementWaitUtils
    {
        private readonly IWebDriver _driver;
        private readonly bool _isTimeToWaitDefined;
        private readonly TimeSpan _timeToWait;

        public WebElementWaitUtils(IWebDriver driver)
        {
            _driver = driver;
            _isTimeToWaitDefined = false;
        }

        public WebElementWaitUtils(IWebDriver driver, TimeSpan timeToWait) : this(driver)
        {
            _driver = driver;
            _isTimeToWaitDefined = true;
            _timeToWait = timeToWait;
        }

        public void WaitUntilExist(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            var wait = new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} was not present within {GetExplicitWait()} seconds."
            };

            wait.Until(VaftExpectedConditions.ElementPresent(_driver, by));

            _driver.VaftExt().TurnOffImplicitlyWait();
        }

        /// <summary>Waits for specified element to be removed from the Web page (DOM).</summary>
        public void WaitUntilNotExist(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            var wait = new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} did not disappear within {GetExplicitWait()} seconds."
            };

            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(VaftExpectedConditions.ElementNotPresent(_driver, by));

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Waits for specified element to be visible.</summary>
        public void WaitUntilVisible(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            var wait = new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} was not displayed within {GetExplicitWait()} seconds."
            };
            var element = wait.Until(VaftExpectedConditions.ElementIsVisible(_driver, by));

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Waits for specified element to be invisible.</summary>
        public void WaitUntilNotVisible(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();
            
            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} did not disappear within {GetExplicitWait()} seconds."
            }.Until(
                VaftExpectedConditions.ElementIsNotVisible(_driver, @by));
            
            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Waits for specified element to be visible and enabled.</summary>
        public void WaitUntilVisibleAndEnabled(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} was not displayed or enabled within {GetExplicitWait()} seconds."
            }.Until(
                driver => VaftExpectedConditions.ElementDisplayedAndEnabled(_driver, by));

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Waits for specified element to be visible and disabled.</summary>
        public void WaitUntilVisibleAndDisabled(By by)
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = $@"Element located by {by} was not displayed or disabled within {GetExplicitWait()} seconds."
            }.Until(
                driver => _driver.FindElement(by).Displayed && !_driver.FindElement(by).Enabled);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        private TimeSpan GetExplicitWait()
        {
            if (_isTimeToWaitDefined)
            {
                return _timeToWait;
            }

            return Config.Settings.RuntimeSettings.ExplicitWaitTimeout;
        }
    }
}
