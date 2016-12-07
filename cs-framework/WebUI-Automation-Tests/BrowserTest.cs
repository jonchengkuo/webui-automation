using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;  // ExpectedConditions
using WebUI.Automation.Framework;

namespace WebUI.Automation.Tests
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
        public void TC01_TestLaunchDispose()
        {

            Browser browser = new Browser();

            Assert.IsFalse(browser.Opened, "The initial Browser state should be non-opened but it was not!");
            Assert.AreEqual(BrowserType.None, browser.BrowserType, "The initial browser type should be None but it was not!");
            //Assert.Throws(Exception, browser.WebDriver, "When the browser state is non-opened, getting the WebDriver reference should result in an exception but it did not!");

            try
            {
                // Launch a web browser instance.
                browser.Launch(this.browserType);
                Assert.IsTrue(browser.Opened, "After successfully launching a web browser instance, the Browser state should be opened but it was not!");
                Assert.AreEqual(this.browserType, browser.BrowserType, "After opening a web browser, the current browser type should match but it did not!");
                Assert.IsNotNull(browser.WebDriver, "After opening a web browser, the WebDriver reference should not be null but it was!");
            }
            finally
            {
                // Always dispose the browser no matter whether it was successfully launched or not.
                browser.Dispose();
            }

            Assert.IsFalse(browser.Opened, "After disposing (i.e., closing the opened web browser instance) the Browser state should be non-opened but it was not!");
            Assert.AreEqual(BrowserType.None, browser.BrowserType, "After disposing (i.e., closing the opened web browser instance), the current browser type should be null but it was not!");
            //Assert.Throws(Exception, browser.WebDriver, "After disposing (i.e., closing the opened web browser instance), getting the WebDriver reference should result in an exception but it did not!");

            // Disposing more than once should be allowed (no exception).
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