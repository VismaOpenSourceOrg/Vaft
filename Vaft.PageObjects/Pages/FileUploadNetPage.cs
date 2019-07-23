using OpenQA.Selenium;
using Vaft.Framework.Core;

namespace Vaft.PageObjects.Pages
{
    public class FileUploadNetPage : PageBase
    {
        public FileUploadNetPage(IWebDriver driver)
            : base(driver)
        {
        }

        private IWebElement UploadWithoutJavaScriptLink => Driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/div/div/p[2]/a"));
        private IWebElement BrowseFileInputField => Driver.FindElement(By.XPath("//*[@id=\"jsupload\"]/div/form/input[1]"));
        private IWebElement UploadBtn => Driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/div/div/div/form/input[4]"));

        public FileUploadNetPage ClickUploadWithoutJavaScript()
        {
            UploadWithoutJavaScriptLink.Click();
            return this;
        }

        public FileUploadNetPage NavigateToFileUploadNetPage()
        {
            Driver.Navigate().GoToUrl("http://en.file-upload.net/");
            return this;
        }

        public IWebElement GetBrowseFileInputField()
        {
            return BrowseFileInputField;
        }

        public FileUploadNetPage ClickUploadBtn()
        {
            UploadBtn.Click();
            return this;
        }
    }
}
