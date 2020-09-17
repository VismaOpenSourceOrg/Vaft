# Vaft C# Test Automation Framework
# Kudos to Domas Puras for creating and maintaining this framework

# VAFT
> *Insert/Generate table of content here*
___

Visma Automated Functional Test framework (shortend "VAFT") is a framework for automated functional testing through the UI, with Selenium Webdriver. The framework extend Webdriver and Webelement features and functionality, hides complexity and promotoes page object pattern. It supports both .Net and Java languages.

The goal of this project was to make it easier for teams to start with automated tesitng through the UI, with Selenium Webdriver, at the same time create robust and maintainable tests from the start. The way to achieve this goal was to establish and offer a common framework in Selenium, both for .Net and Java based projects.

Teams using the framework will find it easier to create and maintain their automated functional tests.

## Some benefits of using VAFT

* Using current "best practices" for Selenium Webdriver ( Page Object Design Pattern and Page Factory )
* Reduce effort for establishing the infrastructure for test project
* Easy to use (hide complexity, simplified functions, easier cross browser testing)
* Easy setup (test setup and teardown handled, multipe browser support)
* Multiple platform support (CI systems - TeamCity, Jenkins, Hudson; Cross browser compatibility with Selenium Grid and BrowserStack)
* Extended Webdriver features
  * Correct wait and time out handling
  * Screenshot at error
  * Screenshot comparison (for testing graphs, images, visual elements, both entire screen and element based)
* Extended WebElement
  * Assertions
  * Checkbox, dropbox, double-click, right-click, page scroll
  * Wait for state handling
  * Configurable global time out
* Always up to date with Selenium Webdriver versions
* Community driven - everyone can contribute

## How to get started with VAFT for .NET?
1. Install the Vaft Nuget package into a C# test project. It will automatically set up Selenium and WebDriver along with the framework.
2. Create App.config using following configuration for initial test environment.
```XML
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <appSettings>
    
<!--Basic VAFT settings-->
    <add key="ApplicationBaseUrl" value="<URL of application>" />
    <add key="SeleniumBrowser" value="firefox" />
    <add key="SeleniumTimeout" value="5" />
    <add key="ChromeServerPath" value="<relative path i.e: \DriverServers>" />
    <add key="IeServerPath" value="<relative path i.e: \DriverServers>" />
    <add key="BrowserLanguage" value="nb-NO"/>
    
    <!--<add key="RunOnRemoteMachine" value="<SeleniumGrid / Browserstack / Appium>"/>-->
    <!--<add key="BrowserVersion" value="10" />-->
    <!--<add key="BrowserResolution" value="1024x768"/>-->
    <!--<add key="AjaxWaitTimeout" value="5" />-->
    <!--<add key="ScreenshotOnFailure" value="true" />-->
    <!--<add key="Proxy" value="localhost:8080"/>-->
    <!--<add key="databaseType" value="MsSql" />-->
    
<!--Selenium Grid configuration-->
    <!--<add key="SeleniumGridUrl" value="http://localhost:4444/wd/hub" />-->
   
<!--BrowserStack configuration-->
    <!--<add key="BrowserStack_User" value="<username>" />-->
    <!--<add key="BrowserStack_Key" value="<browserstack key>" />-->
    <!--<add key="BrowserStack_Project" value="<project name>" />-->  
    <!--<add key="BrowserStack_Args" value="<localhost>,<port>,<SSL on/off>" />-->
    <!--<add key="BrowserStack_Tunnel" value ="<true/false>"/>-->
    <!--<add key="BrowserStack_LocalPath" value="\Resources\BrowserStack\" />-->
    <!--<add key="BrowserStack_Browser" value="Firefox" />-->
    <!--<add key="BrowserStack_BrowserVersion" value="44.0" />-->
    <!--<add key="BrowserStack_Os" value="Windows"/>-->
    <!--<add key="BrowserStack_OsVersion" value="8"/>-->
    <!--<add key="BrowserStack_Resolution" value="1024x768"/>-->
    <!--<add key="BrowserStack_AcceptSslCerts" value="true"/>-->
    <!--<add key="BrowserStack_Debug" value="<true/false>"/>-->
  </appSettings>
</configuration>
```
3. Derive your tests from TestBase class which is based on NUnit attributes and contains test setup and tear down methods. See example below:
```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using Vaft.SeleniumTests.WebTests.Pages;
namespace Vaft.SeleniumTests.WebTests.Tests
{
    [TestFixture]
    public class ErrorMessageWebTests : TestBase
    {
        private LoginWebPage _loginPage;
        private HomePage _homepage;
        [SetUp]
        public void SetUp()
        {
            _loginPage = new LoginWebPage(Driver);
            _homepage = new HomePage(Driver);
            _loginPage.NavigateTo();
        }
        [Test]
        public void ValidateWrongUser()
        {
            _loginPage
                .SetUserName("test")
                .SetPassword("test")
                .ClickSignInButton();
            Assert.True(_loginPage.WaitUntilTextIsDisplayed("Wrong username or password!"));
            _loginPage
                .SetUserName("admin")
                .SetPassword("admin")
                .ClickSignInButton();
            _homepage
                .AssertHomePageTitle();
        }
    }
}
```
4. Each Web PageObject class should be derived from PageBase class to inherit all methods from it. See example below:
```C#
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
namespace Vaft.SeleniumTests.WebTests.Pages
{
    /// <summary>Represents Login Web page in the web site.</summary>
    public class LoginWebPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "username")]
        protected IWebElement Username { get; set; }
 
        [FindsBy(How = How.Id, Using = "password")]
        protected IWebElement Password { get; set; }
 
        [FindsBy(How = How.Id, Using = "submit")]
        protected IWebElement SignInButton { get; set; }
 
        public LoginWebPage(IWebDriver driver) : base(driver)   
        {
        }
        public LoginWebPage NavigateTo()
        {
            NavigateToUrl("/index.html");
            return this;
        }
        public LoginWebPage SetUserName(string value)
        {
            Username.Clear();
            Username.SendKeys(value);
            return this;
        }
        public LoginWebPage SetPassword(string value)
        {
            Password.Clear();
            Password.SendKeys(value);
            return this;
        }
        public LoginWebPage ClickSignInButton()
        {
            SignInButton.Click();
            return this;
        }
        public LoginWebPage AssertLoginPageTitle()
        {
            StringAssert.AreEqualIgnoringCase("Home", Driver.Title);
            return this;
        }
    }
}
```
# App.config file

