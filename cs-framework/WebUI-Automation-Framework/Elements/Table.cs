using System.Collections.Generic;
using OpenQA.Selenium;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Class for representing and interacting with tables on a web page.
    /// </summary>
    ///
    /// <example>
    /// This example shows a simple way of traversing every cell in a table.
    /// It is efficient in that it locates the table only once.
    /// <code>
    ///   using OpenQA.Selenium;
    ///   using WebUI.Automation.Elements;
    ///
    ///   Table table = new Table(By.id("tableID"));
    ///   foreach (var row in table.Rows)
    ///   {
    ///       foreach (var cell in row.Cells)
    ///       {
    ///           string cellText = cell.Text;
    ///       }
    ///   }
    /// </code>
    /// </example>
    ///
    /// <example>
    /// This example shows a more traditional way of traversing every cell in a table.
    /// It is less efficient in that, each time <code>table.GetCell</code> is called,
    /// it will sequentially locate the table, row, and cell every time.
    /// However, this approach may be desired if the able is periodically refreshed, because
    /// you can implement a work around to handle the asynchronous update issue by wrapping
    /// around the <code>table.GetCell</code> call with a while loop and try-catch
    /// <seealso cref="StaleElementReferenceException"/>.
    /// <code>
    ///   using OpenQA.Selenium;
    ///   using WebUI.Automation.Elements;
    ///
    ///   Table table = new Table(By.id("tableID"));
    ///   int rowCount = table.RowCount;
    ///   int columnCount = table.ColumnCount;
    ///   for(int rowIndex = 0; rowIndex &lt; rowCount; rowIndex++)
    ///   {
    ///       for(int columnIndex = 0; columnIndex &lt; columnCount; columnIndex++)
    ///       {
    ///           string cellText = table.GetCell(rowIndex, columnIndex).Text;
    ///       }
    ///   }
    /// </code>
    /// </example>
    public class Table : ContainerElement
    {

        private IReadOnlyList<string> cachedHeaderTexts;
        private IDictionary<string,int> headerIndexMap;

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
        ///                     it should refer to an HTML {@code <table>} tag in a page. </param>
        /// <exception cref="NullPointerException"> if the specified <code>webElement</code> is <code>null</code> </exception>
        public Table(IWebElement webElement) : base(webElement)
        {
        }

        /// <summary>
        /// Returns an instance of <seealso cref="TableHeaderRow"/> to refer to the header row of this table.
        /// This header row must contain elements of the &lt;th&gt; tag.
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached,
        ///            or if this table does not have a header row </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this property is retrieved).</exception>
        public virtual TableHeaderRow HeaderRow
        {
            get
            {
                // Get the table element, subject to implicit waiting.
                IWebElement tableElement = WebElement;

                IWebElement headerRowElement;
                try
                {
                    headerRowElement = tableElement.FindElement(By.XPath("thead/tr"));
                    return new TableHeaderRow(headerRowElement, this, -1);
                }
                catch (NoSuchElementException)
                {
                    // This table may not have a <thead> tag; use the first row instead.
                    headerRowElement = tableElement.FindElement(By.XPath("tr"));
                    return new TableHeaderRow(headerRowElement, this, 0);
                }
            }
        }

        /// <summary>
        /// Returns a list of text strings in the header cells of this table.
        /// The text from each table cell does not have any leading or trailing whitespace,
        /// and with other whitespace collapsed.
        /// </summary>
        public virtual IReadOnlyList<string> HeaderTexts
        {
            get
            {
                if (cachedHeaderTexts != null)
                {
                    return cachedHeaderTexts;
                }
                else
                {
                    return HeaderRow.CellTexts;
                }
            }
        }

        /// <summary>
        /// Keeps the current table header texts in a cache and reuses them until this method is called again.
        /// Caching the table header texts will improve the performance of table lookup by column names.
        /// </summary>
        public virtual void CacheHeaderTexts()
        {
            cachedHeaderTexts = HeaderRow.CellTexts;
            headerIndexMap = new Dictionary<string, int>(cachedHeaderTexts.Count);
            for(int i = 0; i < cachedHeaderTexts.Count; i++)
            {
                headerIndexMap.Add(cachedHeaderTexts[i], i);
            }
        }

        /// <summary>
        /// Returns the number of columns in this table.
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this property is retrieved).</exception>
        public virtual int ColumnCount
        {
            get
            {
                if (cachedHeaderTexts != null)
                {
                    return cachedHeaderTexts.Count;
                }

                try
                {
                    // Use the table header row to count the number of columns.
                    return HeaderRow.CellCount;
                }
                catch (NoSuchElementException)
                {
                    // The table does not have a header row; use the first data row to count.
                    try
                    {
                        return GetRow(0).CellCount;
                    }
                    catch (NoSuchElementException)
                    {
                        // The table does not have any row; just return 0.
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of <seealso cref="IWebElement"/> instances that represent to the <![CDATA[<tr>]]> elements in this table.
        ///
        /// <para>Note: If this table has a header row and the header row <![CDATA[<tr>]]> tag is not side a <![CDATA[<thead>]]> tag,
        /// this header row will be included as the first row in the returned list. </para>
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this property is retrieved).</exception>
        public virtual IReadOnlyList<IWebElement> RowElements
        {
            get
            {
                // Get the table element, subject to implicit waiting.
                IWebElement tableElement = WebElement;
                return TableHelper.FindRowElements(tableElement);
            }
        }

        /// <summary>
        /// Returns the number of rows in this table.
        ///
        /// <para>Note: If this table has a header row and the header row <![CDATA[<tr>]]> tag is not side a <![CDATA[<thead>]]> tag,
        /// this header row will be counted as one of the rows of the table. </para>
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this property is retrieved).</exception>
        public virtual int RowCount
        {
            get
            {
                return RowElements.Count;
            }
        }

        /// <summary>
        /// Returns a list of <seealso cref="TableRow"/> instances that represent all of the rows in this table.
        ///
        /// <para>Note: If this table has a header row and the header row <![CDATA[<tr>]]> tag is not side a <![CDATA[<thead>]]> tag,
        /// this header row will be included as the first row in the returned list. </para>
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this property is retrieved).</exception>
        public virtual IReadOnlyList<TableRow> Rows
        {
            get
            {
                // Get the table element, subject to implicit waiting.
                IWebElement tableElement = WebElement;
                var rowElements = TableHelper.FindRowElements(tableElement);
                var tableRows = new List<TableRow>(rowElements.Count);
                int index = 0;
                foreach (IWebElement rowElement in rowElements)
                {
                    tableRows.Add(new TableRow(rowElement, this, index++));
                }
                return tableRows;
            }
        }

        /// <summary>
        /// Returns an <seealso cref="IWebElement"/> that represents the row of the specified rowIndex in this table (i.e., a &lt;tr&gt; element).
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached,
        ///            or if this table does not have a row at the specified rowIndex </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        protected virtual IWebElement GetRowElement(int rowIndex)
        {
            // Get the table element, subject to implicit waiting.
            IWebElement tableElement = WebElement;
            return TableHelper.FindRowElement(tableElement, rowIndex);
        }

        /// <summary>
        /// Returns a <seealso cref="TableRow"/> that represents the row of the specified rowIndex in this table.
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached,
        ///            or if this table does not have a row at the specified rowIndex </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual TableRow GetRow(int rowIndex)
        {
            IWebElement rowElement = GetRowElement(rowIndex);
            return new TableRow(rowElement, this, rowIndex);
        }

        /// <summary>
        /// Returns an <seealso cref="IWebElement"/> that represents the cell of the specified rowIndex and columnIndex in this table (i.e., a &lt;tr&gt; element).
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached,
        ///            or if this table does not have a row at the specified rowIndex,
        ///            or if the specified row does not have a cell at the specified columnIndex </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        protected virtual IWebElement GetCellElement(int rowIndex, int columnIndex)
        {
            IWebElement rowElement = GetRowElement(rowIndex);
            return TableHelper.FindCellElement(rowElement, columnIndex);
        }

        /// <summary>
        /// Returns a <seealso cref="TableCell"/> that represents the cell of the specified rowIndex and columnIndex in this table.
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached,
        ///            or if this table does not have a row at the specified rowIndex,
        ///            or if the specified row does not have a cell at the specified columnIndex </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual TableCell GetCell(int rowIndex, int columnIndex)
        {
            IWebElement cellElement = GetCellElement(rowIndex, columnIndex);
            return new TableCell(cellElement, rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns a <seealso cref="TableCell"/> that represents the cell of the specified rowIndex and columnName in this table.
        /// </summary>
        /// <exception cref="NoSuchElementException"> if this table is still not visible (default) or does not exist
        ///            after the <seealso cref="WebUI.DefaultImplicitWaitTimeout default implicit wait timeout"/> is reached,
        ///            or if this table does not have a row at the specified rowIndex,
        ///            or if the specified row does not have a cell at the specified columnIndex </exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the <seealso cref="IWebElement"/> of this table becomes invalid
        ///            (unlikely unless the HTML tag of this table is refreshed while this method is invoked).</exception>
        public virtual TableCell GetCell(int rowIndex, string columnName)
        {
            int columnIndex = GetColumnIndex(columnName);
            return GetCell(rowIndex, columnIndex);
        }

        /// <summary>
        /// Returns the index of the column specified by the given columnName.
        /// </summary>
        /// <exception cref="KeyNotFoundException"> if it cannot find a table column header that matches the given columnName </exception>exception>
        public virtual int GetColumnIndex(string columnName)
        {
            if (headerIndexMap != null)
            {
                return headerIndexMap[columnName];
            }
            else
            {
                var headerTexts = HeaderTexts;
                for (int i = 0; i < headerTexts.Count; i++)
                {
                    if (headerTexts[i].Equals(columnName))
                        return i;
                }
                throw new KeyNotFoundException("Cannot find specified table column name (" + columnName + ") in " + Name + ".");
            }
        }
    }

}