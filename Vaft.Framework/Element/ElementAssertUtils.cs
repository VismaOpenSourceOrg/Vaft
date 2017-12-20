using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Vaft.Framework.Driver;
using Vaft.Framework.Settings;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Element
{
    public class ElementAssertUtils
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _element;
        private readonly bool _isTimeToWaitDefined;
        private readonly TimeSpan _timeToWait;

        public ElementAssertUtils(IWebElement element, IWebDriver driver)
        {
            _element = element;
            _driver = driver;
            _isTimeToWaitDefined = false;
        }

        public ElementAssertUtils(IWebElement element, IWebDriver driver, TimeSpan timeToWait)
            : this(element, driver)
        {
            _isTimeToWaitDefined = true;
            _timeToWait = timeToWait;
        }

        /// <summary>Assert that element is displayed in Web page. If element is not displayed the method throws an exception.</summary>
        public void IsDisplayed()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element is not displayed. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => _element.Displayed);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>
        ///     Assert that element is not displayed in Web page or doesn't exist in page source. If element is displayed the
        ///     method throws an exception.
        /// </summary>
        public void IsNotDisplayed()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should not be visible. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                VaftExpectedConditions.ElementIsNotVisible(_element));

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>
        ///     Assert that element is not visible, but exists in page source. If element is visible the method throws an
        ///     exception.
        /// </summary>
        public void IsHidden()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should be hidden. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => !_element.Displayed);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Assert that element is enabled. If element is disabled the method throws an exception.</summary>
        public void IsEnabled()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element is not enabled. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => _element.Enabled);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Assert that element is displayed and enabled. If element is disabled the method throws an exception.</summary>
        public void IsEnabledAndDisplayed()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element is not enabled. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => _element.Displayed && _element.Enabled);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Assert that element is disabled. If element is enabled the method throws an exception.</summary>
        public void IsDisabled()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element is not disabled. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => !_element.Enabled);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Assert that element exists in page source. If element does not exist the method throws an exception.</summary>
        public void IsPresent()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element doesn't exist in page source. Locator: " + WebElementUtils.GetLocator(_element)
            }
                .Until(d => !_element.Equals(null));

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Assert that element does not exist in page source. If element exists the method throws an exception.</summary>
        public bool IsNotPresent()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            var wait = new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should not exist in page source. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                VaftExpectedConditions.ElementDoesNotExist(_element));

            _driver.VaftExt().TurnOnImplicitlyWait();

            return wait;
        }

        /// <summary>Assert that element is selected.</summary>
        public void IsSelected()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should be selected. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => _element.Selected);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        /// <summary>Assert that element is selected.</summary>
        public void IsNotSelected()
        {
            _driver.VaftExt().TurnOffImplicitlyWait();

            new WebDriverWait(_driver, GetExplicitWait())
            {
                Message = "Element should not be selected. Locator: " + WebElementUtils.GetLocator(_element)
            }.Until(
                d => !_element.Selected);

            _driver.VaftExt().TurnOnImplicitlyWait();
        }

        [Obsolete("Use TextEquals instead")]
        /// <summary>Assert text of element ignoring case.</summary>
        public void IsTextEqual(string text)
        {
            try
            {
                StringAssert.AreEqualIgnoringCase(text, _element.Text);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        /// <summary>Assert text of element ignoring case.</summary>
        public void TextEquals(string text)
        {
            try
            {
                StringAssert.AreEqualIgnoringCase(text, _element.Text);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        /// <summary>Assert text of element ignoring case.</summary>
        public void TextEquals(string text, string message)
        {
            try
            {
                StringAssert.AreEqualIgnoringCase(text, _element.Text, message);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        /// <summary>Assert element text ends with another text.</summary>
        public void TextEndsWith(string text)
        {
            try
            {
                StringAssert.EndsWith(text, _element.Text);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        /// <summary>Assert element text ends with another text.</summary>
        public void TextEndsWith(string text, string message)
        {
            try
            {
                StringAssert.EndsWith(text, _element.Text, message);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        /// <summary>Assert element text starts with another text.</summary>
        public void TextStartsWith(string text)
        {
            try
            {
                StringAssert.StartsWith(text, _element.Text);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        /// <summary>Assert element text starts with another text.</summary>
        public void TextStartsWith(string text, string message)
        {
            try
            {
                StringAssert.StartsWith(text, _element.Text, message);
            }
            catch (AssertionException)
            {
                WebElementUtils.HighlightElement(_driver, _element);
                throw;
            }
        }

        private TimeSpan GetExplicitWait()
        {
            if (_isTimeToWaitDefined)
                return _timeToWait;

            return Config.Settings.RuntimeSettings.ExplicitWaitTimeout;
        }
    }
}
