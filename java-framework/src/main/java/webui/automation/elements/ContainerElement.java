package webui.automation.elements;

import java.util.List;

import org.openqa.selenium.By;
import org.openqa.selenium.SearchContext;
import org.openqa.selenium.WebElement;

import webui.automation.framework.BaseElement;

public class ContainerElement extends BaseElement implements SearchContext {

    public ContainerElement(By locator) {
        super(locator);
    }

    /**
     * @param  webElement  The {@link WebElement} of this container element.
     * @throws NullPointerException if the specified <code>webElement</code> is <code>null</code>
     */
    public ContainerElement(WebElement webElement) {
        super(webElement);
    }

    ////// Implements the SearchContext interface.  //////
    
    public WebElement findElement(By by) {
        return getWebElement().findElement(by);
    }

    public List<WebElement> findElements(By by) {
        return getWebElement().findElements(by);
    }

}
