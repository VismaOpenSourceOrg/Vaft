using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

namespace Vaft.Framework.Element.Customization.WebComponents
{
   public class WebComponentsElement : IWebElement, IWrapsElement
    {
        private const string JavaScriptHelperFilePath = "Vaft.Framework.Element.Customization.WebComponents.WebComponentsElementHelper.js";
        private readonly By _by;
        private readonly IWebDriver _webdriver;
        private IWebElement _cashedElement;
        private IWebElement _parrentField;
        private readonly List<By> _parrentFieldSelectors;
        private List<IWebElement> _parrentFields;


        /// <summary> ShadowDomWebElement constructor
        /// </summary>
        /// <param name="driver">WebDriver object</param>
        /// <param name="by">Element selector</param>
        /// <param name="parrentSelectors">List of parent element selectors. This param can be used if there is a need explicitly define parent selector or selectors that are outside target element shadow dom.</param>
        /// <returns></returns>
        public WebComponentsElement(IWebDriver driver, By by, List<By> parrentSelectors = null)
        {
            _webdriver = driver;
            _by = by;
            _parrentFieldSelectors = parrentSelectors;
        }
        
        
        /// <summary> Static ShadowDomWebElement initializator</summary>
        /// <param name="driver">WebDriver object</param>
        /// <param name="by">Element selector</param>
        /// <param name="parentSelectors">List of parent element selectors. This param can be used if there is a need explicitly define parent selector or selectors that are outside target element shadow dom.</param>
        public static WebComponentsElement CreateInstance(IWebDriver driver, By by, List<By> parentSelectors = null)
        {
            return new WebComponentsElement(driver, by, parentSelectors);
        }
      
        public IWebElement Element
        {
            get
            {
                IWebElement parentField = _parrentField;
                if (_parrentFieldSelectors != null && _parrentFieldSelectors.Any())
                {
                    _parrentFields = FindParrentFields();
                    if (_parrentFields != null && _parrentFields.Any())
                    {
                        parentField = _parrentFields.Last();
                    }
                }

                How how = GetFindMethod(_by);
                string usingstring = GetUsingString(how, _by);
                _cashedElement = SearchElement(how, usingstring, parentField);
                return _cashedElement;
            }
        }

