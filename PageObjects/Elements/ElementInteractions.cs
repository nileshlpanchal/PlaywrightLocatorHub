namespace PlaywrightFramework.PageObjects.Elements
{
    /// <summary>
    /// Contains reusable interaction methods for different HTML elements
    /// </summary>
    public class ElementInteractions
    {
        private readonly IPage _page;
        private readonly ElementLocators _locators;
        private readonly WaitHelpers _waitHelpers;

        public ElementInteractions(IPage page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _locators = new ElementLocators(page);
            _waitHelpers = new WaitHelpers(page);
        }

        #region Textbox Interactions

        /// <summary>
        /// Enter text in textbox by ID
        /// </summary>
        public async Task EnterTextInTextboxById(string id, string text, bool clearFirst = true)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in textbox with ID: {id}");
                var textbox = _locators.GetTextboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(textbox);
                
                if (clearFirst)
                {
                    await textbox.ClearAsync();
                }
                
                await textbox.FillAsync(text);
                Logger.Information($"Text entered successfully in textbox: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error entering text in textbox {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Enter text in textbox by name
        /// </summary>
        public async Task EnterTextInTextboxByName(string name, string text, bool clearFirst = true)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in textbox with name: {name}");
                var textbox = _locators.GetTextboxByName(name);
                await _waitHelpers.WaitForElementVisibleAsync(textbox);
                
                if (clearFirst)
                {
                    await textbox.ClearAsync();
                }
                
                await textbox.FillAsync(text);
                Logger.Information($"Text entered successfully in textbox: {name}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error entering text in textbox {name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Enter text in textbox by placeholder
        /// </summary>
        public async Task EnterTextInTextboxByPlaceholder(string placeholder, string text, bool clearFirst = true)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in textbox with placeholder: {placeholder}");
                var textbox = _locators.GetTextboxByPlaceholder(placeholder);
                await _waitHelpers.WaitForElementVisibleAsync(textbox);
                
                if (clearFirst)
                {
                    await textbox.ClearAsync();
                }
                
                await textbox.FillAsync(text);
                Logger.Information($"Text entered successfully in textbox with placeholder: {placeholder}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error entering text in textbox with placeholder {placeholder}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get text value from textbox by ID
        /// </summary>
        public async Task<string> GetTextFromTextboxById(string id)
        {
            try
            {
                Logger.Information($"Getting text from textbox with ID: {id}");
                var textbox = _locators.GetTextboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(textbox);
                
                var text = await textbox.InputValueAsync();
                Logger.Information($"Retrieved text from textbox {id}: {text}");
                return text;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting text from textbox {id}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Radio Button Interactions

        /// <summary>
        /// Select radio button by value
        /// </summary>
        public async Task SelectRadioButtonByValue(string value)
        {
            try
            {
                Logger.Information($"Selecting radio button with value: {value}");
                var radioButton = _locators.GetRadioButtonByValue(value);
                await _waitHelpers.WaitForElementVisibleAsync(radioButton);
                await radioButton.CheckAsync();
                Logger.Information($"Radio button selected successfully: {value}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error selecting radio button {value}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Select radio button by name and value
        /// </summary>
        public async Task SelectRadioButtonByNameAndValue(string name, string value)
        {
            try
            {
                Logger.Information($"Selecting radio button with name '{name}' and value '{value}'");
                var radioButton = _locators.GetRadioButtonByNameAndValue(name, value);
                await _waitHelpers.WaitForElementVisibleAsync(radioButton);
                await radioButton.CheckAsync();
                Logger.Information($"Radio button selected successfully: {name}={value}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error selecting radio button {name}={value}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Select radio button by label text
        /// </summary>
        public async Task SelectRadioButtonByLabel(string labelText)
        {
            try
            {
                Logger.Information($"Selecting radio button with label: {labelText}");
                var radioButton = _locators.GetRadioButtonByLabel(labelText);
                await _waitHelpers.WaitForElementVisibleAsync(radioButton);
                await radioButton.CheckAsync();
                Logger.Information($"Radio button selected successfully by label: {labelText}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error selecting radio button by label {labelText}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Check if radio button is selected by value
        /// </summary>
        public async Task<bool> IsRadioButtonSelectedByValue(string value)
        {
            try
            {
                Logger.Information($"Checking if radio button is selected with value: {value}");
                var radioButton = _locators.GetRadioButtonByValue(value);
                await _waitHelpers.WaitForElementVisibleAsync(radioButton);
                
                var isSelected = await radioButton.IsCheckedAsync();
                Logger.Information($"Radio button {value} selection status: {isSelected}");
                return isSelected;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error checking radio button selection {value}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Checkbox Interactions

        /// <summary>
        /// Check checkbox by ID
        /// </summary>
        public async Task CheckCheckboxById(string id)
        {
            try
            {
                Logger.Information($"Checking checkbox with ID: {id}");
                var checkbox = _locators.GetCheckboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(checkbox);
                await checkbox.CheckAsync();
                Logger.Information($"Checkbox checked successfully: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error checking checkbox {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Uncheck checkbox by ID
        /// </summary>
        public async Task UncheckCheckboxById(string id)
        {
            try
            {
                Logger.Information($"Unchecking checkbox with ID: {id}");
                var checkbox = _locators.GetCheckboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(checkbox);
                await checkbox.UncheckAsync();
                Logger.Information($"Checkbox unchecked successfully: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error unchecking checkbox {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Toggle checkbox by ID
        /// </summary>
        public async Task ToggleCheckboxById(string id)
        {
            try
            {
                Logger.Information($"Toggling checkbox with ID: {id}");
                var checkbox = _locators.GetCheckboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(checkbox);
                
                var isChecked = await checkbox.IsCheckedAsync();
                if (isChecked)
                {
                    await checkbox.UncheckAsync();
                }
                else
                {
                    await checkbox.CheckAsync();
                }
                
                Logger.Information($"Checkbox toggled successfully: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error toggling checkbox {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Check if checkbox is checked by ID
        /// </summary>
        public async Task<bool> IsCheckboxCheckedById(string id)
        {
            try
            {
                Logger.Information($"Checking if checkbox is checked with ID: {id}");
                var checkbox = _locators.GetCheckboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(checkbox);
                
                var isChecked = await checkbox.IsCheckedAsync();
                Logger.Information($"Checkbox {id} checked status: {isChecked}");
                return isChecked;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error checking checkbox status {id}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Dropdown Interactions

        /// <summary>
        /// Select dropdown option by visible text
        /// </summary>
        public async Task SelectDropdownByText(string dropdownId, string optionText)
        {
            try
            {
                Logger.Information($"Selecting dropdown option '{optionText}' in dropdown: {dropdownId}");
                var dropdown = _locators.GetDropdownById(dropdownId);
                await _waitHelpers.WaitForElementVisibleAsync(dropdown);
                await dropdown.SelectOptionAsync(new SelectOptionValue { Label = optionText });
                Logger.Information($"Dropdown option selected successfully: {optionText}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error selecting dropdown option {optionText}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Select dropdown option by value
        /// </summary>
        public async Task SelectDropdownByValue(string dropdownId, string optionValue)
        {
            try
            {
                Logger.Information($"Selecting dropdown option with value '{optionValue}' in dropdown: {dropdownId}");
                var dropdown = _locators.GetDropdownById(dropdownId);
                await _waitHelpers.WaitForElementVisibleAsync(dropdown);
                await dropdown.SelectOptionAsync(new SelectOptionValue { Value = optionValue });
                Logger.Information($"Dropdown option selected successfully by value: {optionValue}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error selecting dropdown option by value {optionValue}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get selected dropdown option text
        /// </summary>
        public async Task<string> GetSelectedDropdownText(string dropdownId)
        {
            try
            {
                Logger.Information($"Getting selected option text from dropdown: {dropdownId}");
                var dropdown = _locators.GetDropdownById(dropdownId);
                await _waitHelpers.WaitForElementVisibleAsync(dropdown);
                
                // Get the current selected value and find corresponding option text
                var selectedValue = await dropdown.InputValueAsync();
                var selectedOption = dropdown.Locator($"option[value='{selectedValue}']");
                var selectedText = await selectedOption.TextContentAsync();
                Logger.Information($"Selected dropdown text: {selectedText}");
                return selectedText ?? string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting selected dropdown text {dropdownId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Combobox Interactions

        /// <summary>
        /// Enter text in combobox by ID
        /// </summary>
        public async Task EnterTextInComboboxById(string id, string text)
        {
            try
            {
                Logger.Information($"Entering text '{text}' in combobox: {id}");
                var combobox = _locators.GetComboboxById(id);
                await _waitHelpers.WaitForElementVisibleAsync(combobox);
                await combobox.FillAsync(text);
                Logger.Information($"Text entered successfully in combobox: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error entering text in combobox {id}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Select combobox option from datalist
        /// </summary>
        public async Task SelectComboboxOption(string comboboxId, string optionText)
        {
            try
            {
                Logger.Information($"Selecting combobox option '{optionText}' in combobox: {comboboxId}");
                var combobox = _locators.GetComboboxById(comboboxId);
                await _waitHelpers.WaitForElementVisibleAsync(combobox);
                
                // Clear and enter text to trigger datalist
                await combobox.ClearAsync();
                await combobox.FillAsync(optionText);
                
                // Press Arrow Down to show options and Enter to select
                await combobox.PressAsync("ArrowDown");
                await combobox.PressAsync("Enter");
                
                Logger.Information($"Combobox option selected successfully: {optionText}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error selecting combobox option {optionText}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region File Input Interactions

        /// <summary>
        /// Upload file using file input by ID
        /// </summary>
        public async Task UploadFileById(string id, string filePath)
        {
            try
            {
                Logger.Information($"Uploading file '{filePath}' using file input: {id}");
                var fileInput = _locators.GetFileInputById(id);
                await _waitHelpers.WaitForElementVisibleAsync(fileInput);
                
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not found: {filePath}");
                }
                
                await fileInput.SetInputFilesAsync(filePath);
                Logger.Information($"File uploaded successfully: {filePath}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error uploading file {filePath}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Upload multiple files using file input by ID
        /// </summary>
        public async Task UploadMultipleFilesById(string id, string[] filePaths)
        {
            try
            {
                Logger.Information($"Uploading {filePaths.Length} files using file input: {id}");
                var fileInput = _locators.GetFileInputById(id);
                await _waitHelpers.WaitForElementVisibleAsync(fileInput);
                
                foreach (var filePath in filePaths)
                {
                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException($"File not found: {filePath}");
                    }
                }
                
                await fileInput.SetInputFilesAsync(filePaths);
                Logger.Information($"Files uploaded successfully: {string.Join(", ", filePaths)}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error uploading multiple files: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Button Interactions

        /// <summary>
        /// Click button by text
        /// </summary>
        public async Task ClickButtonByText(string buttonText)
        {
            try
            {
                Logger.Information($"Clicking button with text: {buttonText}");
                var button = _locators.GetButtonByText(buttonText);
                await _waitHelpers.WaitForElementVisibleAsync(button);
                await button.ClickAsync();
                Logger.Information($"Button clicked successfully: {buttonText}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error clicking button {buttonText}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Click button by ID
        /// </summary>
        public async Task ClickButtonById(string id)
        {
            try
            {
                Logger.Information($"Clicking button with ID: {id}");
                var button = _locators.GetButtonById(id);
                await _waitHelpers.WaitForElementVisibleAsync(button);
                await button.ClickAsync();
                Logger.Information($"Button clicked successfully: {id}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error clicking button {id}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Link Interactions

        /// <summary>
        /// Click link by text
        /// </summary>
        public async Task ClickLinkByText(string linkText)
        {
            try
            {
                Logger.Information($"Clicking link with text: {linkText}");
                var link = _locators.GetLinkByText(linkText);
                await _waitHelpers.WaitForElementVisibleAsync(link);
                await link.ClickAsync();
                Logger.Information($"Link clicked successfully: {linkText}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error clicking link {linkText}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Generic Interactions

        /// <summary>
        /// Click element by selector
        /// </summary>
        public async Task ClickElementBySelector(string selector)
        {
            try
            {
                Logger.Information($"Clicking element with selector: {selector}");
                var element = _locators.GetElementBySelector(selector);
                await _waitHelpers.WaitForElementVisibleAsync(element);
                await element.ClickAsync();
                Logger.Information($"Element clicked successfully: {selector}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error clicking element {selector}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get text content from element by selector
        /// </summary>
        public async Task<string> GetTextBySelector(string selector)
        {
            try
            {
                Logger.Information($"Getting text from element with selector: {selector}");
                var element = _locators.GetElementBySelector(selector);
                await _waitHelpers.WaitForElementVisibleAsync(element);
                
                var text = await element.TextContentAsync();
                Logger.Information($"Retrieved text from element {selector}: {text}");
                return text ?? string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting text from element {selector}: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
