package webui.automation.framework;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;

import com.google.common.base.Function;

import webui.automation.browser.Browser;

/**
 * Base class for representing and interacting with UI elements on a web page.
 * It provides common and utility methods for subclasses.
 * A subclass should be defined for each specific UI element type (e.g., a button)
 * to simulate its specific UI behaviors (e.g., clicking a button).
 *
 * <p><b>Example:</b></p>
 * <pre>
 *   import webui.automation.framework.BaseElement;
 *
 *   public class Button extends BaseElement<Button> {
 *       public void click() {
 *           ...
 *       }
 *   }
 * </pre> 
 */
public class BaseElement<T> {

    private Browser browser = WebUI.getDefaultBrowser();
    private By locator;
    private boolean expectVisible = true;

    /**
     * Constructs the base object of a concrete UI element object that represents a specific UI element on a web page.
     * @param  locator  The {@link By} locator for locating the UI element on a web page.
     * @throws NullPointerException if the specified <code>locator</code> is <code>null</code>
     */
    protected BaseElement(By locator) {
        if (locator == null) {
            throw new NullPointerException("The locator given to the UI element is null.");
        }
        this.locator = locator;
    }

    protected void setExpectVisible(boolean expectVisible) {
        this.expectVisible = expectVisible;
    }

    public String getName() {
        return this.getClass().getSimpleName() + "(" + this.locator.toString() + ")";
    }

    public By getLocator() {
        return this.locator;
    }

    /**
     * Returns <code>true</code> if the locator of this UI element is the special {@link ByTBD} locator. 
     * @return <code>true</code> if the locator of this UI element is the special {@link ByTBD} locator;
     *         <code>false</code> otherwise
     */
    protected boolean isLocatedByTBD() {
        return (this.locator == ByTBD.ByTBD);
    }

    /**
     * Returns the {@link Browser} instance used by this UI element.
     * It is default to the {@link Browser} instance returned by {@link WebUI#getDefaultBrowser}
     * when this UI element is created.
     * @return the {@link Browser} instance used by this UI element
     */
    public Browser getBrowser() {
        return this.browser;
    }

    // Comment out this method until we want the framework to support multiple Browser instances.
    /*
     * Sets the {@link Browser} instance used by this UI element.
     * @return this UI element itself (for supporting the fluid interface) 
     *
    @SuppressWarnings("unchecked")
    private T setBrowser(Browser browser) {
        if (browser == null) {
            throw new NullPointerException("The given browser object is null.");
        }
        this.browser = browser;
        return (T) this;
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
     * @throws TimeoutException if this UI element still does not exist after the default implicit wait timeout is reached 
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
     * @throws TimeoutException if this UI element still does not exist after the specified timeout is reached 
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
     * @return this UI element itself (for supporting the fluid interface) 
     * @throws TimeoutException if this UI element still does not exist after the default implicit wait timeout is reached 
     */
    public T waitUntilExists() {
        return this.waitUntilExists(WebUI.defaultImplicitWaitTimeout);
    }

    /**
     * Waits until this UI element exists, or until the specified timeout is reached.
     *
     * By default, a UI element is considered exist if it is present and visible.
     * A subclass may override this default behavior by calling the {@link #setExpectVisible} method.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @return this UI element itself (for supporting the fluid interface) 
     * @throws TimeoutException if this UI element still does not exist after the specified timeout is reached 
     */
    public T waitUntilExists(int timeOutInSeconds) {
        if (this.expectVisible) {
            return waitUntilVisible(timeOutInSeconds);
        } else {
            return waitUntilPresent(timeOutInSeconds);
        }
    }

    /**
     * Waits until this UI element becomes present, or until the specified timeout is reached.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return this UI element itself (for supporting the fluid interface) 
     * @throws TimeoutException if this UI element is still not present after the specified timeout is reached  
     */
    @SuppressWarnings("unchecked")
    public T waitUntilPresent(int timeOutInSeconds) {
        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become present.";
        getBrowser().waitUntil(
            ExpectedConditions.presenceOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
        return (T) this;
    }

    /**
     * Waits until this UI element becomes present and visible, or until the specified timeout is reached.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return this UI element itself (for supporting the fluid interface) 
     * @throws TimeoutException if this UI element is still not visible after the specified timeout is reached  
     */
    @SuppressWarnings("unchecked")
    public T waitUntilVisible(int timeOutInSeconds) {
        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become visible.";
        getBrowser().waitUntil(
            ExpectedConditions.visibilityOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
        return (T) this;
    }

    /**
     * Waits until this UI element becomes invisible, or until the specified timeout is reached.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return this UI element itself (for supporting the fluid interface) 
     * @throws TimeoutException if this UI element is still visible after the specified timeout is reached  
     */
    @SuppressWarnings("unchecked")
    public T waitUntilInvisible(int timeOutInSeconds) {
        String timeOutMessage = "Expecting " + this.getName() +
            " to disappear but it is still visible after " + timeOutInSeconds + " seconds.";
        getBrowser().waitUntil(
            ExpectedConditions.invisibilityOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
        return (T) this;
    }

}
