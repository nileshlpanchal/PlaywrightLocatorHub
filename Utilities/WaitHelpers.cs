namespace PlaywrightFramework.Utilities
{
    /// <summary>
    /// Helper class for handling waits and synchronization in tests
    /// </summary>
    public class WaitHelpers
    {
        private readonly IPage _page;
        private readonly int _defaultTimeout;

        public WaitHelpers(IPage page)
        {
            _page = page ?? throw new ArgumentNullException(nameof(page));
            _defaultTimeout = ConfigReader.GetTestSettings().Timeout;
        }

        /// <summary>
        /// Wait for element to be visible
        /// </summary>
        public async Task WaitForElementVisibleAsync(ILocator locator, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for element to be visible (timeout: {timeout}ms)");
                
                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = timeout
                });
                
                Logger.Information("Element is now visible");
            }
            catch (Exception ex)
            {
                Logger.Error($"Element not visible within timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for element to be hidden
        /// </summary>
        public async Task WaitForElementHiddenAsync(ILocator locator, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for element to be hidden (timeout: {timeout}ms)");
                
                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Hidden,
                    Timeout = timeout
                });
                
                Logger.Information("Element is now hidden");
            }
            catch (Exception ex)
            {
                Logger.Error($"Element not hidden within timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for element to be attached to DOM
        /// </summary>
        public async Task WaitForElementAttachedAsync(ILocator locator, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for element to be attached (timeout: {timeout}ms)");
                
                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Attached,
                    Timeout = timeout
                });
                
                Logger.Information("Element is now attached");
            }
            catch (Exception ex)
            {
                Logger.Error($"Element not attached within timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for element to be detached from DOM
        /// </summary>
        public async Task WaitForElementDetachedAsync(ILocator locator, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for element to be detached (timeout: {timeout}ms)");
                
                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Detached,
                    Timeout = timeout
                });
                
                Logger.Information("Element is now detached");
            }
            catch (Exception ex)
            {
                Logger.Error($"Element not detached within timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for page to load completely
        /// </summary>
        public async Task WaitForPageLoadAsync(int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for page load (timeout: {timeout}ms)");
                
                await _page.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions
                {
                    Timeout = timeout
                });
                
                Logger.Information("Page load completed");
            }
            catch (Exception ex)
            {
                Logger.Error($"Page load timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for network to be idle
        /// </summary>
        public async Task WaitForNetworkIdleAsync(int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for network idle (timeout: {timeout}ms)");
                
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions
                {
                    Timeout = timeout
                });
                
                Logger.Information("Network is now idle");
            }
            catch (Exception ex)
            {
                Logger.Error($"Network idle timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for DOM content to be loaded
        /// </summary>
        public async Task WaitForDOMContentLoadedAsync(int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for DOM content loaded (timeout: {timeout}ms)");
                
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new PageWaitForLoadStateOptions
                {
                    Timeout = timeout
                });
                
                Logger.Information("DOM content loaded");
            }
            catch (Exception ex)
            {
                Logger.Error($"DOM content load timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for URL to match pattern
        /// </summary>
        public async Task WaitForUrlAsync(string urlPattern, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for URL to match: {urlPattern} (timeout: {timeout}ms)");
                
                await _page.WaitForURLAsync(urlPattern, new PageWaitForURLOptions
                {
                    Timeout = timeout
                });
                
                Logger.Information($"URL now matches pattern: {urlPattern}");
            }
            catch (Exception ex)
            {
                Logger.Error($"URL pattern match timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for specific condition with custom function
        /// </summary>
        public async Task WaitForConditionAsync(Func<Task<bool>> condition, int timeoutMs = 0, int pollingIntervalMs = 500)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for custom condition (timeout: {timeout}ms)");
                
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                
                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    if (await condition())
                    {
                        Logger.Information("Custom condition met");
                        return;
                    }
                    
                    await Task.Delay(pollingIntervalMs);
                }
                
                throw new TimeoutException($"Custom condition not met within {timeout}ms");
            }
            catch (Exception ex)
            {
                Logger.Error($"Custom condition timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Smart wait - combines multiple wait strategies
        /// </summary>
        public async Task SmartWaitAsync(ILocator locator, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Performing smart wait (timeout: {timeout}ms)");
                
                // Wait for element to be attached first
                await WaitForElementAttachedAsync(locator, timeout);
                
                // Then wait for it to be visible
                await WaitForElementVisibleAsync(locator, timeout);
                
                // Finally ensure it's stable (wait a bit for any animations)
                await Task.Delay(100);
                
                Logger.Information("Smart wait completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Smart wait failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for text to appear in element
        /// </summary>
        public async Task WaitForTextAsync(ILocator locator, string expectedText, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for text '{expectedText}' to appear (timeout: {timeout}ms)");
                
                await WaitForConditionAsync(async () =>
                {
                    try
                    {
                        var actualText = await locator.TextContentAsync();
                        return actualText?.Contains(expectedText) == true;
                    }
                    catch
                    {
                        return false;
                    }
                }, timeout);
                
                Logger.Information($"Text '{expectedText}' found in element");
            }
            catch (Exception ex)
            {
                Logger.Error($"Text wait timeout: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Wait for element count to match expected value
        /// </summary>
        public async Task WaitForCountAsync(ILocator locator, int expectedCount, int timeoutMs = 0)
        {
            try
            {
                var timeout = timeoutMs > 0 ? timeoutMs : _defaultTimeout;
                Logger.Information($"Waiting for element count to be {expectedCount} (timeout: {timeout}ms)");
                
                await WaitForConditionAsync(async () =>
                {
                    try
                    {
                        var actualCount = await locator.CountAsync();
                        return actualCount == expectedCount;
                    }
                    catch
                    {
                        return false;
                    }
                }, timeout);
                
                Logger.Information($"Element count is now {expectedCount}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Count wait timeout: {ex.Message}");
                throw;
            }
        }
    }
}
