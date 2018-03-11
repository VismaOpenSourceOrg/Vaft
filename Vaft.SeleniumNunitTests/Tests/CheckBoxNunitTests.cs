using NUnit.Framework;
using Vaft.Framework.Core;
using Vaft.Framework.Element;
using Vaft.PageObjects.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    [TestFixture]
    [Category("Smoke")]
    public class CheckBoxNunitTests : TestBase
    {
        private CheckboxesPage _checkboxesPage;

        [SetUp]
        public void SetUp()
        {
            _checkboxesPage = new CheckboxesPage(Driver);
            _checkboxesPage.NavigateToCheckboxesPage();
        }

        [Test]
        public void OperateWithCheckbox()
        {
            // Assert checkbox state
            _checkboxesPage.Checkbox1.Assert().IsNotSelected();
            _checkboxesPage.Checkbox2.Assert().IsSelected();
            Assert.IsFalse(_checkboxesPage.Checkbox1.Checkbox().IsTicked());
            Assert.IsTrue(_checkboxesPage.Checkbox2.Checkbox().IsTicked());
            _checkboxesPage.Checkbox1.Checkbox().AssertIsNotTicked();
            _checkboxesPage.Checkbox2.Checkbox().AssertIsTicked();

            // Do nothing
            _checkboxesPage.Checkbox1.Checkbox().Untick();;
            _checkboxesPage.Checkbox2.Checkbox().Tick();
            _checkboxesPage.Checkbox1.Checkbox().AssertIsNotTicked();
            _checkboxesPage.Checkbox2.Checkbox().AssertIsTicked();

            // Tick / untick
            _checkboxesPage.Checkbox1.Checkbox().Tick();
            _checkboxesPage.Checkbox2.Checkbox().Untick();
            _checkboxesPage.Checkbox1.Checkbox().AssertIsTicked();
            _checkboxesPage.Checkbox2.Checkbox().AssertIsNotTicked();

            // Toggle
            _checkboxesPage.Checkbox1.Checkbox().Toggle();
            _checkboxesPage.Checkbox2.Checkbox().Toggle();
            _checkboxesPage.Checkbox1.Checkbox().AssertIsNotTicked();
            _checkboxesPage.Checkbox2.Checkbox().AssertIsTicked();
        }
    }
}
