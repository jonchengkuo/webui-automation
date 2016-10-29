package webui.automation.browser;

import static java.util.concurrent.TimeUnit.SECONDS;
import static org.junit.Assert.*;

import java.util.ArrayList;
import java.util.Collection;

import org.junit.After;
import org.junit.Before;
import org.junit.FixMethodOrder;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.TestName;
import org.junit.runner.RunWith;
import org.junit.runners.MethodSorters;
import org.junit.runners.Parameterized;
import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import webui.automation.browser.Browser;
import webui.automation.browser.BrowserType;

@RunWith(Parameterized.class)
@FixMethodOrder(MethodSorters.NAME_ASCENDING)
public class BrowserTest {

    @Parameterized.Parameters(name = " running with {0} browser")
    public static Collection<Object[]> getParameters() {
        ArrayList<Object[]> parameters = new ArrayList<Object[]>();
        //parameters.add(new Object[]{BrowserType.CHROME});
        parameters.add(new Object[]{BrowserType.FIREFOX});
        return parameters;
    }

    // Rule to get the currently running test method name.
    @Rule
    public TestName testName = new TestName();

    private BrowserType browserType;
    private Browser browser;

    public BrowserTest(BrowserType browserType) {
        this.browserType = browserType;
    }

    @Before
    public void setUp() throws Exception {
        // Open the web browser if this test is not "testOpenClose",
        // which uses its own Browser instance.
        if (!testName.getMethodName().equals("testOpenClose")) {
            this.browser = new Browser();
            this.browser.open(this.browserType);
        }
    }

    @After
    public void tearDown() throws Exception {
        if (this.browser != null) {
            this.browser.close();
        }
    }

    @Test
    public void tc01_TestOpenClose() {

        Browser browser = new Browser();

        assertEquals("The initial browser type should be null but it was not!",
            null, browser.getBrowserType());
        assertEquals("The initial WebDriver reference should be null but it was not!",
            null, browser.getWebDriver());

        browser.open(this.browserType);

        try {
            assertEquals("After opening a web browser, the current browser type should match but it did not!",
                this.browserType, browser.getBrowserType());
            assertNotNull("After opening a web browser, the WebDriver reference should not be null but it was!",
                browser.getWebDriver());
        } finally {
            // Always close the web browser as long as the open has succeeded.
            browser.close();

            assertEquals("After closing the web browser, the current browser type should be null but it was not!",
                null, browser.getBrowserType());
            assertEquals("After closing the web browser, the WebDriver reference should be null but it was not!",
                null, browser.getWebDriver());

            // Closing more than once should be allowed.
            browser.close();
        }
    }

    @Test
    public void tc02_TestBasicNavigationAndFind() {
        browser.navigateTo("http://www.google.com");
        WebElement searchBox = browser.findElement(By.name("q"));
        searchBox.sendKeys("Selenium");
        searchBox.submit();
        browser.sleep(3, SECONDS);
        assertEquals("Selenium - Google Search", browser.getTitle());
    }

}
