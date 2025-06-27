namespace PlaywrightFramework.PageObjects.Elements
{
    /// <summary>
    /// Contains reusable locator methods for different HTML elements
    /// </summary>
    public class ElementLocators
    {
        private readonly IPage _page;

        public ElementLocators(IPage page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
        }

        #region Textbox Locators

        /// <summary>
        /// Locate textbox by ID
        /// </summary>
        public ILocator GetTextboxById(string id)
        {
            Logger.Information($"Locating textbox by ID: {id}");
            return _page.Locator($"input[type='text']#{id}, input[type='password']#{id}, input[type='email']#{id}, input[type='number']#{id}, textarea#{id}");
        }

        /// <summary>
        /// Locate textbox by name attribute
        /// </summary>
        public ILocator GetTextboxByName(string name)
        {
            Logger.Information($"Locating textbox by name: {name}");
            return _page.Locator($"input[type='text'][name='{name}'], input[type='password'][name='{name}'], input[type='email'][name='{name}'], input[type='number'][name='{name}'], textarea[name='{name}']");
        }

        /// <summary>
        /// Locate textbox by placeholder text
        /// </summary>
        public ILocator GetTextboxByPlaceholder(string placeholder)
        {
            Logger.Information($"Locating textbox by placeholder: {placeholder}");
            return _page.Locator($"input[placeholder='{placeholder}'], textarea[placeholder='{placeholder}']");
        }

        /// <summary>
        /// Locate textbox by label text
        /// </summary>
        public ILocator GetTextboxByLabel(string labelText)
        {
            Logger.Information($"Locating textbox by label: {labelText}");
            return _page.Locator($"label:has-text('{labelText}') + input, label:has-text('{labelText}') + textarea");
        }

        #endregion

        #region Radio Button Locators

        /// <summary>
        /// Locate radio button by value
        /// </summary>
        public ILocator GetRadioButtonByValue(string value)
        {
            Logger.Information($"Locating radio button by value: {value}");
            return _page.Locator($"input[type='radio'][value='{value}']");
        }

        /// <summary>
        /// Locate radio button by name and value
        /// </summary>
        public ILocator GetRadioButtonByNameAndValue(string name, string value)
        {
            Logger.Information($"Locating radio button by name '{name}' and value '{value}'");
            return _page.Locator($"input[type='radio'][name='{name}'][value='{value}']");
        }

        /// <summary>
        /// Locate radio button by label text
        /// </summary>
        public ILocator GetRadioButtonByLabel(string labelText)
        {
            Logger.Information($"Locating radio button by label: {labelText}");
            return _page.Locator($"label:has-text('{labelText}') input[type='radio'], input[type='radio'] + label:has-text('{labelText}')").First;
        }

        #endregion

        #region Checkbox Locators

        /// <summary>
        /// Locate checkbox by ID
        /// </summary>
        public ILocator GetCheckboxById(string id)
        {
            Logger.Information($"Locating checkbox by ID: {id}");
            return _page.Locator($"input[type='checkbox']#{id}");
        }

        /// <summary>
        /// Locate checkbox by name
        /// </summary>
        public ILocator GetCheckboxByName(string name)
        {
            Logger.Information($"Locating checkbox by name: {name}");
            return _page.Locator($"input[type='checkbox'][name='{name}']");
        }

        /// <summary>
        /// Locate checkbox by label text
        /// </summary>
        public ILocator GetCheckboxByLabel(string labelText)
        {
            Logger.Information($"Locating checkbox by label: {labelText}");
            return _page.Locator($"label:has-text('{labelText}') input[type='checkbox'], input[type='checkbox'] + label:has-text('{labelText}')").First;
        }

        /// <summary>
        /// Locate checkbox by value
        /// </summary>
        public ILocator GetCheckboxByValue(string value)
        {
            Logger.Information($"Locating checkbox by value: {value}");
            return _page.Locator($"input[type='checkbox'][value='{value}']");
        }

        #endregion

        #region Dropdown Locators

        /// <summary>
        /// Locate dropdown (select) by ID
        /// </summary>
        public ILocator GetDropdownById(string id)
        {
            Logger.Information($"Locating dropdown by ID: {id}");
            return _page.Locator($"select#{id}");
        }

        /// <summary>
        /// Locate dropdown by name
        /// </summary>
        public ILocator GetDropdownByName(string name)
        {
            Logger.Information($"Locating dropdown by name: {name}");
            return _page.Locator($"select[name='{name}']");
        }

        /// <summary>
        /// Locate dropdown by label text
        /// </summary>
        public ILocator GetDropdownByLabel(string labelText)
        {
            Logger.Information($"Locating dropdown by label: {labelText}");
            return _page.Locator($"label:has-text('{labelText}') + select");
        }

        #endregion

        #region Combobox Locators

        /// <summary>
        /// Locate combobox (input with datalist) by ID
        /// </summary>
        public ILocator GetComboboxById(string id)
        {
            Logger.Information($"Locating combobox by ID: {id}");
            return _page.Locator($"input[type='text'][list]#{id}");
        }

        /// <summary>
        /// Locate combobox by name
        /// </summary>
        public ILocator GetComboboxByName(string name)
        {
            Logger.Information($"Locating combobox by name: {name}");
            return _page.Locator($"input[type='text'][list][name='{name}']");
        }

        /// <summary>
        /// Locate combobox by label text
        /// </summary>
        public ILocator GetComboboxByLabel(string labelText)
        {
            Logger.Information($"Locating combobox by label: {labelText}");
            return _page.Locator($"label:has-text('{labelText}') + input[type='text'][list]");
        }

        #endregion

        #region File Input Locators

        /// <summary>
        /// Locate file input by ID
        /// </summary>
        public ILocator GetFileInputById(string id)
        {
            Logger.Information($"Locating file input by ID: {id}");
            return _page.Locator($"input[type='file']#{id}");
        }

        /// <summary>
        /// Locate file input by name
        /// </summary>
        public ILocator GetFileInputByName(string name)
        {
            Logger.Information($"Locating file input by name: {name}");
            return _page.Locator($"input[type='file'][name='{name}']");
        }

        /// <summary>
        /// Locate file input by label text
        /// </summary>
        public ILocator GetFileInputByLabel(string labelText)
        {
            Logger.Information($"Locating file input by label: {labelText}");
            return _page.Locator($"label:has-text('{labelText}') + input[type='file']");
        }

        #endregion

        #region Button Locators

        /// <summary>
        /// Locate button by text content
        /// </summary>
        public ILocator GetButtonByText(string text)
        {
            Logger.Information($"Locating button by text: {text}");
            return _page.Locator($"button:has-text('{text}'), input[type='button'][value='{text}'], input[type='submit'][value='{text}']");
        }

        /// <summary>
        /// Locate button by ID
        /// </summary>
        public ILocator GetButtonById(string id)
        {
            Logger.Information($"Locating button by ID: {id}");
            return _page.Locator($"button#{id}, input[type='button']#{id}, input[type='submit']#{id}");
        }

        /// <summary>
        /// Locate button by name
        /// </summary>
        public ILocator GetButtonByName(string name)
        {
            Logger.Information($"Locating button by name: {name}");
            return _page.Locator($"button[name='{name}'], input[type='button'][name='{name}'], input[type='submit'][name='{name}']");
        }

        #endregion

        #region Link Locators

        /// <summary>
        /// Locate link by text content
        /// </summary>
        public ILocator GetLinkByText(string text)
        {
            Logger.Information($"Locating link by text: {text}");
            return _page.Locator($"a:has-text('{text}')");
        }

        /// <summary>
        /// Locate link by href attribute
        /// </summary>
        public ILocator GetLinkByHref(string href)
        {
            Logger.Information($"Locating link by href: {href}");
            return _page.Locator($"a[href='{href}']");
        }

        #endregion

        #region Generic Locators

        /// <summary>
        /// Locate element by custom CSS selector
        /// </summary>
        public ILocator GetElementBySelector(string selector)
        {
            Logger.Information($"Locating element by selector: {selector}");
            return _page.Locator(selector);
        }

        /// <summary>
        /// Locate element by XPath
        /// </summary>
        public ILocator GetElementByXPath(string xpath)
        {
            Logger.Information($"Locating element by XPath: {xpath}");
            return _page.Locator($"xpath={xpath}");
        }

        /// <summary>
        /// Locate element by text content
        /// </summary>
        public ILocator GetElementByText(string text)
        {
            Logger.Information($"Locating element by text: {text}");
            return _page.Locator($"text={text}");
        }

        #endregion
    }
}
