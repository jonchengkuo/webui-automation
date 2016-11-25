using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions

namespace WebUI.Automation.Framework
{

    /// <summary>
    /// Base class for representing and interacting with UI elements (or called UI controls) shown on a web page.
    /// It provides common utility methods for subclasses.
    /// A subclass should be defined for each specific UI element type (e.g., buttons, check boxes, etc.)
    /// to model specific UI behaviors (e.g., clicking a button or checking a check box).
    /// 
    /// <P>Every UI element shown on a web page is defined by an HTML element, which is defined
    /// by a pair of HTML start and end tags.</P>
    /// 
    /// <P>Instances of UI elements can be created in two ways: with a <seealso cref="By"/> locator or with a <seealso cref="IWebElement"/>.
    /// A locator is a mechanism by which the HTML element of a UI element can be located.
    /// A IWebElement represents a located HTML element.
    /// Because every UI element shown on a web page is defined by a particular HTML element,
    /// this framework sometimes uses the term <em>UI element</em> and <em>HTML element</em> interchangeably.</P>
    /// 
    /// For a UI element created with a <seealso cref="By"/> locator, every time your code interacts with the UI element
    /// (by calling its public methods), it will always try to locate its HTML element on the web page with its locator.
    /// This design is less efficient because it does not cache a previously located HTML element (i.e.,
    /// it does not reuse a previously available IWebElement object). However, this design does guarantee that
    /// it will always interact with the current, up-to-date web page. From the framework's perspective, it has no way
    /// to know whether a previously located HTML element still exists or not.</P>
    /// 
    /// <P>On the other hand a UI element created with a <seealso cref="IWebElement"/> does not have a locator.
    /// It will assume that the IWebElement given to it is always available and will use it for any UI interaction.
    /// This design is more efficient, but it relies on the caller (i.e., your code) to properly manage the
    /// life-cycle of a UI element and its IWebElement.
    /// It is designed to be used as a transient UI element that lives for a short period of time.</P>
    /// 
    /// <para><b>Example:</b></para>
    /// <pre>
    ///   import org.openqa.selenium.By;
    ///   import org.openqa.selenium.IWebElement;
    ///   import webui.automation.framework.BaseElement;
    /// 
    ///   public class Button extends BaseElement {
    ///       public Button(By locator) {
    ///           super(locator);
    ///       }
    ///       public Button(IWebElement webElement) {
    ///           super(webElement);
    ///       }
    ///       public void click() {
    ///           ...
    ///       }
    ///   }
    /// 
    /// 
    ///   Button button1 = new Button(By.id("buttonId1"));
    /// 
    ///   IWebElement button2Element = browser.findElement(By.id("buttonId2"));
    ///   Button button2 = new Button(button2Element);
    /// </pre> 
    /// </summary>
    public class BaseElement
    {

        private Browser browser;
        private By locator;
        private bool expectVisible = true;

        private IWebElement webElement;

        /// <summary>
        /// Constructs the base object of a concrete UI element object that represents a specific UI element on a web page.
        /// The given <code>locator</code> will be used to locate the <seealso cref="IWebElement"/> of the UI element. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the UI element on a web page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        protected internal BaseElement(By locator)
        {
            if (locator == null)
            {
                throw new System.NullReferenceException("The locator given to the UI element is null.");
            }
            this.locator = locator;
            this.browser = WebUI.DefaultBrowser;
        }

        /// <summary>
        /// Constructs the base object of a concrete UI element object that represents a specific UI element on a web page.
        /// This UI element is directly tied to the given <seealso cref="IWebElement"/> located by other means. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of the UI element on a web page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        protected internal BaseElement(IWebElement webElement)
        {
            if (webElement == null)
            {
                throw new System.NullReferenceException("The IWebElement given to the UI element is null.");
            }
            this.webElement = webElement;
        }

        protected internal virtual bool ExpectVisible
        {
            set
            {
                this.expectVisible = value;
            }
        }

        /// <summary>
        /// Returns the name of this UI element.
        /// It is default to the simple class name of this UI element, such as "Button", "CheckBox", etc.,
        /// appended with the string representation of either the locator (if this UI element has a locator),
        /// or the <seealso cref="IWebElement"/> given to the constructor of this UI element. </summary>
        /// <returns> the name of this UI element </returns>
        public virtual string Name
        {
            get
            {
                if (this.locator == null)
                {
                    return this.GetType().Name + "(" + this.webElement.ToString() + ")";
                }
                else
                {
                    return this.GetType().Name + "(" + this.locator.ToString() + ")";
                }
            }
        }

