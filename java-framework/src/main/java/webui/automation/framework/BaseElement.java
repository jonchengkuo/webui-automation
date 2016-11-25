package webui.automation.framework;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;

import com.google.common.base.Function;

/**
 * Base class for representing and interacting with UI elements (or called UI controls) shown on a web page.
 * It provides common utility methods for subclasses.
 * A subclass should be defined for each specific UI element type (e.g., buttons, check boxes, etc.)
 * to model specific UI behaviors (e.g., clicking a button or checking a check box).
 *
 * <P>Every UI element shown on a web page is defined by an HTML element, which is defined
 * by a pair of HTML start and end tags.</P>
 *
 * <P>Instances of UI elements can be created in two ways: with a {@link By} locator or with a {@link WebElement}.
 * A locator is a mechanism by which the HTML element of a UI element can be located.
 * A WebElement represents a located HTML element.
 * Because every UI element shown on a web page is defined by a particular HTML element,
 * this framework sometimes uses the term <em>UI element</em> and <em>HTML element</em> interchangeably.</P>
 *
 * For a UI element created with a {@link By} locator, every time your code interacts with the UI element
 * (by calling its public methods), it will always try to locate its HTML element on the web page with its locator.
 * This design is less efficient because it does not cache a previously located HTML element (i.e.,
 * it does not reuse a previously available WebElement object). However, this design does guarantee that
 * it will always interact with the current, up-to-date web page. From the framework's perspective, it has no way
 * to know whether a previously located HTML element still exists or not.</P>
 *
 * <P>On the other hand a UI element created with a {@link WebElement} does not have a locator.
 * It will assume that the WebElement given to it is always available and will use it for any UI interaction.
 * This design is more efficient, but it relies on the caller (i.e., your code) to properly manage the
 * life-cycle of a UI element and its WebElement.
 * It is designed to be used as a transient UI element that lives for a short period of time.</P>
 *
 * <p><b>Example:</b></p>
 * <pre>
 *   import org.openqa.selenium.By;
 *   import org.openqa.selenium.WebElement;
 *   import webui.automation.framework.BaseElement;
 *
 *   public class Button extends BaseElement {
 *       public Button(By locator) {
 *           super(locator);
 *       }
 *       public Button(WebElement webElement) {
 *           super(webElement);
 *       }
 *       public void click() {
 *           ...
 *       }
 *   }
 *
 *
 *   Button button1 = new Button(By.id("buttonId1"));
 *
 *   WebElement button2Element = browser.findElement(By.id("buttonId2"));
 *   Button button2 = new Button(button2Element);
 * </pre> 
 */
public class BaseElement {

    private Browser browser;
    private By locator;
    private boolean expectVisible = true;

    private WebElement webElement;

    /**
     * Constructs the base object of a concrete UI element object that represents a specific UI element on a web page.
     * The given <code>locator</code> will be used to locate the {@link WebElement} of the UI element.
     * @param  locator  The {@link By} locator for locating the UI element on a web page.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    protected BaseElement(By locator) {
        if (locator == null) {
            throw new NullPointerException("The locator given to the UI element is null.");
        }
        this.locator = locator;
        this.browser = WebUI.getDefaultBrowser();
    }

    /**
     * Constructs the base object of a concrete UI element object that represents a specific UI element on a web page.
     * This UI element is directly tied to the given {@link WebElement} located by other means.
     * @param  webElement  The {@link WebElement} of the UI element on a web page.
     * @throws NullPointerException if the specified <code>webElement</code> is <code>null</code>
     */
    protected BaseElement(WebElement webElement) {
        if (webElement == null) {
            throw new NullPointerException("The WebElement given to the UI element is null.");
        }
        this.webElement = webElement;
    }

    protected void setExpectVisible(boolean expectVisible) {
        this.expectVisible = expectVisible;
    }

    /**
     * Returns the name of this UI element.
     * It is default to the simple class name of this UI element, such as "Button", "CheckBox", etc.,
     * appended with the string representation of either the locator (if this UI element has a locator),
     * or the {@link WebElement} given to the constructor of this UI element.
     * @return the name of this UI element
     */
    public String getName() {
        if (this.locator == null) {
            return this.getClass().getSimpleName() + "(" + this.webElement.toString() + ")";
        } else {
            return this.getClass().getSimpleName() + "(" + this.locator.toString() + ")";
        }
    }

