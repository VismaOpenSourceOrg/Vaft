using System.Collections.Generic;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Element.Customization.WebComponents;

namespace Vaft.PageObjects.Pages
{
    public class WebComponentsDemoPage : PageBase
    {
        public WebComponentsDemoPage(IWebDriver driver) : base(driver)
        {
            InitFields();
        }

        private WebComponentsElement _shadow1InputFieldTwo;
        private WebComponentsElement _shadow2DummyText;
        private WebComponentsElement _shadow3InputFieldSub3Two;
        private WebComponentsElement _shadow3DummyText;
        private WebComponentsElement _shadow22NdInputFieldSub2Two;
        private WebComponentsElement _shadow22NdDummyText;

        private void InitFields()
        {
            _shadow1InputFieldTwo = WebComponentsElement.CreateInstance(Driver, By.Id("two"));
            _shadow2DummyText = WebComponentsElement.CreateInstance(Driver, By.CssSelector("div.data-view-text"),new List<By> {By.Id("container"), By.Id("internal")});
            _shadow3InputFieldSub3Two = WebComponentsElement.CreateInstance(Driver, By.Id("sub3-two"));
            _shadow3DummyText = WebComponentsElement.CreateInstance(Driver, By.CssSelector("div.data-view-text"),new List<By> {By.Id("internal1")});
            _shadow22NdInputFieldSub2Two = WebComponentsElement.CreateInstance(Driver, By.Id("sub2-two"));
            _shadow22NdDummyText = WebComponentsElement.CreateInstance(Driver, By.CssSelector("div.data-view-text"),new List<By> {By.Id("container"), By.Id("internal2")});
        }

        public string GetShadow1InputTwoFieldValue()
        {
            return GetValueAttribute(_shadow1InputFieldTwo.Element);
        }

        public string GetShadow2DummyTextValue()
        {
            return _shadow2DummyText.Element.Text;
        }

        public string GetShadow3InputFieldSub3TwoValue()
        {
            return GetValueAttribute(_shadow3InputFieldSub3Two.Element);
        }

        public string GetShadow3DummyTextValue()
        {
            return _shadow3DummyText.Element.Text;
        }

        public string GetShadow22InputFieldSub2TwoValue()
        {
            return GetValueAttribute(_shadow22NdInputFieldSub2Two.Element);
        }

        public string GetShadow22DummyTextValue()
        {
            return _shadow22NdDummyText.Element.Text;
        }

        private string GetValueAttribute(IWebElement element)
        {
            return element.GetAttribute("value");
        }
    }
}