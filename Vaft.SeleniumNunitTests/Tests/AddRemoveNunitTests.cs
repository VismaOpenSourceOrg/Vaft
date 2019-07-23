using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.Framework.Element;
using Vaft.PageObjects.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    [Category("Smoke")]
    public class AddRemoveNunitTests : TestBase
    {
        private AddRemoveElementsPage _addRemovePage;

        [SetUp]
        public void SetUp()
        {
            _addRemovePage = new AddRemoveElementsPage(Driver);
            _addRemovePage.NavigateToAddRemoveElementsPage();
        }

        [Test]
        public void ClickAddAndRemove()
        {
            _addRemovePage.AssertNumberOfDeleteButtons(0);
            _addRemovePage.ClickAddElement();
            _addRemovePage.AssertNumberOfDeleteButtons(1);
            _addRemovePage.ClickAddElement();
            _addRemovePage.AssertNumberOfDeleteButtons(2);
            _addRemovePage.ClickAddElement();
            _addRemovePage.AssertNumberOfDeleteButtons(3);
        }
    }
}
