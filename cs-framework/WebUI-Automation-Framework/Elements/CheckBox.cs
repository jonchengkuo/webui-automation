using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with check boxes on a web page.
    /// </summary>
    public class CheckBox : BaseElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a check box on a web page.
        /// 
        /// Note that the label of the check box is usually a label or text element outside the check box element.
        /// Thus, it needs to be handled separately.
        /// </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this check box;
        ///                  it should select the HTML {@code <input type="checkbox">} tag of the check box. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public CheckBox(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a check box on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this check box;
        ///                     it should refer to an HTML {@code <input type="checkbox">} tag of a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public CheckBox(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Simulates the user interaction of checking this check box.
        /// 
        /// If the check box does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this check box still does not exist after the default implicit timeout is reached </exception>
        public virtual void Check()
        {
            if (this.LocatedByTBD)
            {
                ByTBD.Log(this.Name + ".check()");
            }
            else
            {
                // Get the web element with the default implicit timeout.
                IWebElement webElement = WebElement;
                if (!webElement.Selected)
                {
                    webElement.Click();
                }
            }
        }

        /// <summary>
        /// Simulates the user interaction of unchecking this check box.
        /// 
        /// If the check box does not exist, this method will keep waiting until it appears or until
        /// the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached.  
        /// </summary>
        /// <returns> this check box itself (for supporting the fluid interface) </returns>
        /// <exception cref="NoSuchElementException"> if this check box still does not exist after the default implicit timeout is reached </exception>
        public virtual CheckBox Uncheck()
        {
            if (this.LocatedByTBD)
            {
                ByTBD.Log(this.Name + ".uncheck()");
            }
            else
            {
                // Get the web element with the default implicit timeout.
                IWebElement webElement = WebElement;
                if (webElement.Selected)
                {
                    webElement.Click();
                }
            }
            return this;
        }

    }

}