## Defining application base URL

_ApplicationBaseUrl_ parameter can be added in project App.config file to specify application instance URL.
```XML
<add key="ApplicationBaseUrl" value="www.google.com"/>
```
Call _NavigateToUrl_ method in your page class
```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;

namespace Vaft.SeleniumNunitTests.Pages
{
    public class LoginWebPage : PageBase
    {
        public LoginWebPage(IWebDriver driver) : base(driver)    
        {
        }

        public LoginWebPage NavigateToMyPage()
        {
            NavigateToUrl();
            return this;
        }
    }
}
```
## Using dynamic part in URL path
If you want to open www.google.com/advanced_search URL in web browser, then you can pass "/advanced_search" parameter to _NavigateToUrl()_ method in order to add dynamic part in your ApplicationBaseUrl
```C#
NavigateToUrl("/advanced_search")
```

## Running tests on different Web browsers

Use _SeleniumBrowser_ parameter in App.config file for specifying browser you want to run tests on. The default option is firefox.
```XML
<add key="SeleniumBrowser" value="firefox"/>
```
Available options:
* firefox 
* chrome
* ie

## Running tests on Selenium Grid

Use following parameters in App.config file for using remote WebDriver and defining Selenium Grid instance URL.
```XML
<add key="RunOnRemoteMachine" value="SeleniumGrid"/>
<add key="SeleniumGridUrl" value="http://localhost:4444/wd/hub"/>
```
## Running tests on BrowserStack
VAFT <or another name> framework support execution of your tests on BrowserStack (www.browserstack.com) just by altering parameters in the app.config file. You need to set the RunOnRemoteMachine parameter to "BrowserStack" as shown below
```XML
<add key="RunOnRemoteMachine" value="BrowserStack"/>
```
Then fill out the different parameters required by BrowserStack (username, key, project, etc.) in order to be able to run the tests on BrowserStack.
```XML
<!--BrowserStack configuration-->
    <add key="BrowserStack_User" value="<username>" />
    <add key="BrowserStack_Key" value="<browserstack key>" />
    <add key="BrowserStack_Project" value="<project name>" />
    <add key="BrowserStack_Args" value="<localhost>,<port>,<SSL on/off>" />
    <add key="BrowserStack_Tunnel" value ="<true/false>"/>
    <add key="BrowserStack_LocalPath" value="\Resources\BrowserStack\" />
    <add key="BrowserStack_Browser" value="Firefox" />
    <add key="BrowserStack_BrowserVersion" value="44.0" />
    <add key="BrowserStack_Os" value="Windows"/>
    <add key="BrowserStack_OsVersion" value="8"/>
    <add key="BrowserStack_Resolution" value="1024x768"/>
    <add key="BrowserStack_AcceptSslCerts" value="true"/>
    <add key="BrowserStack_Debug" value="<true/false>"/>
```

