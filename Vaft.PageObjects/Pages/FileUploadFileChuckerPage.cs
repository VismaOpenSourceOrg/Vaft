using OpenQA.Selenium;
using Vaft.Framework.Core;

namespace Vaft.PageObjects.Pages
{
    public class FileUploadFileChuckerPage : PageBase
    {
        public FileUploadFileChuckerPage(IWebDriver driver)
            : base(driver)
        {
        }

        private IWebElement BrowseFileInputField => Driver.FindElement(By.Id("uploadname1"));
        private IWebElement BeginUploadBtn => Driver.FindElement(By.Id("uploadbutton"));
        private IWebElement Email => Driver.FindElement(By.Id("formfield-email_address"));
        private IWebElement FirstName => Driver.FindElement(By.Id("formfield-first_name"));

        public FileUploadFileChuckerPage NavigateToFileUploadPage()
        {
            Driver.Navigate().GoToUrl("https://encodable.com/uploaddemo/");
            return this;
        }

        public IWebElement GetBrowseFileInputField()
        {
            return BrowseFileInputField;
        }

        public FileUploadFileChuckerPage ClickBeginUploadBtn()
        {
            BeginUploadBtn.Click();
            return this;
        }

        public FileUploadFileChuckerPage EnterEmail(string emailAddress)
        {
            Email.SendKeys(emailAddress);
            return this;
        }

        public FileUploadFileChuckerPage EnterFirstName(string firstName)
        {
            FirstName.SendKeys(firstName);
            return this;
        }
    }
}
