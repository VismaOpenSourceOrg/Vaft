using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.PageObjects.Pages
{
    public class CheckboxesPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "checkboxes")]
        public IWebElement Checkboxes { get; set; }

        [FindsBy(How = How.XPath, Using = "//form[@id='checkboxes']/input[1]")]
        public IWebElement Checkbox1 { get; set; }

        [FindsBy(How = How.XPath, Using = "//form[@id='checkboxes']/input[2]")]
        public IWebElement Checkbox2 { get; set; }

        public CheckboxesPage(IWebDriver driver) : base(driver)    
        {
        }

        public CheckboxesPage NavigateToCheckboxesPage()
        {
            Driver.VaftExt().OpenApplicationBaseUrl("/checkboxes");
            return this;
        }
    }
}
