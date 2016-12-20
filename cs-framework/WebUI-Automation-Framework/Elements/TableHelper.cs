using System.Collections.Generic;
using System.Collections.ObjectModel;  // ReadOnlyCollection<T>
using OpenQA.Selenium;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class containing static helper methods for accessing an HTML table.
    /// 
    /// These functions take the IWebElement of a table or row and operate on it.
    /// Because they do not need to get the IWebElement themselves every time,
    /// it is more efficient to use them instead of the general Table.getXXX methods.
    /// </summary>
    public class TableHelper
    {

        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of the given table becomes invalid.</exception>
        public static ReadOnlyCollection<IWebElement> FindRowElements(IWebElement tableElement)
        {
            // Find the <tr> elements within the table.
            ReadOnlyCollection<IWebElement> rowElements = tableElement.FindElements(By.XPath("tbody/tr"));
            if (rowElements.Count == 0)
            {
                rowElements = tableElement.FindElements(By.XPath("tr"));
            }
            return rowElements;
        }

        /// <exception cref="NoSuchElementException"> Thrown if the given table does not have a row at the specified rowIndex. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of the given table becomes invalid.</exception>
        public static IWebElement FindRowElement(IWebElement tableElement, int rowIndex)
        {
            // Check if the table has a <tbody> tag or not.
            IList<IWebElement> tbodyElements = tableElement.FindElements(By.XPath("tbody"));
            bool hasTbody = tbodyElements.Count > 0;

            if (rowIndex >= 0)
            {
                string rowXPath;
                if (hasTbody)
                {
                    rowXPath = "tbody/tr[" + (rowIndex + 1) + "]";
                }
                else
                {
                    rowXPath = "tr[" + (rowIndex + 1) + "]";
                }

                try
                {
                    return tableElement.FindElement(By.XPath(rowXPath));
                }
                catch (NoSuchElementException)
                {
                    // fall through; will be handled below
                }
            }

            // Cannot find the specified row; throw an exception.

            IList<IWebElement> rowElements;
            if (hasTbody)
            {
                rowElements = tableElement.FindElements(By.XPath("tbody/tr"));
            }
            else
            {
                rowElements = tableElement.FindElements(By.XPath("tr"));
            }

            if (rowElements.Count == 0)
            {
                throw new NoSuchElementException("The given table has no rows.");
            }
            else
            {
                int maxIndex = rowElements.Count - 1;
                throw new NoSuchElementException("The table row index (" + rowIndex + ") is out of bounds (0.." + maxIndex + ") in the given table.");
            }
        }

        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of the given table row becomes invalid.</exception>
        public static ReadOnlyCollection<IWebElement> FindCellElements(IWebElement rowElement)
        {
            // Find the <td> elements within a table row.
            return rowElement.FindElements(By.XPath("td"));
        }

        /// <exception cref="NoSuchElementException"> Thrown if the given table row does not have a cell at the specified columnIndex. </exception>
        /// <exception cref="StaleElementReferenceException">Thrown if the <seealso cref="IWebElement"/> of the given table row becomes invalid.</exception>
        public static IWebElement FindCellElement(IWebElement rowElement, int columnIndex)
        {
            if (columnIndex >= 0)
            {
                string cellXPath = "td[" + (columnIndex + 1) + "]";
                try
                {
                    return rowElement.FindElement(By.XPath(cellXPath));
                }
                catch (NoSuchElementException)
                {
                    // fall through; will be handled below
                }
            }

            // Cannot find the specified cell; throw an exception.

            IList<IWebElement> cellElements = FindCellElements(rowElement);
            if (cellElements.Count == 0)
            {
                throw new NoSuchElementException("The given table row has no cells.");
            }
            else
            {
                int maxIndex = cellElements.Count - 1;
                throw new NoSuchElementException("The table column index (" + columnIndex + ") is out of bounds (0.." + maxIndex + ") on the given table row.");
            }
        }

    }

}