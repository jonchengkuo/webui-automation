package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.WebElement;

import webui.automation.framework.BaseElement;
import webui.automation.framework.ByTBD;
import webui.automation.framework.WebUI;

/**
 * Class for representing and interacting with text fields on a web page.
 */
public class TextField extends BaseElement<TextField> {

    /**
     * Constructs an object to represent and interact with a text field on a web page.
     *
     * Note that the label of a text field is outside the text field element and should be handled separately.
     *
     * @param  locator  The {@link By} locator for locating this text field;
     *                  it should select the HTML {@code <input type="text">} tag of the text field.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public TextField(By locator) {
        super(locator);
    }

    /**
     * Simulates the user interaction of clearing this text field and entering the given text into this text field.
     *
     * If the text field does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return this text field itself (for supporting the fluid interface) 
     * @throws NoSuchElementException if this text field still does not exist after the default implicit timeout is reached
     */
    public TextField setText(String text) {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".setText(\"" + text + "\")");
        } else {
            // Get the web element with the default implicit timeout.
            WebElement webElement = getWebElement();
            webElement.clear();
            webElement.sendKeys(text);
        }
        return this;
    }

    /**
     * Returns the actual text in this text field.
     *
     * If the text field does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return a String value representing the actual text in this text field
     * @throws NoSuchElementException if this text field still does not exist after the default implicit timeout is reached
     */
    public String getText() {
        if (this.isLocatedByTBD()) {
            return ByTBD.getMockedStringValue();
        } else {
            // Get the web element with the default implicit timeout.
            WebElement webElement = getWebElement();
            return webElement.getAttribute("value");
        }
    }

    /**
     * Simulates the user interaction of submitting the web form that contains this text field.
     *
     * If the text field does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return this text field itself (for supporting the fluid interface) 
     * @throws NoSuchElementException if this text field still does not exist after the default implicit timeout is reached
     */
    public TextField submit() {
        if (this.isLocatedByTBD()) {
            ByTBD.log(getName() + ".submit()");
        } else {
            // Get the web element with the default implicit timeout and then
            // submit the form that contains this text field.
            getWebElement().submit();
        }
        return this;
    }

}
