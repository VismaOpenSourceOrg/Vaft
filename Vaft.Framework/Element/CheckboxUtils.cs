using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Vaft.Framework.Utilities;

namespace Vaft.Framework.Element
{
    public class CheckboxUtils
    {
        private readonly IWebElement _element;
        private readonly string _locator;

        public CheckboxUtils(IWebElement element)
        {
            _element = element;
            _locator = WebElementUtils.GetLocator(_element);
        }

        /// <summary>Toggle the state of the checkbox.</summary>
        public void Toggle()
        {
            _element.Click();
        }

        /// <summary>Ticks checkbox if unticked.</summary>
        public void Tick()
        {
            if (!IsTicked())
            {
                Toggle();
            }
        }

        /// <summary>Un-ticks checkbox if ticked.</summary>
        public void Untick()
        {
            if (IsTicked())
            {
                Toggle();
            }
        }

        /// <summary>Check if an element is selected, and return boolean.</summary>
        /// <returns>Returns true or false whether or not the checkbox is ticked.</returns>
        public bool IsTicked()
        {
            return _element.Selected;
        }

        /// <summary>Asserts if an element is selected</summary>
        public void AssertIsTicked()
        {
            Assert.IsTrue(IsTicked(), "Checkbox was not ticked. Locator: " + _locator);
        }

        /// <summary>Assertd if an element is not selected</summary>
        public void AssertIsNotTicked()
        {
            Assert.IsFalse(IsTicked(), "Checkbox was ticked. Locator: " + _locator);
        }
    }
}
