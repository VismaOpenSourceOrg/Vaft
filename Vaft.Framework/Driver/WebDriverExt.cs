using OpenQA.Selenium;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Driver
{
    public static class WebDriverExt
    {
        public static WebDriverUtils VaftExt(this IWebDriver driver)
        {
            return new WebDriverUtils(driver);
        }
    }
}
