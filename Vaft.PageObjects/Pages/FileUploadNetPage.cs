using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Framework.Core;

namespace Vaft.PageObjects.Pages
{
    public class FileUploadNetPage : PageBase
    {
        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/div[3]/div/div/div/p[2]/a")]
        protected IWebElement UploadWithoutJavaScriptLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"jsupload\"]/div/form/input[1]")]
        protected IWebElement BrowseFileInputField { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/div[3]/div/div/div/div/form/input[4]")]
        protected IWebElement UploadBtn { get; set; }

        public FileUploadNetPage(IWebDriver driver)
            : base(driver)
        {
        }

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
