using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions
using WebUI.Automation.Framework;

namespace WebUI.Automation.Examples.BrowserExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://google.com";

            using (Browser browser = new Browser())
            {
                browser.Launch(BrowserType.IE).NavigateTo(url);
                IWebElement searchBox = browser.WaitUntil(ExpectedConditions.ElementIsVisible(By.Name("q")), 10);
                searchBox.SendKeys("selenium");
                searchBox.Submit();
                browser.Sleep(5);
            }
        }
    }
}
