namespace WebUI.Automation.Framework
{

    public class WebUIGlobals
    {

        private static Browser s_defaultBrowser;
        private static int s_defaultImplicitWaitTimeout = 3;  // seconds
        private static int s_defaultPageLoadingTimeout = 30;  // seconds

        public static Browser DefaultBrowser
        {
            get
            {
                if (s_defaultBrowser == null)
                {
                    s_defaultBrowser = new Browser();
                }
                return s_defaultBrowser;
            }
            set
            {
                s_defaultBrowser = value;
            }
        }

        /// <summary>
        /// The default implicit waiting timeout used by UI elements
        /// when no explicit timeout is specified (as an argument to a method call).
        /// The default timeout value is 3 (seconds).
        /// <para>
        /// Note that this implicit wait timeout has nothing to do with the Selenium
        /// implicit wait. The UI elements are smart in that they will periodically
        /// (default to every half second) check the availability of themselves. 
        /// </para>
        /// </summary>
        public static int DefaultImplicitWaitTimeout
        {
            get { return s_defaultImplicitWaitTimeout; }
            set { s_defaultImplicitWaitTimeout = value; }
        }

        /// <summary>
        /// The default page loading timeout used by UI pages when the
        /// <seealso cref="BasePage#waitUntilAvailable"/> method is called without a timeout argument.
        /// The default timeout value is 30 (seconds).
        /// </summary>
        public static int DefaultPageLoadingTimeout
        {
            get { return s_defaultPageLoadingTimeout; }
            set { s_defaultPageLoadingTimeout = value; }
        }

    }

}
