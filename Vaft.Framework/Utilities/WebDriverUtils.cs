using System;
using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Vaft.Framework.Element;
using Vaft.Framework.Settings;

namespace Vaft.Framework.Utilities
{
    public class WebDriverUtils
    {
        private readonly IWebDriver _driver;

        public WebDriverUtils(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>Sets Page load timeout defined in App.config</summary>
        public void SetPageLoadTimeout()
        {
            if (Config.Settings.RuntimeSettings.PageLoadTimeout != null)
            {
                _driver.Manage().Timeouts().PageLoad = (TimeSpan)Config.Settings.RuntimeSettings.PageLoadTimeout;
            }
        }

        /// <summary>Sets Page load timeout</summary>
        /// <param name="timeout">timeout</param>
        public void SetPageLoadTimeout(TimeSpan timeout)
        {
            _driver.Manage().Timeouts().PageLoad = timeout;
        }

        /// <summary>Sets Selenium implicitly wait to amount of seconds equal to "SeleniumTimeout" parameter.</summary>
        public void TurnOnImplicitlyWait()
        {
            _driver.Manage().Timeouts().ImplicitWait = Config.Settings.RuntimeSettings.ImplicitWaitTimeout;
        }

        /// <summary>Sets Selenium implicitly wait to 0 seconds.</summary>
        public void TurnOffImplicitlyWait()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }

        /// <summary>Switch WebDriver to new browser window.</summary>
        /// <param name="windowTitle">window title</param>
        public void SwitchToWindow(string windowTitle)
        {
            _driver.SwitchTo().Window(GetWindowHandleByTitle(windowTitle));
        }

        private string GetWindowHandleByTitle(string title)
        {
            var windowHandles = _driver.WindowHandles;
            foreach (string handle in windowHandles)
            {
                if (title.Equals(_driver.SwitchTo().Window(handle).Title))
                {
                    return handle;
                }
            }
            return title;
        }

        /// <summary>Returns ture for Internet Explorer and fale for other browsers.</summary>
        public bool IsInternetExplorer()
        {
            var js = (IJavaScriptExecutor)_driver;
            var browserType = (string)js.ExecuteScript("return navigator.appName;");
            var isIe11 = (bool)js.ExecuteScript("return !!navigator.userAgent.match(/Trident.*rv[ :]?11./);");

            if (Equals(browserType, "Microsoft Internet Explorer") || isIe11)
            {
                return true;
            }

            return false;
        }

        /// <summary>Sets browser window size for given resolution. Resolution example "1024x768"</summary>
        /// <param name="resolution">resolution</param>
        public void SetBrowserSize(string resolution)
        {
            string[] dimensions = resolution.Split('x');
            _driver.Manage().Window.Size = new Size(int.Parse(dimensions[0]), int.Parse(dimensions[1]));
        }

        /// <summary>Sets browser window size</summary>
        /// <param name="width">window width</param>
        /// /// <param name="height">window height</param>
        public void SetBrowserSize(int width, int height)
        {
            _driver.Manage().Window.Size = new Size(width, height);
        }

        /// <summary>Checks for specified text in page title.</summary>
        /// <param name="text">Text message.</param>
        public void AssertPageTitle(string text)
        {
            TurnOffImplicitlyWait();
            new WebDriverWait(_driver, Config.Settings.RuntimeSettings.ExplicitWaitTimeout)
            {
                Message = "Assertion error: \r\n Expected: " + text + "\r\n Actual: " + _driver.Title
            }.Until(
                    VaftExpectedConditions.PageContainsText(text));
            TurnOnImplicitlyWait();
        }

        /// <summary>Waits for specified text to be displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public bool WaitUntilTextIsDisplayed(string text)
        {
            return WaitUntilTextIsDisplayed(text, Config.Settings.RuntimeSettings.ExplicitWaitTimeout);
        }

        /// <summary>Waits for specified text to be displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        /// <param name="timeToWait">Time to wait</param>
        public bool WaitUntilTextIsDisplayed(string text, TimeSpan timeToWait)
        {
            TurnOffImplicitlyWait();
            bool wait =
                new WebDriverWait(_driver, timeToWait)
                {
                    Message = "Expected text was not displayed: '" + text + "'"
                }.Until(
                        VaftExpectedConditions.PageContainsText(text));
            TurnOnImplicitlyWait();
            return wait;
        }

        /// <summary>Checks for specified text to be displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public void VerifyThatTextIsDisplayed(string text)
        {
            NUnit.Framework.Assert.True(WaitUntilTextIsDisplayed(text));
        }