    /**
     * Returns the {@link By} locator of this UI element.
     * If this UI element is created with a {@link WebElement} instead of a locator, this method will return <code>null</code>.
     * @return the {@link By} locator of this UI element;
     *         <code>null</code> if this UI element is created with a {@link WebElement} instead of a locator
     */
    public By getLocator() {
        return this.locator;
    }

    /**
     * Returns <code>true</code> if the locator of this UI element is the special {@link ByTBD} locator. 
     * @return <code>true</code> if the locator of this UI element is the special {@link ByTBD} locator;
     *         <code>false</code> otherwise
     */
    protected boolean isLocatedByTBD() {
        if (this.locator == null) {
            return false;
        } else {
            return (this.locator instanceof ByTBD);
        }
    }

    /**
     * Returns the {@link Browser} instance used by this UI element.
     * It is default to the {@link Browser} instance returned by {@link WebUI#getDefaultBrowser}
     * when this UI element is created with a {@link By} locator.
     * If this UI element is created with a {@link WebElement}, this method will throw an {@link IllegalStateException}.
     * @return the {@link Browser} instance used by this UI element
     * @throws IllegalStateException if this UI element is created with a {@link WebElement}
     */
    public Browser getBrowser() {
        if (this.browser == null) {
            throw new IllegalStateException("No browser is available for this " + getName() + " because it was created with a WebElement.");
        } else {
            return this.browser;
        }
    }

    // Comment out this method until we want the framework to support multiple Browser instances.
    /*
     * Sets the {@link Browser} instance used by this UI element.
     *
    private void setBrowser(Browser browser) {
        if (browser == null) {
            throw new NullPointerException("The given browser object is null.");
        }
        this.browser = browser;
    }*/


    /**
     * Locates and returns the {@link WebElement} of this UI element.
     * It will periodically (every half second) locate it until the default implicit wait timeout is reached.
     * The default implicit wait timeout is determined and can be configured by {@link WebUI#defaultImplicitWaitTimeout}.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @return the {@link WebElement} located using the locator of this UI element
     * @throws NoSuchElementException if this UI element still does not exist after the default implicit wait timeout is reached
     */
    public WebElement getWebElement() {
        return getWebElement(WebUI.defaultImplicitWaitTimeout);
    }

    /**
     * Locates and returns the {@link WebElement} of this UI element.
     * If the specified timeout is greater than 0, it will periodically (every half second)
     * locate it until the specified timeout is reached.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @return the {@link WebElement} located using the locator of this UI element
     * @throws NoSuchElementException if this UI element still does not exist after the specified timeout is reached
     */
    public WebElement getWebElement(int timeOutInSeconds) {
        if (this.webElement != null) {
            // This UI element is already tied to a particular WebElement; just return it.
            return this.webElement;
        }

        WebElement webElement = null;
        if (timeOutInSeconds == 0) {
            webElement = getBrowser().findElement(this.locator);
            if (this.expectVisible && !webElement.isDisplayed()) {
                throw new NoSuchElementException(this.getName() + " is present but not visible.");
            }
        } else {
            try {
                String timeOutMessage = "Timed out after " + timeOutInSeconds +
                    " seconds in waiting for " + this.getName() + " to become " +
                    ((this.expectVisible) ? " visible." : "present.");
                Function<? super WebDriver, WebElement> expectedCondition;
                if (this.expectVisible) {
                    expectedCondition = ExpectedConditions.visibilityOfElementLocated(this.locator);
                } else {
                    expectedCondition = ExpectedConditions.presenceOfElementLocated(this.locator);
                }
                webElement = getBrowser().waitUntil(expectedCondition, timeOutInSeconds, timeOutMessage);
            } catch (TimeoutException e) {
                // Convert a TimeoutException into a NoSuchElementException.
                throw new NoSuchElementException(e.getMessage());
            }
        }
        return webElement;
    }


