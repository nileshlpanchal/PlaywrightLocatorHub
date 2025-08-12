namespace PlaywrightFramework.PageObjects
{
    /// <summary>
    /// Base page class containing common functionality for all page objects
    /// </summary>
    public abstract class BasePage
    {
        public readonly IPage Page;
        public readonly ElementLocators Locators;
        public readonly ElementInteractions Interactions;
        protected readonly WaitHelpers WaitHelpers;

        protected BasePage(IPage page)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Locators = new ElementLocators(page);
            Interactions = new ElementInteractions(page);
            WaitHelpers = new WaitHelpers(page);
        }

        /// <summary>
        /// Navigate to a specific URL
        /// </summary>
        public virtual async Task NavigateToAsync(string url)
        {
            try
            {
                Logger.Information($"Navigating to URL: {url}");
                await Page.GotoAsync(url);
                await WaitHelpers.WaitForPageLoadAsync();
                Logger.Information("Navigation completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error navigating to {url}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get the current page title
        /// </summary>
        public virtual async Task<string> GetPageTitleAsync()
        {
            try
            {
                var title = await Page.TitleAsync();
                Logger.Information($"Page title: {title}");
                return title;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting page title: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get the current page URL
        /// </summary>
        public virtual string GetCurrentUrl()
        {
            try
            {
                var url = Page.Url;
                Logger.Information($"Current URL: {url}");
                return url;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error getting current URL: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for element to be visible
        /// </summary>
        public virtual async Task WaitForElementVisibleAsync(string selector, int timeoutMs = 30000)
        {
            try
            {
                Logger.Information($"Waiting for element to be visible: {selector}");
                await Page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = timeoutMs
                });
                Logger.Information($"Element is now visible: {selector}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Element not visible within timeout: {selector} - {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Check if element exists on the page
        /// </summary>
        public virtual async Task<bool> IsElementPresentAsync(string selector)
        {
            try
            {
                var element = await Page.QuerySelectorAsync(selector);
                var isPresent = element != null;
                Logger.Information($"Element present check for {selector}: {isPresent}");
                return isPresent;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error checking element presence {selector}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Take screenshot of current page
        /// </summary>
        public virtual async Task<string> TakeScreenshotAsync(string? fileName = null)
        {
            try
            {
                fileName ??= $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var path = Path.Combine("screenshots", fileName);
                
                Directory.CreateDirectory("screenshots");
                
                await Page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = path,
                    FullPage = true
                });
                
                Logger.Information($"Screenshot saved: {path}");
                return path;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error taking screenshot: {ex.Message}");
                throw;
            }
        }
    }
}
