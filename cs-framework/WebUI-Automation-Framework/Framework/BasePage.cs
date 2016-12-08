using System;
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
    ///   using OpenQA.Selenium;
    ///   using WebUI.Automation.Elements;
    ///   using WebUI.Automation.Framework;
    ///
    ///   public class LoginPage : BasePage<LoginPage> {
    ///       TextField UserNameTextField = new TextField(By.id("username"));
    ///       TextField PasswordTextField = new TextField(By.id("password"));
    ///       Button    LogInButton       = new Button(By.id("login"));
    ///
    ///       public void LogIn(string username, string password) {
    ///           UserNameTextField.Text = username;
    ///           PasswordTextField.Text = password;
    ///           LogInButton.Click();
    ///       }
    ///   }
    ///
    ///   LoginPage loginPage = new LoginPage();
    ///   loginPage.WaitUntilAvailable().LogIn("username", "password");
    /// </pre> 
    /// </summary>
    public class BasePage<T> where T : BasePage<T>
    {

        /// <summary>
        /// Constructs a base page without a <seealso cref="By"/> locator.
        /// A derived class must set the Locator property before using a page object.
        /// </summary>
        protected BasePage()
        {
        }

        /// <summary>
        /// Constructs a base page with a <seealso cref="By"/> locator.
        /// <param name="locator">  The <seealso cref="By"/> locator for locating a web element
        /// in order to determine whether or not a page exists and is visible. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        /// </summary>
        protected BasePage(By locator)
        {
            if (locator == null)
            {
                throw new System.ArgumentNullException("The locator given to the page is null.");
            }
            Locator = locator;
        }

        /// <summary>
        /// Returns the name of this web page.
        /// It is default to the simple class name of the most derived class.
        /// </summary>
        /// <returns> the name of this web page </returns>
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Returns the <seealso cref="Browser"/> instance used by this page.
        /// It is default to <seealso cref="WebUIGlobals.DefaultBrowser"/> when this page object is constructed.
        /// </summary>
        /// <returns> the <seealso cref="Browser"/> instance used by this page </returns>
        public virtual Browser Browser { get; set; } = WebUIGlobals.DefaultBrowser;

        /// <summary>
        /// Returns or sets the <seealso cref="By"/> locator used by this page to detremine
        /// whether or not this page exists and is visible.
        /// Note: The locator can only be set by a derived class.
        /// </summary>
        public virtual By Locator { get; protected set; }

        /// <summary>
        /// Returns whether this web page is available or not, as of now.
        /// A page is considered available if the web browser can use the locator of this page
        /// to locate a visible <seealso cref="IWebElement"/>.
        /// Note that getting this property does not wait until the default page loading timeout is reached.
        /// </summary>
        /// <returns> <code>true</code> if this web page is available now;
        ///         <code>false</code> otherwise </returns>
        public virtual bool Available
        {
            get
            {
                return BecomeAvailable(0);
            }
        }

        /// <summary>
        /// Returns whether this web page becomes available or not within the default page loading timeout,
        /// which is specified and can be configured by <seealso cref="WebUIGlobals.DefaultPageLoadingTimeout"/>.
        /// A page is considered available if the web browser can use the locator of this page
        /// to locate a visible <seealso cref="IWebElement"/> within the specified timeout.
        /// </summary>
        /// <returns> <code>true</code> if this web page is available within the default page loading timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool BecomeAvailable()
        {
            return BecomeAvailable(WebUIGlobals.DefaultPageLoadingTimeout);
        }

        /// <summary>
        /// Returns whether this web page becomes available or not within the specified timeout.
        /// A page is considered available if the web browser can use the locator of this page
        /// to locate a visible <seealso cref="IWebElement"/> within the specified timeout.
        /// If the specified timeout is 0, it will check the current availability of this web page.
        /// If the specified timeout is greater than 0, it will periodically (every half second by default)
        /// check the existence of this web page until the specified timeout is reached.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> <code>true</code> if this web page is available within the specified timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool BecomeAvailable(int timeOutInSeconds)
        {
            if (timeOutInSeconds == 0)
            {
                AssertLocatorNotNull();
                IWebElement webElement = Browser.FindElement(Locator);
                return webElement.Displayed;
            }
            else
            {
                try
                {
                    WaitUntilAvailable(timeOutInSeconds);
                    return true;
                }
                catch (WebDriverTimeoutException)
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// Waits until this page becomes available (i.e., its key element is visible), or until the default page-loading timeout is reached.
        /// The default page-loading timeout is determined and can be configured by <seealso cref="WebUIGlobals.DefaultPageLoadingTimeout"/>.
        /// 
        /// A page is considered available if the browser can use the locator of this page to locate a visible <seealso cref="WebElement"/>.
        /// </summary>
        /// <returns> this page itself (for supporting the fluid interface) </returns>
        /// <exception cref="WebDriverTimeoutException"> if the page is still not available after the default page-loading timeout expires  </exception>
        public virtual T WaitUntilAvailable()
        {
            return this.WaitUntilAvailable(WebUIGlobals.DefaultPageLoadingTimeout);
        }

        /// <summary>
        /// Waits until this page becomes available (i.e., its key element is visible), or until the specified timeout is reached.
        /// 
        /// A page is considered available if the browser can use the locator of this page to locate a visible <seealso cref="WebElement"/>.
        /// </summary>
        /// <param name="timeOutInSeconds">  time out in seconds </param>
        /// <returns> this page itself (for supporting the fluid interface) </returns>
        /// <exception cref="WebDriverTimeoutException"> if the page is still not available after the specified timeout is reached  </exception>
        public virtual T WaitUntilAvailable(int timeOutInSeconds)
        {
            AssertLocatorNotNull();
            string timeOutMessage = "Waiting for " + this.Name + " to become available (visible).";
            Browser.WaitUntil(ExpectedConditions.ElementIsVisible(Locator), timeOutInSeconds, timeOutMessage);
            return this as T;
        }

        private void AssertLocatorNotNull()
        {
            if (Locator == null)
            {
                throw new System.NullReferenceException("The Locator property of this page object is not set." + " You must set it (preferred in your page class constructor) before using this page.");
            }
        }

    }

}