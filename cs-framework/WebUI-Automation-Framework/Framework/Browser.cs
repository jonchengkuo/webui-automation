﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;  // ReadOnlyCollection<T>
using System.Threading;                // Thread
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;          // ChromeDriver
using OpenQA.Selenium.Firefox;         // FirefoxDriver
using OpenQA.Selenium.IE;              // InternetExplorerDriver
using OpenQA.Selenium.Support.UI;      // WebDriverWait

namespace WebUI.Automation.Framework
{

    /// <summary>
    /// This class provides a reference and an interface to interact with a web browser instance.
    /// Unlike a Selenium <seealso cref="WebDriver"/>, the reference to an instance of this class does not need to change
    /// when the web browser in use changes (e.g., changing the web browser in use from Chrome to Firefox).
    /// 
    /// <para>An instance of this class may be in one of the following states:</para>
    /// <ol>
    ///   <li>Non-opened: The default state; no web browser instance is associated with this <seealso cref="Browser"/> instance.
    ///   <li>Opened: A web browser instance is associated with this <seealso cref="Browser"/> instance.
    /// </ol>
    /// 
    /// <para>An instance of this class may enter the opened state by calling the <seealso cref="#open(BrowserType)"/> method.</para>
    /// 
    /// <para>Example:</para>
    /// The following code opens and closes a Chrome browser:
    /// <pre>
    ///     try (Browser browser = new Browser()) {
    ///         browser.open(BrowserType.CHROME);
    ///         // Do something.
    ///         // The browser will be automatically closed after this line.
    ///     }
    /// </pre>
    /// </summary>
    public class Browser : ISearchContext, IJavaScriptExecutor, IDisposable
    {
        #region Browser constructor and properties (1)

        protected internal BrowserType m_browserType;
        protected internal IWebDriver m_webDriver;

        /// <summary>
        /// Constructs a non-opened browser instance. 
        /// </summary>
        public Browser()
        {
            this.m_browserType = BrowserType.NONE;
            this.m_webDriver = null;
        }

        /// <summary>
        /// Returns the type of the currently opened browser.
        /// If no web browser is currently opened, it returns <seealso cref="BrowserType.NONE"/> .
        /// </summary>
        /// <returns> the browser type of the currently opened browser
        ///         or <seealso cref="BrowserType.NONE"/> if no web browser is currently opened </returns>
        public BrowserType BrowserType
        {
            get
            {
                return this.m_browserType;
            }
            protected internal set
            {
                this.m_browserType = value;
            }
        }

        /// <summary>
        /// Returns the <seealso cref="WebDriver"/> instance of the currently opened browser.
        /// It throws an Exception if no web browser is currently opened.
        /// </summary>
        /// <returns> the <seealso cref="WebDriver"/> instance of the currently opened browser
        /// <exception cref="Exception"> if no web browser is currently opened </exception>
        public IWebDriver WebDriver
        {
            get
            {
                if (this.m_webDriver == null)
                {
                    throw new Exception("This operation is illegal when no web browser is opened.");
                }
                return this.m_webDriver;
            }
            protected internal set
            {
                this.m_webDriver = value;
            }
        }

        #endregion

        #region Browser launch/dispose methods

        /// <summary>
        /// Launches a new browser instance/window according to the specified browser type.
        /// If it succeeds, this <seealso cref="Browser"/> instance is in the opened state.
        /// </summary>
        /// <param name="browserType">  browser type </param>
        /// <returns> this browser instance </returns>
        /// <exception cref="WebDriverException"> if it cannot opens a new browser instance </exception>
        /// <exception cref="Exception"> if this <seealso cref="Browser"/> instance is already opened </exception>
        public virtual Browser Launch(BrowserType browserType, string url = null)
        {
            if (this.m_webDriver != null)
            {
                throw new Exception("This browser is already opened. You must dispose it before openning another browser instance.");
            }

            switch (browserType)
            {
                case BrowserType.CHROME:
                    this.m_webDriver = new ChromeDriver();
                    break;
                case BrowserType.FIREFOX:
                    this.m_webDriver = new FirefoxDriver();
                    break;
                case BrowserType.IE:
                    this.m_webDriver = new InternetExplorerDriver();
                    break;
                default:
                    throw new Exception("Unsupported browser type: " + browserType);
            }
            this.BrowserType = browserType;
            if (url != null)
                NavigateTo(url);
            return this;
        }

