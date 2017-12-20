using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Vaft.Framework.Element
{
    public class ElementCheckUtils
    {
        private readonly IWebElement _element;
        private readonly bool _isTimeToWaitDefined;
        private readonly TimeSpan _timeToWait;

        public ElementCheckUtils(IWebElement element)
        {
            _element = element;
            _isTimeToWaitDefined = false;
        }

        public ElementCheckUtils(IWebElement element, TimeSpan timeToWait)
            : this(element)
        {
            _isTimeToWaitDefined = true;
            _timeToWait = timeToWait;
        }

        /// <summary>Checks that element is displayed in Web page.</summary>
        /// <returns>True if element is displayed; otherwise, false.</returns>
        public bool IsDisplayed()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsDisplayed();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

            try
            {
                _element.Assert().IsDisplayed();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>Checks that element is not displayed in Web page.</summary>
        /// <returns>True if element is not displayed; otherwise, false.</returns>
        public bool IsNotDisplayed()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsNotDisplayed();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsNotDisplayed();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Check that element is enabled.</summary>
        /// <returns>True if element is enabled; otherwise, false.</returns>
        public bool IsEnabled()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsEnabled();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsEnabled();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Check that element is displayed and enabled.</summary>
        /// <returns>True if element is displayed and enabled; otherwise, false.</returns>
        public bool IsEnabledAndDisplayed()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsEnabledAndDisplayed();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsEnabledAndDisplayed();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Check that element is disabled.</summary>
        /// <returns>True if element is displayed and disabled; otherwise, false.</returns>
        public bool IsDisabled()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsPresent();
                    _element.Assert(_timeToWait).IsDisabled();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsPresent();
                _element.Assert().IsDisabled();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Checks that element exists in page source.</summary>
        /// <returns>True if element exists in page source; otherwise, false.</returns>
        public bool IsPresent()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsPresent();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsPresent();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Checks that element does not exist in page source.</summary>
        /// <returns>True if element does not exist in page source; otherwise, false.</returns>
        public bool IsNotPresent()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsNotPresent();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsNotPresent();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Check that element is selected.</summary>
        /// <returns>True if element is selected; otherwise, false.</returns>
        public bool IsSelected()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsSelected();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsSelected();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>Check that element is not selected.</summary>
        /// <returns>True if element is not selected; otherwise, false.</returns>
        public bool IsNotSelected()
        {
            if (_isTimeToWaitDefined)
            {
                try
                {
                    _element.Assert(_timeToWait).IsNotSelected();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                _element.Assert().IsNotSelected();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