## Selenium Timeout

SeleniumTimeout parameter can be added in project App.config file to specify amount of seconds that WebDriver should wait in cases of specified element is not available on the UI (DOM) after page reloaded. Default value is 5 seconds.
```XML
<add key="SeleniumTimeout" value="<amount_of_seconds>"/>
```

## Ajax Wait Timeout
_Description to be added_
```XML
<add key="AjaxWaitTimeout" value="5" />
```

## Webdrivers (IE and Chrome)
ChromeServerPath and IeServerPath must be added in the app.config file with the path to respective drivers, if running your tests on these browsers. 
```XML
<add key="ChromeServerPath" value="<relative path i.e: \DriverServers>" />
<add key="IeServerPath" value="<relative path i.e: \DriverServers>" />
```

## Browser options
When creating remote connections in order to exectue the tests on a grid, you can specify different parameters in order to execute the test(s) in the desired environment, on a desired browser version, language or resolution. If there is a match it will be executed on the grid. <review>
###### BrowserLanguage
```XML
<add key="BrowserLanguage" value="nb-NO"/>
```
###### BrowserVersion
```XML
<add key="BrowserVersion" value="10"/>
```
###### BrowserResolution
```XML
<add key="BrowserResolution" value="1024x768"/>
```

## Screenshot On Failure
_Description to be added_
```XML
<add key="ScreenshotOnFailure" value="true"/>
```

## Proxy
_Description to be added_
```XML
<add key="Proxy" value="localhost:8080"/>
```

## Database type
_Description to be added_
```XML
<add key="databaseType" value="MsSql"/>
```

