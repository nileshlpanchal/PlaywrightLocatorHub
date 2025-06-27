using TechTalk.SpecFlow;

namespace PlaywrightFramework.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;

        public TestHooks(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Initialize logging
            Logger.Initialize();
            Logger.Information("Test run started");
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            try
            {
                Logger.Information($"Starting scenario: {_scenarioContext.ScenarioInfo.Title}");
                
                var config = ConfigReader.GetTestSettings();
                
                // Initialize Playwright
                _playwright = await Playwright.CreateAsync();
                
                // Launch browser based on configuration
                var browserType = config.Browser.ToLower() switch
                {
                    "firefox" => _playwright.Firefox,
                    "webkit" => _playwright.Webkit,
                    _ => _playwright.Chromium
                };

                _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = config.Headless,
                    SlowMo = config.SlowMo,
                    ExecutablePath = "/nix/store/zi4f80l169xlmivz8vja8wlphq74qqk0-chromium-125.0.6422.141/bin/chromium"
                });

                // Create browser context with configuration
                _context = await _browser.NewContextAsync(new BrowserNewContextOptions
                {
                    ViewportSize = new ViewportSize
                    {
                        Width = config.ViewportWidth,
                        Height = config.ViewportHeight
                    },
                    RecordVideoDir = config.Video ? "videos/" : null
                });

                // Enable tracing if configured
                if (config.Trace)
                {
                    await _context.Tracing.StartAsync(new TracingStartOptions
                    {
                        Screenshots = true,
                        Snapshots = true
                    });
                }

                // Create new page
                _page = await _context.NewPageAsync();
                _page.SetDefaultTimeout(config.Timeout);

                // Store page in scenario context for step definitions
                _scenarioContext["Page"] = _page;
                _scenarioContext["Context"] = _context;
                
                Logger.Information("Browser and page initialized successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during scenario setup: {ex.Message}");
                throw;
            }
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            try
            {
                Logger.Information($"Ending scenario: {_scenarioContext.ScenarioInfo.Title}");
                
                var config = ConfigReader.GetTestSettings();
                
                // Take screenshot on failure
                if (_scenarioContext.TestError != null && config.Screenshot && _page != null)
                {
                    var screenshotPath = $"screenshots/{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    await _page.ScreenshotAsync(new PageScreenshotOptions
                    {
                        Path = screenshotPath,
                        FullPage = true
                    });
                    Logger.Information($"Screenshot saved: {screenshotPath}");
                }

                // Stop tracing
                if (config.Trace && _context != null)
                {
                    var tracePath = $"traces/{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                    await _context.Tracing.StopAsync(new TracingStopOptions
                    {
                        Path = tracePath
                    });
                    Logger.Information($"Trace saved: {tracePath}");
                }

                // Cleanup resources
                if (_page != null) await _page.CloseAsync();
                if (_context != null) await _context.CloseAsync();
                if (_browser != null) await _browser.CloseAsync();
                _playwright?.Dispose();
                
                Logger.Information("Cleanup completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during scenario cleanup: {ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Logger.Information("Test run completed");
            Log.CloseAndFlush();
        }
    }
}
