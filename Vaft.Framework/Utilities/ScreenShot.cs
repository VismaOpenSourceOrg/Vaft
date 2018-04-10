using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NLog;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Vaft.Framework.Utilities
{
    public static class ScreenShot
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string ScreenshotsDir = BaseDirectory + "/Screenshots/";
        private static readonly string DefaultBaselineImgDir = BaseDirectory + "/Resources/BaselineScreenshots/";
        private static readonly string ComparisonDir = BaseDirectory + "/Screenshots/Comparison/";

        /// <summary>Saves a screen shot of current active screen in bin directory</summary>
        /// <param name="driver">The current browser you're using</param>
        /// <param name="testMethodName">The name of test to be used in filename construction</param>
        public static string SaveScreenShot(IWebDriver driver, string testMethodName)
        {
            Directory.CreateDirectory(ScreenshotsDir);
            var filename = ScreenshotsDir + TimeStamp() + "-" + testMethodName + ".png";
            return CaptureScreenhot(driver, filename);
        }

        /// <summary>Saves a screen shot of current active screen in specified directory</summary>
        /// <param name="driver">The current browser you're using</param>
        /// <param name="directory">Directory for saving screenshot</param>
        /// <param name="fileName">Screenshot file name</param>
        public static string SaveScreenShot(IWebDriver driver, string directory, string fileName)
        {
            Directory.CreateDirectory(directory);
            var filename = directory + fileName + ".png";
            return CaptureScreenhot(driver, filename);
        }

        private static string CaptureScreenhot(IWebDriver driver, string fileName)
        {
            var screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
            Log.Info("Screenshot captured: " + fileName);
            return fileName;
        }

        /// <summary> Saves a screen shot of an element on current browser </summary>
        /// <param name="driver">The current browser you're using</param>
        /// <param name="directory">Directory for saving screenshot</param>
        /// <param name="fileName">Screenshot file name</param>
        /// <param name="element">The element on the current browser you want a screenshot of</param>
        /// <param name="imageFormat">Image saving format. Defaults to jpeg</param>
        public static string CaptureElementBitmap(IWebDriver driver, string directory, string fileName, IWebElement element, ImageFormat imageFormat = null)
        {
            imageFormat = imageFormat ?? ImageFormat.Jpeg;

            Directory.CreateDirectory(directory);
            var filename = directory + fileName + ".png";

            // Take ScreenCap of Entire Screen
            var screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            var bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));
            // Crop ScreenCap to Element
            var cropArea = new Rectangle(element.Location, element.Size);
            Bitmap bmpCrop = bmpScreen.Clone(cropArea, bmpScreen.PixelFormat);
            //Save
            bmpCrop.Save(filename, imageFormat);
            Log.Info("Screenshot captured: " + fileName);
            return filename;
        }

        /// <summary> Compares image of entire Web browser window</summary>
        /// <param name="driver">Driver</param>
        /// <param name="baselineImageDir">Baseline image directory</param>
        /// <param name="baselineImageName">Baseline image name</param>
        /// <param name="tolerance">Comparison tolerance</param>
        public static void CompareWindow(IWebDriver driver, string baselineImageDir, string baselineImageName, double tolerance = 0.9)
        {
            var timeStamp = TimeStamp();
            var copiedBaselineImageFileName = timeStamp + "-" + baselineImageName + "-Expected";
            var actualImageFileName = timeStamp + "-" + baselineImageName + "-Actual";
            var diffImageFileName = timeStamp + "-" + baselineImageName + "-Diff";

            var baselineImage = baselineImageDir + baselineImageName + ".png";
            var copiedBaselineImage = ComparisonDir + copiedBaselineImageFileName + ".png";
            var diffImage = ComparisonDir + diffImageFileName + ".png";

            Directory.CreateDirectory(ComparisonDir);
            var actualImage = SaveScreenShot(driver, ComparisonDir, actualImageFileName);
            File.Copy(baselineImage, copiedBaselineImage);

            ImageOperations.CompareResult result = ImageOperations.CompareImage(copiedBaselineImage, actualImage, diffImage, tolerance);
            Log.Info("result: " + result);
            if (result != ImageOperations.CompareResult.Match)
            {
                Assert.Fail("Window image is not the same within tolerance value " + tolerance);
            }
        }

        /// <summary> Compares screenshot of an element </summary>
        /// <param name="driver">Driver</param>
        /// <param name="baselineImageDir">Baseline screenshot directory</param>
        /// <param name="baselineImageName">Baseline screenshot name</param>
        /// <param name="element">Web element</param>
        /// <param name="tolerance"></param>
        public static void CompareElementImage(IWebDriver driver, string baselineImageDir, string baselineImageName, IWebElement element, double tolerance = 0.1)
        {
            var timeStamp = TimeStamp();
            var copiedBaselineImageFileName = timeStamp + "-" + baselineImageName + "-Expected";
            var actualImageFileName = timeStamp + "-" + baselineImageName + "-Actual";
            var diffImageFileName = timeStamp + "-" + baselineImageName + "-Diff";

            var baselineImage = baselineImageDir + baselineImageName + ".png";
            var copiedBaselineImage = ComparisonDir + copiedBaselineImageFileName + ".png";
            var diffImage = ComparisonDir + diffImageFileName + ".png";

            Directory.CreateDirectory(ComparisonDir);
            var actualImage = CaptureElementBitmap(driver, ComparisonDir, actualImageFileName, element);
            File.Copy(baselineImage, copiedBaselineImage);

            ImageOperations.CompareResult result = ImageOperations.CompareImage(copiedBaselineImage, actualImage, diffImage, tolerance);
            Log.Info("result: " + result);
            if (result != ImageOperations.CompareResult.Match)
            {
                Assert.Fail("Web element image is not the same within tolerance value " + tolerance);
            }
        }

        /// <summary> Compares image of an element. Baseline images are taken frome directory ../Resources/BaselineScreenshots/</summary>
        /// <param name="driver">Driver</param>
        /// <param name="baselineImageName">Baseline image name</param>
        /// <param name="element">Web element</param>
        public static void CompareElementImage(IWebDriver driver, string baselineImageName, IWebElement element)
        {
            CompareElementImage(driver, DefaultBaselineImgDir, baselineImageName, element);
        }

        /// <summary> Compares image of an element. Baseline images are taken frome directory ../Resources/BaselineScreenshots/</summary>
        /// <param name="baselineImageName">Baseline image name</param>
        /// <param name="element">Web element</param>
        /// <param name="tolerance">Comparison tolerance</param>
        public static void CompareElementImage(IWebDriver driver, string baselineImageName, IWebElement element, double tolerance)
        {
            CompareElementImage(driver, DefaultBaselineImgDir, baselineImageName, element, tolerance);
        }

        private static string TimeStamp()
        {
            return DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");
        }
    }
}
