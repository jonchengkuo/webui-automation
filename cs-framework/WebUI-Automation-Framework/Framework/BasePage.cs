﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions

namespace WebUI.Automation.Framework
{

    /// <summary>
    /// Base class for representing and interacting with a web page using the page object model.
    /// A subclass should be defined for each specific web page
    /// to simulate its application-specific UI behaviors (e.g., logging in).
    /// 
    /// <para><b>Example:</b></para>
    /// <pre>
    ///   using WebUI.Automation.Framework;
    /// 
    ///   public class LoginPage : BasePage<LoginPage> {
    ///       public void LogIn(string user, string password) {
    ///           ...
    ///       }
    ///   }
    /// </pre> 
    /// </summary>
    public class BasePage<T> where T : BasePage<T>
    {

        private Browser browser;
        private By locator;

        protected internal BasePage()
        {
        }

        protected internal BasePage(By locator)
        {
            if (locator == null)
            {
                throw new System.NullReferenceException("The locator given to the page is null.");
            }
            this.locator = locator;
        }

        protected internal virtual BaseElement KeyElement
        {
            set
            {
                if (value == null)
                {
                    throw new System.NullReferenceException("The key element given to the page is null.");
                }
                this.locator = value.Locator;
            }
        }

        private void AssertLocatorNotNull()
        {
            if (this.locator == null)
            {
                throw new System.NullReferenceException("The locator of this page object is not set." + " You must set it (preferred in your page class constructor) using either" + " the BasePage constructor or the BasePage.setKeyElement method.");
            }
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Returns the <seealso cref="Browser"/> instance used by this page.
        /// It is default to the <seealso cref="Browser"/> instance returned by <seealso cref="WebUI#getDefaultBrowser"/>
        /// when this page is created. </summary>
        /// <returns> the <seealso cref="Browser"/> instance used by this page </returns>
        public Browser Browser
        {
            get
            {
                if (this.browser == null)
                {
                    this.browser = WebUI.DefaultBrowser;
                }
                return this.browser;
            }
        }

        // Comment out this method until we want the framework to support multiple Browser instances.
        /*
         * Sets the {@link Browser} instance used by this page.
         * TODO: It should also set the Browser instance of all of the UI elements in this page object.
         * @return this page itself (for supporting the fluid interface) 
         *
        @SuppressWarnings("unchecked")
        public T setBrowser(Browser browser) {
            if (browser == null) {
                throw new NullPointerException("The given browser object is null.");
            }
            this.browser = browser;
            // TODO: Set all UI elements on this page object to use the given browser.
            return (T) this;
        }*/


        /// <summary>
        /// Returns whether this web page is visible or not within the default page loading timeout,
        /// which is specified and can be configured by <seealso cref="WebUI.DefaultPageLoadingTimeout"/>.
        /// 
        /// A page is considered available if the browser can use the locator of this page to locate a visible <seealso cref="WebElement"/>.
        /// </summary>
        /// <returns> <code>true</code> if this web page is available within the default page loading timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool Visible
        {
            get
            {
                return BecomeVisible(WebUI.DefaultPageLoadingTimeout);
            }
        }

        /// <summary>
        /// Returns whether this web page becomes visible or not within the specified timeout.
        /// If the specified timeout is 0, it will check the current availability of this web page.
        /// If the specified timeout is greater than 0, it will periodically (every half second)
        /// check the existence of this web page until the specified timeout is reached.
        /// 
        /// A page is considered available if the browser can use the locator of this page to locate a visible <seealso cref="WebElement"/>.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> <code>true</code> if this web page is available within the specified timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool BecomeVisible(int timeOutInSeconds)
        {
            AssertLocatorNotNull();
            if (timeOutInSeconds == 0)
            {
                IWebElement webElement = Browser.FindElement(this.locator);
                return webElement.Displayed;
            }
            else
            {
                try
                {
                    WaitUntilAvailable(timeOutInSeconds);
                    return true;
                }
                catch (TimeoutException)
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// Waits until this page becomes available (i.e., its key element is visible), or until the default page-loading timeout is reached.
        /// The default page-loading timeout is determined and can be configured by <seealso cref="WebUI#defaultPageLoadingTimeout"/>.
        /// 
        /// A page is considered available if the browser can use the locator of this page to locate a visible <seealso cref="WebElement"/>.
        /// </summary>
        /// <returns> this page itself (for supporting the fluid interface) </returns>
        /// <exception cref="TimeoutException"> if the page is still not available after the default page-loading timeout expires  </exception>
        public virtual T WaitUntilAvailable()
        {
            return this.WaitUntilAvailable(WebUI.DefaultPageLoadingTimeout);
        }

        /// <summary>
        /// Waits until this page becomes available (i.e., its key element is visible), or until the specified timeout is reached.
        /// 
        /// A page is considered available if the browser can use the locator of this page to locate a visible <seealso cref="WebElement"/>.
        /// </summary>
        /// <param name="timeOutInSeconds">  time out in seconds </param>
        /// <returns> this page itself (for supporting the fluid interface) </returns>
        /// <exception cref="TimeoutException"> if the page is still not available after the specified timeout is reached  </exception>
        public virtual T WaitUntilAvailable(int timeOutInSeconds)
        {
            AssertLocatorNotNull();
            string timeOutMessage = "Timed out after " + timeOutInSeconds + " seconds in waiting for " + this.Name + " to become available.";
            Browser.WaitUntil(ExpectedConditions.ElementIsVisible(this.locator), timeOutInSeconds, timeOutMessage);
            return this as T;
        }

    }

}