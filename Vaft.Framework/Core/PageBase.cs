using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace Vaft.Framework.Core
{
    public abstract class PageBase : WebOperationsBase
    {
        public string BaseUrl;

        protected PageBase(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }
    }
}
