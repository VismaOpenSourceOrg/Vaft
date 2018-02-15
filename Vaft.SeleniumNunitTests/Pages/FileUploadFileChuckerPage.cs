using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Framework.Core;

namespace Vaft.SeleniumNunitTests.Pages
{
    public class FileUploadFileChuckerPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "uploadname1")]
        protected IWebElement BrowseFileInputField { get; set; }

        [FindsBy(How = How.Id, Using = "uploadbutton")]
        protected IWebElement BeginUploadBtn { get; set; }

        [FindsBy(How = How.Id, Using = "formfield-email_address")]
        protected IWebElement Email { get; set; }

        [FindsBy(How = How.Id, Using = "formfield-first_name")]
        protected IWebElement FirstName { get; set; }


        public FileUploadFileChuckerPage(IWebDriver driver)
            : base(driver)
        {
        }

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
