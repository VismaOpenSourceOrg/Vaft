using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Framework.Core;

namespace Vaft.SeleniumNunitTests.Pages
{
    public class FileUploadAjaxPage : PageBase
    {
        [FindsBy(How = How.Name, Using = "file")]
        protected IWebElement UploadFileElement { get; set; }

        public FileUploadAjaxPage(IWebDriver driver)
            : base(driver)
        {
        }

        public FileUploadAjaxPage NavigateToFileUploadAjaxPage()
        {
            Driver.Navigate().GoToUrl("http://valums-file-uploader.github.io/file-uploader/");
            return this;
        }

        public IWebElement GetUploadFileElement()
        {
            return UploadFileElement;
        }
    }
}
