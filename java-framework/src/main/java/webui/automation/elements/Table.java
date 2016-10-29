package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import webui.automation.elements.ContainerElement;

import java.util.ArrayList;
import java.util.List;

/**
 * Class for representing and interacting with tables on a web page.
 */
public class Table extends ContainerElement<Table> {

    /**
     * Constructs an object to represent and interact with a table on a web page.
     *
     * @param  locator  The {@link By} locator for locating this table.
     *                  it should select the HTML {@code <table>} tag of the table.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public Table(By locator) {
        super(locator);
    }


    public int getRowCount() {
        return getRowElements().size();
    }

    /**
     * Returns the {@link WebElement} instances that point to the <tr> elements of each table row.
     * @return
     */
    public List<WebElement> getRowElements() {
        WebElement tableElement = getWebElement();
        return findRowElements(tableElement);
    }

    public WebElement getRowElement(int rowIndex) {
        WebElement tableElement = getWebElement();
        return findRowElement(tableElement, rowIndex);
    }

    public List<WebElement> getCellElements(int rowIndex) {
        WebElement rowElement = getRowElement(rowIndex);
        return findCellElements(rowElement);
    }

    public List<String> getCellTexts(int rowIndex) {
        List<WebElement> cellElements = getCellElements(rowIndex);
        List<String> textList = new ArrayList<String>(cellElements.size());
        for (WebElement cellElement : cellElements) {
            textList.add(cellElement.getText());
        }
        return textList;
    }

    public WebElement getCellElement(int rowIndex, int columnIndex) {
        WebElement rowElement = getRowElement(rowIndex);
        return findCellElement(rowElement, columnIndex);
    }

    public String getCellText(int rowIndex, int columnIndex) {
        return getCellElement(rowIndex, columnIndex).getText();
    }


    //////  (Static) Table Helper Functions  //////
    // These functions take the WebElement of a table or row and operate on it.
    // Because they do not need to get the WebElement themselves every time,
    // it is more efficient to use them instead of the general Table.getXXX methods.

    public static List<WebElement> findRowElements(WebElement tableElement) {
        // Find the <tr> elements within the table.
        List<WebElement> rowElements = tableElement.findElements(By.xpath("tbody/tr"));
        if (rowElements.size() == 0) {
            rowElements = tableElement.findElements(By.xpath("tr"));
        }
        return rowElements;
    }

    public static WebElement findRowElement(WebElement tableElement, int rowIndex) {
        List<WebElement> rowElements = findRowElements(tableElement);
        int maxIndex = rowElements.size() - 1;
        if (rowIndex < 0 || rowIndex > maxIndex) {
            throw new IndexOutOfBoundsException(
                "The table row index (" + rowIndex + ") is out of bounds (0.." + maxIndex + ").");
        }
        return rowElements.get(rowIndex);
    }

    public static List<WebElement> findCellElements(WebElement rowElement) {
        // Find the <td> elements within a table row.
        return rowElement.findElements(By.xpath("td"));
    }

    public static WebElement findCellElement(WebElement rowElement, int columnIndex) {
        List<WebElement> cellElements = findCellElements(rowElement);
        int maxIndex = cellElements.size() - 1;
        if (columnIndex < 0 || columnIndex > maxIndex) {
            throw new IndexOutOfBoundsException(
                "The table column index (" + columnIndex + ") is out of bounds (0.." + maxIndex + ").");
        }
        return cellElements.get(columnIndex);
    }

}
