package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;

import webui.automation.framework.BaseElement;
import webui.automation.framework.ByTBD;
import webui.automation.framework.WebUI;

/**
 * Class for representing and interacting with buttons on a web page.
 */
public class Button extends BaseElement {

    /**
     * Constructs an object to represent and interact with a button on a web page.
     * @param  locator  The {@link By} locator for locating the {@link WebElement} of this button;
     *                  it should select the HTML {@code <input type="button">} tag of the button.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public Button(By locator) {
        super(locator);
    }

    /**
     * Constructs an object to represent and interact with a button on a web page.
     * @param  webElement  The {@link WebElement} of this button;
     *                     it should refer to an HTML {@code <input type="button">} tag of a page.
     * @throws NullPointerException if the specified <code>webElement</code> is <code>null</code>
     */
    public Button(WebElement webElement) {
        super(webElement);
    }

    /**
     * Simulates the user interaction of clicking this button on UI.
     *
     * If the button does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @throws NoSuchElementException if this button still does not exist after the default implicit timeout is reached
     */
    public void click() {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".click()");
        } else {
            // Get the web element with the default implicit timeout and then click it.
            getWebElement().click();
        }
    }

    /**
     * Returns the actual text on this button.
     *
     * If the button does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return a String value representing the actual text on this button
     * @throws NoSuchElementException if this button still does not exist after the default implicit timeout is reached
     */
    public String getText() {
        if (this.isLocatedByTBD()) {
            return ByTBD.getMockedStringValue();
        } else {
            // Get the web element with the default implicit timeout.
            WebElement webElement = getWebElement();
            // Get and check the web element text. If it is an empty string,
            // get the text from the value attribute of the button
            String text = webElement.getText();
            if (text.isEmpty()) {
                text = webElement.getAttribute("value");
            }
            return text;
        }
    }

    /**
     * Waits until this button becomes clickable, or until the specified timeout is reached.
     * It returns the located clickable {@link WebElement}.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return the located clickable {@link WebElement}
     * @throws TimeoutException if this button is still not clickable after the specified timeout is reached  
     */
    public WebElement waitUntilClickable(int timeOutInSeconds) {
        if (getLocator() == null) {
            // TODO: Implement waiting for a WebElement to become clickable (rarely needed).
            throw new RuntimeException("This method is not yet implemented for a UI element that is tied to a particular WebElement.");
        } else {
            String timeOutMessage = "Timed out after " + timeOutInSeconds +
                    " seconds in waiting for " + this.getName() + " to become clickable.";
            return getBrowser().waitUntil(
                    ExpectedConditions.elementToBeClickable(getLocator()),
                    timeOutInSeconds, timeOutMessage);
        }
    }

}
