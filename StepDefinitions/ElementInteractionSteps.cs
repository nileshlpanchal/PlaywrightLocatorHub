using TechTalk.SpecFlow;

namespace PlaywrightFramework.StepDefinitions
{
    /// <summary>
    /// Step definitions for element interactions using the reusable locator methods
    /// </summary>
    [Binding]
    public class ElementInteractionSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IPage _page;
        private readonly TestPage _testPage;
        private readonly ElementInteractions _interactions;

        public ElementInteractionSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _page = (IPage)_scenarioContext["Page"];
            _testPage = new TestPage(_page);
            _interactions = new ElementInteractions(_page);
        }

        #region Textbox Steps

        [When(@"I enter ""(.*)"" in the textbox with id ""(.*)""")]
        public async Task WhenIEnterInTheTextboxWithId(string text, string id)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in textbox with id '{id}'");
                await _interactions.EnterTextInTextboxById(id, text);
                Logger.Information("Text entry completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to enter text in textbox {id}: {ex.Message}");
                throw;
            }
        }

        [When(@"I enter ""(.*)"" in the textbox with name ""(.*)""")]
        public async Task WhenIEnterInTheTextboxWithName(string text, string name)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in textbox with name '{name}'");
                await _interactions.EnterTextInTextboxByName(name, text);
                Logger.Information("Text entry completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to enter text in textbox {name}: {ex.Message}");
                throw;
            }
        }

        [When(@"I enter ""(.*)"" in the textbox with placeholder ""(.*)""")]
        public async Task WhenIEnterInTheTextboxWithPlaceholder(string text, string placeholder)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in textbox with placeholder '{placeholder}'");
                await _interactions.EnterTextInTextboxByPlaceholder(placeholder, text);
                Logger.Information("Text entry completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to enter text in textbox with placeholder {placeholder}: {ex.Message}");
                throw;
            }
        }

        [Then(@"the textbox with id ""(.*)"" should contain ""(.*)""")]
        public async Task ThenTheTextboxWithIdShouldContain(string id, string expectedText)
        {
            try
            {
                Logger.Information($"Verifying textbox {id} contains '{expectedText}'");
                var actualText = await _interactions.GetTextFromTextboxById(id);
                Assert.That(actualText, Is.EqualTo(expectedText), 
                    $"Expected textbox to contain '{expectedText}' but got '{actualText}'");
                Logger.Information("Textbox content verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Textbox content verification failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Radio Button Steps

        [When(@"I select the radio button with value ""(.*)""")]
        public async Task WhenISelectTheRadioButtonWithValue(string value)
        {
            try
            {
                Logger.Information($"Selecting radio button with value '{value}'");
                await _interactions.SelectRadioButtonByValue(value);
                Logger.Information("Radio button selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select radio button {value}: {ex.Message}");
                throw;
            }
        }

        [When(@"I select the radio button with name ""(.*)"" and value ""(.*)""")]
        public async Task WhenISelectTheRadioButtonWithNameAndValue(string name, string value)
        {
            try
            {
                Logger.Information($"Selecting radio button with name '{name}' and value '{value}'");
                await _interactions.SelectRadioButtonByNameAndValue(name, value);
                Logger.Information("Radio button selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select radio button {name}={value}: {ex.Message}");
                throw;
            }
        }

        [When(@"I select the radio button with label ""(.*)""")]
        public async Task WhenISelectTheRadioButtonWithLabel(string labelText)
        {
            try
            {
                Logger.Information($"Selecting radio button with label '{labelText}'");
                await _interactions.SelectRadioButtonByLabel(labelText);
                Logger.Information("Radio button selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select radio button with label {labelText}: {ex.Message}");
                throw;
            }
        }

        [Then(@"the radio button with value ""(.*)"" should be selected")]
        public async Task ThenTheRadioButtonWithValueShouldBeSelected(string value)
        {
            try
            {
                Logger.Information($"Verifying radio button {value} is selected");
                var isSelected = await _interactions.IsRadioButtonSelectedByValue(value);
                Assert.That(isSelected, Is.True, $"Expected radio button '{value}' to be selected");
                Logger.Information("Radio button selection verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Radio button selection verification failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Checkbox Steps

        [When(@"I check the checkbox with id ""(.*)""")]
        public async Task WhenICheckTheCheckboxWithId(string id)
        {
            try
            {
                Logger.Information($"Checking checkbox with id '{id}'");
                await _interactions.CheckCheckboxById(id);
                Logger.Information("Checkbox check completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to check checkbox {id}: {ex.Message}");
                throw;
            }
        }

        [When(@"I uncheck the checkbox with id ""(.*)""")]
        public async Task WhenIUncheckTheCheckboxWithId(string id)
        {
            try
            {
                Logger.Information($"Unchecking checkbox with id '{id}'");
                await _interactions.UncheckCheckboxById(id);
                Logger.Information("Checkbox uncheck completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to uncheck checkbox {id}: {ex.Message}");
                throw;
            }
        }

        [When(@"I toggle the checkbox with id ""(.*)""")]
        public async Task WhenIToggleTheCheckboxWithId(string id)
        {
            try
            {
                Logger.Information($"Toggling checkbox with id '{id}'");
                await _interactions.ToggleCheckboxById(id);
                Logger.Information("Checkbox toggle completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to toggle checkbox {id}: {ex.Message}");
                throw;
            }
        }

        [Then(@"the checkbox with id ""(.*)"" should be checked")]
        public async Task ThenTheCheckboxWithIdShouldBeChecked(string id)
        {
            try
            {
                Logger.Information($"Verifying checkbox {id} is checked");
                var isChecked = await _interactions.IsCheckboxCheckedById(id);
                Assert.That(isChecked, Is.True, $"Expected checkbox '{id}' to be checked");
                Logger.Information("Checkbox state verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Checkbox state verification failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"the checkbox with id ""(.*)"" should not be checked")]
        public async Task ThenTheCheckboxWithIdShouldNotBeChecked(string id)
        {
            try
            {
                Logger.Information($"Verifying checkbox {id} is not checked");
                var isChecked = await _interactions.IsCheckboxCheckedById(id);
                Assert.That(isChecked, Is.False, $"Expected checkbox '{id}' to not be checked");
                Logger.Information("Checkbox state verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Checkbox state verification failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Dropdown Steps

        [When(@"I select ""(.*)"" from the dropdown with id ""(.*)""")]
        public async Task WhenISelectFromTheDropdownWithId(string optionText, string id)
        {
            try
            {
                Logger.Information($"Selecting '{optionText}' from dropdown with id '{id}'");
                await _interactions.SelectDropdownByText(id, optionText);
                Logger.Information("Dropdown selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select dropdown option {optionText}: {ex.Message}");
                throw;
            }
        }

        [When(@"I select option with value ""(.*)"" from the dropdown with id ""(.*)""")]
        public async Task WhenISelectOptionWithValueFromTheDropdownWithId(string optionValue, string id)
        {
            try
            {
                Logger.Information($"Selecting option with value '{optionValue}' from dropdown with id '{id}'");
                await _interactions.SelectDropdownByValue(id, optionValue);
                Logger.Information("Dropdown selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select dropdown option by value {optionValue}: {ex.Message}");
                throw;
            }
        }

        [Then(@"the dropdown with id ""(.*)"" should have ""(.*)"" selected")]
        public async Task ThenTheDropdownWithIdShouldHaveSelected(string id, string expectedText)
        {
            try
            {
                Logger.Information($"Verifying dropdown {id} has '{expectedText}' selected");
                var selectedText = await _interactions.GetSelectedDropdownText(id);
                Assert.That(selectedText, Is.EqualTo(expectedText), 
                    $"Expected dropdown to have '{expectedText}' selected but got '{selectedText}'");
                Logger.Information("Dropdown selection verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Dropdown selection verification failed: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region File Input Steps

        [When(@"I upload the file ""(.*)"" using file input with id ""(.*)""")]
        public async Task WhenIUploadTheFileUsingFileInputWithId(string filePath, string id)
        {
            try
            {
                Logger.Information($"Uploading file '{filePath}' using file input with id '{id}'");
                await _interactions.UploadFileById(id, filePath);
                Logger.Information("File upload completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to upload file {filePath}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Button Steps

        [When(@"I click the button with text ""(.*)""")]
        public async Task WhenIClickTheButtonWithText(string buttonText)
        {
            try
            {
                Logger.Information($"Clicking button with text '{buttonText}'");
                await _interactions.ClickButtonByText(buttonText);
                Logger.Information("Button click completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to click button {buttonText}: {ex.Message}");
                throw;
            }
        }

        [When(@"I click the button with id ""(.*)""")]
        public async Task WhenIClickTheButtonWithId(string id)
        {
            try
            {
                Logger.Information($"Clicking button with id '{id}'");
                await _interactions.ClickButtonById(id);
                Logger.Information("Button click completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to click button {id}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Form Interaction Steps

        [When(@"I fill the user registration form with:")]
        public async Task WhenIFillTheUserRegistrationFormWith(Table table)
        {
            try
            {
                Logger.Information("Filling user registration form");
                var formData = table.Rows[0];
                
                await _testPage.FillUserRegistrationFormAsync(
                    formData["FirstName"],
                    formData["LastName"], 
                    formData["Email"],
                    formData["Password"]);
                
                Logger.Information("Form filled successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to fill registration form: {ex.Message}");
                throw;
            }
        }

        [When(@"I select ""(.*)"" as gender")]
        public async Task WhenISelectAsGender(string gender)
        {
            try
            {
                Logger.Information($"Selecting gender: {gender}");
                await _testPage.SelectGenderAsync(gender);
                Logger.Information("Gender selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select gender {gender}: {ex.Message}");
                throw;
            }
        }

        [When(@"I select interests: (.*)")]
        public async Task WhenISelectInterests(string interests)
        {
            try
            {
                Logger.Information($"Selecting interests: {interests}");
                var interestList = interests.Split(',').Select(i => i.Trim()).ToArray();
                await _testPage.SelectInterestsAsync(interestList);
                Logger.Information("Interest selection completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to select interests {interests}: {ex.Message}");
                throw;
            }
        }

        [When(@"I submit the form")]
        public async Task WhenISubmitTheForm()
        {
            try
            {
                Logger.Information("Submitting form");
                await _testPage.SubmitFormAsync();
                Logger.Information("Form submission completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to submit form: {ex.Message}");
                throw;
            }
        }

        [Then(@"the form should be submitted successfully")]
        public async Task ThenTheFormShouldBeSubmittedSuccessfully()
        {
            try
            {
                Logger.Information("Verifying form submission success");
                var isSubmitted = await _testPage.IsFormSubmittedSuccessfullyAsync();
                Assert.That(isSubmitted, Is.True, "Expected form to be submitted successfully");
                Logger.Information("Form submission verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Form submission verification failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"I should see the success message ""(.*)""")]
        public async Task ThenIShouldSeeTheSuccessMessage(string expectedMessage)
        {
            try
            {
                Logger.Information($"Verifying success message: {expectedMessage}");
                var actualMessage = await _testPage.GetSuccessMessageAsync();
                Assert.That(actualMessage, Does.Contain(expectedMessage), 
                    $"Expected success message to contain '{expectedMessage}' but got '{actualMessage}'");
                Logger.Information("Success message verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Success message verification failed: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
