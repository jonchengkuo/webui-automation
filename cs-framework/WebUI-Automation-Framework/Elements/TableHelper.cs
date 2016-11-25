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

        public static IWebElement FindRowElement(IWebElement tableElement, int rowIndex)
        {
            IList<IWebElement> rowElements = FindRowElements(tableElement);
            int maxIndex = rowElements.Count - 1;
            if (rowIndex < 0 || rowIndex > maxIndex)
            {
                throw new System.IndexOutOfRangeException("The table row index (" + rowIndex + ") is out of bounds (0.." + maxIndex + ").");
            }
            return rowElements[rowIndex];
        }

        public static ReadOnlyCollection<IWebElement> FindCellElements(IWebElement rowElement)
        {
            // Find the <td> elements within a table row.
            return rowElement.FindElements(By.XPath("td"));
        }

        public static IWebElement FindCellElement(IWebElement rowElement, int columnIndex)
        {
            ReadOnlyCollection<IWebElement> cellElements = FindCellElements(rowElement);
            int maxIndex = cellElements.Count - 1;
            if (columnIndex < 0 || columnIndex > maxIndex)
            {
                throw new System.IndexOutOfRangeException("The table column index (" + columnIndex + ") is out of bounds (0.." + maxIndex + ").");
            }
            return cellElements[columnIndex];
        }

    }

}