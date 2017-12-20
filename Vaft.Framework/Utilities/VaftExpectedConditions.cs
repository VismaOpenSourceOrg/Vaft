using System;
using OpenQA.Selenium;

namespace Vaft.Framework.Utilities
{
    public sealed class VaftExpectedConditions
    {
        ///Prevents a default instance of the class from being created.
        private VaftExpectedConditions()
        {
        }

        /// <summary>An expectation for checking text in a page title.</summary>
        /// <param name="text">Text.</param>
        public static Func<IWebDriver, bool> PageTitleContainsText(string text)
        {
            return driver => driver.Title.Contains(text);
        }

        /// <summary>An expectation for checking that text exists in a page.</summary>
        /// <param name="text">Text.</param>
        public static Func<IWebDriver, bool> PageContainsText(string text)
        {
            return driver => driver.PageSource.Contains(text);
        }

        /// <summary>An expectation for checking that text does not exist in a page.</summary>
        /// <param name="text">Text.</param>
        public static Func<IWebDriver, bool> PageDoesNotContainText(string text)
        {
            return driver => !driver.PageSource.Contains(text);
        }

        /// <summary>An expectation for checking that an element is not present on the DOM of a page.</summary>
        /// <param name="element">Web element.</param>
        public static Func<IWebDriver, bool> ElementDoesNotExist(IWebElement element)
        {
            return driver =>
            {
                bool present;
                try
                {
                    present = element.TagName != null;
                }
                catch (NoSuchElementException)
                {
                    //return true when the find throws an exception.
                    return true;
                }
                //return false when find element succeeds.
                return false;
            };
        }

        /// <summary> An expectation for checking that an element is present on the DOM of a page
        /// and visible. Visibility means that the element is not only displayed but
        /// also has a height and width that is greater than 0.</summary>
        /// <param name="element">Web element.</param>
        public static Func<IWebDriver, IWebElement> ElementIsVisible(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    return ElementIfVisible(element);
                }
                catch (StaleElementReferenceException)
                {
                    return (IWebElement)null;
                }
            };
        }

        private static IWebElement ElementIfVisible(IWebElement element)
        {
            if (element.Displayed)
                return element;
            else
                return (IWebElement)null;
        }

        /// <summary>An expectation for checking that an element is not present on the DOM of a page.</summary>
        /// <param name="element">Web element.</param>
        public static Func<IWebDriver, bool> ElementIsNotVisible(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    // If find element fails, then element does not 
                    // exist, and by definition, cannot then be visible.
                    return true;
                }
            };
        }
    }
}