        /// <summary>Checks for specified text to be not displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public void VerifyThatTextIsNotDisplayed(string text)
        {
            NUnit.Framework.Assert.True(WaitUntilTextIsNotDisplayed(text));
        }

        /// <summary>Waits for specified text to be not displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        public bool WaitUntilTextIsNotDisplayed(string text)
        {
            return WaitUntilTextIsNotDisplayed(text, Config.Settings.RuntimeSettings.ExplicitWaitTimeout);
        }

        /// <summary>Waits for specified text to be not displayed in Web page.</summary>
        /// <param name="text">Text message.</param>
        /// <param name="timeToWait">Time to wait</param>
        public bool WaitUntilTextIsNotDisplayed(string text, TimeSpan timeToWait)
        {
            TurnOffImplicitlyWait();
            bool wait =
                new WebDriverWait(_driver, timeToWait)
                {
                    Message = "Unexpected text was displayed: '" + text + "'"
                }.Until(
                        VaftExpectedConditions.PageDoesNotContainText(text));
            TurnOnImplicitlyWait();
            return wait;
        }

        /// <summary>Waits until ajax processing is finished.</summary>
        public void WaitForAjax()
        {
            WaitForAjax(Config.Settings.RuntimeSettings.AjaxWaitSeconds);
        }

        /// <summary>Waits until ajax processing is finished.</summary>
        /// <param name="timeToWait">Time to wait.</param>
        public void WaitForAjax(TimeSpan timeToWait)
        {
            var wait = new WebDriverWait(_driver, timeToWait);
            wait.Until(driver => (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
        }

        /// <summary>Waits until jquery animation process finished.</summary>
        public void WaitForAnimation()
        {
            WaitForAnimation(Config.Settings.RuntimeSettings.AnimationWaitSeconds);
        }

        /// <summary>Waits until jquery animation process finished.</summary>
        /// <param name="timeToWait">Time to wait.</param>
        public void WaitForAnimation(TimeSpan timeToWait)
        {
            var js = (IJavaScriptExecutor)_driver;
            new WebDriverWait(_driver, timeToWait).Until(driver => (bool)js.ExecuteScript("var animated = $(':animated'); if(animated.length > 0) return false; else return true"));
        }

        /// <summary>Waits until Angular's requests are finished.</summary>
        public void WaitForAngular()
        {
            WaitForAngular(Config.Settings.RuntimeSettings.AngularWaitSeconds);
        }

        /// <summary>Waits until Angular's requests are finished.</summary>
        /// <param name="timeToWait">Time to wait.</param>
        public void WaitForAngular(TimeSpan timeToWait)
        {
            new WebDriverWait(_driver, timeToWait).Until(
                driver =>
                {
                    try
                    {
                        ((IJavaScriptExecutor)_driver).ExecuteAsyncScript(
                            "var callback = arguments[arguments.length - 1];" +
                            "angular.element(document.body).injector().get('$browser').notifyWhenNoOutstandingRequests(callback);");
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
        }

        public void ReloadPage()
        {
            _driver.Navigate().Refresh();
        }

        public void Pause(int millisecondsTimeout)
        {
            System.Threading.Thread.Sleep(millisecondsTimeout);
        }

        /// <summary>Opens ApplicationBaseUrl defined in app.config</summary>
        public void OpenApplicationBaseUrl()
        {
            var url = Config.Settings.RuntimeSettings.AppBaseUrl;

            try
            {
                _driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Page: " + url + " did not load within timeout seconds defined!");
                throw;
            }
        }

        /// <summary>Opens ApplicationBaseUrl defined in app.config</summary>
        /// <param name="urlPath">Dynamic part of URL path (excluding base URL).</param>
        public void OpenApplicationBaseUrl(string urlPath)
        {
            var url = Config.Settings.RuntimeSettings.AppBaseUrl + urlPath;

            try
            {
                _driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Page: " + url + " did not load within timeout seconds defined!");
                throw;
            }
        }

        /// <summary>Opens Url in Web browser</summary>
        /// <param name="url">Full URL.</param>
        public void OpenUrl(string url)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Page: " + url + " did not load within timeout seconds defined!");
                throw;
            }
        }

        public WebElementWaitUtils Wait()
        {
            return new WebElementWaitUtils(_driver);
        }

        public WebElementWaitUtils Wait(TimeSpan timeToWait)
        {
            return new WebElementWaitUtils(_driver, timeToWait);
        }

        public WebElementAssertUtils Assert()
        {
            return new WebElementAssertUtils(_driver);
        }

    }
}
