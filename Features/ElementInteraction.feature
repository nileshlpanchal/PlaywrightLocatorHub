Feature: Element Interaction Testing
    As a test automation engineer
    I want to test different HTML element interactions
    So that I can verify the framework's reusable locator methods work correctly

Background:
    Given I navigate to the test page

Scenario: Text Input Interactions
    When I enter "John" in the textbox with id "firstName"
    And I enter "Doe" in the textbox with id "lastName"
    And I enter "john.doe@example.com" in the textbox with placeholder "Enter your email"
    Then the textbox with id "firstName" should contain "John"
    And the textbox with id "lastName" should contain "Doe"

Scenario: Radio Button Interactions
    When I select the radio button with value "male"
    Then the radio button with value "male" should be selected
    When I select the radio button with label "Female"
    Then the radio button with value "female" should be selected

Scenario: Checkbox Interactions
    When I check the checkbox with id "sports"
    And I check the checkbox with id "music"
    Then the checkbox with id "sports" should be checked
    And the checkbox with id "music" should be checked
    When I uncheck the checkbox with id "sports"
    Then the checkbox with id "sports" should not be checked
    And the checkbox with id "music" should be checked

Scenario: Dropdown Interactions
    When I select "United States" from the dropdown with id "country"
    Then the dropdown with id "country" should have "United States" selected
    When I select option with value "canada" from the dropdown with id "country"
    Then the dropdown with id "country" should have "Canada" selected

Scenario: Complete Form Submission
    When I fill the user registration form with:
        | FirstName | LastName | Email                | Password    |
        | Alice     | Smith    | alice.smith@test.com | SecurePass1 |
    And I select "Female" as gender
    And I select interests: Sports, Music, Reading
    And I select "Canada" from the dropdown with id "country"
    And I enter "Toronto" in the textbox with id "city"
    And I click the button with id "submitBtn"
    Then the form should be submitted successfully
    And I should see the success message "Registration completed successfully"

Scenario: Button Click Interactions
    When I click the button with text "Reset Form"
    Then the textbox with id "firstName" should contain ""
    And the textbox with id "lastName" should contain ""

Scenario Outline: Multiple Text Input Scenarios
    When I enter "<text>" in the textbox with id "<elementId>"
    Then the textbox with id "<elementId>" should contain "<text>"

    Examples:
        | text        | elementId |
        | Test User   | firstName |
        | Admin       | lastName  |
        | 123456789   | phone     |

Scenario: Element Visibility and Interaction
    When I click the button with text "Show Advanced Options"
    And I wait for 2 seconds
    When I enter "Advanced Settings" in the textbox with id "advancedField"
    Then the textbox with id "advancedField" should contain "Advanced Settings"
