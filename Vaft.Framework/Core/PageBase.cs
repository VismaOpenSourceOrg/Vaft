using OpenQA.Selenium;

namespace Vaft.Framework.Core
{
    public abstract class PageBase : WebOperationsBase
    {
        public string BaseUrl;

        protected PageBase(IWebDriver driver)
            : base(driver)
        {
        }
    }
}
