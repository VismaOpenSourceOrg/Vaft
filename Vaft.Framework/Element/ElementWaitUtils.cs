using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Vaft.Framework.Driver;
using Vaft.Framework.Settings;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Element
{
    public class ElementWaitUtils
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _element;
        private readonly bool _isTimeToWaitDefined;
        private readonly string _locator;
        private readonly TimeSpan _timeToWait;

        public ElementWaitUtils(IWebElement element, IWebDriver driver)
        {
            _element = element;
            _locator = WebElementUtils.GetLocator(_element);
            _driver = driver;
            _isTimeToWaitDefined = false;
        }

        public ElementWaitUtils(IWebElement element, IWebDriver driver, TimeSpan timeToWait)
            : this(element, driver)
        {
            _element = element;
            _locator = WebElementUtils.GetLocator(_element);
            _driver = driver;
            _isTimeToWaitDefined = true;
            _timeToWait = timeToWait;
        }

        /// <summary>Waits for specified element to be removed from the Web page (DOM).</summary>
        public void WaitUntilNotExist()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            var wait = new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should not exist. Locator: " + _locator
            };

            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(VaftExpectedConditions.ElementDoesNotExist(_element));

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Waits for specified element to be visible.</summary>
        public IWebElement WaitUntilVisible()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            var wait = new WebDriverWait(_driver, GetExplicitWait());
            wait.Message = "Element should be visible. Locator: " + _locator;
            var element = wait.Until(VaftExpectedConditions.ElementIsVisible(_element));

            _driver.VaftExt().TurnOnImplicitlyWait();

            return element;
        }

        /// <summary>Waits for specified element to be invisible.</summary>
        public bool WaitUntilInvisible()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();
            var wait =
                new WebDriverWait(_driver, GetExplicitWait())
                {
                    Message = "Element should be not visible. Locator: " + _locator
                }.Until(
                    VaftExpectedConditions.ElementIsNotVisible(_element));
            _driver.VaftExt().TurnOnImplicitlyWait();
            return wait;
        }

        /// <summary>Waits for specified element to be visible and enabled.</summary>
        public IWebElement WaitUntilVisibleAndEnabled()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();
            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should be visible and be enabled. Locator: " + _locator
            }.Until(
                driver => _element.Displayed && _element.Enabled);
            _driver.VaftExt().TurnOnImplicitlyWait();
            return _element;
        }

        /// <summary>Waits for specified element to be disabled.</summary>
        public IWebElement WaitUntilDisabled()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();
            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should be disabled. Locator: " + _locator
            }.Until(
                driver => _element.Displayed && !_element.Enabled);
            _driver.VaftExt().TurnOnImplicitlyWait();
            return _element;
        }

        /// <summary>Waits for specified element to be clickable.</summary>
        public IWebElement WaitUntilClickable()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();
            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element is not clickable. Locator: " + _locator
            }.Until(ExpectedConditions.ElementToBeClickable(_element));
            _driver.VaftExt().TurnOnImplicitlyWait();
            return _element;
        }

        private TimeSpan GetExplicitWait()
        {
            if (_isTimeToWaitDefined)
                return _timeToWait;

            return Config.Settings.RuntimeSettings.ExplicitWaitTimeout;
        }
    }
}
