using NUnit.Framework;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;
using Vaft.Framework.Element;

namespace Vaft.SeleniumNunitTests.Tests
{
    public class RightClickNunitTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            Driver.VaftExt().OpenUrl("https://swisnl.github.io/jQuery-contextMenu/demo.html");
        }

        [Test]
        public void ExpandContextMenu()
        {
            IWebElement button = Driver.FindElement(By.XPath("//span[text()='right click me']"));

            Assert.IsFalse(IsContextMenuDisplayed());
            button.AdvancedAction().RightClick();
            Assert.IsTrue(IsContextMenuDisplayed());
        }

        private bool IsContextMenuDisplayed()
        {
            try
            {
                Driver.FindElement(By.Id("context-menu-layer"));
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }
    }
}
