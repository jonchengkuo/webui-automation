using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Collections.ObjectModel;  // ReadOnlyCollection<T>

namespace WebUI.Automation.Framework
{

    /// <summary>
    /// This class implements a special <seealso cref="By"/> locator object that signifies that it is a mocked locator.
    /// A UI element that is given this special locator will not interact with the browser; its methods will
    /// log actions instead. Methods that return a value should return a mocked value.
    /// </summary>
    public sealed class ByTBD : By
    {

        private static readonly ByTBD s_instance = new ByTBD();
        private static readonly ReadOnlyCollection<IWebElement> s_EmptyWebElementList =
            new ReadOnlyCollection<IWebElement>(new List<IWebElement>());

        // Private constructor prohibits outside instantiation.
        // It can be changed to protected if there is really a need for subclassing.
        private ByTBD()
        {
        }

        /// <summary>
        /// The shared, default instance of the <seealso cref="ByTBD"/> class.
        /// It is to be passed to a UI element constructor when the locator of the UI element is not yet properly defined.
        /// 
        /// <para><b>Example:</b></para>
        /// <pre>
        ///   import static webui.automation.framework.ByTBD.ByTBD;
        ///   import static webui.automation.elements.Button;
        ///   ...
        ///   Button button = new Button(ByTBD);
        ///   button.click();
        /// </pre> 
        /// </summary>
        public static ByTBD Instance
        {
            get { return s_instance;  }
        }

        /// <summary>
        /// Returns a special, mocked text string.
        /// It is to be returned by a UI element subclass method that returns a String value when the UI element
        /// is given the special <seealso cref="ByTBD"/> locator.
        /// </summary>
        public static string MockedStringValue
        {
            get
            {
                return "!!MOCKED STRING VALUE!!";
            }
        }

        /// <summary>
        /// Logs a mocked action.
        /// It is to be used by a UI element subclass to log an action instead of performing an actual action. </summary>
        /// <param name="actionMessage">  Message about an action that is supposed to be performed by a UI element </param>
        public static void Log(string actionMessage)
        {
            Console.WriteLine("!!MOCKED ACTION: " + actionMessage + "!!");
        }

        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            return s_EmptyWebElementList;
        }

    }

}