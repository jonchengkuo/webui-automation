using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with texts on a web page.
    /// </summary>
    public class TextElement : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a text on a web page.
        /// A text is usually displayed by an HTML {@code <div>}, {@code <span>}, or {@code <label>} tag.
        /// </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this text.
        ///                  it should select the HTML tag that contains the text. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public TextElement(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a text on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this text;
        ///                     it should refer to the HTML tag that contains the text. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TextElement(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Gets the actual text of this text element, without any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// 
        /// If the text element is not visible (default) or does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this text element is still not visible (default) or does not exist
        ///     after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this text element becomes invalid
        ///     (unlikely unless the HTML tag of this text element is refreshed while this property is retrieved).</exception>
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

    }

}