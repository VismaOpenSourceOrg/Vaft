using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Element
{
    public class AdvancedUtils
    {
        private readonly IWebElement _element;
        private readonly IWebDriver _driver;

        public AdvancedUtils(IWebElement element, IWebDriver driver)
        {
            _element = element;
            _driver = driver;
        }

        /// <summary>Performs double click on Web element</summary>
        public void DoubleClick()
        {
            var action = new Actions(_driver);
            action.DoubleClick(_element).Perform();
        }

        /// <summary>Performs right click on Web element</summary>
        public void RightClick()
        {
            var action = new Actions(_driver);
            action.ContextClick(_element).Perform();
        }

        /// <summary>Scrolls web page to element and alligns it with the top or bottom scrollIntoView(true/false).</summary>
        /// <param name="allignment">Top or Bottom</param>
        public void ScrollToElement(string allignment = "Bottom")
        {
            var js = (IJavaScriptExecutor)_driver;
            if (Equals(allignment, "Top"))
            {
                js.ExecuteScript("arguments[0].scrollIntoView(true);", _element);
                return;
            }
            if (Equals(allignment, "Bottom"))
            {
                js.ExecuteScript("arguments[0].scrollIntoView(false);", _element);
            }
            else
            {
                throw new ArgumentOutOfRangeException("allignment cannot be '" + allignment + "'. Available option 'Top' or 'Bottom'");
            }
        }

        /// <summary>Uploads file to input element with type="file"</summary>
        /// <param name="filePath">File path</param>
        public void UploadFile(string filePath)
        {
            if (_element.GetAttribute("class").Split(' ').Contains("hidden"))
            {
                WebElementUtils.UnhideElement(_driver, _element);
                _element.SendKeys(filePath);
                WebElementUtils.HideElement(_driver, _element);
                return;
            }
            _element.SendKeys(filePath);
        }
    }
}
