using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.Framework.Element;
using Vaft.PageObjects.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    public class DropdownNunitTests : TestBase
    {
        private DropdownPage _dropdownPage;

        [SetUp]
        public void SetUp()
        {
            _dropdownPage = new DropdownPage(Driver);
            _dropdownPage.NavigateToDropdownPage();
        }

        [Test]
        public void MakeDropdownSelected()
        {
            _dropdownPage.DropdownElement.Dropdown().SelectByText("Option 1");
            StringAssert.AreEqualIgnoringCase("Option 1", _dropdownPage.DropdownElement.Dropdown().GetTextOfSelectedValue());

            _dropdownPage.DropdownElement.Dropdown().SelectByIndex(2);
            StringAssert.AreEqualIgnoringCase("Option 2", _dropdownPage.DropdownElement.Dropdown().GetTextOfSelectedValue());
        }
    }
}
