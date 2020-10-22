using System.Drawing.Text;
using NUnit.Framework;
using Vaft.Framework.Core;

namespace GoogleTests
{
    public class Tests : TestBase
    {
        private Google_Page _page;

        [SetUp]
        public void Setup()
        { 
            _page = new Google_Page(Driver);
        }

        [Test]
        public void Test1()
        {
            _page.NavigateToHomePage();
            Assert.That(1 == 1);
        }
    }
}