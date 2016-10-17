# Web UI Automation

Web UI Automation is a layered, extensible UI automation framework based on [Selenium](http://www.seleniumhq.org/) and Java. It strives to provide layers of UI abstractions so that test writers can focus on UI interactions instead of dealing with the nitty-gritty details of the Selenium framework.

## Overview

The Web UI Automation helps implement UI automation by providing a framework that consists of three layers:

* Browser layer: This layer provides a persistent reference and simplified usage to interact with the web browser currently in use. It wraps around the Selenium WebDriver interface and various web browser drivers for easier usage such as opening or selecting any browser by a simple method call (instead of creating and destroying a WebDriver instance). Note: The other two layers use this layer to reference to the browser even before the browser is opened and when the browser is changed (e.g., changing the browser from Chrome to Firefox at runtime for testing purpose.
* UI element layer: This layer provides an abstraction to many basic UI elements (e.g., buttons, text fields, etc.). By using the abstraction (i.e., different object types for different UI element types), a test writer can directly deal with the basic UI element behavior in an object-oriented manner without knowing their underlying Selenium implementation.
* Application page layer: This layer is usually custom-built to each particular application UI using the page model design pattern (e.g., a login page, a landing page, etc.). The Web UI Automation provides a base class and helper methods to support the implementation of custom-built pages.
