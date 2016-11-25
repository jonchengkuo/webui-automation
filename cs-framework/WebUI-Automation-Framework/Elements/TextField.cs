using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with text fields (or called text boxes) on a web page.
    /// </summary>
    public class TextField : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a text field (or called a text box) on a web page.
        /// 
        /// Note that the label of a text field is outside the text field element and should be handled separately.
        /// </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this text field;
        ///                  it should select the HTML {@code <input type="text">} tag of the text field. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public TextField(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a text field (or called a text box) on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this text field;
        ///                     it should refer to an HTML {@code <input type="text">} tag of a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TextField(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Simulates the user interaction of clearing this text field and entering the given text into this text field.
        /// 
        /// If the text field does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> this text field itself (for supporting the fluid interface) </returns>
        /// <exception cref="NoSuchElementException"> if this text field still does not exist after the default implicit timeout is reached </exception>
        public virtual TextField SetText(string text)
        {
            if (this.LocatedByTBD)
            {
                ByTBD.Log(this.Name + ".setText(\"" + text + "\")");
            }
            else
            {
                // Get the web element with the default implicit timeout.
                IWebElement webElement = WebElement;
                webElement.Clear();
                webElement.SendKeys(text);
            }
            return this;
        }

        /// <summary>
        /// Returns the actual text in this text field.
        /// 
        /// If the text field does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> a String value representing the actual text in this text field </returns>
        /// <exception cref="NoSuchElementException"> if this text field still does not exist after the default implicit timeout is reached </exception>
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
                    return webElement.GetAttribute("value");
                }
            }
        }

        /// <summary>
        /// Simulates the user interaction of submitting the web form that contains this text field.
        /// 
        /// If the text field does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> this text field itself (for supporting the fluid interface) </returns>
        /// <exception cref="NoSuchElementException"> if this text field still does not exist after the default implicit timeout is reached </exception>
        public virtual TextField Submit()
        {
            if (this.LocatedByTBD)
            {
                ByTBD.Log(Name + ".submit()");
            }
            else
            {
                // Get the web element with the default implicit timeout and then
                // submit the form that contains this text field.
                WebElement.Submit();
            }
            return this;
        }

    }

}