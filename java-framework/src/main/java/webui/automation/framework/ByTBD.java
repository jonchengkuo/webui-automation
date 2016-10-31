package webui.automation.framework;

import java.util.ArrayList;
import java.util.List;

import org.openqa.selenium.By;
import org.openqa.selenium.SearchContext;
import org.openqa.selenium.WebElement;

/**
 * This class implements a special {@link By} locator object that signifies that it is a mocked locator.
 * A UI element that is given this special locator will not interact with the browser; its methods will
 * log actions instead. Methods that return a value should return a mocked value.
 */
public class ByTBD extends By {

    /**
     * The shared, default instance of the {@link ByTBD} class.
     * It is to be passed to a UI element constructor when the locator of the UI element is not yet properly defined.
     *
     * <p><b>Example:</b></p>
     * <pre>
     *   import static webui.automation.framework.ByTBD.ByTBD;
     *   import static webui.automation.elements.Button;
     *   ...
     *   Button button = new Button(ByTBD);
     *   button.click();
     * </pre> 
     */
    public static final ByTBD ByTBD = new ByTBD();

    /**
     * Logs a mocked action.
     * It is to be used by a UI element subclass to log an action instead of performing an actual action.
     * @param actionMessage  Message about an action that is supposed to be performed by a UI element
     */
    public static void log(String actionMessage) {
        System.out.println("!!MOCKED ACTION: " + actionMessage + "!!");
    }

    /**
     * Returns a special, mocked text string.
     * It is to be returned by a UI element subclass method that returns a String value when the UI element
     * is given the special {@link ByTBD} locator.
     */
    public static String getMockedStringValue() {
        return MOCKED_STRING_VALUE;
    }

    private static final String MOCKED_STRING_VALUE = "!!MOCKED STRING VALUE!!";

    private static final List<WebElement> EMPTY_WEBELEMENT_LIST = new ArrayList<WebElement>();


    // Private constructor prohibits outside instantiation.
    // It can be changed to protected if there is really a need for subclassing.
    private ByTBD() {
    }

    @Override
    public List<WebElement> findElements(SearchContext context) {
        return EMPTY_WEBELEMENT_LIST;
    }

}