# Current VAFT Utilities
_<This section needs to be reviewd, as some of these features have changed in the framework>_
```C# 
using Vaft.Selenium.Utilities;
```
###### AdvancedAction()
```C# 
//
<WebElement>.AdvancedAction().DoubleClick();
<WebElement>.AdvancedAction().RightClick();
<WebElement>.AdvancedAction().ScrollToElement(string text);
```
###### AssertElement()
```C# 
//
<WebElement>.AssertElement().IsDisabled();
<WebElement>.AssertElement().IsDisplayed();
<WebElement>.AssertElement().IsEnabled();
<WebElement>.AssertElement().IsEnabledAndDisplayed();
<WebElement>.AssertElement().IsHidden();
<WebElement>.AssertElement().IsNotDisplayed();
<WebElement>.AssertElement().IsNotPresent();
<WebElement>.AssertElement().IsPresent();
<WebElement>.AssertElement().IsSelected();
<WebElement>.AssertElement().IsTextEqual(string text);
<WebElement>.AssertElement().TextEndsWith(string text);
<WebElement>.AssertElement().TextStartsWith(string text);
```
###### CheckBox()
```C# 
//
<WebElement>.CheckBox().AssertIsNotTicked();
<WebElement>.CheckBox().AssertIsTicked();
<WebElement>.CheckBox().IsTicked();
<WebElement>.CheckBox().Tick();
<WebElement>.CheckBox().Toggle();
<WebElement>.CheckBox().UnTick(); 
```
###### CheckElement()
```C# 
//
<WebElement>.CheckElement().IsDisabled();
<WebElement>.CheckElement().IsDisplayed();
<WebElement>.CheckElement().IsEnabled();
<WebElement>.CheckElement().IsEnabledAndDisplayed();
<WebElement>.CheckElement().IsNotDisplayed();
<WebElement>.CheckElement().IsNotPresent();
<WebElement>.CheckElement().IsNotSelected();
<WebElement>.CheckElement().IsPresent();
<WebElement>.CheckElement().IsSelected();
```
###### Dropdown()
```C#
//
<WebElement>.Dropdown().GetTextOfSelectedValue();
<WebElement>.Dropdown().SelectByIndex(int index);
<WebElement>.Dropdown().SelectByText(string text);
```
###### Wait()
```C#
//
<WebElement>.Wait().WaitUntilDisabled();
<WebElement>.Wait().WaitUntilInvisible();
<WebElement>.Wait().WaitUntilNotExist();
<WebElement>.Wait().WaitUntilVisible();
<WebElement>.Wait().WaitUntilVisibleAndEnabled();
```
###### Screenshot.<CompareWindow / CompareElement>()
```C# 
//Compare entire page screenshot
ScreenShot.CompareWindow(<screenshotDirectory>, <screenshotFileName>);
  
//Compare elements within page
ScreenShot.CompareElement(<screenshotDirectory>, <screenshotFileName>, <WebElement>);
  
//Save screenshot upon failure
//Setting in App.conig - false by default
    <add key="DefaultScreenshotting" value="true" />
```

# VAFT Utility Examples

## VAFT Utility - Checkbox 
#### Example
##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
 
namespace VTestFunctionality
{
    class CheckBoxPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "RememberMe")]
        protected IWebElement RememberPasswordCheckbox { get; set; }
        
        public CheckBoxPage(IWebDriver driver) : base(driver)
        {
        }
        public IWebElement GetRememberPasswordCheckBox()
        {
            return RememberPasswordCheckbox;
        }
        public CheckBoxPage NavigateTo()
        {
            Driver.NavigateTo().GoToUrl("http://www.sometestpagethatyouwanttottest.com");
            return this;
        }
    }
}
```
##### Test class

```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using Vaft.Selenium.Element;
using Vaft.Selenium.Utilities;
 
namespace VTestFunctionality
{
    class CheckBoxTest : TestBase
    {
        private CheckBoxPage _checkBoxPage;
 
        [SetUp]
        public void SetUp()
        {
            _checkBoxPage = new CheckBoxPage(Driver);
        }
        [Test]
        public void VerifyUntickedCheckBoxThenTickAndVerifyTickedCheckbox()
        {
            _checkBoxPage.NavigateTo();
            _checkBoxPage.GetRememberPasswordCheckBox().Checkbox().AssertIsNotTicked();
            _checkBoxPage.GetRememberPasswordCheckBox().Checkbox().Tick();
            _checkBoxPage.GetRememberPasswordCheckBox().Checkbox().AssertIsTicked();
        }
    }
}
```
## VAFT Utility - Right click 
#### Example
##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using Vaft.Selenium.Core;
using Vaft.Selenium.Utilities;
 
namespace VTestFunctionality
{
    class ContextMenuPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "context-link")]
        protected IWebElement ClickMeLink { get; set; }
        
        [FindsBy(How = How.Id, Using = "contextMenu")]
        protected IWebElement ContextMenuList { get; set; }
 
        public ContextMenuPage(IWebDriver driver)
            : base(driver)
        {
        }
        public ContextMenuPage NavigateTo()
        {
            NavigateToUrl("/Examples/HtmlElements");
            return this;
        }
        public IWebElement GetClickMeLink()
        {
            return ClickMeLink;
        }
        public ContextMenuPage AssertThatContextMenuIsDisplayed()
        {
            CheckContextMenuOption(ContextMenuList, "Action");
            CheckContextMenuOption(ContextMenuList, "Another action");
            CheckContextMenuOption(ContextMenuList, "Something else here");
            CheckContextMenuOption(ContextMenuList, "Separated link");
            return this;
        }
        /*
         * Get entire list from menu, when right clicking and verify that each entry (stated in 
         * AssertThatContextMenuIsDisplayed()) is displayed. If one menu item is missing in the list, 
         * throw an error stating that the item is not available in the menu
         */
        private void CheckContextMenuOption(IWebElement contextMenu, string text)
        {
            IList<IWebElement> options = contextMenu.FindElements(By.XPath("ul/li/a"));
            var selectionFlag = 0;
            foreach (var option in options)
            {
                if (option.Text.Equals(text))
                {
                    option.AssertElement().IsDisplayed();
                    selectionFlag = 1;
                    break;
                }
            }
            if (selectionFlag != 1)
            {
                throw new InvalidOperationException("Option '" + text + "' is not available in menu selection");
            }
        }
    }
}
```