    /**
     * Returns whether this UI element becomes exist or not within the default implicit wait timeout.
     * The default implicit wait timeout is determined and can be configured by {@link WebUI#defaultImplicitWaitTimeout}.
     *
     * By default, a UI element is considered exist if it is present and visible.
     * A subclass may override this default behavior by calling the {@link #setExpectVisible} method.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @return <code>true</code> if this UI element exists within the default implicit wait timeout;
     *         <code>false</code> otherwise
     */
    public boolean exists() {
        return exists(WebUI.defaultImplicitWaitTimeout);
    }

    /**
     * Returns whether this UI element exists or not.
     * If the specified timeout is 0, it will check the current existence of this UI element.
     * If the specified timeout is greater than 0, it will periodically (every half second)
     * check the existence of this UI element until the specified timeout is reached.
     *
     * By default, a UI element is considered exist if it is present and visible.
     * A subclass may override this default behavior by calling the {@link #setExpectVisible} method.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @return <code>true</code> if this UI element exists within the specified timeout;
     *         <code>false</code> otherwise
     */
    public boolean exists(int timeOutInSeconds) {
        try {
            getWebElement(timeOutInSeconds);
            return true;
        } catch (NoSuchElementException e) {
            return false;
        }
    }


    /**
     * Waits until this UI element exists, or until the default implicit wait timeout is reached.
     * The default implicit wait timeout is determined and can be configured by {@link WebUI#defaultImplicitWaitTimeout}.
     *
     * By default, a UI element is considered exist if it is present and visible.
     * A subclass may override this default behavior by calling the {@link #setExpectVisible} method.
     *
     * @throws TimeoutException if this UI element still does not exist after the default implicit wait timeout is reached 
     */
    public void waitUntilExists() {
        this.waitUntilExists(WebUI.defaultImplicitWaitTimeout);
    }

    /**
     * Waits until this UI element exists, or until the specified timeout is reached.
     *
     * By default, a UI element is considered exist if it is present and visible.
     * A subclass may override this default behavior by calling the {@link #setExpectVisible} method.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @throws TimeoutException if this UI element still does not exist after the specified timeout is reached 
     */
    public void waitUntilExists(int timeOutInSeconds) {
        if (this.expectVisible) {
            waitUntilVisible(timeOutInSeconds);
        } else {
            waitUntilPresent(timeOutInSeconds);
        }
    }

    /**
     * Waits until this UI element becomes present (i.e., its HTML element is present in the DOM tree),
     * or until the specified timeout is reached.
     * It returns the located {@link WebElement}.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return the located {@link WebElement}
     * @throws TimeoutException if this UI element is still not present after the specified timeout is reached  
     */
    public WebElement waitUntilPresent(int timeOutInSeconds) {
        if (this.webElement != null) {
            // This UI element is already tied to a particular WebElement; there is no need to wait.
            return this.webElement;
        }
        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become present.";
        return getBrowser().waitUntil(
            ExpectedConditions.presenceOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
    }

    /**
     * Waits until this UI element becomes visible, or until the specified timeout is reached.
     * It returns the located {@link WebElement}.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return the located {@link WebElement}
     * @throws TimeoutException if this UI element is still not visible after the specified timeout is reached  
     */
    public WebElement waitUntilVisible(int timeOutInSeconds) {
        if (this.webElement != null) {
            // TODO: Implement waiting for a WebElement to become visible (rarely needed).
            throw new RuntimeException("This method is not yet implemented for a UI element that is tied to a particular WebElement.");
        }
        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become visible.";
        return getBrowser().waitUntil(
            ExpectedConditions.visibilityOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
    }

    /**
     * Waits until this UI element becomes invisible, or until the specified timeout is reached.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return the return value from {@link WebDriverWait#until}.
     * @throws TimeoutException if this UI element is still visible after the specified timeout is reached  
     */
    public Boolean waitUntilNotVisible(int timeOutInSeconds) {
        if (this.webElement != null) {
            // TODO: Implement waiting for a WebElement to become invisible (rarely needed).
            throw new RuntimeException("This method is not yet implemented for a UI element that is tied to a particular WebElement.");
        }
        String timeOutMessage = "Expecting " + this.getName() +
            " to disappear but it is still visible after " + timeOutInSeconds + " seconds.";
        return getBrowser().waitUntil(
            ExpectedConditions.invisibilityOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
    }

}
