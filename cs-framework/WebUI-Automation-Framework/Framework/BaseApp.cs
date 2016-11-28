using System;
using OpenQA.Selenium;

namespace WebUI.Automation.Framework
{

    /// <summary>
    /// Base class for representing and interacting with a web application using the page object model.
    /// A subclass should be defined for each specific web application
    /// to simulate its application-specific UI behaviors (e.g., logging in).
    /// 
    /// <para><b>Example:</b></para>
    /// <pre>
    ///   using OpenQA.Selenium;
    ///   using WebUI.Automation.Elements;
    ///   using WebUI.Automation.Framework;
    ///   using static WebUI.Automation.Elements.ElementFactory;
    ///
    ///   public class MyApp : BaseApp {
    ///       final LoginPage LoginPage = new LoginPage();
    ///       final MainPage MainPage = new MainPage();
    /// 
    ///       public MyApp(BrowserType browserType) {
    ///           super(browserType, "http://mywebapp");
    ///       }
    ///
    ///       public bool LogIn(string user, string password) {
    ///           if (this.LoginPage.IsAvailable()) {
    ///               this.LoginPage.LogIn(user, password);
    ///               if (this.MainPage.IsAvailable()) {
    ///                   return true;
    ///               } else {
    ///                   // Error: Login failed.
    ///                   return false;
    ///               }
    ///           } else {
    ///               // Error: Login page not available.
    ///               return false;
    ///           }
    ///       }
    ///   }
    ///
    ///   public class LoginPage : BasePage<LoginPage> {
    ///       TextField UserNameTextField = CreateTextField(By.id("username"));
    ///       TextField PasswordTextField = CreateTextField(By.id("password"));
    ///
    ///       public void LogIn(string user, string password) {
    ///           ...
    ///       }
    ///   }
    ///
    ///   try (MyApp app = new MyApp(BrowserType.IE)) {
    ///       app.Launch();
    ///       if (app.LogIn("user", "password")) {
    ///           app.MainPage.doSomething();
    ///           ....
    ///       }
    ///       // The browser will be automatically closed after this line.
    ///   }
    /// </pre> 
    /// </summary>
    public class BaseApp : IDisposable
    {

        private Browser browser;
        private BrowserType browserType;
        private string url;

        protected internal BaseApp(BrowserType browserType, string url)
        {
            this.browserType = browserType;
            this.url = url;
        }

        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Returns the <seealso cref="Browser"/> instance used by this web application.
        /// It is default to the <seealso cref="Browser"/> instance returned by <seealso cref="WebUIGlobals.DefaultBrowser"/>
        /// when this page is created. </summary>
        /// <returns> the <seealso cref="Browser"/> instance used by this web application </returns>
        public Browser Browser
        {
            get
            {
                if (this.browser == null)
                {
                    this.browser = WebUIGlobals.DefaultBrowser;
                }
                return this.browser;
            }
        }

        public virtual string Url
        {
            get
            {
                return this.url;
            }
        }

        /// <summary>
        /// Opens a new browser instance/window and navigates to the web application home page.
        /// </summary>
        /// <exception cref="WebDriverException"> if it cannot opens a new browser instance or cannot communicate with the browser instance </exception>
        /// <exception cref="Exception"> if this web application is already opened </exception>
        public virtual void Launch()
        {
            browser.Launch(this.browserType);
            browser.NavigateTo(this.url);
        }

        public virtual void Dispose()
        {
            browser.Dispose();
        }

    }

}