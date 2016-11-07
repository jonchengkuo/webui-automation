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

    ////// Implements the SearchContext interface.  //////
    
    public WebElement findElement(By by) {
        return getWebElement().findElement(by);
    }

    public List<WebElement> findElements(By by) {
        return getWebElement().findElements(by);
    }

}