##### Test class

```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using Vaft.Selenium.Element;
namespace VTestFunctionality
{
    class ContextMenuTest : TestBase
    {
        ContextMenuPage _contextMenuPage;
 
        [SetUp]
        public void SetUp()
        {
            _contextMenuPage = new ContextMenuPage(Driver);
        }
        [Test]
        public void RightClickAndVerifyContextInMenu()
        {
            _contextMenuPage.NavigateTo();
            _contextMenuPage.GetClickMeLink().AdvancedAction().RightClick();
            _contextMenuPage.AssertThatContextMenuIsDisplayed();
        }
    }
}
```
## VAFT Utility - Double click 
### Example
##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
namespace VTestFunctionality
{
    class DoubleClickPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "dbl-click-button")]
        protected IWebElement ClickMeTwiceBtn { get; set; }
 
        [FindsBy(How = How.Id, Using = "message")]
        protected IWebElement Message { get; set; }
 
        public DoubleClickPage(IWebDriver driver)
            : base(driver)
        {
        }
        public DoubleClickPage NavigateTo()
        {
            NavigateToUrl("/Examples/HtmlElements");
            return this;
        }
        public IWebElement GetDblClikcBtn()
        {
            return ClickMeTwiceBtn;
        }
        public IWebElement GetMessage()
        {
            return Message;
        }
    }
}
```
##### Test class

```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using Vaft.Selenium.Element;
namespace VTestFunctionality
{
    public class DoubleClickTest : TestBase
    {
        private DoubleClickPage _doubleClickPage;
 
        [SetUp]
        public void SetUp()
        {
            _doubleClickPage = new DoubleClickPage(Driver);
        }
        
        /* Go to website and check that the message (which is not displayed before the button "click me twice"
         * is double clicked) is not displayed. Click on the button twice and verify that the message now 
         * is displayed on the screen.
         */
        [Test]
        public void DoubleClick()
        {
            _doubleClickPage.NavigateTo();
            _doubleClickPage.GetMessage().AssertElement().IsNotDisplayed();
            _doubleClickPage.GetDblClikcBtn().AdvancedAction().DoubleClick();
            _doubleClickPage.GetMessage().AssertElement().IsDisplayed();
        }
    }
}
```
## VAFT Utility - Drop-down list  
### Example
##### Page class

```C#
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
using Vaft.Selenium.Element;
 
namespace VTestFunctionality
{
    class DropDownPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "enabledselection")]
        protected IWebElement EnabledDropdown { get; set; }
 
        [FindsBy(How = How.Id, Using = "disabledselection")]
        protected IWebElement DisabledDropdown { get; set; }
 
        public DropDownPage(IWebDriver driver)
            : base(driver)   
        {
        }
        public DropDownPage NavigateTo()
        {
            NavigateToUrl("/Examples/HtmlElements");
            return this;
        }
        public IWebElement GetEnabledDropdown()
        {
            return EnabledDropdown;
        }
        public IWebElement GetDisabledDropdown()
        {
            return DisabledDropdown;
        }
        public DropDownPage SelectEnabledDropDown(string text)
        {
            EnabledDropdown.Dropdown().SelectByText(text);
            return this;
        }
        public DropDownPage SelectEnabledDropDown(int id)
        {
            EnabledDropdown.Dropdown().SelectByIndex(id);
            return this;
        }
        public DropDownPage AssertEnabledDropDownValueSelected(string text)
        {
            StringAssert.AreEqualIgnoringCase(text, EnabledDropdown.Dropdown().GetTextOfSelectedValue());
            return this;
        }
        public DropDownPage AssertDisabledDropDownValueSelected(string text)
        {
            StringAssert.AreEqualIgnoringCase(text, DisabledDropdown.Dropdown().GetTextOfSelectedValue());
            return this;
        }
    }
}
```
##### Test class

```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using Vaft.Selenium.Element;
 
