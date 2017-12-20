using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Vaft.Framework.Driver;

namespace Vaft.Framework.Core
{
    /// <summary>Operations which can be performed with pages, sections or Web elements</summary>
    public abstract class WebOperationsBase
    {
        protected internal IWebDriver Driver { get; set; }

        protected WebOperationsBase(IWebDriver driver)
        {
            Driver = driver;
        }

        public IWebDriver GetDriver()
        {
            return Driver;
        }

        /// <summary>Checks for specified text in page title.</summary>
        /// <param name="text">Text message.</param>
        public WebOperationsBase AssertPageTitle(string text)
        {
            Driver.VaftExt().AssertPageTitle(text);
            return this;
        }

        /// <summary>Checks for specified text to be displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public WebOperationsBase VerifyThatTextIsDisplayed(string text)
        {
            Assert.True(WaitUntilTextIsDisplayed(text));
            return this;
        }

        /// <summary>Waits for specified text to be displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public bool WaitUntilTextIsDisplayed(string text)
        {
            return Driver.VaftExt().WaitUntilTextIsDisplayed(text);
        }

        /// <summary>Waits for specified text to be displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        /// <param name="timeToWait">Time to wait</param>
        public bool WaitUntilTextIsDisplayed(string text, TimeSpan timeToWait)
        {
            return Driver.VaftExt().WaitUntilTextIsDisplayed(text, timeToWait);
        }

        /// <summary>Checks for specified text to be not displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public WebOperationsBase VerifyThatTextIsNotDisplayed(string text)
        {
            Driver.VaftExt().VerifyThatTextIsNotDisplayed(text);
            return this;
        }

        /// <summary>Waits for specified text to be not displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public bool WaitUntilTextIsNotDisplayed(string text)
        {
            return Driver.VaftExt().WaitUntilTextIsNotDisplayed(text);
        }

        /// <summary>Waits for specified text to be not displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        /// <param name="timeToWait">Time to wait</param>
        public bool WaitUntilTextIsNotDisplayed(string text, TimeSpan timeToWait)
        {
            return Driver.VaftExt().WaitUntilTextIsNotDisplayed(text, timeToWait);
        }

        /// <summary>Waits until ajax processing is finished.</summary>
        public void WaitForAjax()
        {
            Driver.VaftExt().WaitForAjax();
        }

        /// <summary>Waits until ajax processing is finished.</summary>
        /// <param name="timeToWait">Text message.</param>
        public void WaitForAjax(TimeSpan timeToWait)
        {
            Driver.VaftExt().WaitForAjax(timeToWait);
        }

        /// <summary>Waits until jquery animation process finished.</summary>
        public void WaitForAnimation()
        {
            Driver.VaftExt().WaitForAnimation();
        }

        /// <summary>Waits until jquery animation process finished.</summary>
        /// <param name="timeToWait">Text message.</param>
        public void WaitForAnimation(TimeSpan timeToWait)
        {
            Driver.VaftExt().WaitForAnimation(timeToWait);
        }

        /// <summary>Waits until Angular's requests are finished.</summary>
        public void WaitForAngular()
        {
            Driver.VaftExt().WaitForAngular();
        }

        /// <summary>Waits until Angular's requests are finished.</summary>
        /// <param name="timeToWait">Text message.</param>
        public void WaitForAngular(TimeSpan timeToWait)
        {
            Driver.VaftExt().WaitForAngular(timeToWait);
        }
    }
}
