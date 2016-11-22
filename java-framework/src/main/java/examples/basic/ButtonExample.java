package examples.basic;

import static webui.automation.framework.BrowserType.CHROME;

import org.openqa.selenium.By;

import webui.automation.elements.Button;
import webui.automation.framework.Browser;
import webui.automation.framework.WebUI;

/**
 * Hello world!
 *
 */
public class ButtonExample 
{
    public static void main( String[] args )
    {
        String url = "http://google.com";
        Browser browser = WebUI.getDefaultBrowser();
        int timeOut = WebUI.defaultPageLoadingTimeout;  // in seconds

        browser.open(CHROME).navigateTo(url);
        try {
            Button button = new Button(By.id("id"));
            button.waitUntilVisible(timeOut);
            button.click();

            System.out.println( "Hello World!" );
        } finally {
            browser.close();
        }
    }
}
