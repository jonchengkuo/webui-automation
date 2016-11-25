using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions
using WebUI.Automation.Elements;
using WebUI.Automation.Framework;

namespace TextFieldExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://google.com";
            TextField searchBox = new TextField(By.Name("q"));

            using (Browser browser = searchBox.Browser)
            {
                browser.Launch(BrowserType.IE).NavigateTo(url);
                searchBox.WaitUntilVisible(10);
                searchBox.SetText("selenium");
                searchBox.Submit();
                browser.Sleep(5);
            }
        }
    }
}
