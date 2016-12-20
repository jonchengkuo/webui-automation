using System.Collections.Generic;
using OpenQA.Selenium;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with a row in a table on a web page.
    /// </summary>
    public class TableRow : ContainerElement
    {

        private Table table;

        /// <summary>
        /// Constructs an object to represent and interact with a row in a table on a web page. </summary>
        /// <param name="webElement">  The <seealso cref="IWebElement"/> of this table row;
        ///                     it should refer to an HTML {@code <tr>} tag in a page. </param>
        /// <param name="table">  The <seealso cref="Table"/> element of this table row </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public TableRow(IWebElement webElement, Table table) : base(webElement)
        {
            this.table = table;
        }

        /// <summary>
        /// Returns a list of <seealso cref="IWebElement"/> instances that point to the <td> element of each cell in this table row.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        public virtual IReadOnlyList<IWebElement> CellElements
        {
            get
            {
                return TableHelper.FindCellElements(WebElement);
            }
        }

        /// <summary>
        /// Returns the number of cells in this table row.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        public virtual int CellCount
        {
            get
            {
                return CellElements.Count;
            }
        }

        /// <summary>
        /// Returns a list of text strings in the cells of this table row.
        /// The text from each table cell does not have any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        public virtual IReadOnlyList<string> CellTexts
        {
            get
            {
                var cellElements = CellElements;
                var textList = new List<string>(cellElements.Count);
                foreach (IWebElement cellElement in cellElements)
                {
                    textList.Add(cellElement.Text);
                }
                return textList;
            }
        }

        /// <summary>
        /// Returns a list of <seealso cref="TableCell"/> instances that represent the cells in this table row.
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        public virtual IReadOnlyList<TableCell> Cells
        {
            get
            {
                var cellElements = CellElements;
                var tableCells = new List<TableCell>(cellElements.Count);
                foreach (IWebElement cellElement in cellElements)
                {
                    tableCells.Add(new TableCell(cellElement));
                }
                return tableCells;
            }
        }

        /// <summary>
        /// Returns a <seealso cref="TableCell"/> that represents the cell of the specified columnIndex in this table row.
        /// </summary>
        /// <exception cref="NoSuchElementException"> Thrown if this table row does not have a cell at the specified columnIndex. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        public virtual TableCell GetCell(int columnIndex)
        {
            IWebElement cellElement = TableHelper.FindCellElement(WebElement, columnIndex);
            return new TableCell(cellElement);
        }

        /// <summary>
        /// Returns a <seealso cref="TableCell"/> that represents the cell of the specified columnName in this table row.
        /// </summary>
        /// <exception cref="NoSuchElementException"> Thrown if this table row does not have a cell at the specified columnIndex. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of this table row becomes invalid.</exception>
        /// <exception cref="KeyNotFoundException"> if it cannot find a table column header that has the given columnName </exception>exception>
        public virtual TableCell GetCell(string columnName)
        {
            int columnIndex = table.GetColumnIndex(columnName);
            return GetCell(columnIndex);
        }

    }
}