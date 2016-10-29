package webui.automation.browser;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.concurrent.TimeUnit;

import org.openqa.selenium.By;
import org.openqa.selenium.Dimension;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.SearchContext;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.TimeoutException;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebDriverException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.support.ui.WebDriverWait;

import com.google.common.base.Function;

/**
 * This class provides a reference and an interface to interact with a web browser instance.
 * Unlike a Selenium {@link WebDriver}, the reference to an instance of this class does not need to change
 * when the web browser in use changes (e.g., changing the web browser in use from Chrome to Firefox).
 * 
 * <p>An instance of this class may be in one of the following states:</p>
 * <ol>
 *   <li>Non-opened: The default state; no web browser instance is associated with this {@link Browser} instance.
 *   <li>Opened: A web browser instance is associated with this {@link Browser} instance.
 * </ol>
 *
 * <p>An instance of this class may enter the opened state by calling the {@link #open(BrowserType)} method.</p>
 *
 * <p>Example:</p>
 * The following code opens and closes a Chrome browser:
 * <pre>
 *     Browser browser = new Browser();
 *     browser.open(BrowserType.CHROME);
 *     // do something
 *     browser.close();
 * </pre>
 */
public class Browser implements SearchContext, JavascriptExecutor {

    protected BrowserType browserType;

    protected WebDriver   webDriver;

    /**
     * Constructs a non-opened browser instance. 
     */
    public Browser() {
    }

    /**
     * Opens a new browser instance/window according to the specified browser type.
     * If it succeeds, this {@link Browser} instance is in the opened state.
     *
     * @param  browserType  browser type
     * @return this browser instance
     * @throws WebDriverException if it cannot opens a new browser instance
     * @throws RuntimeException if this {@link Browser} instance is already opened
     */
    public Browser open(BrowserType browserType) {
        if (this.webDriver != null) {
            throw new RuntimeException("This browser is already opened. You must close it before openning another browser instance.");
        }

        switch (browserType) {
            case CHROME:
                this.webDriver = new ChromeDriver();
                break;
            case FIREFOX:
                this.webDriver = new FirefoxDriver();
                break;
            case IE:
                this.webDriver = new InternetExplorerDriver();
                break;
            default:
                throw new RuntimeException("Unsupported browser type: " + browserType.name());
        }
        this.browserType = browserType;
        return this;
    }

    /**
     * Closes the currently opened browser instance/window.
     * It does nothing if no web browser is currently opened.
     * <p>
     * Note: After calling this method, this {@link Browser} instance will always enter the non-opened state.
     * @return this browser instance
     */
    public Browser close() {
        if (this.webDriver != null) {
            try {
                this.webDriver.quit();
            } finally {
                // No matter closing the browser succeeds or not,
                // we'll discard the current browser/web driver instance.
                this.webDriver = null;
                this.browserType = null;
            }
        }
        return this;
    }

    /**
     * Returns the {@link WebDriver} instance of the currently opened browser.
     * It returns <code>null</code> if no web browser is currently opened.
     *
     * @return the {@link WebDriver} instance of the currently opened browser
     *         or <code>null</code> if no web browser is currently opened
     */
    public WebDriver getWebDriver() {
        return this.webDriver;
    }

    protected void assertBrowserOpened() {
        if (this.webDriver == null) {
            throw new RuntimeException("This operation is illegal when no web browser is opened.");
        }
    }


    /**
     * Returns the type of the currently opened browser.
     * It returns <code>null</code> if no web browser is currently opened.
     *
     * @return the browser type of the currently opened browser
     *         or <code>null</code> if no web browser is currently opened
     */
    public BrowserType getBrowserType() {
        return this.browserType;
    }

    /**
     * Returns <code>true</code> if the currently opened browser is Chrome.
     * If no web browser is currently opened, it returns <code>false</code>. 
     *
     * @return <code>true</code> if the currently opened browser is Chrome; <code>false</code> otherwise
     */
    public boolean isChrome() {
        return (getBrowserType() == BrowserType.CHROME); 
    }

    /**
     * Returns <code>true</code> if the currently opened web browser is Firefox.
     * If no web browser is currently opened, it returns <code>false</code>. 
     *
     * @return <code>true</code> if the currently opened web browser is Firefox; <code>false</code> otherwise
     */
    public boolean isFirefox() {
        return (getBrowserType() == BrowserType.FIREFOX);
    }

    /**
     * Returns <code>true</code> if the currently opened web browser is Internet Explorer.
     * If no web browser is currently opened, it returns <code>false</code>. 
     *
     * @return <code>true</code> if the currently opened web browser is Internet Explorer; <code>false</code> otherwise
     */
    public boolean isIE() {
        return (getBrowserType() == BrowserType.IE); 
    }


    /**
     * Sets the current web browser window size.
     * @return this browser instance
     */
    public Browser setSize(int width, int height) {
        assertBrowserOpened();
        Dimension size = new Dimension(width, height);
        this.webDriver.manage().window().setSize(size);
        return this;
    }

