using System.IO;
using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    public class ScreenshotNunitTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            Driver.VaftExt().OpenApplicationBaseUrl();
        }

        [Test]
        public void MakeScreenshotTest()
        {
            var screenshotFilename = TakeScreenshot();
            Assert.IsTrue(File.Exists(screenshotFilename));
            File.Delete(screenshotFilename);
        }
    }
}
