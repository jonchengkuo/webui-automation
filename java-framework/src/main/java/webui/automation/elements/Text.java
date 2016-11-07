package webui.automation.elements;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;

import webui.automation.framework.BaseElement;
import webui.automation.framework.WebUI;

/**
 * Class for representing and interacting with texts on a web page.
 */
public class Text extends BaseElement {

    /**
     * Constructs an object to represent and interact with a text on a web page.
     * A text is usually displayed by an HTML {@code <div>}, {@code <span>}, or {@code <label>} tag.
     *
     * @param  locator  The {@link By} locator for locating this text.
     *                  it should select the HTML tag that contains the text.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    public Text(By locator) {
        super(locator);
    }

    /**
     * Returns the actual text in this text element.
     *
     * If the text element does not exist, this method will keep waiting until it appears or until
     * the {@link WebUI#defaultImplicitWaitTimeout default implicit wait timeout} is reached.  
     *
     * @return a String value representing the actual text in this text element
     * @throws NoSuchElementException if this text element still does not exist after the default implicit timeout is reached
     */
    public String getText() {
        return getWebElement().getText();
    }

}