        /// <summary>
        /// Search for IwebElement by given By selector. Element search is done at first in the main page DOM and if no elements were found then element search is done in all existing Shadow DOM's
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public IWebElement FindElement(By by)
        {
            IWebElement expectedElement = null;

            //1st seach in current element shadow root
            How howMethod = GetFindMethod(by);
            string usingString = GetUsingString(howMethod, by);
            try
            {
                expectedElement = SearchElement(howMethod, usingString, Element);
                if (expectedElement != null)
                {
                    return expectedElement;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            //2nd search in current element
            try
            {
                expectedElement = Element.FindElement(by);
                if (expectedElement != null)
                {
                    return expectedElement;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return expectedElement;
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Element.FindElements(by);
        }

        public void SetParrentField(IWebElement parrentField)
        {
            _parrentField = parrentField;
        }

        public void Clear()
        {
            Element.Clear();
        }

        public void SendKeys(string text)
        {
            Element.SendKeys(text);
        }

        public void Submit()
        {
            Element.Submit();
        }

        public void Click()
        {
            Element.Click();
        }

        public string GetAttribute(string attributeName)
        {
            return Element.GetAttribute(attributeName);
        }

        public string GetProperty(string propertyName)
        {
            return Element.GetProperty(propertyName);
        }

        public string GetCssValue(string propertyName)
        {
            return Element.GetCssValue(propertyName);
        }

        public string TagName => Element.TagName;
        public string Text => Element.Text;
        public bool Enabled => Element.Enabled;
        public bool Selected => Element.Selected;
        public Point Location => Element.Location;
        public Size Size => Element.Size;
        public bool Displayed => Element.Displayed;
        public IWebElement WrappedElement => Element;

       
        /// <summary>
        /// Search for IwebElement in ShadowDOM.  
        /// </summary>
        /// <param name="how"></param>
        /// <param name="usingString"></param>
        /// <param name="rootElement">If rootElement is defined then the search is performed inside rootElement</param>
        /// <returns></returns>
        /// <exception cref="NoSuchElementException"></exception>
        public IWebElement SearchElement(How how, string usingString, IWebElement rootElement = null)
        {
            List<RemoteWebElement> elementsList;
            elementsList = SearchElements(how, usingString, rootElement);
            if (elementsList.Count > 0)
            {
                return elementsList.First();
            }

            throw new NoSuchElementException($"Failed to find element in shadowDom By:{_by}");
        }

        public IWebElement SearchElement(By by, IWebElement rooElement = null)
        {
            How how = GetFindMethod(by);
            string usingString = GetUsingString(how, by);
            return SearchElement(how, usingString, rooElement);
        }

        public IWebElement SearchElement(IWebElement rooElement = null)
        {
            How how = GetFindMethod(_by);
            string usingString = GetUsingString(how, _by);
            return SearchElement(how, usingString, rooElement);
        }

        public List<RemoteWebElement> SearchElements(How how, string usingstring, IWebElement rootElement = null)
        {
            List<RemoteWebElement> elementsList = new List<RemoteWebElement>();
            switch (how)
            {
                case How.CssSelector:
                    elementsList = SearchElementsByCss(rootElement, usingstring);
                    break;
                case How.XPath:
                    elementsList = SearchElementByXpath(rootElement, usingstring);
                    if (elementsList != null && elementsList.Count > 0)
                    {
                        foreach (RemoteWebElement element in elementsList)
                        {
                            try
                            {
                                IWebElement webElement = element.FindElement(_by);
                                elementsList.Add((RemoteWebElement)webElement);
                            }
                            catch (Exception)
                            { 
                                //ignored
                            }
                        }
                    }

                    break;
                case How.ClassName:
                    throw new InvalidSelectorException("How.ClassName is not supported yet");
                case How.Id:
                    elementsList = SearchElementById(rootElement, usingstring);
                    break;

            }

            if (elementsList != null && elementsList.Count.Equals(0))
            {
                throw new NoSuchElementException($"Failed to find elements in shadowDom By:{_by}");
            }

            return elementsList;
        }

        public List<RemoteWebElement> SearchElements(By by, IWebElement rootField = null)
        {
            How how = GetFindMethod(by);
            string usingString = GetUsingString(how, by);
            return SearchElements(how, usingString, rootField);
        }

        public List<RemoteWebElement> SearchElements(IWebElement rootField = null)
        {
            return SearchElements(_by, rootField);
        }

        private List<RemoteWebElement> FilterRezultsList(IReadOnlyCollection<object> list)
        {
            List<RemoteWebElement> remoteWebElements = new List<RemoteWebElement>();
            if (list != null && list.Count > 0)
            {
                foreach (object element in list)
                {
                    var elementType = element.GetType();
                    if (elementType.Equals(typeof(RemoteWebElement)) || 
                        elementType.Equals(typeof(FirefoxWebElement))||  
                        elementType.Equals(typeof(ChromeWebElement)) || 
                        elementType.Equals(typeof(InternetExplorerWebElement)) ||
                        elementType.Equals(typeof(EdgeWebElement)) ||
                        elementType.Equals(typeof(OperaWebElement))
                      )
                    {
                        remoteWebElements.Add((RemoteWebElement)element);
                    }

                    if (element.GetType() == typeof(ReadOnlyCollection<IWebElement>))
                    {
                        foreach (IWebElement subElement in (ReadOnlyCollection<IWebElement>)element)
                        {
                            if (subElement.GetType() == typeof(RemoteWebElement))
                            {
                                remoteWebElements.Add((RemoteWebElement)subElement);
                            }
                        }
                    }
                }
            }

            return remoteWebElements;
        }

        private List<IWebElement> FindParrentFields()
        {
            _parrentFields = new List<IWebElement>();
            IWebElement previuosElement = null;
            for (int i = 0; i < _parrentFieldSelectors.Count; i++)
            {
                How how = GetFindMethod(_parrentFieldSelectors[i]);
                string usingString = GetUsingString(how, _parrentFieldSelectors[i]);
                if (i.Equals(0))
                {
                    //Try search first in regular DOM
                    try
                    {
                        if (_parrentField != null)
                        {
                            previuosElement = _parrentField.FindElement(_parrentFieldSelectors[i]);
                        }
                        else
                        {
                            previuosElement = _webdriver.FindElement(_parrentFieldSelectors[i]);
                        }

                        _parrentFields.Add(previuosElement);
                        continue;
                    }
                    catch (Exception)
                    {
                        //ignored
                    }

                    //search in shadowdom
                    try
                    {
                        previuosElement = SearchElements(how, usingString, _parrentField).First();
                        _parrentFields.Add(previuosElement);
                        continue;
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Failed to find first parrent field", e);
                    }
                }

                try
                {
                    var subsequentElement = SearchElement(_parrentFieldSelectors[i], previuosElement);
                    _parrentFields.Add(subsequentElement);
                    previuosElement = subsequentElement;
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to find subsequent element using: {_parrentFieldSelectors[i]}", e);
                }
            }

            return _parrentFields;
        }

        private List<RemoteWebElement> SearchElementByCss(IWebElement rootElement, string selectorString)
        {
            string jsMethodCall = "return searchForElementByCss(arguments[0])";
            if (rootElement != null)
            {
                jsMethodCall = "return searchForElementByCss(arguments[0],arguments[1])";
            }

            return SearchForElements(jsMethodCall, selectorString, rootElement);
        }

        private List<RemoteWebElement> SearchElementsByCss(IWebElement rootElement, string selectorString)
        {
            string jsMethodCall = "return searchForElementByCss(arguments[0])";
            if (rootElement != null)
            {
                jsMethodCall = "return searchForAllElementsByCss(arguments[0],arguments[1])";
            }

            return SearchForElements(jsMethodCall, selectorString, rootElement);
        }

        private List<RemoteWebElement> SearchElementByXpath(IWebElement rootElement, string selectorString)
        {
            string jsMethodCall = "return searchForElementByXpath(arguments[0])";
            if (rootElement != null)
            {
                jsMethodCall = "return searchForElementByXpath(arguments[0],arguments[1])";
            }

            return SearchForElements(jsMethodCall, selectorString, rootElement);
        }
        
        private List<RemoteWebElement> SearchElementById(IWebElement rootElement, string id)
        {
            string jsMethodCall = "return searchForElementById(arguments[0])";
            if (rootElement != null)
            {
                jsMethodCall = "return searchForElementById(arguments[0],arguments[1])";
            }

            return SearchForElements(jsMethodCall, id, rootElement);
        }

        private List<RemoteWebElement> SearchForElements(string javascriptMethod, string selector, IWebElement rootElement = null)
        {
            IReadOnlyCollection<object> searchRezults = (IReadOnlyCollection<object>)((IJavaScriptExecutor)_webdriver).ExecuteScript(GetElementsSearchJavascript() + javascriptMethod, selector, rootElement);
            return FilterRezultsList(searchRezults);
        }

        private string GetElementsSearchJavascript()
        {
            var asm = Assembly.GetExecutingAssembly();
            using (var stream = asm.GetManifestResourceStream(JavaScriptHelperFilePath))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                throw new Exception($"ShadowIwebElement javacript helper file is not found. Path [{JavaScriptHelperFilePath}]. Check file build actions settings or file path");
            }
        }

        private How GetFindMethod(By by)
        {
            How? how = null;
            string stringVersion = by.ToString();
            stringVersion = stringVersion.Replace("By.", string.Empty);
            if (stringVersion.StartsWith(How.CssSelector.ToString()))
            {
                how = How.CssSelector;
            }

            if (stringVersion.StartsWith(How.ClassName.ToString()))
            {
                how = How.ClassName;
            }

            if (stringVersion.StartsWith(How.Id.ToString()))
            {
                how = How.Id;
            }

            if (stringVersion.StartsWith(How.XPath.ToString()))
            {
                how = How.XPath;
            }

            if (how == null)
            {
                throw new ArgumentException("How: is not identified  ");
            }

            return how.Value;
        }

        private string GetUsingString(How how, By by)
        {
            string usingString = by.ToString().Replace("By." + how + ": ", string.Empty);
            return usingString;
        }
        
    }

  
}