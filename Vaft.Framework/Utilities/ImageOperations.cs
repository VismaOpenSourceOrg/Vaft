using System;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vaft.Framework.Utilities
{
    public class ImageOperations
    {
        public enum CompareResult
        {
            NoMatch,
            Match,
            Error
        }

        public enum ApprovedScreenshotName
        {
            AccumulatorList
        }

        public static Exception LastException { get; set; }

        public static CompareResult CompareImage(string baselineFile, string actualFile, string diffFile, double tolerance)
        {
            var baselineImg = new MagickImage(baselineFile);
            var actualImg = new MagickImage(actualFile);

            Assert.IsNotNull(baselineImg);
            Assert.IsNotNull(actualImg);

            var result = CompareResult.NoMatch;

            using (var delta = new MagickImage())
            {
                double compareResult = actualImg.Compare(baselineImg, Metric.PeakAbsoluteError, delta);

                if (compareResult < tolerance)
                {
                    result = CompareResult.Match;
                }
                else
                {
                    delta.Write(diffFile);
                }
            }
            return result;
        }
    }
}
