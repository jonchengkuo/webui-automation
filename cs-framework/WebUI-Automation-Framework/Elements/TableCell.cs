using System.Collections.Generic;
using System;
using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with a cell in a table on a web page.
    /// </summary>
    public class TableCell : ContainerElement
    {

        /// <summary>
        /// Returns the row index of this cell in the containing table.
        /// </summary>
        public int RowIndex { get; }

        /// <summary>
        /// Returns the column index of this cell in the containing table.
        /// </summary>
        public int ColumnIndex { get; }

        /// <summary>
        /// Constructs an object to represent and interact with a cell in a table on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this table cell;
        ///                  it should refer to an HTML {@code <td>} tag in a page. </param>
        /// <param name="rowIndex">  The row index of this table cell </param>
        /// <param name="columnIndex">  The column index of this table cell </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TableCell(IWebElement webElement, int rowIndex, int columnIndex) : base(webElement)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        /// <summary>
        /// Gets the text in this table cell, without any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table cell becomes invalid.</exception>
        public virtual string Text
        {
            get
            {
                return WebElement.Text;
            }
        }

    }

}