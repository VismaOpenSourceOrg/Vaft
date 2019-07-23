using OpenQA.Selenium;
using Vaft.Framework.Core;

namespace Vaft.PageObjects.Pages
{
    public class AngularLoadingBarPage : PageBase
    {
        public AngularLoadingBarPage(IWebDriver driver)
            : base(driver)
        {
        }

        private IWebElement Spinner => Driver.FindElement(By.Id("loading-bar-spinner"));
        private IWebElement StartButton => Driver.FindElement(By.XPath("/html/body/div[2]/div/a[1]"));
        private IWebElement RealExampleButton => Driver.FindElement(By.XPath("/html/body/div[2]/div/a[3]"));
        private IWebElement Post1 => Driver.FindElement(By.XPath("//div[@class='panel panel-default ng-scope'][1]"));

        public AngularLoadingBarPage NavigateToAngularPage()
        {
            Driver.Navigate().GoToUrl("http://chieffancypants.github.io/angular-loading-bar/");
            return this;
        }

        public IWebElement GetSpinner()
        {
            return Spinner;
        }

        public void ClickStartButton()
        {
            StartButton.Click();
        }

        public void ClickRealExampleButton()
        {
            RealExampleButton.Click();
        }

        public IWebElement GetFirstPost()
        {
            return Post1;
        }

        public IWebElement GetSecondPost()
        {
            return Driver.FindElement(By.XPath("//div[@class='panel panel-default ng-scope'][2]"));
        }
    }
}
