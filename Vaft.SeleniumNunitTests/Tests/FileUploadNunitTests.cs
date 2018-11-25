using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Element;
using Vaft.PageObjects.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    public class FileUploadNunitTest : TestBase
    {
        private FileUploadFileChuckerPage _fileUploadPage;
        private FileUploadNetPage _fileUploadNetPage;
        private FileUploadAjaxPage _fileUploadAjaxPage;
        private string _filePath;

        [SetUp]
        public void SetUp()
        {
            _filePath = AppDomain.CurrentDomain.BaseDirectory + "/Resources/TestUpload.jpeg";
        }

        [Test]
        public void UploadFile()
        {
            VaftLog.Info("File Path: " + _filePath);

            _fileUploadPage = new FileUploadFileChuckerPage(Driver);
            _fileUploadPage.NavigateToFileUploadPage();

            _fileUploadPage
                .EnterEmail("selenium@gmail.com")
                .EnterFirstName("Selenium");

            _fileUploadPage.GetBrowseFileInputField().AdvancedAction().UploadFile(_filePath);
            _fileUploadPage.ClickBeginUploadBtn();
            Thread.Sleep(5000);
            _fileUploadPage.WaitUntilTextIsDisplayed("uploaded successfully.");
        }

        [Test]
        public void UploadFileInFileUploadNet()
        {
            VaftLog.Info("File Path: " + _filePath);

            _fileUploadNetPage = new FileUploadNetPage(Driver);
            _fileUploadNetPage.NavigateToFileUploadNetPage();
            _fileUploadNetPage.GetBrowseFileInputField().AdvancedAction().UploadFile(_filePath);
            _fileUploadNetPage.WaitUntilTextIsDisplayed("Your file has succesfully been stored!");

            string downloadLink = Driver.FindElement(By.CssSelector("input[name=\"default\"]")).GetAttribute("value");
            Driver.Navigate().GoToUrl(downloadLink);
            StringAssert.AreEqualIgnoringCase("TestUpload.jpeg", Driver.FindElement(By.XPath("//h1[@class='dateiname']")).Text);
        }

        [Test]
        public void UploadFileInAjaxUploader()
        {
            VaftLog.Info("File Path: " + _filePath);

            _fileUploadAjaxPage = new FileUploadAjaxPage(Driver);
            _fileUploadAjaxPage.NavigateToFileUploadAjaxPage();
            _fileUploadAjaxPage.GetUploadFileElement().AdvancedAction().UploadFile(_filePath);
            _fileUploadAjaxPage.WaitUntilTextIsDisplayed("TestUpload.jpeg");
        }
    }
}
