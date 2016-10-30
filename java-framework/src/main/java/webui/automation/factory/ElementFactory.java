package webui.automation.factory;

import org.openqa.selenium.By;
//import org.openqa.selenium.WebElement;

import webui.automation.elements.*;

/**
 * Factory class for creating UI elements.
 * It defines static factory methods for creating different types of UI elements
 * (e.g., button, text, etc.).
 *
 * <P>This class is designed to provide customization of UI elements.
 * In automating the UI of a particular web application, you may choose to use the factory methods
 * provided by a factory class instead of directly calling the constructor of each UI element class.
 * The advantage of doing so is that you can easily switch or customize UI element classes by
 * customizing the factory class instead of changing your UI automation code.</P>
 *
 * <P>You can customize the factory class for your particular application by telling it to use
 * a {@link ElementFlavorType commonly used UI flavor} or extend this factory class and override
 * its factory methods. When a factory method creates a UI element, it may use the flavor to
 * parameter a created UI element or to use a different UI element class instead of the basic
 * UI element class provided by this framework.</P> 
 */
public class ElementFactory {

    /**
     * The flavor to be considered when a factory method creates a UI element.
     */
    protected static ElementFlavorType elementFlavor = ElementFlavorType.BASIC_HTML;

    /**
     * Sets the flavor to be considered when a factory method creates a UI element.
     * @param newFlavor  a new UI element flavor to be used
     */
    public static void setElementFlavor(ElementFlavorType newFlavor) {
        elementFlavor = newFlavor;
    }

    /**
     * Creates an object to represent a button on a web page.
     * @param  locator  The {@link By} locator for locating the button on a web page.
     */
    public static Button createButton(By locator) {
        return new Button(locator);
    }

    /**
     * Creates an object to represent a check box on a web page.
     * @param  locator  The {@link By} locator for locating the check box on a web page.
     */
    public static CheckBox createCheckBox(By locator) {
        return new CheckBox(locator);
    }

    /**
     * Creates an object to represent a radio button on a web page.
     * @param  locator  The {@link By} locator for locating the radio button on a web page.
     */

    public static RadioButton createRadioButton(By locator) {
        return new RadioButton(locator);
    }

    /**
     * Creates an object to represent a table on a web page.
     * @param  locator  The {@link By} locator for locating the table on a web page.
     */

    public static Table createTable(By locator) {
        return new Table(locator);
    }

    /**
     * Creates an object to represent a text on a web page.
     * @param  locator  The {@link By} locator for locating the text on a web page.
     */

    public static Text createText(By locator) {
        return new Text(locator);
    }

    /**
     * Creates an object to represent a text field on a web page.
     * @param  locator  The {@link By} locator for locating the text field on a web page.
     */

    public static TextField createTextField(By locator) {
        return new TextField(locator);
    }

    /**
     * Creates an object to represent a text link on a web page.
     * @param  locator  The {@link By} locator for locating the text link on a web page.
     */

    public static TextLink createTextLink(By locator) {
        return new TextLink(locator);
    }

}
