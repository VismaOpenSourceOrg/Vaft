using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.PageObjects.Pages
{
    public class DropdownPage : PageBase    
    {
        public DropdownPage(IWebDriver driver) : base(driver)    
        {
        }

        public IWebElement DropdownElement => Driver.FindElement(By.Id("dropdown"));

        public DropdownPage NavigateToDropdownPage()
        {
            Driver.VaftExt().OpenApplicationBaseUrl("/dropdown");
            return this;
        }
    }
}
