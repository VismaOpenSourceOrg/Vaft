using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;

namespace Vaft.PageObjects.Pages
{
    public class AddRemoveElementsPage : PageBase
    {
        public AddRemoveElementsPage(IWebDriver driver) : base(driver)
        {
        }

        public IWebElement AddElement => Driver.FindElement(By.XPath("//div[@id='content']/div/button"));
        public AddRemoveElementsPage NavigateToAddRemoveElementsPage()
        {
            Driver.VaftExt().OpenApplicationBaseUrl("/add_remove_elements/");
            return this;
        }

        public AddRemoveElementsPage ClickAddElement()
        {
            AddElement.Click();
            return this;
        }

        public AddRemoveElementsPage AssertNumberOfDeleteButtons(int number)
        {
            var elements = Driver.FindElement(By.Id("elements")).FindElements(By.ClassName("added-manually"));
            Assert.AreEqual(elements.Count, number);
            return this;
        }
    }
}