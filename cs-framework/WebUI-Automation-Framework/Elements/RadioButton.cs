using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with check boxes on a web page.
    /// </summary>
    public class RadioButton : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a radio button on a web page.
        /// 
        /// Note that the label of the radio button is usually a label or text element outside the radio button element.
        /// Thus, it needs to be handled separately.
        /// </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this radio button;
        ///                  it should select the HTML {@code <input type="radio">} tag of the radio button. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public RadioButton(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a radio button on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this radio button;
        ///                     it should refer to an HTML {@code <input type="radio">} tag of a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public RadioButton(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Simulates the user interaction of selecting this radio button.
        /// 
        /// If the radio button is not visible (default) or does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this radio button is still not visible (default) or does not exist
        ///     after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this radio button becomes invalid
        ///     (unlikely unless the HTML tag of this radio button is refreshed while this method is invoked).</exception>
        public virtual void Select()
        {
            if (this.IsLocatedByTBD)
            {
                ByTBD.Log(this.Name + ".select()");
            }
            else
            {
                // Get the web element with the default implicit timeout and then click it.
                WebElement.Click();
            }
        }

    }

}