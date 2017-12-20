using System;
using System.Collections;
using System.Text;
using Fasterflect;
using OpenQA.Selenium;

namespace Vaft.Framework.Utilities
{
    public static class WebElementUtils
    {
        /// <summary>Highlights WebElement in browser</summary>
        public static void HighlightElement(IWebDriver driver, IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, " border: 2px solid red;");
        }

        /// <summary>Get WebElement FindsBy attribute</summary>
        public static string GetLocator(IWebElement element)
        {
            try
            {
                var path = new StringBuilder();
                var listOfElements = ((IList)element.UnwrapIfWrapped().GetFieldValue("bys"));
                foreach (var item in listOfElements)
                {
                    path.Append(item.GetFieldValue("description"));
                }
                return path.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static void UnhideElement(IWebDriver driver, IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].classList.remove('hidden');", element);
        }

        public static void HideElement(IWebDriver driver, IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].className += \" \" + 'hidden';", element);
        }

    }
}