namespace VTestFunctionality
{
    class DropDownTest : TestBase
    {
        private DropDownPage _dropDownPage;
 
        [SetUp]
        public void SetUp()
        {
            _dropDownPage = new DropDownPage(Driver);
        }
        [Test]
        public void SelectDropDownEnabled()
        {
            _dropDownPage.NavigateTo();
            _dropDownPage.GetEnabledDropdown().Dropdown().SelectByText("Oslo");
            _dropDownPage.AssertEnabledDropDownValueSelected("Oslo");
        }
        [Test]
        public void SelectDropDownEnabledById()
        {
            _dropDownPage.NavigateTo();
            _dropDownPage.GetEnabledDropdown().Dropdown().SelectByIndex(3);
            _dropDownPage.AssertEnabledDropDownValueSelected("New York");
        }
        [Test]
        public void VerifyThatDropDownIsDisabledAndCheckCorrectValue()
        {
            _dropDownPage.NavigateTo();
            _dropDownPage.GetDisabledDropdown().AssertElement().IsDisabled();
            _dropDownPage.AssertDisabledDropDownValueSelected("Oslo");
        }
    }
}
```
## VAFT Utility - Verify displayed and not displayed text  
### Example
##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
 
namespace VaftExamples.ExampleVaftTests.Pages
{
    public class VaftLoginPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "username")]
        protected IWebElement UserName { get; set; }
 
        [FindsBy(How = How.Id, Using = "password")]
        protected IWebElement Password { get; set; }
 
        [FindsBy(How = How.Id, Using = "RememberMe")]
        protected IWebElement RememberPasswordCheckbox { get; set; }
 
        [FindsBy(How = How.Id, Using = "submit")]
        protected IWebElement LoginBtn { get; set; }
 
        public VaftLoginPage(IWebDriver driver) : base(driver)
        {
        }
        public VaftLoginPage NavigateTo()
        {
            Driver.Navigate().GoToUrl("http://vuu-care-web1.vsw.datakraftverk.no/VaftWeb/Account/Login");
            return this;
        }
        public VaftLoginPage TypeUserName(string userName)
        {
            UserName.SendKeys(userName);
            return this;
        }
        public VaftLoginPage TypePassword(string password)
        {
            Password.SendKeys(password);
            return this;
        }
        //This is not used in the example
        public IWebElement GetRememberPasswordCheckbox()
        {
            return RememberPasswordCheckbox;
        }
        public VaftLoginPage ClickLoginButton()
        {
            LoginBtn.Click();
            return this;
        }
    }
}
```
##### Test class

```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using VaftExamples.ExampleVaftTests.Pages;
 
namespace VaftExamples.ExampleVaftTests.Tests
{
    public class VaftLoginTestExample : TestBase
    {
        private VaftLoginPage _vaftLoginPage;
        private VaftHomePage _vaftHomePage;
 
        [SetUp]
        public void SetUp()
        {
            _vaftLoginPage = new VaftLoginPage(Driver);
            _vaftHomePage = new VaftHomePage(Driver);
            _vaftLoginPage.NavigateTo();
        }
        [Test]
        public void TryToLoginWithIncorrectUsernamePassword()
        {
            _vaftLoginPage.NavigateTo();
            _vaftLoginPage.VerifyThatTextIsNotDisplayed("The user name or password provided is incorrect.");
            _vaftLoginPage
                .TypeUserName("test")
                .TypePassword("test")
                .ClickLoginButton();
            _vaftLoginPage.VerifyThatTextIsDisplayed("The user name or password provided is incorrect.");
        }
    }
}
```
## VAFT Utility - Scrolling example  
### Example
##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
 
