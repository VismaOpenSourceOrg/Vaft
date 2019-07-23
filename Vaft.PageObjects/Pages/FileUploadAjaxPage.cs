using OpenQA.Selenium;
using Vaft.Framework.Core;

namespace Vaft.PageObjects.Pages
{
    public class FileUploadAjaxPage : PageBase
    {
        public FileUploadAjaxPage(IWebDriver driver)
            : base(driver)
        {
        }

        private IWebElement UploadFileElement => Driver.FindElement(By.Name("file"));

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