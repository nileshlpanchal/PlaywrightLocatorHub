# Playwright Test Automation Framework

## Overview

This is a C# test automation framework built with Playwright, NUnit, and SpecFlow. The framework provides a robust foundation for web application testing with support for both BDD (Behavior-Driven Development) using SpecFlow features and traditional NUnit test classes. It emphasizes reusable components, comprehensive logging, and configurable test execution.

## System Architecture

The framework follows a layered architecture pattern with clear separation of concerns:

**Test Layer**: Contains NUnit test classes and SpecFlow feature files
**Page Object Layer**: Implements the Page Object Model pattern for maintainable UI interactions
**Element Layer**: Provides reusable locator strategies and interaction methods
**Utility Layer**: Handles configuration management, logging, and wait strategies
**Data Layer**: Manages test data through JSON configuration and HTML test files

The architecture promotes code reusability and maintainability by separating element location logic from interaction logic, and both from test logic.

## Key Components

### Page Objects
- **BasePage**: Abstract base class providing common functionality for all page objects
- **TestPage**: Concrete implementation demonstrating framework usage with sample HTML elements
- **ElementLocators**: Centralized locator strategies for different HTML element types
- **ElementInteractions**: Reusable interaction methods for web elements
- **AxeAccessibilityTester**: Comprehensive accessibility testing using Axe-core engine with detailed reporting

### Test Infrastructure
- **NUnit Tests**: Traditional unit test approach in `ElementInteractionTests` and `AccessibilityTests`
- **SpecFlow BDD**: Behavior-driven tests in `ElementInteraction.feature` and `AccessibilityTesting.feature`
- **Test Hooks**: Manages browser lifecycle and test setup/teardown
- **Step Definitions**: Implements SpecFlow step bindings for BDD scenarios including accessibility testing
- **Accessibility Testing**: Integrated Axe-core engine for automated accessibility compliance validation

### Utilities
- **ConfigReader**: Manages application configuration from `appsettings.json`
- **Logger**: Provides structured logging using Serilog with console and file output
- **WaitHelpers**: Handles explicit waits and synchronization strategies

## Data Flow

1. **Test Execution**: Tests are executed through either NUnit test runner or SpecFlow scenarios
2. **Configuration Loading**: Test settings are loaded from `appsettings.json` during initialization
3. **Browser Management**: Playwright browser instances are created based on configuration
4. **Page Navigation**: Tests navigate to target pages using configurable base URLs
5. **Element Interaction**: Tests interact with elements through the locator and interaction layers
6. **Assertion & Verification**: Results are validated using NUnit assertions
7. **Cleanup**: Browser resources are disposed and screenshots/logs are captured for failures

## External Dependencies

### Core Testing Framework
- **Microsoft.Playwright**: Browser automation engine
- **NUnit**: Testing framework for assertions and test management
- **SpecFlow**: BDD framework for Gherkin feature files
- **Deque.AxeCore.Playwright**: Accessibility testing engine for WCAG compliance validation

### Configuration & Logging
- **Microsoft.Extensions.Configuration**: Configuration management
- **Serilog**: Structured logging with multiple sinks
- **Newtonsoft.Json**: JSON serialization for test data

### Browser Support
- Chromium (default)
- Firefox
- WebKit (Safari)

## Deployment Strategy

The framework is designed for local development and CI/CD integration:

**Local Development**: 
- Tests run through `dotnet test` command
- Browser can be configured as headed or headless
- Screenshots and videos captured for debugging

**CI/CD Integration**:
- Framework supports headless execution for pipeline environments
- Configurable timeouts and retry mechanisms
- Structured logging for build artifact analysis
- Test results exported in standard formats

**Replit Environment**:
- Configured to run in C# module environment
- Automated workflow runs restore, build, and test commands
- Console logging enabled for immediate feedback

## Changelog

```
Changelog:
- August 12, 2025. Integrated Axe accessibility testing - Added AxeAccessibilityTester class, accessibility step definitions, SpecFlow feature, and NUnit tests for automated WCAG compliance validation and detailed HTML reporting
- June 27, 2025. Initial setup
```

## User Preferences

```
Preferred communication style: Simple, everyday language.
```