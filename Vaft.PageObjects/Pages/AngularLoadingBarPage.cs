using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Framework.Core;

namespace Vaft.PageObjects.Pages
{
    public class AngularLoadingBarPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "loading-bar-spinner")]
        protected IWebElement Spinner { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/div/a[1]")]
        protected IWebElement StartButton { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/div/a[3]")]
        protected IWebElement RealExampleButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='panel panel-default ng-scope'][1]")]
        protected IWebElement Post1 { get; set; }

        public AngularLoadingBarPage(IWebDriver driver)
            : base(driver)
        {
        }

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

        public IWebElement GetRealExampleButton()
        {
            return RealExampleButton;
        }

        public void ClickRealExampleButton()
        {
            GetRealExampleButton().Click();
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
