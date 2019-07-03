using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.PageObjects.Pages
{
    public class CheckboxesPage : PageBase
    {
        public CheckboxesPage(IWebDriver driver) : base(driver)
        {
        }

        public IWebElement Checkboxes => Driver.FindElement(By.Id("checkboxes"));
        public IWebElement Checkbox1 => Driver.FindElement(By.XPath("//form[@id='checkboxes']/input[1]"));
        public IWebElement Checkbox2 => Driver.FindElement(By.XPath("//form[@id='checkboxes']/input[2]"));

        public CheckboxesPage NavigateToCheckboxesPage()
        {
            Driver.VaftExt().OpenApplicationBaseUrl("/checkboxes");
            return this;
        }
    }
}