package examples.basic;

import static java.util.concurrent.TimeUnit.SECONDS;
import static webui.automation.browser.BrowserType.FIREFOX;

import org.openqa.selenium.By;
import org.openqa.selenium.WebElement;

import webui.automation.browser.Browser;

/**
 * This example tests the {@link Browser} class.
 */
public class BrowserExample 
{
    public static void main( String[] args )
    {
        String url = "http://google.com";

        Browser browser = new Browser();
        browser.open(FIREFOX).navigateTo(url);
        try {
            WebElement searchBox = browser.findElement(By.name("q"));
            searchBox.sendKeys("current movies");
            searchBox.submit();
            browser.sleep(3, SECONDS);
        } finally {
            browser.close();
        }
    }
}
