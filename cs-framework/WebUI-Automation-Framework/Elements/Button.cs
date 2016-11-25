﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with buttons on a web page.
    /// </summary>
    public class Button : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a button on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the <seealso cref="WebElement"/> of this button;
        ///                  it should select the HTML {@code <input type="button">} tag of the button. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public Button(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a button on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this button;
        ///                     it should refer to an HTML {@code <input type="button">} tag of a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public Button(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Simulates the user interaction of clicking this button on UI.
        /// 
        /// If the button does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this button still does not exist after the default implicit timeout is reached </exception>
        public virtual void Click()
        {
            if (this.LocatedByTBD)
            {
                ByTBD.Log(this.Name + ".click()");
            }
            else
            {
                // Get the web element with the default implicit timeout and then click it.
                WebElement.Click();
            }
        }

        /// <summary>
        /// Returns the actual text on this button.
        /// 
        /// If the button does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> a String value representing the actual text on this button </returns>
        /// <exception cref="NoSuchElementException"> if this button still does not exist after the default implicit timeout is reached </exception>
        public virtual string Text
        {
            get
            {
                if (this.LocatedByTBD)
                {
                    return ByTBD.MockedStringValue;
                }
                else
                {
                    // Get the web element with the default implicit timeout.
                    IWebElement webElement = WebElement;
                    // Get and check the web element text. If it is an empty string,
                    // get the text from the value attribute of the button
                    string text = webElement.Text;
                    if (text.Length == 0)
                    {
                        text = webElement.GetAttribute("value");
                    }
                    return text;
                }
            }
        }

        /// <summary>
        /// Waits until this button becomes clickable, or until the specified timeout is reached.
        /// It returns the located clickable web element.
        /// </summary>
        /// <param name="timeOutInSeconds">  time out in seconds </param>
        /// <returns> the located clickable web element </returns>
        /// <exception cref="TimeoutException"> if this button is still not clickable after the specified timeout is reached   </exception>
        public virtual IWebElement WaitUntilClickable(int timeOutInSeconds)
        {
            if (Locator == null)
            {
                // TODO: Implement waiting for a WebElement to become clickable (rarely needed).
                throw new Exception("This method is not yet implemented for a UI element that is tied to a particular WebElement.");
            }
            else
            {
                string timeOutMessage = "Timed out after " + timeOutInSeconds + " seconds in waiting for " + this.Name + " to become clickable.";
                return Browser.WaitUntil(ExpectedConditions.ElementToBeClickable(Locator), timeOutInSeconds, timeOutMessage);
            }
        }

    }

}