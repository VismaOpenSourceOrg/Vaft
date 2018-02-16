using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.SeleniumMsTests.Tests
{
    [TestClass]
    public class ScreenshotMsTests : MsTestBase
    {
        [TestInitialize]
        public void SetUp()
        {
            Driver.VaftExt().OpenApplicationBaseUrl();
        }

        [TestMethod]
        public void MakeScreenshotTest()
        {
            var screenshotFilename = TakeScreenshot();
            Assert.IsTrue(File.Exists(screenshotFilename));
            File.Delete(screenshotFilename);
        }
    }
}
