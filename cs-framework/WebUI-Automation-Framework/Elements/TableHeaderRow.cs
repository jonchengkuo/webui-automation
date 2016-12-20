using System.Collections.Generic;
using OpenQA.Selenium;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with the header row of a table on a web page.
    /// </summary>
    public class TableHeaderRow : TableRow
    {

        /// <summary>
        /// Constructs an object to represent and interact with the header row of a table on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this table row;
        ///                     it should refer to an HTML {@code <tr>} tag in a page. </param>
        /// <param name="table">  The <seealso cref="Table"/> element of this table row </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TableHeaderRow(IWebElement webElement, Table table) : base(webElement, table)
        {
        }

        /// <summary>
        /// Returns a list of <seealso cref="IWebElement"/> instances that point to the <th> element of each cell in this table header row.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        public override IReadOnlyList<IWebElement> CellElements
        {
            get
            {
                return FindElements(By.XPath("th"));
            }
        }

    }
}