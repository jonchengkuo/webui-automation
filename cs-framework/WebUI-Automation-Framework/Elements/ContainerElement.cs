using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;  // ReadOnlyCollection<T>
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    public class ContainerElement : BaseElement, ISearchContext
    {

        public ContainerElement(By locator) : base(locator)
        {
        }

        /// <param name="webElement">  The <seealso cref="WebElement"/> of this container element. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public ContainerElement(IWebElement webElement) : base(webElement)
        {
        }

        ////// Implements the SearchContext interface.  //////

        public virtual IWebElement FindElement(By by)
        {
            return WebElement.FindElement(by);
        }

        public virtual ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return WebElement.FindElements(by);
        }

    }

}