namespace VTestFunctionality
{
    class ScrollToItemPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "topText")]
        protected IWebElement TopText { get; set; }
 
        [FindsBy(How = How.Id, Using = "link")]
        protected IWebElement ScrollDownLink { get; set; }
 
        [FindsBy(How = How.Id, Using = "middleText")]
        protected IWebElement MiddleText { get; set; }
 
        [FindsBy(How = How.Id, Using = "bottomText")]
        protected IWebElement BottomText { get; set; }
 
        public ScrollToItemPage(IWebDriver driver)
            : base(driver)
        {
        }
        public ScrollToItemPage NavigateTo()
        {
            NavigateToUrl("/Examples/ScrollingPage");
            return this;
        }
        public IWebElement GetTopText()
        {
            return TopText;
        }
        public IWebElement GetMiddleText()
        {
            return MiddleText;
        }
        public IWebElement GetBottomText()
        {
            return BottomText;
        }
        public ScrollToItemPage ClickScrollDownLink()
        {
            ScrollDownLink.Click();
            WaitForAnimation();
            return this;
        }
    }
}
```
##### Test class

```C#
using NUnit.Framework;
using Vaft.Selenium.Core;
using Vaft.Selenium.Element;
 
namespace VTestFunctionality
{
    class ScrollToItemTest : TestBase
    {
        private ScrollToItemPage _scrollablePage;
 
        [SetUp]
        public void SetUp()
        {
            _scrollablePage = new ScrollToItemPage(Driver);
        }
        [Test]
        public void ScrollToElementOnBottomAndVerifyText()
        {
            _scrollablePage.NavigateTo();
            _scrollablePage.GetMiddleText().AdvancedAction().ScrollToElement("Bottom");
            _scrollablePage.VerifyThatTextIsDisplayed("Bottom of the page");
        }
        [Test]
        public void ScrollToElementOnTopAndVerifyText()
        {
            _scrollablePage.NavigateTo();
            _scrollablePage.GetTopText().AdvancedAction().ScrollToElement("Bottom");
            _scrollablePage.GetTopText().AdvancedAction().ScrollToElement("Top");
            _scrollablePage.VerifyThatTextIsDisplayed("Click element below to scroll down the page.");
        }
        [Test]
        public void ClickScrollDownLinkAndVerifyText()
        {
            _scrollablePage.NavigateTo();
            _scrollablePage.ClickScrollDownLink();
            _scrollablePage.VerifyThatTextIsDisplayed("Bottom of the page");
        }
    }
}
```
## VAFT Utility - Compare entire window example  
### Example
Regression test of entire window, make sure that the elements present on a web page still are as they were in previous version. Compare entire window with a baseline (previous version of the window) .NET VAFT. In this example we have reused the code from the Login example, and are comparing the login window.

##### Baseline (expected) screenshot
This is the expected screenshot that we compare all new screenshots with. (Note: If testing on different browsers you will need to have a baseline for each browser, as they render the web page differently).
<add screenshots>

##### Actual screenshot
The actual screenshot, what was taken upon execution of the test.
<add screenshots>

#### Deviations between the screenshots (marked in red)
The two screenshots compared (actual and baseline), where differences are marked with red. (Note: If for example an unexpected element appears on top of the (actual) screen, the entire screen will be marked red since it).
<add screenshots>

##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
 
namespace VaftExamples.ExampleVaftTests.Pages
{
    public class VaftLoginPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "username")]
        protected IWebElement UserName { get; set; }
 
        [FindsBy(How = How.Id, Using = "password")]
        protected IWebElement Password { get; set; }
  
        [FindsBy(How = How.Id, Using = "RememberMe")]
        protected IWebElement RememberPasswordCheckbox { get; set; }
 
 
        [FindsBy(How = How.Id, Using = "submit")]
        protected IWebElement LoginBtn { get; set; }
        
        public VaftLoginPage(IWebDriver driver)
            : base(driver)
        {
        }
        public VaftLoginPage NavigateTo()
        {
            Driver.Navigate().GoToUrl("http://vuu-care-web1.vsw.datakraftverk.no/VaftWeb/Account/Login");
            return this;
        }
        public VaftLoginPage TypeUserName(string userName)
        {
            UserName.SendKeys(userName);
            return this;
        }
        public VaftLoginPage TypePassword(string password)
        {
            Password.SendKeys(password);
            return this;
        }
        public IWebElement GetRememberPasswordCheckbox()
        {
            return RememberPasswordCheckbox;
        }
        public VaftLoginPage ClickLoginButton()
        {
            LoginBtn.Click();
            return this;
        }
    }
}
```
##### Test class

