package examples.basic;

import static java.util.concurrent.TimeUnit.SECONDS;
import static webui.automation.framework.BrowserType.CHROME;

import org.openqa.selenium.By;

import webui.automation.elements.TextField;
import webui.automation.framework.Browser;
import webui.automation.framework.WebUI;

/**
 *
 */
public class TextFieldExample 
{
    public static void main( String[] args )
    {
        String url = "http://google.com";
        Browser browser = WebUI.getDefaultBrowser();
        int timeOut = WebUI.defaultPageLoadingTimeout;  // in seconds

        browser.open(CHROME).navigateTo(url);
        try {
            TextField textField = new TextField(By.name("q"));
            textField.waitUntilVisible(timeOut);
            textField.setText("current movies").submit();
            browser.sleep(3, SECONDS);
        } finally {
            browser.close();
        }
    }
}
