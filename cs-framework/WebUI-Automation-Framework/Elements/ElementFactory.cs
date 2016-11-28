using OpenQA.Selenium;
using WebUI.Automation.Framework;

namespace WebUI.Automation.Elements
{

    /// <summary>
    /// Factory class for creating UI elements.
    /// It defines static factory methods for creating different types of UI elements
    /// (e.g., button, text, etc.).
    /// 
    /// <P>This class is designed to provide customization of UI elements.
    /// In automating the UI of a particular web application, you may choose to use the factory methods
    /// provided by a factory class instead of directly calling the constructor of each UI element class.
    /// The advantage of doing so is that you can easily switch or customize UI element classes by
    /// customizing the factory class instead of changing your UI automation code.</P>
    /// 
    /// <P>You can customize the factory class for your particular application by telling it to use
    /// a <seealso cref="ElementFlavorType commonly used UI flavor"/> or extend this factory class and override
    /// its factory methods. When a factory method creates a UI element, it may use the flavor to
    /// parameter a created UI element or to use a different UI element class instead of the basic
    /// UI element class provided by this framework.</P> 
    ///
    /// <para><b>Example:</b></para>
    /// <pre>
    ///   using OpenQA.Selenium;
    ///   using WebUI.Automation.Elements;
    ///   using static WebUI.Automation.Elements.ElementFactory;
    ///
    ///   TextField UserNameTextField = CreateTextField(By.id("username"));
    ///   TextField PasswordTextField = CreateTextField(By.id("password"));
    /// </pre>
    /// </summary>
    public class ElementFactory
    {

        /// <summary>
        /// The flavor to be considered when a factory method creates a UI element.
        /// </summary>
        protected internal static ElementFlavorType elementFlavor = ElementFlavorType.BasicHtml;

        /// <summary>
        /// Sets the flavor to be considered when a factory method creates a UI element. </summary>
        /// <param name="newFlavor">  a new UI element flavor to be used </param>
        public static ElementFlavorType ElementFlavor
        {
            set
            {
                elementFlavor = value;
            }
        }

        /// <summary>
        /// Creates an object to represent a button on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the button on a web page. </param>
        /// <returns> the created <seealso cref="Button"/> element </returns>
        public static Button CreateButton(By locator)
        {
            return new Button(locator);
        }

        /// <summary>
        /// Creates an object to represent a check box on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the check box on a web page. </param>
        /// <returns> the created <seealso cref="CheckBox"/> element </returns>
        public static CheckBox CreateCheckBox(By locator)
        {
            return new CheckBox(locator);
        }

        /// <summary>
        /// Creates an object to represent a radio button on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the radio button on a web page. </param>
        /// <returns> the created <seealso cref="RadioButton"/> element </returns>
        public static RadioButton CreateRadioButton(By locator)
        {
            return new RadioButton(locator);
        }

        /// <summary>
        /// Creates an object to represent a table on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the table on a web page. </param>
        /// <returns> the created <seealso cref="Table"/> element </returns>
        public static Table CreateTable(By locator)
        {
            return new Table(locator);
        }

        /// <summary>
        /// Creates an object to represent a text element on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the text on a web page. </param>
        /// <returns> the created <seealso cref="TextElement"/> </returns>
        public static TextElement CreateTextElement(By locator)
        {
            return new TextElement(locator);
        }

        /// <summary>
        /// Creates an object to represent a text field (or called text box) on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the text field on a web page. </param>
        /// <returns> the created <seealso cref="TextField"/> element </returns>
        public static TextField CreateTextField(By locator)
        {
            return new TextField(locator);
        }

        /// <summary>
        /// Creates an object to represent a text link on a web page. </summary>
        /// <param name="locator">  The <seealso cref="By"/> locator for locating the text link on a web page. </param>
        /// <returns> the created <seealso cref="TextLink"/> element </returns>
        public static TextLink CreateTextLink(By locator)
        {
            return new TextLink(locator);
        }

    }

}