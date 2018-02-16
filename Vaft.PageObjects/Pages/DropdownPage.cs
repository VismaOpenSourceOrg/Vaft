using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.PageObjects.Pages
{
    public class DropdownPage : PageBase    
    {
        [FindsBy(How = How.Id, Using = "dropdown")]
        public IWebElement DropdownElement { get; set; }

        public DropdownPage(IWebDriver driver) : base(driver)    
        {
        }

        public DropdownPage NavigateToDropdownPage()
        {
            Driver.VaftExt().OpenApplicationBaseUrl("/dropdown");
            return this;
        }
    }
}
