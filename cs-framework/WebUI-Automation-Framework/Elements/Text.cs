using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with texts on a web page.
    /// </summary>
    public class Text : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a text on a web page.
        /// A text is usually displayed by an HTML {@code <div>}, {@code <span>}, or {@code <label>} tag.
        /// </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this text.
        ///                  it should select the HTML tag that contains the text. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public Text(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a text on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this text;
        ///                     it should refer to the HTML tag that contains the text. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public Text(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Returns the actual text in this text element.
        /// 
        /// If the text element does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> a String value representing the actual text in this text element </returns>
        /// <exception cref="NoSuchElementException"> if this text element still does not exist after the default implicit timeout is reached </exception>
        public virtual string GetText()
        {
            return WebElement.Text;
        }

    }

}