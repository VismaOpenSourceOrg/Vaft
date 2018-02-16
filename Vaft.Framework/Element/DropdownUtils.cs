using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Vaft.Framework.Exceptions;

namespace Vaft.Framework.Element
{
    public class DropdownUtils
    {
        private readonly IWebElement _element;

        public DropdownUtils(IWebElement element)
        {
            _element = element;
        }

        /// <summary>Selects dropdown value by text</summary>
        /// <param name="text">Dropdown value available for selection</param>
        public void SelectByText(string text)
        {
            if (_element.Enabled)
            {
                var dropdownSelection = new SelectElement(_element);
                dropdownSelection.SelectByText(text);
            }
            else
            {
                throw new ElementNotEnabledException($"Dropdown element is disabled! Tried to select visible text: {text}.");
            }
        }

        /// <summary>Selects dropdown value by index. Index value starts from 0.</summary>
        /// <param name="index">Index of dropdown value</param>
        public void SelectByIndex(int index)
        {
            if (_element.Enabled)
            {
                var dropdownSelection = new SelectElement(_element);
                dropdownSelection.SelectByIndex(index);
            }
            else
            {
                throw new InvalidDropdownValueException($"Dropdown element is disabled! Tried to select index: {index}.");
            }
        }

        /// <summary>Returns index of seleced dropdown value</summary>
        /// <returns>Index</returns>
        public int GetIndexOfSelectedValue()
        {
            var index = -1;
            var dropdownSelection = new SelectElement(_element);
            var slectedValue = dropdownSelection.SelectedOption.Text;
            for (var i = 0; i < dropdownSelection.Options.Count; i++)
            {
                if (!dropdownSelection.Options[i].Text.Equals(slectedValue)) continue;
                index = i;
                break;
            }
            return index;
        }

        /// <summary>Gets text of selected dropdown value</summary>
        /// <returns>Returns text of selected dropdown value</returns>
        public string GetTextOfSelectedValue()
        {
            var dropdownSelection = new SelectElement(_element);
            return dropdownSelection.SelectedOption.Text;
        }
    }
}
