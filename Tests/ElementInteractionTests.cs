namespace PlaywrightFramework.Tests
{
    /// <summary>
    /// NUnit test class demonstrating framework usage without SpecFlow
    /// </summary>
    [TestFixture]
    public class ElementInteractionTests
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private TestPage? _testPage;
        private string _baseUrl = string.Empty;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Initialize logging
            Logger.Initialize();
            Logger.Information("Starting Element Interaction Tests");
            
            // Get base URL from configuration
            _baseUrl = ConfigReader.GetTestSettings().BaseUrl;
        }

        [SetUp]
        public async Task SetUp()
        {
            // Initialize Playwright
            _playwright = await Playwright.CreateAsync();
            
            // Configure browser options to use system browser
            var browserOptions = new BrowserTypeLaunchOptions
            {
                Headless = true,
                ExecutablePath = "/nix/store/zi4f80l169xlmivz8vja8wlphq74qqk0-chromium-125.0.6422.141/bin/chromium"  // Use system Chromium
            };
            
            // Launch browser
            _browser = await _playwright.Chromium.LaunchAsync(browserOptions);
            _page = await _browser.NewPageAsync();
            
            // Create test page instance
            _testPage = new TestPage(_page);
            
            // Navigate to test page
            var testPageUrl = $"{_baseUrl}/TestData/SampleTestPage.html";
            await _testPage.NavigateToAsync(testPageUrl);
            
            Logger.Information("Test setup completed");
        }

        [TearDown]
        public async Task TearDown()
        {
            // Take screenshot on failure
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                await _testPage!.TakeScreenshotAsync($"failed-{TestContext.CurrentContext.Test.Name}-{DateTime.Now:yyyyMMdd-HHmmss}.png");
                Logger.Information("Screenshot taken for failed test");
            }
            
            // Dispose resources
            if (_page != null)
            {
                await _page.CloseAsync();
                _page = null;
            }
            
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
            }
            
            if (_playwright != null)
            {
                _playwright.Dispose();
                _playwright = null;
            }
            
            Logger.Information("Test teardown completed");
        }

        [Test]
        public async Task TextInput_ShouldAcceptAndRetainValues()
        {
            // Arrange
            const string firstName = "John";
            const string lastName = "Doe";
            const string email = "john.doe@example.com";

            // Act
            await _testPage!.Interactions.EnterTextInTextboxById("firstName", firstName);
            await _testPage.Interactions.EnterTextInTextboxById("lastName", lastName);
            await _testPage.Interactions.EnterTextInTextboxByPlaceholder("Enter your email", email);

            // Assert
            var actualFirstName = await _testPage.Interactions.GetTextFromTextboxById("firstName");
            var actualLastName = await _testPage.Interactions.GetTextFromTextboxById("lastName");

            Assert.Multiple(() =>
            {
                Assert.That(actualFirstName, Is.EqualTo(firstName), "First name should match entered value");
                Assert.That(actualLastName, Is.EqualTo(lastName), "Last name should match entered value");
            });

            Logger.Information("Text input test completed successfully");
        }

        [Test]
        public async Task RadioButtons_ShouldSelectCorrectValues()
        {
            // Act & Assert - Select Male
            await _testPage!.Interactions.SelectRadioButtonByValue("male");
            var isMaleSelected = await _testPage.Interactions.IsRadioButtonSelectedByValue("male");
            Assert.That(isMaleSelected, Is.True, "Male radio button should be selected");

            // Act & Assert - Select Female
            await _testPage.Interactions.SelectRadioButtonByLabel("Female");
            var isFemaleSelected = await _testPage.Interactions.IsRadioButtonSelectedByValue("female");
            var isMaleStillSelected = await _testPage.Interactions.IsRadioButtonSelectedByValue("male");
            
            Assert.Multiple(() =>
            {
                Assert.That(isFemaleSelected, Is.True, "Female radio button should be selected");
                Assert.That(isMaleStillSelected, Is.False, "Male radio button should no longer be selected");
            });

            Logger.Information("Radio button test completed successfully");
        }

        [Test]
        public async Task Checkboxes_ShouldToggleIndependently()
        {
            // Act - Check multiple checkboxes
            await _testPage!.Interactions.CheckCheckboxById("sports");
            await _testPage.Interactions.CheckCheckboxById("music");
            await _testPage.Interactions.CheckCheckboxById("reading");

            // Assert - All should be checked
            var isSportsChecked = await _testPage.Interactions.IsCheckboxCheckedById("sports");
            var isMusicChecked = await _testPage.Interactions.IsCheckboxCheckedById("music");
            var isReadingChecked = await _testPage.Interactions.IsCheckboxCheckedById("reading");

            Assert.Multiple(() =>
            {
                Assert.That(isSportsChecked, Is.True, "Sports checkbox should be checked");
                Assert.That(isMusicChecked, Is.True, "Music checkbox should be checked");
                Assert.That(isReadingChecked, Is.True, "Reading checkbox should be checked");
            });

            // Act - Uncheck one checkbox
            await _testPage.Interactions.UncheckCheckboxById("sports");

            // Assert - Only sports should be unchecked
            var isSportsUnchecked = await _testPage.Interactions.IsCheckboxCheckedById("sports");
            var isMusicStillChecked = await _testPage.Interactions.IsCheckboxCheckedById("music");

            Assert.Multiple(() =>
            {
                Assert.That(isSportsUnchecked, Is.False, "Sports checkbox should be unchecked");
                Assert.That(isMusicStillChecked, Is.True, "Music checkbox should still be checked");
            });

            Logger.Information("Checkbox test completed successfully");
        }

        [Test]
        public async Task Dropdown_ShouldSelectCorrectOptions()
        {
            // Act - Select by text
            await _testPage!.Interactions.SelectDropdownByText("country", "United States");
            var selectedText = await _testPage.Interactions.GetSelectedDropdownText("country");
            Assert.That(selectedText, Is.EqualTo("United States"), "Should select United States");

            // Act - Select by value
            await _testPage.Interactions.SelectDropdownByValue("country", "canada");
            selectedText = await _testPage.Interactions.GetSelectedDropdownText("country");
            Assert.That(selectedText, Is.EqualTo("Canada"), "Should select Canada");

            Logger.Information("Dropdown test completed successfully");
        }

        [Test]
        public async Task CompleteFormSubmission_ShouldSucceed()
        {
            // Arrange
            const string firstName = "Alice";
            const string lastName = "Smith";
            const string email = "alice.smith@test.com";
            const string password = "SecurePass1";

            // Act - Fill form
            await _testPage!.FillUserRegistrationFormAsync(firstName, lastName, email, password);
            await _testPage.SelectGenderAsync("Female");
            await _testPage.SelectInterestsAsync("Sports", "Music", "Reading");
            await _testPage.SelectCountryAsync("Canada");
            await _testPage.EnterCityAsync("Toronto");
            await _testPage.SubmitFormAsync();

            // Assert
            var isSubmitted = await _testPage.IsFormSubmittedSuccessfullyAsync();
            Assert.That(isSubmitted, Is.True, "Form should be submitted successfully");

            var successMessage = await _testPage.GetSuccessMessageAsync();
            Assert.That(successMessage, Does.Contain("Registration completed successfully"), 
                "Should display success message");

            Logger.Information("Complete form submission test completed successfully");
        }

        [Test]
        [TestCase("John", "firstName")]
        [TestCase("Doe", "lastName")]
        [TestCase("test@example.com", "email")]
        public async Task ParameterizedTextInput_ShouldAcceptValues(string inputText, string elementId)
        {
            // Act
            await _testPage!.Interactions.EnterTextInTextboxById(elementId, inputText);

            // Assert
            var actualText = await _testPage.Interactions.GetTextFromTextboxById(elementId);
            Assert.That(actualText, Is.EqualTo(inputText), $"Element {elementId} should contain {inputText}");

            Logger.Information($"Parameterized test completed for {elementId}");
        }

        [Test]
        public async Task ButtonClicks_ShouldTriggerCorrectActions()
        {
            // Arrange - Enter some text first
            await _testPage!.Interactions.EnterTextInTextboxById("firstName", "Test");
            await _testPage.Interactions.EnterTextInTextboxById("lastName", "User");

            // Act - Click reset button
            await _testPage.Interactions.ClickButtonByText("Reset Form");

            // Allow time for reset to complete
            await Task.Delay(1000);

            // Assert - Fields should be cleared
            var firstNameAfterReset = await _testPage.Interactions.GetTextFromTextboxById("firstName");
            var lastNameAfterReset = await _testPage.Interactions.GetTextFromTextboxById("lastName");

            Assert.Multiple(() =>
            {
                Assert.That(firstNameAfterReset, Is.Empty, "First name should be cleared after reset");
                Assert.That(lastNameAfterReset, Is.Empty, "Last name should be cleared after reset");
            });

            Logger.Information("Button click test completed successfully");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Logger.Information("Element Interaction Tests completed");
            Log.CloseAndFlush();
        }
    }
}
