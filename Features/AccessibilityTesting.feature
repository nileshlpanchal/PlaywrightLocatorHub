Feature: Accessibility Testing with Axe
    As a QA engineer
    I want to test web page accessibility
    So that I can ensure the application is accessible to all users

Background:
    Given I navigate to the test page
    And I initialize the accessibility tester

Scenario: Full page accessibility scan
    When I run a full accessibility scan
    Then the accessibility scan should have no critical violations
    And the accessibility scan should have less than 5 violations

Scenario: WCAG 2.1 AA compliance check
    When I run accessibility scan with WCAG 2.1 AA standards
    Then the accessibility scan should have no critical violations
    When I generate an accessibility report
    Then the accessibility report should be generated

Scenario: Form elements accessibility validation
    When I run accessibility scan on the "textbox" element with id "firstName"
    Then the accessibility scan should pass
    When I run accessibility scan on the "button" element with id "submitBtn"
    Then the accessibility scan should pass

Scenario: Generate comprehensive accessibility report
    When I run a full accessibility scan
    And I generate an accessibility report
    Then the accessibility report should be generated

Scenario Outline: Individual element accessibility testing
    When I run accessibility scan on the "<elementType>" element with id "<elementId>"
    Then the accessibility scan should have less than 3 violations

    Examples:
    | elementType | elementId   |
    | textbox     | firstName   |
    | textbox     | lastName    |
    | textbox     | email       |
    | dropdown    | country     |
    | button      | submitBtn   |
    | checkbox    | sports      |