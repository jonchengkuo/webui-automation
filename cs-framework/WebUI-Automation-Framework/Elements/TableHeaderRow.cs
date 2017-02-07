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
        ///                       it should refer to an HTML {@code <tr>} tag in a page. </param>
        /// <param name="table">  The table that contains this table header row </param>
        /// <param name="index">  The row index of this table header row.
        ///                       It is -1 if the header row is inside the <![CDATA[<thead>]]> tag;
        ///                       it is 0 if the header row is not inside the  <![CDATA[<thead>]]> tag. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TableHeaderRow(IWebElement webElement, Table table, int index) : base(webElement, table, index)
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