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
    ///       LoginPage LoginPage = new LoginPage();
    ///       MainPage MainPage = new MainPage();
    /// 
    ///       public MyApp() {
    ///           super("http://mywebapp");
    ///       }
    ///
    ///       public bool LogIn(string username, string password) {
    ///           if (LoginPage.BecomeAvailable()) {
    ///               LoginPage.LogIn(username, password);
    ///               if (MainPage.BecomeAvailable()) {
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
    ///
    ///       public void DoSomething() {
    ///           ...
    ///       }
    ///   }
    ///
    ///   // See the <seealso cref="BasePage"/> documentation for the LoginPage class example.
    ///
    ///   using (MyApp app = new MyApp()) {
    ///       // Launch a web browser and navigate to the home page of MyApp.
    ///       app.Launch(BrowserType.IE);
    ///       if (app.LogIn("username", "password")) {
    ///           app.DoSomething();
    ///           ....
    ///       }
    ///       // The web browser will be automatically closed after this line.
    ///   }
    /// </pre> 
    /// </summary>
    public class BaseApp : IDisposable
    {

        // Private fields that can only be set by constructors.
        readonly string homeUrl;

        protected BaseApp(string url)
        {
            this.homeUrl = url;
        }

        /// <summary>
        /// Returns the name of this web application.
        /// It is default to the simple class name of a derived class.
        /// </summary>
        /// <returns> the name of this web application </returns>
        public virtual string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Returns the <seealso cref="Browser"/> instance used by this web app.
        /// It is default to <seealso cref="WebUIGlobals.DefaultBrowser"/> when this web app instance is constructed.
        /// </summary>
        /// <returns> the <seealso cref="Browser"/> instance used by this web application </returns>
        public virtual Browser Browser { get; set; } = WebUIGlobals.DefaultBrowser;

        /// <summary>
        /// Returns the home URL of this web application.
        /// </summary>
        /// <returns> the home URL of this web application </returns>
        public virtual string HomeUrl { get { return this.homeUrl; } }

        /// <summary>
        /// Opens a new web browser instance/window and navigates to the home page of this web application.
        /// If the web browser is already opened, it will reuse the currently opened web browser instance.
        /// </summary>
        /// <exception cref="ArgumentException"> if the given browser type is invalid or is not supported </exception>
        /// <exception cref="WebDriverException"> if it cannot opens a new browser instance or cannot communicate with the browser instance </exception>
        public virtual void Launch(BrowserType browserType)
        {
            if (!Browser.Opened)
            {
                Browser.Launch(browserType);
            }
            Browser.NavigateTo(HomeUrl);
        }

        public virtual void Dispose()
        {
            Browser.Dispose();
        }

    }

}