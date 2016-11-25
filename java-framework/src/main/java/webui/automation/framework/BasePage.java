package webui.automation.framework;

import org.openqa.selenium.By;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;

/**
 * Base class for representing and interacting with a web page using the page object model.
 * A subclass should be defined for each specific web page
 * to simulate its application-specific UI behaviors (e.g., logging in).
 *
 * <p><b>Example:</b></p>
 * <pre>
 *   import webui.automation.framework.BasePage;
 *
 *   public class LoginPage extends BasePage<LoginPage> {
 *       public void login(String user, String password) {
 *           ...
 *       }
 *   }
 * </pre> 
 */
public class BasePage<T extends BasePage<T>> {

    private Browser browser = WebUI.getDefaultBrowser();
    private By locator;

    protected BasePage() {
    }

    protected BasePage(By locator) {
        if (locator == null) {
            throw new NullPointerException("The locator given to the page is null.");
        }
        this.locator = locator;
    }

    protected void setKeyElement(BaseElement keyElement) {
        if (keyElement == null) {
            throw new NullPointerException("The key element given to the page is null.");
        }
        this.locator = keyElement.getLocator();
    }

    private void assertLocatorNotNull() {
        if (this.locator == null) {
            throw new NullPointerException("The locator of this page object is not set." +
                " You must set it (preferred in your page class constructor) using either" +
                " the BasePage constructor or the BasePage.setKeyElement method.");
        }
    }

    public String getName() {
        return this.getClass().getSimpleName();
    }

    /**
     * Returns the {@link Browser} instance used by this page.
     * It is default to the {@link Browser} instance returned by {@link WebUI#getDefaultBrowser}
     * when this page is created.
     * @return the {@link Browser} instance used by this page
     */
    public Browser getBrowser() {
        return this.browser;
    }

    // Comment out this method until we want the framework to support multiple Browser instances.
    /*
     * Sets the {@link Browser} instance used by this page.
     * TODO: It should also set the Browser instance of all of the UI elements in this page object.
     * @return this page itself (for supporting the fluid interface) 
     *
    @SuppressWarnings("unchecked")
    public T setBrowser(Browser browser) {
        if (browser == null) {
            throw new NullPointerException("The given browser object is null.");
        }
        this.browser = browser;
        // TODO: Set all UI elements on this page object to use the given browser.
        return (T) this;
    }*/


    /**
     * Returns whether this web page is available or not within the default page loading timeout,
     * which is specified and can be configured by {@link WebUI#defaultPageLoadingTimeout}.
     *
     * A page is considered available if the browser can use the locator of this page to locate a visible {@link WebElement}.
     *
     * @return <code>true</code> if this web page is available within the default page loading timeout;
     *         <code>false</code> otherwise
     */
    public boolean isAvailable() {
        return isAvailable(WebUI.defaultPageLoadingTimeout);
    }

    /**
     * Returns whether this web page is available or not within the specified timeout.
     * If the specified timeout is 0, it will check the current availability of this web page.
     * If the specified timeout is greater than 0, it will periodically (every half second)
     * check the existence of this web page until the specified timeout is reached.
     *
     * A page is considered available if the browser can use the locator of this page to locate a visible {@link WebElement}.
     *
     * @param  timeOutInSeconds  timeout in seconds
     * @return <code>true</code> if this web page is available within the specified timeout;
     *         <code>false</code> otherwise
     */
    public boolean isAvailable(int timeOutInSeconds) {
        assertLocatorNotNull();
        if (timeOutInSeconds == 0) {
            WebElement webElement = getBrowser().findElement(this.locator);
            return webElement.isDisplayed();
        } else {
            try {
                waitUntilAvailable(timeOutInSeconds);
                return true;
            } catch (TimeoutException e) {
                return false;
            }
        }
    }


    /**
     * Waits until this page becomes available, or until the default page-loading timeout is reached.
     * The default page-loading timeout is determined and can be configured by {@link WebUI#defaultPageLoadingTimeout}.
     *
     * A page is considered available if the browser can use the locator of this page to locate a visible {@link WebElement}.
     *
     * @return this page itself (for supporting the fluid interface) 
     * @throws TimeoutException if the page is still not available after the default page-loading timeout expires 
     */
    public T waitUntilAvailable() {
        return this.waitUntilAvailable(WebUI.defaultPageLoadingTimeout);
    }

    /**
     * Waits until this page becomes available, or until the specified timeout is reached.
     *
     * A page is considered available if the browser can use the locator of this page to locate a visible {@link WebElement}.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return this page itself (for supporting the fluid interface) 
     * @throws TimeoutException if the page is still not available after the specified timeout is reached 
     */
    @SuppressWarnings("unchecked")
    public T waitUntilAvailable(int timeOutInSeconds) {
        assertLocatorNotNull();
        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become available.";
        getBrowser().waitUntil(
            ExpectedConditions.visibilityOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
        return (T) this;
    }

}
