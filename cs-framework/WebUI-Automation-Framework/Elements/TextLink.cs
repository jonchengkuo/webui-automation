using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with text links on a web page.
    /// </summary>
    public class TextLink : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a hypertext link on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this text link;
        ///                  it should select the HTML {@code <a href="...">} tag of the text link. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public TextLink(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a hypertext link on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this text link;
        ///                     it should refer to an HTML {@code <a href="...">} tag of a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TextLink(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Simulates the user interaction of clicking this text link on UI.
        /// 
        /// If the text link does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this text link still does not exist after the default implicit timeout is reached </exception>
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
        /// Returns the actual text on this text link.
        /// 
        /// If the text link does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> a String value representing the actual text on this text link </returns>
        /// <exception cref="NoSuchElementException"> if this text link still does not exist after the default implicit timeout is reached </exception>
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
                    return WebElement.Text;
                }
            }
        }

    }

}