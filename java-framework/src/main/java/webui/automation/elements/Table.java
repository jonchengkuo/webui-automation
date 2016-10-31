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
        return TableHelper.findRowElements(tableElement);
    }

    public WebElement getRowElement(int rowIndex) {
        WebElement tableElement = getWebElement();
        return TableHelper.findRowElement(tableElement, rowIndex);
    }

    public List<WebElement> getCellElements(int rowIndex) {
        WebElement rowElement = getRowElement(rowIndex);
        return TableHelper.findCellElements(rowElement);
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
        return TableHelper.findCellElement(rowElement, columnIndex);
    }

    public String getCellText(int rowIndex, int columnIndex) {
        return getCellElement(rowIndex, columnIndex).getText();
    }

}
