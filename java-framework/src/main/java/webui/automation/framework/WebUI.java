package webui.automation.framework;

import webui.automation.browser.Browser;

public class WebUI {

    private static Browser defaultBrowser = new Browser();

    public static Browser getDefaultBrowser() {
        return defaultBrowser;
    }

    public static void setDefaultBrowser(Browser browser) {
        defaultBrowser = browser;
    }

    /**
     * The default implicit waiting timeout used by UI elements
     * when no explicit timeout is specified (as an argument to a method call).
     * The default timeout value is 3 (seconds).
     * <p>
     * Note that this implicit wait timeout has nothing to do with the Selenium
     * implicit wait. The UI elements are smart in that they will periodically
     * (default to every half second) check the availability of themselves. 
     */
    public static int defaultImplicitWaitTimeout = 3;   // seconds

    /**
     * The default page loading timeout used by UI pages when the
     * {@link BasePage#waitUntilAvailable} method is called without a timeout argument.
     * The default timeout value is 30 (seconds).
     */
    public static int defaultPageLoadingTimeout = 30;   // seconds

}
