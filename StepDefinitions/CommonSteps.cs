using TechTalk.SpecFlow;

namespace PlaywrightFramework.StepDefinitions
{
    /// <summary>
    /// Common step definitions for basic browser and page operations
    /// </summary>
    [Binding]
    public class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IPage _page;
        private readonly TestPage _testPage;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _page = (IPage)_scenarioContext["Page"];
            _testPage = new TestPage(_page);
        }

        [Given(@"I navigate to the test page")]
        public async Task GivenINavigateToTheTestPage()
        {
            try
            {
                Logger.Information("Navigating to test page");
                await _testPage.NavigateToTestPageAsync();
                Logger.Information("Successfully navigated to test page");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to navigate to test page: {ex.Message}");
                throw;
            }
        }

        [Given(@"I navigate to ""(.*)""")]
        public async Task GivenINavigateTo(string url)
        {
            try
            {
                Logger.Information($"Navigating to URL: {url}");
                await _testPage.NavigateToAsync(url);
                Logger.Information($"Successfully navigated to: {url}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to navigate to {url}: {ex.Message}");
                throw;
            }
        }

        [Then(@"the page title should be ""(.*)""")]
        public async Task ThenThePageTitleShouldBe(string expectedTitle)
        {
            try
            {
                Logger.Information($"Verifying page title: {expectedTitle}");
                var actualTitle = await _testPage.GetPageTitleAsync();
                Assert.That(actualTitle, Is.EqualTo(expectedTitle), 
                    $"Expected page title '{expectedTitle}' but got '{actualTitle}'");
                Logger.Information("Page title verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"Page title verification failed: {ex.Message}");
                throw;
            }
        }

        [Then(@"the current URL should contain ""(.*)""")]
        public async Task ThenTheCurrentUrlShouldContain(string expectedUrlPart)
        {
            try
            {
                Logger.Information($"Verifying URL contains: {expectedUrlPart}");
                var currentUrl = _testPage.GetCurrentUrl();
                Assert.That(currentUrl, Does.Contain(expectedUrlPart), 
                    $"Expected URL to contain '{expectedUrlPart}' but current URL is '{currentUrl}'");
                Logger.Information("URL verification successful");
            }
            catch (Exception ex)
            {
                Logger.Error($"URL verification failed: {ex.Message}");
                throw;
            }
        }

        [When(@"I wait for (.*) seconds")]
        public async Task WhenIWaitForSeconds(int seconds)
        {
            try
            {
                Logger.Information($"Waiting for {seconds} seconds");
                await Task.Delay(seconds * 1000);
                Logger.Information($"Wait completed: {seconds} seconds");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during wait: {ex.Message}");
                throw;
            }
        }

        [Then(@"I take a screenshot")]
        public async Task ThenITakeAScreenshot()
        {
            try
            {
                Logger.Information("Taking screenshot");
                var screenshotPath = await _testPage.TakeScreenshotAsync();
                Logger.Information($"Screenshot saved: {screenshotPath}");
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to take screenshot: {ex.Message}");
                throw;
            }
        }
    }
}