```C#

using NUnit.Framework;
using System;
using System.Drawing;
using Vaft.Selenium.Core;
using Vaft.Selenium.Utilities;
using VaftExamples.ExampleVaftTests.Pages;
namespace VaftExamples.ExampleVaftTests.Tests
{
    public class VaftLoginTestExample : TestBase
    {
        private VaftLoginPage _vaftLoginPage;
        private VaftHomePage _vaftHomePage;
        [SetUp]
        public void SetUp()
        {
            _vaftLoginPage = new VaftLoginPage(Driver);
            _vaftHomePage = new VaftHomePage(Driver);
            _vaftLoginPage.NavigateTo();
        }
        [Test]
        public void CompareWindowScreenshot()
        {
            Driver.Manage().Window.Size = new Size(1024, 768);
            _vaftLoginPage.NavigateTo();
            _vaftLoginPage
                .TypeUserName("test")
                .TypePassword("test")
                .ClickLoginButton();
            var baselineScrDir = AppDomain.CurrentDomain.BaseDirectory + "Screenshots\\Comparison\\";
            ScreenShot.CompareWindow(baselineScrDir, "LoginPageScr");
        }
    }
}
```
## VAFT Utility - Screenshot - Compare an element example  
### Example
Regression test of an element on the web page, make sure that the element is present on a web page still are as they were in previous version. Compare the element with a baseline (previous version of the element) .NET VAFT. In this example we have reused the code from the check box example, and are comparing the login window.

##### Baseline (expected) screenshot
This is the expected screenshot that we compare all new screenshots with. (Note: If testing on different browsers you will need to have a baseline for each browser, as they render the web page differently).
<add screenshots>

##### Actual screenshot
The actual screenshot, what was taken upon execution of the test.
<add screenshots>

#### Deviations between the screenshots (marked in red)
The two screenshots compared (actual and baseline), where differences are marked with red. (Note: If for example an unexpected element appears on top of the (actual) screen, the entire screen will be marked red since it).
<add screenshots>

##### Page class

```C#
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Vaft.Selenium.Core;
 
namespace VTestFunctionality
{
    class CheckBoxPage : PageBase
    {
        [FindsBy(How = How.Id, Using = "RememberMe")]
        protected IWebElement RememberPasswordCheckbox { get; set; }
 
        public CheckBoxPage(IWebDriver driver)
            : base(driver)
        {
        }
        public IWebElement GetRememberPasswordCheckBox()
        {
            return RememberPasswordCheckbox;
        }
        public CheckBoxPage NavigateTo()
        {
            Driver.Navigate().GoToUrl("http://vuu-care-web1.vsw.datakraftverk.no/VaftWeb/Account/Login");
            return this;
        }
    }
}
```
##### Test class

```C#
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using Vaft.Selenium.Core;
using Vaft.Selenium.Utilities;
 
namespace VTestFunctionality
{
    class CheckBoxTest : TestBase
    {
        private CheckBoxPage _checkBoxPage;
 
        [SetUp]
        public void SetUp()
        {
            _checkBoxPage = new CheckBoxPage(Driver);
        }
        [Test]
        public void CompareElementScreenshot()
        {
            _checkBoxPage.NavigateTo();
            IWebElement checkboxDiv = Driver.FindElement(By.ClassName("checkbox"));
            var baselineScrDir = AppDomain.CurrentDomain.BaseDirectory + "Screenshots\\Comparison\\";
             
            // Uncomment to make the test fail by checking the checkbox
            //_checkBoxPage.GetRememberPasswordCheckBox().Checkbox().Tick();
            ScreenShot.CompareElementImage(baselineScrDir, "CheckBoxDiv", checkboxDiv);
        }
    }
}
```
