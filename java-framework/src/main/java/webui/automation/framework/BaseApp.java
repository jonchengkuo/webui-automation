package webui.automation.framework;

import org.openqa.selenium.WebDriverException;

/**
 * Base class for representing and interacting with a web application using the page object model.
 * A subclass should be defined for each specific web application
 * to simulate its application-specific UI behaviors (e.g., logging in).
 *
 * <p><b>Example:</b></p>
 * <pre>
 *   import webui.automation.browser.BrowserType;
 *   import webui.automation.framework.BaseApp;
 *
 *   public class MyApp extends BaseApp {
 *       final LoginPage loginPage = new LoginPage();
 *       final MainPage mainPage = new MainPage();
 *
 *       public MyApp(BrowserType browserType) {
 *           super(browserType, "http://mywebapp");
 *       }
 *
 *       public boolean login(String user, String password) {
 *           if (this.loginPage.isAvailable()) {
 *               this.loginPage.login(user, password);
 *               if (this.mainPage.isAvailable()) {
 *                   return true;
 *               } else {
 *                   // Error: Login failed.
 *                   return false;
 *               }
 *           } else {
 *               // Error: Login page not available.
 *               return false;
 *           }
 *       }
 *   }
 *
 *   try (MyApp app = new MyApp(BrowserType.IE)) {
 *       app.open();
 *       if (app.login("user", "password")) {
 *           app.mainPage.doSomething();
 *           ....
 *       }
 *       // The browser will be automatically closed after this line.
 *   }
 * </pre> 
 */
public class BaseApp implements AutoCloseable {

    private Browser browser = WebUI.getDefaultBrowser();
    private BrowserType browserType;

    private String url;

    protected BaseApp(BrowserType browserType, String url) {
        this.browserType = browserType;
        this.url = url;
    }

    public String getName() {
        return this.getClass().getSimpleName();
    }

    public String getURL() {
        return this.url;
    }

    /**
     * Returns the {@link Browser} instance used by this web application.
     * It is default to the {@link Browser} instance returned by {@link WebUI#getDefaultBrowser}
     * when this page is created.
     * @return the {@link Browser} instance used by this web application
     */
    public Browser getBrowser() {
        return this.browser;
    }

    /**
     * Opens a new browser instance/window and navigates to the web application home page.
     *
     * @throws WebDriverException if it cannot opens a new browser instance or cannot communicate with the browser instance
     * @throws RuntimeException if this web application is already opened
     */
    public void open() {
        browser.open(this.browserType);
        browser.navigateTo(this.url);
    }

    public void close() {
        browser.close();
    }

}
