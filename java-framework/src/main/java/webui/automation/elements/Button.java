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
public class Button extends BaseElement<Button> {

    /**
     * Constructs an object to represent and interact with a button on a web page.
     * @param  locator  The {@link By} locator for locating this button;
     *                  it should select the HTML {@code <input type="button">} tag of the button.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public Button(By locator) {
        super(locator);
    }

    /**
     * Simulates the user interaction of clicking this button on UI.
     *
     * If the button does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return this button itself (for supporting the fluid interface) 
     * @throws NoSuchElementException if this button still does not exist after the default implicit timeout is reached
     */
    public Button click() {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".click()");
        } else {
            // Get the web element with the default implicit timeout and then click it.
            getWebElement().click();
        }
        return this;
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
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return this button itself (for supporting the fluid interface) 
     * @throws TimeoutException if this button is still not clickable after the specified timeout is reached  
     */
    public Button waitUntilClickable(int timeOutInSeconds) {
        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become clickable.";
        getBrowser().waitUntil(
            ExpectedConditions.elementToBeClickable(getLocator()),
            timeOutInSeconds, timeOutMessage);
        return this;
    }

}
