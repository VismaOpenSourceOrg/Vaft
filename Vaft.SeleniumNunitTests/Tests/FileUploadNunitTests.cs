using System.IO;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Element;
using Vaft.SeleniumNunitTests.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    public class FileUploadNunitTest : TestBase
    {
        private FileUploadFileChuckerPage _fileUploadPage;
        private FileUploadNetPage _fileUploadNetPage;
        private FileUploadAjaxPage _fileUploadAjaxPage;

        [Test]
        public void UploadFile()
        {
            string filePath = Path.GetFullPath(@"Resources/TestUpload.jpeg");
            //            string filePath = AppDomain.CurrentDomain.BaseDirectory + "/Resources/TestUpload.jpeg";
            VaftLog.Info("File Path: " + filePath);

            _fileUploadPage = new FileUploadFileChuckerPage(Driver);
            _fileUploadPage.NavigateToFileUploadPage();

            _fileUploadPage
                .EnterEmail("selenium@gmail.com")
                .EnterFirstName("Selenium");

            _fileUploadPage.GetBrowseFileInputField().AdvancedAction().UploadFile(filePath);
            _fileUploadPage.ClickBeginUploadBtn();
            Thread.Sleep(5000);
            _fileUploadPage.WaitUntilTextIsDisplayed("uploaded successfully.");
        }

        [Test]
        public void UploadFileInFileUploadNet()
        {
            string filePath = Path.GetFullPath(@"Resources/TestUpload.jpeg");
            VaftLog.Info("File Path: " + filePath);

            _fileUploadNetPage = new FileUploadNetPage(Driver);
            _fileUploadNetPage
                .NavigateToFileUploadNetPage()
                .ClickUploadWithoutJavaScript();
            _fileUploadNetPage.GetBrowseFileInputField().AdvancedAction().UploadFile(filePath);
            _fileUploadNetPage.ClickUploadBtn();
            _fileUploadNetPage.WaitUntilTextIsDisplayed("Your file has succesfully been stored!");

            string downloadLink = Driver.FindElement(By.CssSelector("input[name=\"default\"]")).GetAttribute("value");
            Driver.Navigate().GoToUrl(downloadLink);
            StringAssert.AreEqualIgnoringCase("TestUpload.jpeg", Driver.FindElement(By.XPath("//h1[@class='dateiname']")).Text);
        }

        [Test]
        public void UploadFileInAjaxUploader()
        {
            string filePath = Path.GetFullPath(@"Resources/TestUpload.jpeg");
            VaftLog.Info("File Path: " + filePath);

            _fileUploadAjaxPage = new FileUploadAjaxPage(Driver);
            _fileUploadAjaxPage
                .NavigateToFileUploadAjaxPage();
            _fileUploadAjaxPage.GetUploadFileElement().AdvancedAction().UploadFile(filePath);
            _fileUploadAjaxPage.WaitUntilTextIsDisplayed("TestUpload.jpeg");
        }
    }
}
