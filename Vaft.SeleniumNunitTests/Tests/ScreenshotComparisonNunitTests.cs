using System;
using System.Drawing;
using NUnit.Framework;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;
using Vaft.Framework.Utilities;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    public class ScreenshotComparisonNunitTests : TestBase
    {
        readonly string _baselineScrDir = AppDomain.CurrentDomain.BaseDirectory + "/Resources/BaselineScreenshots/";

        [SetUp]
        public void SetUp()
        {
            Driver.VaftExt().OpenApplicationBaseUrl("/broken_images");
        }

        [Test]
        public void CompareLoginPageScreenshot()
        {
            Driver.Manage().Window.Size = new Size(1024, 768);
            System.Threading.Thread.Sleep(1000);

            ScreenShot.CompareWindow(Driver, _baselineScrDir, "images-page-scr");
        }

        [Test]
        public void CompareCheckBoxElementScreenshot()
        {
            Driver.Manage().Window.Size = new Size(1024, 768);
            System.Threading.Thread.Sleep(1000);

            IWebElement avatar = Driver.FindElement(By.XPath("//*[@id=\"content\"]/div/img[3]"));
            ScreenShot.CompareElementImage(Driver, _baselineScrDir, "avatar-blank", avatar);
        }

        [Test]
        public void CompareCheckBoxElementScreenshot2()
        {
            Driver.Manage().Window.Size = new Size(1024, 768);

            IWebElement avatar = Driver.FindElement(By.XPath("//*[@id=\"content\"]/div/img[3]"));
            ScreenShot.CompareElementImage(Driver, "avatar-blank", avatar, 0.1);
        }
    }
}
