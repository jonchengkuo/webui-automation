package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import java.util.List;

/**
 * Class containing static helper methods for accessing an HTML table.
 *
 * These functions take the WebElement of a table or row and operate on it.
 * Because they do not need to get the WebElement themselves every time,
 * it is more efficient to use them instead of the general Table.getXXX methods.
 */
public class TableHelper {

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
