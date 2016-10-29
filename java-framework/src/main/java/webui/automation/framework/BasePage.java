package webui.automation.framework;

import org.openqa.selenium.By;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;

import webui.automation.browser.Browser;

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
public class BasePage<T> {

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

    protected void setKeyElement(BaseElement<?> keyElement) {
        if (keyElement == null) {
            throw new NullPointerException("The key element given to the page is null.");
        }
        this.locator = keyElement.getLocator();
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
     * Waits until this page becomes available, or until the default page-loading timeout is reached.
     * The default page-loading timeout is determined and can be configured by {@link WebUI#defaultPageLoadingTimeout}.
     *
     * A page is considered available if the browser can use the locator of the page to locate a visible {@link WebElement}.
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
     * A page is considered available if the browser can use the locator of the page to locate a visible {@link WebElement}.
     *
     * @param  timeOutInSeconds  time out in seconds
     * @return this page itself (for supporting the fluid interface) 
     * @throws TimeoutException if the page is still not available after the specified timeout is reached 
     */
    @SuppressWarnings("unchecked")
    public T waitUntilAvailable(int timeOutInSeconds) {
        if (this.locator == null) {
            throw new NullPointerException("The locator of this page object is not set." +
                " You must set it (preferred in your page class constructor) using either" +
                " the BasePage constructor or the BasePage.setKeyElement method.");
        }

        String timeOutMessage = "Timed out after " + timeOutInSeconds +
            " seconds in waiting for " + this.getName() + " to become available.";
        getBrowser().waitUntil(
            ExpectedConditions.visibilityOfElementLocated(this.locator),
            timeOutInSeconds, timeOutMessage);
        return (T) this;
    }

}
