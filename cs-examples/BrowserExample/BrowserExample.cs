namespace Examples.Basic
{

//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//    import static webui.automation.framework.BrowserType.FIREFOX;

    using By = org.openqa.selenium.By;
    using WebElement = org.openqa.selenium.WebElement;

    using Browser = webui.automation.framework.Browser;

    /// <summary>
    /// This example tests the <seealso cref="Browser"/> class.
    /// </summary>
    public class BrowserExample
    {
        public static void Main(string[] args)
        {
            string url = "http://google.com";

            Browser browser = new Browser();
            browser.Open(FIREFOX).NavigateTo(url);
            try
            {
                WebElement searchBox = browser.FindElement(By.Name("q"));
                searchBox.SendKeys("current movies");
                searchBox.Submit();
                browser.Sleep(3, SECONDS);
            }
            finally
            {
                browser.Close();
            }
        }
    }

}