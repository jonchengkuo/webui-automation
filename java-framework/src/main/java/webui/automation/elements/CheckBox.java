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
public class CheckBox extends BaseElement<CheckBox> {

    /**
     * Constructs an object to represent and interact with a check box on a web page.
     *
     * Note that the label of the check box is usually a label or text element outside the check box element.
     * Thus, it needs to be handled separately.
     *
     * @param  locator  The {@link By} locator for locating this check box;
     *                  it should select the HTML {@code <input type="checkbox">} tag of the check box.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public CheckBox(By locator) {
        super(locator);
    }

    /**
     * Simulates the user interaction of checking this check box.
     *
     * If the check box does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return this check box itself (for supporting the fluid interface) 
     * @throws NoSuchElementException if this check box still does not exist after the default implicit timeout is reached
     */
    public CheckBox check() {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".check()");
        } else {
            // Get the web element with the default implicit timeout.
            WebElement webElement = getWebElement();
            if (!webElement.isSelected()) {
                webElement.click();
            }
        }
        return this;
    }

    /**
     * Simulates the user interaction of unchecking this check box.
     *
     * If the check box does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return this check box itself (for supporting the fluid interface) 
     * @throws NoSuchElementException if this check box still does not exist after the default implicit timeout is reached
     */
    public CheckBox uncheck () {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".uncheck()");
        } else {
            // Get the web element with the default implicit timeout.
            WebElement webElement = getWebElement();
            if (webElement.isSelected()) {
                webElement.click();
            }
        }
        return this;
    }

}