    /**
     * Maximizes the current web browser window size if it is not already maximized.
     * @return this browser instance
     */
    public Browser maximize() {
        assertBrowserOpened();
        this.webDriver.manage().window().maximize();
        return this;
    }


    /**
     * Causes the opened web browser to navigate to the specified URL.
     *
     * @param  url  URL to navigate to
     * @return this browser instance
     * @throws NullPointerException if no web browser is currently opened
     */
    public Browser navigateTo(String url) {
        assertBrowserOpened();
        this.webDriver.get(url);
        return this;
    }

    /**
     * Causes the opened web browser to navigate to the previous screen in the browser history.
     *
     * @return this browser instance
     * @throws NullPointerException if no web browser is currently opened
     */
    public Browser navigateBack() {
        assertBrowserOpened();
        this.webDriver.navigate().back();
        return this;
    }


    /**
     * Returns the title of the web browser.
     */
    public String getTitle() {
        assertBrowserOpened();
        return this.webDriver.getTitle();
    }

    /**
     * Returns the title text of all windows of the currently opened web browser instance.
     */
    public List<String> getWindowTitles() {
        assertBrowserOpened();
        Set<String> windowHandles = this.webDriver.getWindowHandles();
        List<String> windowTitles = new ArrayList<String>(windowHandles.size());
        WebDriver.TargetLocator switchTo = this.webDriver.switchTo();
        for (String handle : windowHandles) {
           String title = switchTo.window(handle).getTitle();
           windowTitles.add(title);
        }
        return windowTitles;
    }


    ////// Implements the SearchContext interface.  //////
    
    public WebElement findElement(By by) {
        assertBrowserOpened();
        return this.webDriver.findElement(by);
    }

    public List<WebElement> findElements(By by) {
        assertBrowserOpened();
        return this.webDriver.findElements(by);
    }

    ////// Implements the JavascriptExecutor interface.  //////
    
    public Object executeScript(String script, Object... args) {
        assertBrowserOpened();
        return ((JavascriptExecutor)this.webDriver).executeScript(script, args);
    }

    public Object executeAsyncScript(String script, Object... args) {
        assertBrowserOpened();
        return ((JavascriptExecutor)this.webDriver).executeAsyncScript(script, args);
    }


    /**
     * Repeatedly calling the specified expected condition function until either it returns
     * a non-null value or the specified timeout expires.
     * <p>
     * Example:
     * <pre>
     *     // Wait until the "username" text field is visible or timeout. 
     *     WebElement e = browser.waitUntil(
     *         ExpectedConditions.visibleOfElementLocated(By.name("username")),
     *         timeOutInSeconds);
     * </pre>
     * @param  timeOutInSeconds  the timeout in seconds when an expectation is called
     * @return the non-null value returned by the specified expected condition function
     * @throws TimeoutException if the specified expected condition function still returns <code>null</code>
     *         when the specified timeout is reached
     * @see WebDriverWait
     */
    public <V> V waitUntil(Function<? super WebDriver, V> expectedCondition, int timeOutInSeconds) {
        assertBrowserOpened();
        return new WebDriverWait(this.webDriver, timeOutInSeconds)
            .until(expectedCondition);
    }

    /**
     * Repeatedly calling the specified expected condition function until either it returns
     * a non-null value or the specified timeout expires.
     * <p>
     * Example:
     * <pre>
     *     // Wait until the "username" text field is visible or timeout. 
     *     WebElement e = browser.waitUntil(
     *         ExpectedConditions.visibleOfElementLocated(By.name("username")),
     *         timeOutInSeconds);
     * </pre>
     * @param  timeOutInSeconds  the timeout in seconds when an expectation is called
     * @param  timeOutMessage    the message to be included in the TimeoutException if it is thrown
     * @return the non-null value returned by the specified expected condition function
     * @throws TimeoutException if the specified expected condition function still returns <code>null</code>
     *         when the specified timeout is reached
     * @see WebDriverWait
     */
    public <V> V waitUntil(Function<? super WebDriver, V> expectedCondition, int timeOutInSeconds, String timeOutMessage) {
        assertBrowserOpened();
        return new WebDriverWait(this.webDriver, timeOutInSeconds)
            .withMessage(timeOutMessage)
            .until(expectedCondition);
    }

    /**
     * Sleeps for the specified number of seconds.
     * <p>
     * <b>WARNING:</b> Use of this method is NOT recommended because it will introduced
     * instability in your web UI automation. Consider using {@link #waitUntil(Function,int)} instead.  
     *
     * @param numberOfUnits  number of time units to sleep
     * @param timeUnit       time unit
     * @return this Browser instance
     */
    public Browser sleep(int numberOfUnits, TimeUnit timeUnit) {
        try {
            timeUnit.sleep(numberOfUnits);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        }
        return this;
    }

}
