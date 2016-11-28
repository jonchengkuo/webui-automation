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
        /// Gets the actual text of this text link, without any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// 
        /// If the text link is not visible (default) or does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this text link is still not visible (default) or does not exist
        ///     after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this text link becomes invalid
        ///     (unlikely unless the HTML tag of this text link is refreshed while this property is retrieved).</exception>
        public virtual string Text
        {
            get
            {
                if (this.IsLocatedByTBD)
                {
                    return ByTBD.MockedStringValue;
                }
                else
                {
                    return WebElement.Text;
                }
            }
        }

        /// <summary>
        /// Simulates the user interaction of clicking this text link on UI.
        /// 
        /// If the text link is not visible (default) or does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this text link is still not visible (default) or does not exist
        ///     after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this text link becomes invalid
        ///            (unlikely unless the HTML tag of this text link is refreshed while this method is invoked).</exception>
        public virtual void Click()
        {
            if (this.IsLocatedByTBD)
            {
                ByTBD.Log(this.Name + ".click()");
            }
            else
            {
                // Get the web element with the default implicit timeout and then click it.
                WebElement.Click();
            }
        }

    }

}