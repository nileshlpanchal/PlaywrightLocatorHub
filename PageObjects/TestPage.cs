namespace PlaywrightFramework.PageObjects
{
    /// <summary>
    /// Test page object demonstrating framework usage with sample HTML elements
    /// </summary>
    public class TestPage : BasePage
    {
        // Page URL
        private readonly string _baseUrl;

        public TestPage(IPage page) : base(page)
        {
            _baseUrl = ConfigReader.GetTestSettings().BaseUrl;
        }

        /// <summary>
        /// Navigate to test page
        /// </summary>
        public async Task NavigateToTestPageAsync()
        {
            var testPageUrl = $"{_baseUrl}/TestData/SampleTestPage.html";
            await NavigateToAsync(testPageUrl);
        }

        /// <summary>
        /// Fill user registration form
        /// </summary>
        public async Task FillUserRegistrationFormAsync(string firstName, string lastName, string email, string password)
        {
            await Interactions.EnterTextInTextboxById("firstName", firstName);
            await Interactions.EnterTextInTextboxById("lastName", lastName);
            await Interactions.EnterTextInTextboxById("email", email);
            await Interactions.EnterTextInTextboxById("password", password);
        }

        /// <summary>
        /// Select gender using radio button
        /// </summary>
        public async Task SelectGenderAsync(string gender)
        {
            await Interactions.SelectRadioButtonByValue(gender.ToLower());
        }

        /// <summary>
        /// Select interests using checkboxes
        /// </summary>
        public async Task SelectInterestsAsync(params string[] interests)
        {
            foreach (var interest in interests)
            {
                await Interactions.CheckCheckboxById(interest.ToLower());
            }
        }

        /// <summary>
        /// Select country from dropdown
        /// </summary>
        public async Task SelectCountryAsync(string country)
        {
            await Interactions.SelectDropdownByText("country", country);
        }

        /// <summary>
        /// Enter city using combobox
        /// </summary>
        public async Task EnterCityAsync(string city)
        {
            await Interactions.EnterTextInComboboxById("city", city);
        }

        /// <summary>
        /// Upload profile picture
        /// </summary>
        public async Task UploadProfilePictureAsync(string filePath)
        {
            await Interactions.UploadFileById("profilePicture", filePath);
        }

        /// <summary>
        /// Submit the form
        /// </summary>
        public async Task SubmitFormAsync()
        {
            await Interactions.ClickButtonById("submitBtn");
        }

        /// <summary>
        /// Verify form submission success
        /// </summary>
        public async Task<bool> IsFormSubmittedSuccessfullyAsync()
        {
            try
            {
                await WaitForElementVisibleAsync("#successMessage", 10000);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get success message text
        /// </summary>
        public async Task<string> GetSuccessMessageAsync()
        {
            return await Interactions.GetTextBySelector("#successMessage");
        }
    }
}