        /// <summary>
        /// Returns the <seealso cref="By"/> locator of this UI element.
        /// If this UI element is created with a <seealso cref="IWebElement"/> instead of a locator, this method will return <code>null</code>. </summary>
        /// <returns> the <seealso cref="By"/> locator of this UI element;
        ///         <code>null</code> if this UI element is created with a <seealso cref="IWebElement"/> instead of a locator </returns>
        public virtual By Locator
        {
            get
            {
                return this.locator;
            }
        }

        /// <summary>
        /// Returns <code>true</code> if the locator of this UI element is the special <seealso cref="ByTBD"/> locator. </summary>
        /// <returns> <code>true</code> if the locator of this UI element is the special <seealso cref="ByTBD"/> locator;
        ///         <code>false</code> otherwise </returns>
        protected internal virtual bool LocatedByTBD
        {
            get
            {
                if (this.locator == null)
                {
                    return false;
                }
                else
                {
                    return (this.locator is ByTBD);
                }
            }
        }

        /// <summary>
        /// Returns the <seealso cref="Browser"/> instance used by this UI element.
        /// It is default to the <seealso cref="Browser"/> instance returned by <seealso cref="WebUI#getDefaultBrowser"/>
        /// when this UI element is created with a <seealso cref="By"/> locator.
        /// If this UI element is created with a <seealso cref="IWebElement"/>, this method will throw an <seealso cref="IllegalStateException"/>. </summary>
        /// <returns> the <seealso cref="Browser"/> instance used by this UI element </returns>
        /// <exception cref="IllegalStateException"> if this UI element is created with a <seealso cref="IWebElement"/> </exception>
        public virtual Browser Browser
        {
            get
            {
                if (this.browser == null)
                {
                    throw new System.InvalidOperationException("No browser is available for this " + Name + " because it was created with a IWebElement.");
                }
                else
                {
                    return this.browser;
                }
            }
        }

        // Comment out this method until we want the framework to support multiple Browser instances.
        /*
         * Sets the {@link Browser} instance used by this UI element.
         *
        private void setBrowser(Browser browser) {
            if (browser == null) {
                throw new NullPointerException("The given browser object is null.");
            }
            this.browser = browser;
        }*/


        /// <summary>
        /// Locates and returns the <seealso cref="IWebElement"/> of this UI element.
        /// It will periodically (every half second) locate it until the default implicit wait timeout is reached.
        /// The default implicit wait timeout is determined and can be configured by <seealso cref="WebUI#defaultImplicitWaitTimeout"/>.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> the <seealso cref="IWebElement"/> located using the locator of this UI element </returns>
        /// <exception cref="NoSuchElementException"> if this UI element still does not exist after the default implicit wait timeout is reached </exception>
        public virtual IWebElement WebElement
        {
            get
            {
                return GetWebElement(WebUI.DefaultImplicitWaitTimeout);
            }
        }

        /// <summary>
        /// Locates and returns the <seealso cref="IWebElement"/> of this UI element.
        /// If the specified timeout is greater than 0, it will periodically (every half second)
        /// locate it until the specified timeout is reached.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> the <seealso cref="IWebElement"/> located using the locator of this UI element </returns>
        /// <exception cref="NoSuchElementException"> if this UI element still does not exist after the specified timeout is reached </exception>
        public virtual IWebElement GetWebElement(int timeOutInSeconds)
        {
            if (this.webElement != null)
            {
                // This UI element is already tied to a particular IWebElement; just return it.
                return this.webElement;
            }

            IWebElement webElement = null;
            if (timeOutInSeconds == 0)
            {
                webElement = Browser.FindElement(this.locator);
                if (this.expectVisible && !webElement.Displayed)
                {
                    throw new NoSuchElementException(this.Name + " exists in the DOM tree but not visible.");
                }
            }
            else
            {
                try
                {
                    if (this.expectVisible)
                    {
                        webElement = WaitUntilVisible(timeOutInSeconds);
                    }
                    else
                    {
                        webElement = WaitUntilExists(timeOutInSeconds);
                    }
                }
                catch (TimeoutException e)
                {
                    // Convert a TimeoutException into a NoSuchElementException.
                    throw new NoSuchElementException(e.Message);
                }
            }
            return webElement;
        }

        /// <summary>
        /// Returns whether this UI element exists (i.e., present in the DOM tree) or not,
        /// within the default implicit wait timeout, specified and can be configured by
        /// <seealso cref="WebUI.DefaultImplicitWaitTimeout"/>.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> <code>true</code> if this UI element exists within the default implicit wait timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool Exists
        {
            get
            {
                return BecomeExists(WebUI.DefaultImplicitWaitTimeout);
            }
        }

