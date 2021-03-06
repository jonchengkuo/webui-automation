package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.WebElement;

import webui.automation.framework.BaseElement;
import webui.automation.framework.ByTBD;
import webui.automation.framework.WebUI;

/**
 * Class for representing and interacting with check boxes on a web page.
 */
public class RadioButton extends BaseElement {

    /**
     * Constructs an object to represent and interact with a radio button on a web page.
     *
     * Note that the label of the radio button is usually a label or text element outside the radio button element.
     * Thus, it needs to be handled separately.
     *
     * @param  locator  The {@link By} locator for locating this radio button;
     *                  it should select the HTML {@code <input type="radio">} tag of the radio button.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public RadioButton (By locator) {
        super(locator);
    }

    /**
     * Constructs an object to represent and interact with a radio button on a web page.
     * @param  webElement  The {@link WebElement} of this radio button;
     *                     it should refer to an HTML {@code <input type="radio">} tag of a page.
     * @throws NullPointerException if the specified <code>webElement</code> is <code>null</code>
     */
    public RadioButton(WebElement webElement) {
        super(webElement);
    }

    /**
     * Simulates the user interaction of selecting this radio button.
     *
     * If the radio button does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @throws NoSuchElementException if this radio button still does not exist after the default implicit timeout is reached
     */
    public void select() {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".select()");
        } else {
            // Get the web element with the default implicit timeout and then click it.
            getWebElement().click();
        }
    }

}
