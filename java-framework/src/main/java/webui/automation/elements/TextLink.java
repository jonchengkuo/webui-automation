package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;

import webui.automation.framework.BaseElement;
import webui.automation.framework.ByTBD;
import webui.automation.framework.WebUI;

/**
 * Class for representing and interacting with text links on a web page.
 */
public class TextLink extends BaseElement<TextLink> {

    /**
     * Constructs an object to represent and interact with a text link on a web page.
     * @param  locator  The {@link By} locator for locating this text link;
     *                  it should select the HTML {@code <a>} tag of the text link.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public TextLink(By locator) {
        super(locator);
    }


    /**
     * Simulates the user interaction of clicking this text link on UI.
     *
     * If the text link does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return this text link itself (for supporting the fluid interface) 
     * @throws NoSuchElementException if this text link still does not exist after the default implicit timeout is reached
     */
    public TextLink click () {
        if (this.isLocatedByTBD()) {
            ByTBD.log(this.getName() + ".click()");
        } else {
            // Get the web element with the default implicit timeout and then click it.
            getWebElement().click();
        }
        return this;
    }

    /**
     * Returns the actual text on this text link.
     *
     * If the text link does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return a String value representing the actual text on this text link
     * @throws NoSuchElementException if this text link still does not exist after the default implicit timeout is reached
     */
    public String getText() {
        if (this.isLocatedByTBD()) {
            return ByTBD.getMockedStringValue();
        } else {
            return getWebElement().getText();
        }
    }

}
