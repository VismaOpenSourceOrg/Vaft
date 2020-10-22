using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace GoogleTests
{
    public class Google_Page : PageBase
    {
        public Google_Page(IWebDriver driver) : base(driver)
        {
        }

        public Google_Page NavigateToHomePage()
        {
            Driver.VaftExt().OpenApplicationBaseUrl();
            return this;
        }
    }
}
