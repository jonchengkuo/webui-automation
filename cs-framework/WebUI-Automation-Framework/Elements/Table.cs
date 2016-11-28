using System.Collections.Generic;
using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with tables on a web page.
    /// </summary>
    public class Table : ContainerElement
    {

        /// <summary>
        /// Constructs an object to represent and interact with a table on a web page.
        /// </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating this table.
        ///                  it should select the HTML {@code <table>} tag of the table. </param>
        /// <exception cref="NullPointerException"> if the specified <code>locator</code> is <code>null</code> </exception>
        public Table(By locator) : base(locator)
        {
        }

        /// <summary>
        /// Constructs an object to represent and interact with a table on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this table;
        ///                     it should refer to an HTML {@code <table>} tag of a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public Table(IWebElement webElement) : base(webElement)
        {
        }

        public virtual int RowCount
        {
            get
            {
                return RowElements.Count;
            }
        }

        /// <summary>
        /// Returns the <seealso cref="IWebElement"/> instances that point to the <tr> elements of each table row.
        /// @return
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this property is retrieved).</exception>
        public virtual IList<IWebElement> RowElements
        {
            get
            {
                IWebElement tableElement = WebElement;
                return TableHelper.FindRowElements(tableElement);
            }
        }

        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual IWebElement GetRowElement(int rowIndex)
        {
            IWebElement tableElement = WebElement;
            return TableHelper.FindRowElement(tableElement, rowIndex);
        }

        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual IList<IWebElement> GetCellElements(int rowIndex)
        {
            IWebElement rowElement = GetRowElement(rowIndex);
            return TableHelper.FindCellElements(rowElement);
        }

        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual IList<string> GetCellTexts(int rowIndex)
        {
            IList<IWebElement> cellElements = GetCellElements(rowIndex);
            IList<string> textList = new List<string>(cellElements.Count);
            foreach (IWebElement cellElement in cellElements)
            {
                textList.Add(cellElement.Text);
            }
            return textList;
        }

        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual IWebElement GetCellElement(int rowIndex, int columnIndex)
        {
            IWebElement rowElement = GetRowElement(rowIndex);
            return TableHelper.FindCellElement(rowElement, columnIndex);
        }

        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual string GetCellText(int rowIndex, int columnIndex)
        {
            return GetCellElement(rowIndex, columnIndex).Text;
        }

    }

}