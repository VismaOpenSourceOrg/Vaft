using Vaft.Framework.Core;
using Vaft.Framework.Element;
using Vaft.PageObjects.Pages;
using Xunit;

namespace Vaft.SeleniumXunitTests.Tests
{
    public class DropdownXunitTests : XUnitTestBase
    {
        private readonly DropdownPage _dropdownPage;
        
        public DropdownXunitTests()
        {
            _dropdownPage = new DropdownPage(Driver);
            _dropdownPage.NavigateToDropdownPage();
        }

        [Fact]
        public void MakeDropdownSelected()
        {
            _dropdownPage.DropdownElement.Dropdown().SelectByText("Option 1");
            Assert.Equal("Option 1".ToLower(), _dropdownPage.DropdownElement.Dropdown().GetTextOfSelectedValue().ToLower());

            _dropdownPage.DropdownElement.Dropdown().SelectByIndex(2);
            Assert.Equal("Option 2".ToLower(), _dropdownPage.DropdownElement.Dropdown().GetTextOfSelectedValue().ToLower());
        }
    }
}