        /// <summary>
        /// Returns whether this UI element becomes exists (i.e., present in the DOM tree) or not
        /// within the specified timeout.
        /// If the specified timeout is 0, it will check the current existence of this UI element.
        /// If the specified timeout is greater than 0, it will periodically (every half second)
        /// check until the specified timeout is reached.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> <code>true</code> if this UI element exists within the specified timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool BecomeExists(int timeOutInSeconds)
        {
            if (this.webElement != null)
            {
                // This UI element is already tied to a particular IWebElement; assume it exist.
                return true;
            }

            try
            {
                WaitUntilExists(timeOutInSeconds);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Property indicating whether this UI element is visible or not,
        /// within the default implicit wait timeout, specified and can be configured by
        /// <seealso cref="WebUI.DefaultImplicitWaitTimeout"/>.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> <code>true</code> if this UI element exists within the default implicit wait timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool Visible
        {
            get
            {
                return BecomeVisible(WebUI.DefaultImplicitWaitTimeout);
            }
        }

        /// <summary>
        /// Returns whether this UI element becomes visible or not within the specified timeout.
        /// If the specified timeout is 0, it will check the current visibility of this UI element.
        /// If the specified timeout is greater than 0, it will periodically (every half second)
        /// check until the specified timeout is reached.
        /// </summary>
        /// <param name="timeOutInSeconds">  timeout in seconds </param>
        /// <returns> <code>true</code> if this UI element is visible within the specified timeout;
        ///         <code>false</code> otherwise </returns>
        public virtual bool BecomeVisible(int timeOutInSeconds)
        {
            if (this.webElement != null)
            {
                // This UI element is already tied to a particular IWebElement; check its visibility.
                return this.webElement.Displayed;
            }

            try
            {
                WaitUntilVisible(timeOutInSeconds);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Waits until this UI element becomes exist (i.e., present in the DOM tree), or until the specified timeout is reached.
        /// It returns the located web element.
        /// </summary>
        /// <param name="timeOutInSeconds">  time out in seconds </param>
        /// <returns> the located web element </returns>
        /// <exception cref="TimeoutException"> if this UI element is still not present after the specified timeout is reached   </exception>
        public virtual IWebElement WaitUntilExists(int timeOutInSeconds)
        {
            if (this.webElement != null)
            {
                // This UI element is already tied to a particular IWebElement; there is no need to wait.
                return this.webElement;
            }
            string timeOutMessage = "Timed out after " + timeOutInSeconds + " seconds in waiting for " + this.Name + " to become exists.";
            return Browser.WaitUntil(ExpectedConditions.ElementExists(this.locator), timeOutInSeconds, timeOutMessage);
        }

        /// <summary>
        /// Waits until this UI element becomes present and visible, or until the specified timeout is reached.
        /// It returns the located visible web element.
        /// </summary>
        /// <param name="timeOutInSeconds">  time out in seconds </param>
        /// <returns> the located visible web element </returns>
        /// <exception cref="TimeoutException"> if this UI element is still not visible after the specified timeout is reached   </exception>
        public virtual IWebElement WaitUntilVisible(int timeOutInSeconds)
        {
            if (this.webElement != null)
            {
                // TODO: Implement waiting for a IWebElement to become visible (rarely needed).
                throw new Exception("This method is not yet implemented for a UI element that is tied to a particular IWebElement.");
            }
            string timeOutMessage = "Timed out after " + timeOutInSeconds + " seconds in waiting for " + this.Name + " to become visible.";
            return Browser.WaitUntil<IWebElement>(ExpectedConditions.ElementIsVisible(this.locator), timeOutInSeconds, timeOutMessage);
        }

        /// <summary>
        /// Waits until this UI element becomes invisible, or until the specified timeout is reached.
        /// </summary>
        /// <param name="timeOutInSeconds">  time out in seconds </param>
        /// <returns> the return value from <seealso cref="WebDriverWait.Until"/> </returns>
        /// <exception cref="TimeoutException"> if this UI element is still visible after the specified timeout is reached   </exception>
        public virtual bool WaitUntilNotVisible(int timeOutInSeconds)
        {
            if (this.webElement != null)
            {
                // TODO: Implement waiting for a IWebElement to become invisible (rarely needed).
                throw new Exception("This method is not yet implemented for a UI element that is tied to a particular IWebElement.");
            }
            else
            {
                string timeOutMessage = "Timed out after " + timeOutInSeconds + " seconds in waiting for " + this.Name + " to become invisible.";
                return Browser.WaitUntil(ExpectedConditions.InvisibilityOfElementLocated(this.locator), timeOutInSeconds, timeOutMessage);
            }
        }

    }

}