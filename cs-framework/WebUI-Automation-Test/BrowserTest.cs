using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions
using WebUI.Automation.Framework;

namespace WebUI.Automation.Test.Framework
{

    [TestClass]
    public class BrowserTest
    {

        private BrowserType browserType;

        public BrowserTest()
        {
            this.browserType = BrowserType.IE;
        }

        [TestMethod]
        public void TC01_TestOpenClose()
        {

            Browser browser = new Browser();
            try
            {
                Assert.AreEqual(BrowserType.NONE, browser.BrowserType, "The initial browser type should be NONE but it was not!");
                //Assert.Throws(Exception, browser.WebDriver, "The initial WebDriver reference should be null but it was not!");

                browser.Launch(this.browserType);

                Assert.AreEqual(this.browserType, browser.BrowserType, "After opening a web browser, the current browser type should match but it did not!");
                Assert.IsNotNull(browser.WebDriver, "After opening a web browser, the WebDriver reference should not be null but it was!");
            }
            finally
            {
                // Explicitly close the web browser as long as the open has succeeded.
                browser.Dispose();
            }
            Assert.AreEqual(BrowserType.NONE, browser.BrowserType, "After closing the web browser, the current browser type should be null but it was not!");
            //Assert.Throws(Exception, browser.WebDriver, "After closing the web browser, the WebDriver reference should be null but it was not!");

            // Closing more than once should be allowed.
            browser.Dispose();
        }

        [TestMethod]
        public void TC02_TestBasicNavigationAndFind()
        {
            using (Browser browser = new Browser())
            {
                browser.Launch(this.browserType).NavigateTo("http://www.google.com");
                IWebElement searchBox = browser.WaitUntil(ExpectedConditions.ElementIsVisible(By.Name("q")), 10);
                searchBox.SendKeys("Selenium");
                searchBox.Submit();
                browser.Sleep(3);
                Assert.AreEqual("Selenium - Google Search", browser.Title);
            }
        }

    }

}