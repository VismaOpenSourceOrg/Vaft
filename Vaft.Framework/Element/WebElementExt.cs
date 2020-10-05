using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using Vaft.Framework.Driver;

namespace Vaft.Framework.Element
{
    public static class WebElementExt
    {
        /// <summary>WebElement extension methods for checking WebElement condition.</summary>
        [Obsolete("Use Check() instead of CheckElement()")]
        public static ElementCheckUtils CheckElement(this IWebElement element)
        {
            return new ElementCheckUtils(element);
        }

        /// <summary>WebElement extension methods for checking WebElement condition.</summary>
        public static ElementCheckUtils Check(this IWebElement element)
        {
            return new ElementCheckUtils(element);
        }

        /// <summary>WebElement extension methods for checking WebElement condition.</summary>
        /// <param name="element">WebElement</param>
        /// <param name="timeToWait">Time to wait</param>
        public static ElementCheckUtils Check(this IWebElement element, TimeSpan timeToWait)
        {
            return new ElementCheckUtils(element, timeToWait);
        }

        /// <summary>WebElement extension methods for asserting WebElement attributes.</summary>
        [Obsolete("Use Assert() instead of AssertElement()")]
        public static ElementAssertUtils AssertElement(this IWebElement element)
        {
            return new ElementAssertUtils(element, ToDriver(element));
        }

        /// <summary>WebElement extension methods for asserting WebElement attributes.</summary>
        public static ElementAssertUtils Assert(this IWebElement element)
        {
            return new ElementAssertUtils(element, ToDriver(element));
        }

        /// <summary>WebElement extension methods for asserting WebElement attributes.</summary>
        /// <param name="element">WebElement</param>
        /// <param name="timeToWait">time to wait</param>
        public static ElementAssertUtils Assert(this IWebElement element, TimeSpan timeToWait)
        {
            return new ElementAssertUtils(element, ToDriver(element), timeToWait);
        }

        /// <summary>WebElement extension methods for waiting until WebElement matches given condition.</summary>
        [Obsolete("Please use Driver.VaftExt.Wait() instead.")]
        public static ElementWaitUtils Wait(this IWebElement element)
        {
            return new ElementWaitUtils(element, ToDriver(element));
        }

        /// <summary>WebElement extension methods for waiting until WebElement matches given condition.</summary>
        /// <param name="element">WebElement</param>
        /// <param name="timeToWait">time to wait</param>
        [Obsolete("Please use Driver.VaftExt.Wait(TimeSpan timeToWait) instead.")]
        public static ElementWaitUtils Wait(this IWebElement element, TimeSpan timeToWait)
        {
            return new ElementWaitUtils(element, ToDriver(element), timeToWait);
        }

        /// <summary>WebElement extension methods for dropdown.</summary>
        public static DropdownUtils Dropdown(this IWebElement element)
        {
            return new DropdownUtils(element);
        }

        /// <summary>WebElement extension methods for performing various actions.</summary>
        public static AdvancedUtils AdvancedAction(this IWebElement element)
        {
            return new AdvancedUtils(element, ToDriver(element));
        }

        /// <summary>WebElement extension methods for checkbox.</summary>
        public static CheckboxUtils Checkbox(this IWebElement element)
        {
            return new CheckboxUtils(element);
        }

        private static IWebDriver ToDriver(IWebElement element)
        {
            //var realElement = element.GetType() != typeof(RemoteWebElement) ? element : ((IWrapsElement)element).WrappedElement;
            //return ((IWrapsDriver)realElement).WrappedDriver;


            var wrapsDriver = element as IWrapsDriver;

            if (wrapsDriver != null)
            {
                return wrapsDriver.WrappedDriver;
            }
            else if (VaftDriver.Driver != null)
            {
                return VaftDriver.Driver;
            }
            else
            {
                throw new Exception("Please pass Driver for WebElement extension method");
            }
        }
    }
}
