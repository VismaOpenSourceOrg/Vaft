using NUnit.Framework;
using OpenQA.Selenium;
using Vaft.Framework.Core;
using Vaft.Framework.Driver;
using Vaft.Framework.Element;
using Vaft.SeleniumNunitTests.Pages;

namespace Vaft.SeleniumNunitTests.Tests
{
    public class AngularNunitTests : TestBase
    {
        private AngularLoadingBarPage _angularLoadingBarPage;

        [SetUp]
        public void SetUp()
        {
            Driver.VaftExt().TurnOffImplicitlyWait();
            _angularLoadingBarPage = new AngularLoadingBarPage(Driver);
            _angularLoadingBarPage.NavigateToAngularPage();
        }

        [Test]
        public void WaitUntilAngularIsLoaded()
        {
            _angularLoadingBarPage.WaitForAngular();
            _angularLoadingBarPage.ClickRealExampleButton();
            _angularLoadingBarPage.WaitForAngular();

            if (_angularLoadingBarPage.GetFirstPost().Displayed)
            {
                Assert.Pass(); //pass test
            }
            else
            {
                Assert.Fail(); //fail test
            }
        }

        [Test]
        public void DoNotWaitUntilAngularIsLoaded()
        {
            _angularLoadingBarPage.WaitForAngular();
            _angularLoadingBarPage.ClickRealExampleButton();

            bool displayed;

            try
            {
                displayed = _angularLoadingBarPage.GetFirstPost().Displayed;
            }
            catch (NoSuchElementException)
            {
                Assert.Pass(); //pass test
            }

            Assert.Fail(); //fail test
        }

        [Test]
        public void WaitForSpinnerToDisappear()
        {
            _angularLoadingBarPage.WaitForAngular();
            _angularLoadingBarPage.ClickRealExampleButton();
            _angularLoadingBarPage.GetSpinner().Wait().WaitUntilNotExist();

            try
            {
                var displayed = _angularLoadingBarPage.GetSpinner().Displayed;
            }
            catch (NoSuchElementException)
            {
                Assert.Pass(); //pass test
            }

            Assert.Fail(); //fail test 
        }
    }
}