        /// <summary>
        /// Closes the currently opened browser instance/window.
        /// It does nothing if no web browser is currently opened.
        /// <para>
        /// Note: After calling this method, this <seealso cref="Browser"/> instance will always enter the non-opened state.
        /// </para>
        /// </summary>
        public virtual void Dispose()
        {
            if (this.m_webDriver != null)
            {
                try
                {
                    this.m_webDriver.Quit();
                }
                finally
                {
                    // No matter closing the browser succeeds or not,
                    // we'll discard the current browser/web driver instance.
                    this.m_webDriver = null;
                    this.BrowserType = BrowserType.NONE;
                }
            }
        }

        #endregion

        #region Browser properties (2)

        public IWindow Window
        {
            get
            {
                return this.WebDriver.Manage().Window;
            }
        }

        /// <summary>
        /// Returns the title of the web browser.
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this.WebDriver.Title;
            }
        }

        /// <summary>
        /// Returns the title text of all windows of the currently opened web browser instance.
        /// </summary>
        public virtual IList<string> WindowTitles
        {
            get
            {
                ReadOnlyCollection<string> windowHandles = this.WebDriver.WindowHandles;
                IList<string> windowTitles = new List<string>(windowHandles.Count);
                ITargetLocator switchTo = this.WebDriver.SwitchTo();
                foreach (string handle in windowHandles)
                {
                    string title = switchTo.Window(handle).Title;
                    windowTitles.Add(title);
                }
                return windowTitles;
            }
        }

        #endregion

        #region Browser navigation methods

        /// <summary>
        /// Causes the opened web browser to navigate to the specified URL.
        /// </summary>
        /// <param name="url">  URL to navigate to </param>
        /// <returns> this browser instance </returns>
        /// <exception cref="Exception"> if no web browser is currently opened </exception>
        public Browser NavigateTo(String url)
        {
            this.WebDriver.Navigate().GoToUrl(url);
            return this;
        }

        /// <summary>
        /// Causes the opened web browser to navigate to the previous screen in the browser history.
        /// </summary>
        /// <returns> this browser instance </returns>
        /// <exception cref="Exception"> if no web browser is currently opened </exception>
        public Browser NavigateBack()
        {
            this.WebDriver.Navigate().Back();
            return this;
        }

        /// <summary>
        /// Causes the opened web browser to reload its current page.
        /// </summary>
        /// <returns> this browser instance </returns>
        /// <exception cref="Exception"> if no web browser is currently opened </exception>
        public Browser Refresh()
        {
            this.WebDriver.Navigate().Refresh();
            return this;
        }

        #endregion

        #region Browser wait methods

        /// <summary>
        /// Repeatedly calling the specified expected condition function until either it returns
        /// a non-null value or the specified timeout expires.
        /// <para>
        /// Example:
        /// <pre>
        ///     // Wait until the "username" text field is visible or timeout. 
        ///     IWebElement e = browser.waitUntil(
        ///         ExpectedConditions.visibleOfElementLocated(By.name("username")),
        ///         timeOutInSeconds);
        /// </pre>
        /// </para>
        /// </summary>
        /// <param name="timeOutInSeconds">  the timeout in seconds when an expectation is called </param>
        /// <param name="timeOutMessage">    the message to be included in the TimeoutException if it is thrown </param>
        /// <returns> the non-null value returned by the specified expected condition function </returns>
        /// <exception cref="TimeoutException"> if the specified expected condition function still returns <code>null</code>
        ///         when the specified timeout is reached </exception>
        /// <seealso cref= WebDriverWait </seealso>
        public T WaitUntil<T>(Func<IWebDriver, T> expectedCondition, int timeOutInSeconds, string timeOutMessage = null)
        {
            WebDriverWait wait = new WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(timeOutInSeconds));
            if (timeOutMessage != null)
                wait.Message = timeOutMessage;
            return wait.Until<T>(expectedCondition);
        }

        /// <summary>
        /// Sleeps for the specified number of seconds.
        /// <para>
        /// <b>WARNING:</b> Use of this method is NOT recommended because it will introduced
        /// instability in your web UI automation. Consider using <seealso cref="#waitUntil(Function,int)"/> instead.  
        /// 
        /// </para>
        /// </summary>
        /// <param name="numberOfSeconds">  number of seconds to sleep </param>
        /// <returns> this Browser instance </returns>
        public Browser Sleep(int numberOfSeconds)
        {
            Thread.Sleep(numberOfSeconds * 1000);
            return this;
        }

        #endregion

        #region Methods implementing ISearchContext

        public IWebElement FindElement(By by)
        {
            return this.WebDriver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return this.WebDriver.FindElements(by);
        }

        #endregion

        #region Methods implementing

        public object ExecuteScript(string script, params object[] args)
        {
            return (this.WebDriver as IJavaScriptExecutor).ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return (this.WebDriver as IJavaScriptExecutor).ExecuteAsyncScript(script, args);
        }

        #endregion

    }

}
